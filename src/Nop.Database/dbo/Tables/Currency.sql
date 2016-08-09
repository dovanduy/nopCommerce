CREATE TABLE [dbo].[Currency] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)   NOT NULL,
    [CurrencyCode]     NVARCHAR (5)    NOT NULL,
    [Rate]             DECIMAL (18, 4) NOT NULL,
    [DisplayLocale]    NVARCHAR (50)   NULL,
    [CustomFormatting] NVARCHAR (50)   NULL,
    [LimitedToStores]  BIT             NOT NULL,
    [Published]        BIT             NOT NULL,
    [DisplayOrder]     INT             NOT NULL,
    [CreatedOnUtc]     DATETIME        NOT NULL,
    [UpdatedOnUtc]     DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Currency_DisplayOrder]
    ON [dbo].[Currency]([DisplayOrder] ASC);

