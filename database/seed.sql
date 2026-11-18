USE ParkingControlDb;
GO

INSERT INTO dbo.ParkingZones (Name, Capacity, Description) VALUES
(N'Зона A', 20, N'Крытая парковка'),
(N'Зона B', 15, N'Открытая парковка'),
(N'Зона C', 10, N'VIP-зона');

INSERT INTO dbo.ParkingSpaces (ZoneId, SpaceNumber) VALUES
(1, N'A-01'), (1, N'A-02'), (1, N'A-03'), (1, N'A-04'),
(2, N'B-01'), (2, N'B-02'), (2, N'B-03'),
(3, N'C-01'), (3, N'C-02');

INSERT INTO dbo.Tariffs (Name, PricePerHour) VALUES
(N'Стандарт', 150.00),
(N'Ночь',     80.00),
(N'VIP',      300.00);

INSERT INTO dbo.Clients (FullName, Phone, Email) VALUES
(N'Иванов Иван Иванович', N'+79000000001', N'ivanov@example.com'),
(N'Петрова Анна Сергеевна', N'+79000000002', N'petrova@example.com'),
(N'Сидоров Олег Петрович', N'+79000000003', NULL);

INSERT INTO dbo.Vehicles (ClientId, PlateNumber, Model) VALUES
(1, N'А123ВС77', N'Toyota Camry'),
(1, N'В456ЕК77', N'Kia Rio'),
(2, N'С789МН77', N'Hyundai Solaris'),
(3, N'Е012ОР77', N'BMW X5');

INSERT INTO dbo.ParkingSessions (VehicleId, SpaceId, TariffId, EntryTime, ExitTime, TotalCost) VALUES
(1, 1, 1, DATEADD(HOUR, -5, SYSDATETIME()), DATEADD(HOUR, -2, SYSDATETIME()), 450.00),
(2, 5, 1, DATEADD(HOUR, -3, SYSDATETIME()), NULL, NULL),
(3, 2, 1, DATEADD(HOUR, -2, SYSDATETIME()), NULL, NULL);
