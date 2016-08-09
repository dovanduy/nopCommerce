CREATE TABLE [dbo].[MessageTemplate] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (200)  NOT NULL,
    [BccEmailAddresses]  NVARCHAR (200)  NULL,
    [Subject]            NVARCHAR (1000) NULL,
    [Body]               NVARCHAR (MAX)  NULL,
    [IsActive]           BIT             NOT NULL,
    [DelayBeforeSend]    INT             NULL,
    [DelayPeriodId]      INT             NOT NULL,
    [AttachedDownloadId] INT             NOT NULL,
    [EmailAccountId]     INT             NOT NULL,
    [LimitedToStores]    BIT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

