---
tags:
  - Database
  - Entity
Дата создания: 2024-04-21T17:40
---
# TrainingType (Тип тренировки)
Представляет собой справочную запись о конкретном типе тренировки, обладающим определенной длительностью и стоимостью. Является категориальной сущностью и будет использоваться для анализа.

| Наименование атрибута | Тип данных   | Ограничения     | Комментарий              |
|:--------------------- |:------------ |:--------------- |:------------------------ |
| TrainingTypeId        | SMALLINT     | PK IDENTITY     | Идентификатор            |
| TypeName              | VARCHAR(60)  | NOT NULL UNIQUE | Наименование             |
| TypePrice             | INT          | NOT NULL        | Стандартная цена оплаты  |
| TypeDuration          | DECIMAL(2,1) | NOT NULL        | Стандартная длительность |
| IsAvailable           | BIT          | DEFAULT 1       | Статус                   |
```sql
CREATE TABLE TrainingTypes(
	TrainingTypeId INT CONSTRAINT PK_TrainingTypeId PRIMARY KEY(TrainingTypeId) IDENTITY,
	TypeName VARCHAR(60) NOT NULL UNIQUE,
	TypePrice INT NOT NULL,
	TypeDuration DECIMAL(2,1) NOT NULL,
	IsAvailable BIT NOT NULL DEFAULT 1
);
GO
```