---
tags:
  - Database
  - Entity
Дата создания: 2024-04-21T14:34
---
# MoneyAccount (Денежный счёт)
Представляет собой абстрактный (или физический) счёт, на балансе которого числятся денежные средства.

| Наименование атрибута | Тип данных  | Ограничения     | Комментарий    |
| :-------------------- | ----------- | --------------- | -------------- |
| MoneyAccountId        | TINYINT     | PK IDENTITY     | Идентификатор  |
| AccountName           | VARCHAR(20) | NOT NULL UNIQUE | Наименование   |
| Balance               | MONEY       | DEFAULT 0       | Текущий баланс |

```sql
CREATE TABLE MoneyAccounts(
	MoneyAccountId TINYINT CONSTRAINT PK_MoneyAccountId PRIMARY KEY(MoneyAccountId) IDENTITY,
	AccountName VARCHAR(20) NOT NULL UNIQUE,
	Balance MONEY NOT NULL DEFAULT 0
);
GO
```