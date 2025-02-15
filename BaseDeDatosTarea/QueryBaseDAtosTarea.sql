-- Crear la base de datos
CREATE DATABASE ToolBorrowing;
GO

-- Usar la base de datos
USE ToolBorrowing;
GO

-- Crear logins (usuarios de SQL Server con credenciales propias)
CREATE LOGIN Christian WITH PASSWORD = 'Password123';
CREATE LOGIN Kendall WITH PASSWORD = 'Password124';
CREATE LOGIN Santiago WITH PASSWORD = 'Password125';
CREATE LOGIN Fabiana WITH PASSWORD = 'Password126';

-- Crear usuarios en la base de datos y asignar roles
USE ToolBorrowing;
CREATE USER Christian FOR LOGIN Christian;
CREATE USER Kendall FOR LOGIN Kendall;
CREATE USER Santiago FOR LOGIN Santiago;
CREATE USER Fabiana FOR LOGIN Fabiana;

-- Crear rol personalizado
CREATE ROLE DataEditor;
CREATE ROLE DataManager;

-- Conceder permisos específicos al rol
USE ToolBorrowing;
GRANT SELECT, UPDATE TO DataEditor;
GRANT SELECT, UPDATE, DELETE TO DataManager;

-- Conceder permiso para crear ciertos objetos
GRANT CREATE TABLE TO DataManager;
GRANT CREATE VIEW TO DataManager;
GRANT CREATE PROCEDURE TO DataManager;
GRANT CREATE FUNCTION TO DataManager;
GRANT CREATE TABLE TO DataEditor;
GRANT CREATE VIEW TO DataEditor;
GRANT CREATE PROCEDURE TO DataEditor;
GRANT CREATE FUNCTION TO DataEditor;

-- Asignar roles
ALTER ROLE db_owner ADD MEMBER Christian; -- Permisos completos sobre la base de datos
ALTER ROLE DataManager ADD MEMBER Kendall; -- Puede leer, actualizar, crear, y eliminar datos
ALTER ROLE DataEditor ADD MEMBER Santiago; -- Puede leer, actualizar, y agregar datos
ALTER ROLE db_datareader ADD MEMBER Fabiana; -- Solo puede leer datos




--Script tablas de Préstamos, Historial, y Herramienta

 -- Crear la tabla de Herramientas (Tools)
CREATE TABLE Tools (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(255) NULL,
    Quantity INT NOT NULL CHECK (Quantity >= 0),
    Status BIT DEFAULT 1 NOT NULL -- 1: Disponible, 0: No disponible
);
GO

-- Crear la tabla de Préstamos (Loans)
CREATE TABLE Loans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId VARCHAR(20) NOT NULL,
    ToolId INT NOT NULL,
    LoanDate DATETIME DEFAULT GETDATE() NOT NULL,
    ReturnDate DATETIME NULL,
    Status VARCHAR(20) DEFAULT 'Pending' NOT NULL, -- Pending, Returned, Overdue
    CONSTRAINT FK_Loans_Tools FOREIGN KEY (ToolId) REFERENCES Tools(Id)
);
GO

-- Crear la tabla de Historial (History)
CREATE TABLE History (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId VARCHAR(20),
    ToolId INT NOT NULL,
    Action VARCHAR(50) NOT NULL, -- Borrowed, Returned, Overdue
    ActionDate DATETIME DEFAULT GETDATE() NOT NULL,
   -- CONSTRAINT FK_History_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_History_Tools FOREIGN KEY (ToolId) REFERENCES Tools(Id)
);
GO


-- Procedimiento para insertar una nueva herramienta
CREATE PROCEDURE InsertTool
    @Name VARCHAR(100),
    @Description VARCHAR(255),
    @Quantity INT
AS
BEGIN
    INSERT INTO Tools (Name, Description, Quantity)
    VALUES (@Name, @Description, @Quantity);
END;
GO

-- Procedimiento para insertar un nuevo préstamo
CREATE OR ALTER PROCEDURE InsertLoan
    @UserId VARCHAR(20),
    @ToolId INT
AS
BEGIN
    INSERT INTO Loans (UserId, ToolId, LoanDate, Status)
    VALUES (@UserId, @ToolId, GETDATE(), 'Pending');
    
    -- Agregar al historial
    INSERT INTO History (UserId, ToolId, Action, ActionDate)
    VALUES (@UserId, @ToolId, 'Borrowed', GETDATE());
END;
GO

-- Procedimiento para actualizar el estado de un préstamo (devolver herramienta)
CREATE OR ALTER PROCEDURE ReturnLoan
    @LoanId INT
