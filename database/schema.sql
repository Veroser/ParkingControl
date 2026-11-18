IF DB_ID('ParkingControlDb') IS NULL
    CREATE DATABASE ParkingControlDb;
GO
USE ParkingControlDb;
GO

IF OBJECT_ID('dbo.ParkingSessions','U') IS NOT NULL DROP TABLE dbo.ParkingSessions;
IF OBJECT_ID('dbo.Vehicles','U') IS NOT NULL DROP TABLE dbo.Vehicles;
IF OBJECT_ID('dbo.Clients','U') IS NOT NULL DROP TABLE dbo.Clients;
IF OBJECT_ID('dbo.ParkingSpaces','U') IS NOT NULL DROP TABLE dbo.ParkingSpaces;
IF OBJECT_ID('dbo.ParkingZones','U') IS NOT NULL DROP TABLE dbo.ParkingZones;
IF OBJECT_ID('dbo.Tariffs','U') IS NOT NULL DROP TABLE dbo.Tariffs;
GO

CREATE TABLE dbo.ParkingZones (
    ZoneId      INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(50)  NOT NULL UNIQUE,
    Capacity    INT           NOT NULL CHECK (Capacity > 0),
    Description NVARCHAR(200) NULL
);

CREATE TABLE dbo.ParkingSpaces (
    SpaceId     INT IDENTITY(1,1) PRIMARY KEY,
    ZoneId      INT           NOT NULL,
    SpaceNumber NVARCHAR(20)  NOT NULL,
    IsActive    BIT           NOT NULL CONSTRAINT DF_Spaces_IsActive DEFAULT (1),
    CONSTRAINT UQ_Spaces_Zone_Number UNIQUE (ZoneId, SpaceNumber),
    CONSTRAINT FK_Spaces_Zones FOREIGN KEY (ZoneId) REFERENCES dbo.ParkingZones(ZoneId)
);

CREATE TABLE dbo.Tariffs (
    TariffId    INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(50)  NOT NULL UNIQUE,
    PricePerHour DECIMAL(10,2) NOT NULL CHECK (PricePerHour > 0)
);

CREATE TABLE dbo.Clients (
    ClientId    INT IDENTITY(1,1) PRIMARY KEY,
    FullName    NVARCHAR(100) NOT NULL,
    Phone       NVARCHAR(20)  NOT NULL UNIQUE,
    Email       NVARCHAR(100) NULL
);

CREATE TABLE dbo.Vehicles (
    VehicleId   INT IDENTITY(1,1) PRIMARY KEY,
    ClientId    INT           NOT NULL,
    PlateNumber NVARCHAR(15)  NOT NULL UNIQUE,
    Model       NVARCHAR(50)  NOT NULL,
    CONSTRAINT FK_Vehicles_Clients FOREIGN KEY (ClientId) REFERENCES dbo.Clients(ClientId)
);

CREATE TABLE dbo.ParkingSessions (
    SessionId   INT IDENTITY(1,1) PRIMARY KEY,
    VehicleId   INT           NOT NULL,
    SpaceId     INT           NOT NULL,
    TariffId    INT           NOT NULL,
    EntryTime   DATETIME2     NOT NULL,
    ExitTime    DATETIME2     NULL,
    TotalCost   DECIMAL(10,2) NULL CHECK (TotalCost IS NULL OR TotalCost >= 0),
    CONSTRAINT FK_Sessions_Vehicles FOREIGN KEY (VehicleId) REFERENCES dbo.Vehicles(VehicleId),
    CONSTRAINT FK_Sessions_Spaces   FOREIGN KEY (SpaceId)   REFERENCES dbo.ParkingSpaces(SpaceId),
    CONSTRAINT FK_Sessions_Tariffs  FOREIGN KEY (TariffId)  REFERENCES dbo.Tariffs(TariffId),
    CONSTRAINT CK_Sessions_Times    CHECK (ExitTime IS NULL OR ExitTime > EntryTime)
);

CREATE UNIQUE INDEX UX_Sessions_ActiveSpace
    ON dbo.ParkingSessions(SpaceId)
    WHERE ExitTime IS NULL;

CREATE UNIQUE INDEX UX_Sessions_ActiveVehicle
    ON dbo.ParkingSessions(VehicleId)
    WHERE ExitTime IS NULL;
