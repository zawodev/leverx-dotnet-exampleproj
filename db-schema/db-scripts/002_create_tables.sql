USE ComputerStoreProd;
GO

-- core.Product
CREATE TABLE core.Product (
    Id          INT IDENTITY PRIMARY KEY,
    Name        VARCHAR(200) NOT NULL,
    Description TEXT NULL,
    Price       DECIMAL(18,2) NOT NULL,
    Stock       INT NOT NULL,
    CategoryId  INT NOT NULL
);

-- core.Category
CREATE TABLE core.Category (
    Id          INT IDENTITY PRIMARY KEY,
    Name        VARCHAR(100) NOT NULL,
    Description VARCHAR(250) NULL,
    IsActive    BIT NOT NULL DEFAULT(1)
);

-- core.Customer
CREATE TABLE core.Customer (
    Id        INT IDENTITY PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName  VARCHAR(100) NOT NULL,
    Email     VARCHAR(200) NOT NULL UNIQUE,
    Phone     VARCHAR(20)  NULL
);

-- sales.Order
CREATE TABLE sales.[Order] (
    Id           INT IDENTITY PRIMARY KEY,
    CustomerId   INT NOT NULL,
    OrderDate    DATETIME NOT NULL DEFAULT(GETDATE()),
    TotalAmount  DECIMAL(18,2) NOT NULL,
    Status       VARCHAR(50) NOT NULL,
    CONSTRAINT   FK_Order_Customer FOREIGN KEY(CustomerId) REFERENCES core.Customer(Id)
);

-- sales.OrderItem
CREATE TABLE sales.OrderItem (
    OrderId    INT NOT NULL,
    ProductId  INT NOT NULL,
    Quantity   INT NOT NULL,
    UnitPrice  DECIMAL(18,2) NOT NULL,
    PRIMARY    KEY(OrderId, ProductId),
    CONSTRAINT FK_OrderItem_Order FOREIGN KEY(OrderId) REFERENCES sales.[Order](Id),
    CONSTRAINT FK_OrderItem_Product FOREIGN KEY(ProductId) REFERENCES core.Product(Id)
);
