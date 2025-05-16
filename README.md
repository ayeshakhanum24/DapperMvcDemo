the code for setting the databases :

-- Create the DapperMvcDemo database
CREATE DATABASE DapperMvcDemo;
GO

-- Use the DapperMvcDemo database
USE DapperMvcDemo;
GO

-- Create the Department table
CREATE TABLE Department (
    DeptId INT IDENTITY(1,1) PRIMARY KEY,
    DeptName NVARCHAR(100) NOT NULL
);
GO

-- Create the Person table
CREATE TABLE Person (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200) NOT NULL,
    DeptId INT,
    FOREIGN KEY (DeptId) REFERENCES Department(DeptId)
);
GO

-- Create stored procedure sp_create_person
CREATE PROCEDURE [dbo].[sp_create_person]
    @name NVARCHAR(100),
    @email NVARCHAR(100),
    @address NVARCHAR(200),
    @deptId INT
AS
BEGIN
    INSERT INTO dbo.Person (Name, Email, Address, DeptId)
    VALUES (@name, @email, @address, @deptId)
END
GO

-- Create stored procedure sp_delete_person
CREATE PROCEDURE [dbo].[sp_delete_person]
    @id INT
AS
BEGIN
    DELETE FROM dbo.Person
    WHERE Id = @id
END
GO

-- Create stored procedure sp_get_departments
CREATE PROCEDURE [dbo].[sp_get_departments]
AS
BEGIN
    SELECT * FROM dbo.Department
END
GO

-- Create stored procedure sp_get_people
CREATE PROCEDURE [dbo].[sp_get_people]
AS
BEGIN
    SELECT 
        p.Id, 
        p.Name, 
        p.Email, 
        p.Address, 
        p.DeptId,
        d.DeptName AS DepartmentName
    FROM dbo.Person p
    LEFT JOIN dbo.Department d ON p.DeptId = d.DeptId
END
GO

-- Create stored procedure sp_get_person
CREATE PROCEDURE [dbo].[sp_get_person]
    @id INT
AS
BEGIN
    SELECT p.*, d.DeptName
    FROM dbo.Person p
    LEFT JOIN dbo.Department d ON p.DeptId = d.DeptId
    WHERE p.Id = @id
END
GO

-- Create stored procedure sp_insert_person
CREATE PROCEDURE [dbo].[sp_insert_person]
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Address NVARCHAR(200),
    @DeptId INT
AS
BEGIN
    INSERT INTO Person (Name, Email, Address, DeptId)
    VALUES (@Name, @Email, @Address, @DeptId);
END
GO

-- Create stored procedure sp_update_person
CREATE PROCEDURE [dbo].[sp_update_person]
    @id INT,
    @name NVARCHAR(100),
    @email NVARCHAR(100),
    @address NVARCHAR(200),
    @deptId INT
AS
BEGIN
    UPDATE dbo.Person
    SET Name = @name,
        Email = @email,
        Address = @address,
        DeptId = @deptId
    WHERE Id = @id
END
GO
