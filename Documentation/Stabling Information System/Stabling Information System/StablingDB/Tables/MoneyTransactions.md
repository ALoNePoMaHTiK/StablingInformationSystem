---
tags:
  - Database
  - Entity
Дата создания: 2024-04-21T17:45
---
# MoneyTransaction (Денежная транзакция)

```sql
CREATE TABLE MoneyTransactions(
	MoneyTransactionId INT CONSTRAINT PK_MoneyTransactionId PRIMARY KEY(MoneyTransactionId) IDENTITY,
	TrainerId INT NOT NULL CONSTRAINT FK_MoneyTransaction_TrainerId FOREIGN KEY(TrainerId) REFERENCES Trainers(TrainerId),
	MoneyAccountId TINYINT NOT NULL CONSTRAINT FK_MoneyTransaction_MoneyAccountId FOREIGN KEY(MoneyAccountId) REFERENCES MoneyAccounts(MoneyAccountId),
	TransactionDate DATE NOT NULL,
	Amount FLOAT NOT NULL,
);
```

# Триггеры

## TR_MoneyTransactions_AfterInsert
Изменение баланса расчетного счета после транзакции

```sql
CREATE TRIGGER TR_MoneyTransactions_AfterInsert ON MoneyTransactions AFTER INSERT AS
BEGIN
	DECLARE @Amount SMALLMONEY;
	DECLARE @Balance MONEY;
	DECLARE @MoneyAccountId SMALLMONEY;
	
	SELECT @Amount = Amount FROM inserted;
	SELECT @MoneyAccountId = MoneyAccountId FROM inserted;
	SELECT @Balance = Balance FROM MoneyAccounts WHERE MoneyAccountId = @MoneyAccountId;

	UPDATE MoneyAccounts
	SET Balance = @Balance + @Amount
	WHERE MoneyAccountId = @MoneyAccountId;
END
GO
```