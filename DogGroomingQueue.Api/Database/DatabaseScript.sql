CREATE DATABASE DogGroomingQueue;
GO

USE DogGroomingQueue;
GO

CREATE TABLE User_Ta
(
    UserId_Int INT IDENTITY(1,1) PRIMARY KEY,
    Username_Vch NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash_Vch NVARCHAR(255) NOT NULL,
    FirstName_Vch NVARCHAR(50) NOT NULL,
    CreatedAt_Dat DATETIME NOT NULL DEFAULT GETDATE()
);
CREATE TABLE DogGroomingTypes_Ta
(
    GroomingTypeId_Int INT IDENTITY(1,1) PRIMARY KEY,
    TypeName_Vch NVARCHAR(50) NOT NULL,
    DurationMinutes_Int INT NOT NULL,
    Price_Dec DECIMAL(10,2) NOT NULL
);
INSERT INTO DogGroomingTypes_Ta
(TypeName_Vch, DurationMinutes_Int, Price_Dec)
VALUES
(N'כלב קטן', 30, 100),
(N'כלב בינוני', 45, 150),
(N'כלב גדול', 60, 200);

CREATE TABLE Appointments_Ta
(
    AppointmentId_Int INT IDENTITY(1,1) PRIMARY KEY,
    UserId_Int INT NOT NULL,
    GroomingTypeId_Int INT NOT NULL,
    AppointmentDateTime_Dat DATETIME NOT NULL,
    CreatedAt_Dat DATETIME NOT NULL DEFAULT GETDATE(),
    IsDeleted_Bit BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Appointments_Users
        FOREIGN KEY (UserId_Int) REFERENCES User_Ta(UserId_Int),

    CONSTRAINT FK_Appointments_GroomingTypes
        FOREIGN KEY (GroomingTypeId_Int) REFERENCES DogGroomingTypes_Ta(GroomingTypeId_Int)
);

USE [DogGroomingQueue]
GO

/****** Object:  View [dbo].[vw_AppointmentsDetailsSwish]    Script Date: 29/06/2026 07:52:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_AppointmentsDetailsSwish]
AS
SELECT
    a.AppointmentId_Int,
    a.UserId_Int,
    u.FirstName_Vch AS CustomerName,
    u.Username_Vch,
    a.AppointmentDateTime_Dat,
    a.CreatedAt_Dat,
    gt.GroomingTypeId_Int,
    gt.TypeName_Vch AS DogTypeName,
    gt.DurationMinutes_Int,
    gt.Price_Dec,
    a.IsDeleted_Bit
FROM Appointments_Ta a
INNER JOIN User_Ta u
    ON u.UserId_Int = a.UserId_Int
INNER JOIN DogGroomingTypes_Ta gt
    ON gt.GroomingTypeId_Int = a.GroomingTypeId_Int;
GO


USE [DogGroomingQueue]
GO

/****** Object:  StoredProcedure [dbo].[sp_CreateAppointment]    Script Date: 29/06/2026 07:53:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_CreateAppointment]
    @UserId INT,
    @GroomingTypeId INT,
    @AppointmentDateTime DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Appointments_Ta
    (
        UserId_Int,
        GroomingTypeId_Int,
        AppointmentDateTime_Dat
    )
    VALUES
    (
        @UserId,
        @GroomingTypeId,
        @AppointmentDateTime
    );

    SELECT SCOPE_IDENTITY() AS NewAppointmentId;
END
GO


