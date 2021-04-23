USE master;

GO

CREATE DATABASE FluentSqlBuilder;

GO

USE FluentSqlBuilder;

GO

CREATE SCHEMA Checkout;

GO

CREATE SCHEMA Customers;

GO

CREATE SCHEMA Products;

GO

CREATE TABLE [Products].[Product] (
	[Id] INT PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(250) NOT NULL
);

GO

CREATE TABLE [Customers].[Customer] (
	[Id] INT PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(250) NOT NULL,
	[Type] SMALLINT NOT NULL
);

GO

CREATE TABLE [Checkout].[Order] (
	[Id] INT PRIMARY KEY IDENTITY(1, 1),
	[Customer_id] INT NOT NULL,
	[Status] SMALLINT NOT NULL,
	FOREIGN KEY (Customer_id) REFERENCES [Customers].[Customer](Id)
);

GO

CREATE TABLE [Checkout].[Order_Item] (
	[Id] INT PRIMARY KEY IDENTITY(1, 1),
	[Quantity] INT NOT NULL,
	[Order_id] INT NOT NULL,
	[Product_id] INT NOT NULL,
	FOREIGN KEY (Product_id) REFERENCES [Products].[Product](Id),
	FOREIGN KEY (Order_id) REFERENCES [checkout].[Order](Id)
);