AS
BEGIN
    UPDATE Loans
    SET ReturnDate = GETDATE(), Status = 'Returned'
    WHERE Id = @LoanId;
    
    DECLARE @UserId varchar(20), @ToolId INT;
    SELECT @UserId = UserId, @ToolId = ToolId FROM Loans WHERE Id = @LoanId;
    
    -- Agregar al historial
    INSERT INTO History (UserId, ToolId, Action, ActionDate)
    VALUES (@UserId, @ToolId, 'Returned', GETDATE());
END;
GO

-- Procedimiento para obtener todos los préstamos activos
CREATE OR ALTER PROCEDURE GetActiveLoans (@UserId VARCHAR(20))
AS
BEGINA
    SELECT l.Id, l.UserId, t.Name AS ToolName, l.LoanDate, l.Status
    FROM Loans l
    INNER JOIN Tools t ON l.ToolId = t.Id
    WHERE l.Status = 'Pending' and l.UserId=@UserId;
END;
GO

select * from Tools

select * from History

insert into History (UserId, ToolId, Action)values 
(1, 1, 'Insertar');

INSERT INTO Tools (Name, Description, Quantity)
VALUES 
  ('Taladro', 'Taladro eléctrico 500W', 10),
  ('Martillo', 'Martillo de carpintero', 15),
  ('Sierra', 'Sierra circular profesional', 5),
  ('Llave Inglesa', 'Llave inglesa ajustable', 12),
  ('Destornillador', 'Destornillador de precisión', 30),
  ('Pala', 'Pala de jardín', 8),
  ('Rastrillo', 'Rastrillo de metal', 10),
  ('Cortacésped', 'Cortacésped eléctrico', 3),
  ('Cepillo', 'Cepillo de alambre', 20),
  ('Taladro Percutor', 'Taladro percutor 800W', 7),
  ('Compresor', 'Compresor de aire portátil', 4),
  ('Amoladora', 'Amoladora angular 230V', 6),
  ('Multímetro', 'Multímetro digital', 25),
  ('Escalera', 'Escalera de aluminio 3 metros', 9),
  ('Sierra de Calar', 'Sierra de calar eléctrica', 8),
  ('Pistola de Calor', 'Pistola de calor profesional', 11),
  ('Llana', 'Llana para mortero', 14),
  ('Cincel', 'Cincel para madera', 22),
  ('Taladro Inalámbrico', 'Taladro inalámbrico recargable', 5),
  ('Broca', 'Set de brocas para metal', 50);

  INSERT INTO Loans (UserId, ToolId, LoanDate, ReturnDate, Status)
VALUES
  ('Santiago', 5, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'),  -- Préstamo 1: devuelto
  ('Fabiana', 6, GETDATE(), NULL, 'Pending'),                         -- Préstamo 2: pendiente
  ('Santiago', 7, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'),  -- Préstamo 3: devuelto
  ('Fabiana', 8, GETDATE(), NULL, 'Pending'),                         -- Préstamo 4: pendiente
  ('Christian', 5, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'),  -- Préstamo 5: devuelto
  ('Christian', 6, GETDATE(), NULL, 'Pending'),                         -- Préstamo 6: pendiente
  ('Christian', 7, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'),  -- Préstamo 7: devuelto
  ('Santiago', 8, GETDATE(), NULL, 'Pending'),                         -- Préstamo 8: pendiente
  ('Fabiana', 9, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'),  -- Préstamo 9: devuelto
  ('Christian', 10, GETDATE(), NULL, 'Pending'),                       -- Préstamo 10: pendiente
  ('Santiago', 11, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'), -- Préstamo 11: devuelto
  ('Kendall', 12, GETDATE(), NULL, 'Pending'),                       -- Préstamo 12: pendiente
  ('Kendall', 13, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'), -- Préstamo 13: devuelto
  ('Santiago', 14, GETDATE(), NULL, 'Pending'),                       -- Préstamo 14: pendiente
  ('Kendall', 15, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'), -- Préstamo 15: devuelto
  ('Fabiana', 16, GETDATE(), NULL, 'Pending'),                       -- Préstamo 16: pendiente
  ('Kendall', 17, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'), -- Préstamo 17: devuelto
  ('Santiago', 18, GETDATE(), NULL, 'Pending'),                       -- Préstamo 18: pendiente
  ('Kendall', 19, GETDATE(), DATEADD(day, 7, GETDATE()), 'Returned'), -- Préstamo 19: devuelto
  ('Santiago', 20, GETDATE(), NULL, 'Pending');                       -- Préstamo 20: pendiente

  SELECT * FROM sys.database_role_members
SELECT * FROM sys.database_principals