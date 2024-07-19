---
tags:
  - Database
  - Entity
Дата создания: 2024-04-21T17:43
---
# Client (Клиент)


```sql
CREATE TABLE Clients(
	ClientId INT CONSTRAINT PK_ClientId PRIMARY KEY(ClientId) IDENTITY,
	TrainerId INT NOT NULL CONSTRAINT FK_Clients_TrainerId FOREIGN KEY(TrainerId) REFERENCES Trainers(TrainerId),
	FullName VARCHAR(60) NOT NULL,
	PhoneNumber CHAR(11) NOT NULL,
	Balance FLOAT NOT NULL DEFAULT 0,
	Email VARCHAR(60),
	IsAvailable BIT NOT NULL DEFAULT 1
);
GO
```