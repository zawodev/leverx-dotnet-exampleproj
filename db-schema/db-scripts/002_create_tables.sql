USE ComputerStoreProd;
GO

-- core.Category
CREATE TABLE core.Category (
    Id          INT IDENTITY PRIMARY KEY,
    Name        VARCHAR(100)    NOT NULL,
    Description VARCHAR(250)    NULL,
    IsActive    BIT             NOT NULL DEFAULT(1)
);

-- core.Customer
CREATE TABLE core.Customer (
    Id           INT IDENTITY PRIMARY KEY,
    FirstName    VARCHAR(100)    NULL,
    LastName     VARCHAR(100)    NULL,
    Email        VARCHAR(200)    NOT NULL UNIQUE,
    Phone        VARCHAR(20)     NULL,
);


-- core.Address
CREATE TABLE core.Address (
    Id          INT IDENTITY PRIMARY KEY,
    Line1       VARCHAR(250)    NOT NULL,
    Line2       VARCHAR(250)    NULL,
    City        VARCHAR(100)    NOT NULL,
    PostalCode  VARCHAR(20)     NOT NULL,
    Country     VARCHAR(100)    NOT NULL,
    IsBilling   BIT             NOT NULL,
    CustomerId  INT             NOT NULL,
    CONSTRAINT FK_Address_Customer FOREIGN KEY(CustomerId) REFERENCES core.Customer(Id)
);

-- core.Product
CREATE TABLE core.Product (
    Id          INT IDENTITY PRIMARY KEY,
    Name        VARCHAR(200)    NOT NULL,
    Description TEXT            NULL,
    Price       DECIMAL(18,2)   NOT NULL,
    Stock       INT             NOT NULL,
    CategoryId  INT             NOT NULL,
    CONSTRAINT FK_Product_Category FOREIGN KEY(CategoryId) REFERENCES core.Category(Id)
);

-- sales.Order
CREATE TABLE sales.[Order] (
    Id            INT IDENTITY PRIMARY KEY,
    CustomerId    INT             NOT NULL,
    OrderDate     DATETIME        NOT NULL DEFAULT(GETDATE()),
    TotalAmount   DECIMAL(18,2)   NOT NULL,
    Status        VARCHAR(50)     NOT NULL,
    PaymentMethod VARCHAR(50)     NOT NULL,
    CONSTRAINT FK_Order_Customer FOREIGN KEY(CustomerId) REFERENCES core.Customer(Id)
);

-- sales.OrderItem
CREATE TABLE sales.OrderItem (
    OrderId    INT             NOT NULL,
    ProductId  INT             NOT NULL,
    Quantity   INT             NOT NULL,
    UnitPrice  DECIMAL(18,2)   NOT NULL,
    PRIMARY KEY (OrderId, ProductId),
    CONSTRAINT FK_OrderItem_Order   FOREIGN KEY (OrderId)   REFERENCES sales.[Order](Id),
    CONSTRAINT FK_OrderItem_Product FOREIGN KEY (ProductId) REFERENCES core.Product(Id)
);

-- core.Cart
CREATE TABLE core.Cart (
    Id          INT IDENTITY PRIMARY KEY,
    CustomerId  INT             NOT NULL,
    CreatedAt   DATETIME        NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT FK_Cart_Customer FOREIGN KEY(CustomerId) REFERENCES core.Customer(Id)
);

-- core.CartItem
CREATE TABLE core.CartItem (
    CartId     INT             NOT NULL,
    ProductId  INT             NOT NULL,
    Quantity   INT             NOT NULL,
    PRIMARY KEY (CartId, ProductId),
    CONSTRAINT FK_CartItem_Cart    FOREIGN KEY (CartId)    REFERENCES core.Cart(Id),
    CONSTRAINT FK_CartItem_Product FOREIGN KEY (ProductId) REFERENCES core.Product(Id)
);

-- core.ProductReview
CREATE TABLE core.ProductReview (
    Id          INT IDENTITY PRIMARY KEY,
    ProductId   INT             NOT NULL,
    CustomerId  INT             NOT NULL,
    Rating      INT             NOT NULL,
    Comment     TEXT            NULL,
    CreatedAt   DATETIME        NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT FK_Review_Product  FOREIGN KEY (ProductId)  REFERENCES core.Product(Id),
    CONSTRAINT FK_Review_Customer FOREIGN KEY (CustomerId) REFERENCES core.Customer(Id)
);

-- sales.ReturnRequest
CREATE TABLE sales.ReturnRequest (
    Id           INT IDENTITY PRIMARY KEY,
    OrderId      INT             NOT NULL,
    RequestedAt  DATETIME        NOT NULL DEFAULT(GETDATE()),
    Reason       VARCHAR(250)    NULL,
    Status       VARCHAR(50)     NOT NULL,
    CONSTRAINT FK_ReturnRequest_Order FOREIGN KEY (OrderId) REFERENCES sales.[Order](Id)
);

-- sales.ReturnItem
CREATE TABLE sales.ReturnItem (
    ReturnId   INT             NOT NULL,
    ProductId  INT             NOT NULL,
    Quantity   INT             NOT NULL,
    PRIMARY KEY (ReturnId, ProductId),
    CONSTRAINT FK_ReturnItem_Request FOREIGN KEY (ReturnId)  REFERENCES sales.ReturnRequest(Id),
    CONSTRAINT FK_ReturnItem_Product FOREIGN KEY (ProductId) REFERENCES core.Product(Id)
);
