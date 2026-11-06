/*
   Additive schema upgrade for the existing OnlineStore database.
   Run this against the existing database; it does not create or replace a database.
*/
IF COL_LENGTH(N'dbo.Products', N'StockQuantity') IS NULL
BEGIN
    ALTER TABLE dbo.Products
        ADD StockQuantity int NOT NULL
            CONSTRAINT DF_Products_StockQuantity DEFAULT (100) WITH VALUES;
END;
GO

IF COL_LENGTH(N'dbo.Products', N'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.Products
        ADD CreatedAt datetime2 NOT NULL
            CONSTRAINT DF_Products_CreatedAt DEFAULT (SYSUTCDATETIME()) WITH VALUES;
END;
GO

IF COL_LENGTH(N'dbo.Categories', N'CreatedAt') IS NULL
BEGIN
    ALTER TABLE dbo.Categories
        ADD CreatedAt datetime2 NOT NULL
            CONSTRAINT DF_Categories_CreatedAt DEFAULT (SYSUTCDATETIME()) WITH VALUES;
END;
GO

IF OBJECT_ID(N'dbo.ProductReservations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProductReservations
    (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_ProductReservations PRIMARY KEY,
        ProductId int NOT NULL,
        UserId int NOT NULL,
        Quantity int NOT NULL,
        ReservedAt datetime2 NOT NULL,
        IsActive bit NOT NULL,
        CONSTRAINT FK_ProductReservations_Products_ProductId
            FOREIGN KEY (ProductId) REFERENCES dbo.Products(ID_product),
        CONSTRAINT FK_ProductReservations_Users_UserId
            FOREIGN KEY (UserId) REFERENCES dbo.Users(ID_user),
        CONSTRAINT CK_ProductReservations_Quantity CHECK (Quantity > 0)
    );
END;
GO

IF EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_ProductReservations_Products_ProductId'
      AND delete_referential_action_desc <> N'CASCADE'
)
BEGIN
    ALTER TABLE dbo.ProductReservations DROP CONSTRAINT FK_ProductReservations_Products_ProductId;
    ALTER TABLE dbo.ProductReservations
        ADD CONSTRAINT FK_ProductReservations_Products_ProductId
        FOREIGN KEY (ProductId) REFERENCES dbo.Products(ID_product) ON DELETE CASCADE;
END;
GO
