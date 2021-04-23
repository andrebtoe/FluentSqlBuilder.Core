USE FluentSqlBuilder;

GO

INSERT INTO [Products].[Product] ([Name]) VALUES ('JBL FLIP 4');
INSERT INTO [Products].[Product] ([Name]) VALUES ('SanDisk 128GB');
INSERT INTO [Products].[Product] ([Name]) VALUES ('HP 65 | 2 Ink Cartridges');
INSERT INTO [Products].[Product] ([Name]) VALUES ('USB Wall Charger');
INSERT INTO [Products].[Product] ([Name]) VALUES ('Webcam Logitech');
INSERT INTO [Products].[Product] ([Name]) VALUES ('Duracell - CopperTop AA');
INSERT INTO [Products].[Product] ([Name]) VALUES ('Logitech G533 Wireless Gaming Headset');
INSERT INTO [Products].[Product] ([Name]) VALUES ('Arteck 2.4G Wireless Keyboard');

INSERT INTO [Customers].[Customer] ([Name], [Type]) VALUES('Oliver', 1);
INSERT INTO [Customers].[Customer] ([Name], [Type]) VALUES('Harry', 1);
INSERT INTO [Customers].[Customer] ([Name], [Type]) VALUES('George', 1);
INSERT INTO [Customers].[Customer] ([Name], [Type]) VALUES('William', 1);
INSERT INTO [Customers].[Customer] ([Name], [Type]) VALUES('Jacob', 1);
INSERT INTO [Customers].[Customer] ([Name], [Type]) VALUES('Michael', 1);
INSERT INTO [Customers].[Customer] ([Name], [Type]) VALUES('Tech FE', 2);
INSERT INTO [Customers].[Customer] ([Name], [Type]) VALUES('Williams COM', 2);

INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (1, 1);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (1, 1);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (1, 1);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (1, 1);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (1, 2);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (1, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (1, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (1, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (1, 3);

INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (2, 1);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (2, 2);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (2, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (2, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (2, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (2, 3);

INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (3, 1);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (3, 2);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (3, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (3, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (3, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (3, 3);

INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (4, 1);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (4, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (4, 3);
INSERT INTO [Checkout].[Order] ([Customer_id], [Status]) VALUES (4, 3);


INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 1, 1);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (5, 1, 2);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 1, 2);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 3, 5);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 4, 5);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 5, 5);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 6, 5);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 6, 3);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 6, 2);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 7, 8);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 8, 2);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 8, 2);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 9, 5);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 10, 1);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 11, 3);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 12, 6);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 13, 8);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 14, 1);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (2, 15, 2);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (4, 16, 4);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (6, 17, 7);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (2, 18, 6);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (3, 19, 5);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 20, 5);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 21, 1);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 22, 3);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 23, 4);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 24, 3);
INSERT INTO [Checkout].[Order_Item] ([Quantity], [Order_id], [Product_id]) VALUES (1, 25, 3);