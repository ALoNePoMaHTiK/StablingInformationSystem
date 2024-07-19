---
tags:
  - Database
  - Entity
Дата создания: 2024-04-21T17:39
---
# Trainer (Тренер)
Представляет собой тренера, как основного актора (исполнителя) бизнес-процесса.

| Наименование атрибута | Тип данных   | Ограничения     | Комментарий     |
| :-------------------- | :----------- | :-------------- | :-------------- |
| TrainerId             | SMALLINT     | PK IDENTITY     | Идентификатор   |
| FullName              | VARCHAR(150) | NOT NULL        | ФИО             |
| Color                 | CHAR(7)      | NOT NULL UNIQUE | Уникальный цвет |
| SalaryRate            | INT          | NOT NULL        | Ставка зарплаты |
| IsAvailable           | BIT          | DEFAULT 1       | Статус          |
```sql
CREATE TABLE Trainers(
	TrainerId INT CONSTRAINT PK_TrainerId PRIMARY KEY(TrainerId) IDENTITY,
	FullName VARCHAR(60) NOT NULL,
	Color CHAR(7) NOT NULL UNIQUE,
	SalaryRate INT NOT NULL, --- Ставка зарплаты ---
	IsAvailable BIT NOT NULL DEFAULT 1
);
GO
```