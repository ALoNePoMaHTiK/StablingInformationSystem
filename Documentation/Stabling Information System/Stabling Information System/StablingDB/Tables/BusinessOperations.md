---
tags:
  - Database
  - Entity
Дата создания: 2024-04-22T15:28
---
# BusinessOperations (Бизнес-операция)

```sql
CREATE TABLE BusinessOperations(
	BusinessOperationId INT CONSTRAINT PK_BusinessOperationId PRIMARY KEY(BusinessOperationId) IDENTITY,
	OperationTypeId INT NOT NULL,
	CONSTRAINT FK_BusinessOperations_OperationTypeId FOREIGN KEY(OperationTypeId) REFERENCES BusinessOperationTypes(BusinessOperationTypeId),
	MoneyAccountId TINYINT NOT NULL,
	CONSTRAINT FK_BusinessOperations_MoneyAccountId FOREIGN KEY(MoneyAccountId) REFERENCES MoneyAccounts(MoneyAccountId),
	Amount SMALLMONEY NOT NULL,
	OperationDateTime SMALLDATETIME NOT NULL,
	Comment VARCHAR(100) NULL
);
GO
```

# Триггеры

Изменение баланса расчетного счета после бизнес-операции
```sql
CREATE TRIGGER TR_BusinessOperations_AfterInsert ON BusinessOperations AFTER INSERT AS
BEGIN
	DECLARE @Amount SMALLMONEY;
	DECLARE @Balance MONEY;
	DECLARE @MoneyAccountId SMALLMONEY;
	DECLARE @BusinessOperationTypeId INT;
	DECLARE @IsIncome BIT;
	
	SELECT @Amount = Amount FROM inserted;
	SELECT @MoneyAccountId = MoneyAccountId FROM inserted;
	SELECT @Balance = Balance FROM MoneyAccounts WHERE MoneyAccountId = @MoneyAccountId;
	SELECT @BusinessOperationTypeId = OperationTypeId FROM inserted;
	SELECT @IsIncome = IsIncome FROM BusinessOperationTypes WHERE BusinessOperationTypeId = @BusinessOperationTypeId;


	IF @IsIncome = 1
		BEGIN
			UPDATE MoneyAccounts
			SET Balance = @Balance + @Amount
			WHERE MoneyAccountId = @MoneyAccountId;
		END
	ELSE
		BEGIN
		UPDATE MoneyAccounts
			SET Balance = @Balance - @Amount
			WHERE MoneyAccountId = @MoneyAccountId;
		END
END
GO
```

# Представления
Представление для получения списка бизнес=операций с указанным флагом типа операции (доход/расход)
```sql
CREATE VIEW VW_BusinessOperationsByIncome AS
	SELECT op.*,t.IsIncome FROM BusinessOperations AS op 
	LEFT JOIN BusinessOperationTypes t
	ON op.OperationTypeId = t.BusinessOperationTypeId;
GO
```