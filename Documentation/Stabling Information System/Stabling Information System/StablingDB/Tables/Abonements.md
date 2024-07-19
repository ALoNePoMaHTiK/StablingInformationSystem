---
tags:
  - Database
  - Entity
Дата создания: 2024-05-02T11:05
---
# Abonement (Абонемент)
Представляет собой абонемент для оплаты [[Trainings|Тренировок]]. При открытии абонемент имеет ссылку на конкретного [[Clients|Клиента]] и [[AbonementTypes|Тип абонемента]].
```sql
CREATE TABLE Abonements(
	AbonementId INT CONSTRAINT PK_AbonementId PRIMARY KEY(AbonementId) IDENTITY,
	ClientId INT NOT NULL,
	CONSTRAINT FK_Abonements_ClientId FOREIGN KEY(ClientId) REFERENCES Clients(ClientId),
	AbonementTypeId INT NOT NULL,
	CONSTRAINT FK_Abonement_AbonementTypeId FOREIGN KEY(AbonementTypeId) REFERENCES AbonementTypes(AbonementTypeId),
	UsesCount INT NOT NULL,
	OpenDateTime SMALLDATETIME NOT NULL, --- Заполняется при помощи триггера ---
	IsAvailable BIT NOT NULL DEFAULT 1
);
```
