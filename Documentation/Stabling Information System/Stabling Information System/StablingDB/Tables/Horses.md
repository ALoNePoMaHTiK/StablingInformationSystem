---
tags:
  - Database
  - Entity
Дата создания: 2024-04-21T14:31
---
# Horse (Лошадь)
Представляет собой лошадь как основной бизнес-ресурс.

| Наименование атрибута | Тип данных  | Ограничения | Комментарий   |
|:--------------------- | ----------- | ----------- | ------------- |
| HorseId               | SMALLINT    | PK IDENTITY | Идентификатор |
| HorseName             | VARCHAR(50) | NOT NULL    | Кличка        |
| IsAvailable           | BIT         | DEFAULT 1   | Статус        |

```sql
CREATE TABLE Horses(
	HorseId INT CONSTRAINT PK_HorseId PRIMARY KEY(HorseId) IDENTITY, 
	HorseName VARCHAR(25) NOT NULL UNIQUE,
	IsAvailable BIT NOT NULL DEFAULT 1
);
GO
```