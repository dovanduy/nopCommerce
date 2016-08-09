CREATE TABLE [dbo].[Warehouse] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (400) NOT NULL,
    [AdminComment] NVARCHAR (MAX) NULL,
    [AddressId]    INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

