USE ComputerStoreProd;
GO

-- 1) Categories
INSERT INTO core.Category (Name, Description, IsActive)
SELECT Name, Description, 1
FROM ComputerStoreDbOld.dbo.Category;
GO

-- 2) Customers
INSERT INTO core.Customer (FirstName, LastName, Email, Phone)
SELECT FirstName, LastName, Email, Phone
FROM ComputerStoreDbOld.dbo.Customer;
GO

-- 3) Products
INSERT INTO core.Product (Name, Description, Price, Stock, CategoryId)
SELECT Name, Description, Price, Stock, CategoryId
FROM ComputerStoreDbOld.dbo.Product;
GO

-- 4) Orders
INSERT INTO sales.[Order] (CustomerId, OrderDate, TotalAmount, Status)
SELECT CustomerId, OrderDate, TotalAmount, Status
FROM ComputerStoreDbOld.dbo.[Order];
GO

-- 5) OrderItems (we dont run this part, because we didnt have OrderItem in old db)
INSERT INTO sales.OrderItem (OrderId, ProductId, Quantity, UnitPrice)
SELECT OrderId, ProductId, Quantity, UnitPrice
FROM ComputerStoreDbOld.dbo.OrderItem; 
GO


-- nu get commands:
-- Add-Migration InitialCreate
-- Update-Database