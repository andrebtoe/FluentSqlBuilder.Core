USE FluentSqlBuilder;

GO

INSERT INTO [Products].[Product] ([Name], [DateTime]) VALUES ('JBL FLIP 4', GETDATE());
INSERT INTO [Products].[Product] ([Name], [DateTime]) VALUES ('SanDisk 128GB', GETDATE());
INSERT INTO [Products].[Product] ([Name], [DateTime]) VALUES ('HP 65 | 2 Ink Cartridges', GETDATE());
INSERT INTO [Products].[Product] ([Name], [DateTime]) VALUES ('USB Wall Charger', GETDATE());
INSERT INTO [Products].[Product] ([Name], [DateTime]) VALUES ('Webcam Logitech', GETDATE());
INSERT INTO [Products].[Product] ([Name], [DateTime]) VALUES ('Duracell - CopperTop AA', GETDATE());
INSERT INTO [Products].[Product] ([Name], [DateTime]) VALUES ('Logitech G533 Wireless Gaming Headset', GETDATE());
INSERT INTO [Products].[Product] ([Name], [DateTime]) VALUES ('Arteck 2.4G Wireless Keyboard', GETDATE());

INSERT INTO [Customers].[Customer] ([Name], [Type], [DateTime]) VALUES('Oliver', 1, GETDATE());
INSERT INTO [Customers].[Customer] ([Name], [Type], [DateTime]) VALUES('Harry', 1, GETDATE());
INSERT INTO [Customers].[Customer] ([Name], [Type], [DateTime]) VALUES('George', 1, GETDATE());
INSERT INTO [Customers].[Customer] ([Name], [Type], [DateTime]) VALUES('William', 1, GETDATE());
INSERT INTO [Customers].[Customer] ([Name], [Type], [DateTime]) VALUES('Jacob', 1, GETDATE());
INSERT INTO [Customers].[Customer] ([Name], [Type], [DateTime]) VALUES('Michael', 1, GETDATE());
INSERT INTO [Customers].[Customer] ([Name], [Type], [DateTime]) VALUES('Tech FE', 2, GETDATE());
INSERT INTO [Customers].[Customer] ([Name], [Type], [DateTime]) VALUES('Williams COM', 2, GETDATE());

INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (1, 1, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (1, 1, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (1, 1, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (1, 1, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (1, 2, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (1, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (1, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (1, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (1, 3, GETDATE());

INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (2, 1, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (2, 2, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (2, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (2, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (2, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (2, 3, GETDATE());

INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (3, 1, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (3, 2, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (3, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (3, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (3, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (3, 3, GETDATE());

INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (4, 1, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (4, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (4, 3, GETDATE());
INSERT INTO [Checkout].[Order] ([Customer_id], [Status], [DateTime]) VALUES (4, 3, GETDATE());


INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 1, 1, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (5, 1, 2, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 1, 2, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 3, 5, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 4, 5, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 5, 5, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 6, 5, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 6, 3, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 6, 2, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 7, 8, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 8, 2, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 8, 2, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 9, 5, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 10, 1, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 11, 3, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 12, 6, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 13, 8, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 14, 1, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (2, 15, 2, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (4, 16, 4, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (6, 17, 7, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (2, 18, 6, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (3, 19, 5, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 20, 5, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 21, 1, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 22, 3, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 23, 4, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 24, 3, GETDATE());
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id], [DateTime]) VALUES (1, 25, 3, GETDATE());