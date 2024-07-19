---
tags:
  - Database
  - Entity
Дата создания: 2024-05-02T11:01
---
# AbonementType (Тип абонемента)
Представляет собой тип абонемента, обладающий уникальными стоимостью/длительностью/количеством использований.

```sql
CREATE TABLE AbonementTypes(
	AbonementTypeId INT CONSTRAINT PK_AbonementTypeId PRIMARY KEY(AbonementTypeId) IDENTITY,
	TypeName VARCHAR(60) NOT NULL UNIQUE,
	TypeUsesCount INT NOT NULL,
	TypePrice INT NOT NULL,
	IsAvailable BIT NOT NULL DEFAULT 1
);
```
