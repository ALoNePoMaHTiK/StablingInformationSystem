---
tags:
  - Database
  - Entity
Дата создания: 2024-04-21T17:44
---
# Training (Тренировка)
Представляет собой тренировку как основную услугу, поставляемую клиентам. При инициализации имеет ссылки на [[TrainingTypes|тип тренировки]], [[Trainers|тренера]], [[Horses|лошадь]], [[Clients|клиента]].
```sql
CREATE TABLE Trainings(
	TrainingId INT CONSTRAINT PK_TrainingId PRIMARY KEY(TrainingId) IDENTITY,
	TrainingTypeId INT NOT NULL,
	CONSTRAINT FK_Training_TrainingTypeId FOREIGN KEY(TrainingTypeId) 
	REFERENCES TrainingTypes(TrainingTypeId),
	TrainerId INT NOT NULL,
	CONSTRAINT FK_Training_TrainerId FOREIGN KEY(TrainerId) 
	REFERENCES Trainers(TrainerId),
	HorseId INT NOT NULL,
	CONSTRAINT FK_Training_HorseId FOREIGN KEY(HorseId) 
	REFERENCES Horses(HorseId),
	ClientId INT NOT NULL,
	CONSTRAINT FK_Training_ClientId FOREIGN KEY(ClientId) 
	REFERENCES Clients(ClientId),
	TrainingDateTime DATETIME NOT NULL,
	Duration DECIMAL(2,1) NOT NULL DEFAULT 1.0
);
GO
```

# Триггеры

## TR_Trainings_InsteadInsert
Триггер на автоматическое вычисление длительности тренировки исходя из типа тренировки

```sql
CREATE TRIGGER TR_Trainings_InsteadInsert ON Trainings INSTEAD OF INSERT AS
BEGIN
	DECLARE @trainingTypeId INT;
	DECLARE @clientId INT;
	DECLARE @horseId INT;
	DECLARE @trainerId INT;
	DECLARE @trainingDateTime DATETIME;
	SELECT @trainingTypeId = TrainingTypeId FROM inserted;
	SELECT @clientId = ClientId FROM inserted;
	SELECT @horseId = HorseId FROM inserted;
	SELECT @trainerId = TrainerId FROM inserted;
	SELECT @trainingDateTime = TrainingDateTime FROM inserted;

	DECLARE @trainingDuration DECIMAL(2,1);
	SELECT @trainingDuration = (SELECT TypeDuration FROM TrainingTypes WHERE TrainingTypeId = @trainingTypeId);
	INSERT INTO Trainings (TrainingTypeId,ClientId,HorseId,TrainerId,TrainingDateTime,Duration) VALUES
		(@trainingTypeId,@clientId,@horseId,@trainerId,@trainingDateTime,@trainingDuration);
	SELECT TrainingId FROM Trainings WHERE @@ROWCOUNT > 0 AND TrainingId = SCOPE_IDENTITY()		--Entity Framework жалуется если нет вывода TrainingId
END
GO
```

## TR_Trainings_AfterUpdate
Триггер автоматического обновления длительности тренировки при изменении типа тренировки

```sql
CREATE TRIGGER TR_Trainings_AfterUpdate ON Trainings AFTER UPDATE AS
BEGIN
	DECLARE @trainingId INT;
	DECLARE @trainingTypeId INT;
	DECLARE @trainingDuration DECIMAL(2,1);
	SELECT @trainingId = TrainingId FROM inserted;
	SELECT @trainingTypeId = TrainingTypeId FROM inserted;
	SELECT @trainingDuration = (SELECT TypeDuration FROM TrainingTypes WHERE TrainingTypeId = @trainingTypeId);
	UPDATE Trainings
	SET Duration = @trainingDuration
	WHERE TrainingId = @trainingId;  --Entity Framework жалуется если нет вывода TrainingId
END
GO
```


# Представления

## VW_TrainingsForShow
Представление для отображения списка тренировок в клиентском приложении
```sql
CREATE VIEW VW_TrainingsForShow AS
(SELECT t.TrainingId,c.ClientId,c.FullName AS ClientName,
	t.TrainerId,tr.FullName AS TrainerName,t.TrainingDateTime,
	h.HorseId,h.HorseName,tt.TrainingTypeId,
	tt.TypeName,t.TrainingDateTime AS TrainingStart, 
	DATEADD(MINUTE, t.Duration*60, t.TrainingDateTime) AS TrainingFinish, t.IsPaid,
	CONCAT(c.FullName,'_',h.HorseName,'_',tr.FullName,'_',
	FORMAT( t.TrainingDateTime, 'dd.MM.yyyy HH:mm', 'en-US' )
	) as TrainingLabel,
	ISNULL(SUM(bw.Amount),0) AS TrainingFunds FROM Trainings t
	LEFT JOIN Clients c
	ON c.ClientId = t.ClientId
	LEFT JOIN Trainers tr
	ON tr.TrainerId = t.TrainerId
	LEFT JOIN TrainingTypes tt
	ON tt.TrainingTypeId = t.TrainingTypeId
	LEFT JOIN Horses h
	ON h.HorseId = t.HorseId
	LEFT JOIN TrainingWithdrawings tw
	ON t.TrainingId = tw.TrainingId
	LEFT JOIN BalanceWithdrawings bw
	ON tw.BalanceWithdrawingId = bw.BalanceWithdrawingId
	GROUP BY t.TrainingId, c.ClientId,c.FullName,
	t.TrainerId,tr.FullName,t.TrainingDateTime,
	h.HorseId,h.HorseName,tt.TrainingTypeId,
	tt.TypeName,t.TrainingDateTime, DATEADD(MINUTE, t.Duration*60, t.TrainingDateTime), t.IsPaid
);
GO
```