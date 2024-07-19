---
tags:
  - Database
  - Entity
Дата создания: 2024-04-22T15:32
---
# BusinessOperationType (Тип бизнес-операции)

```sql
CREATE TABLE BusinessOperationTypes(
	BusinessOperationTypeId INT CONSTRAINT PK_BusinessOperationTypeId PRIMARY KEY(BusinessOperationTypeId) IDENTITY,
	TypeName VARCHAR(60) NOT NULL UNIQUE,
	TypeAmount SMALLMONEY NOT NULL,
	IsIncome BIT NOT NULL DEFAULT 1,
);
GO
```