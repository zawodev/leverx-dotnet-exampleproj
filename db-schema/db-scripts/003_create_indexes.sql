-- indexes are created to speed up frequent queries
USE ComputerStoreProd;
GO
CREATE INDEX IX_Product_CategoryId ON core.Product(CategoryId);
CREATE INDEX IX_Order_CustomerId ON sales.[Order](CustomerId);
