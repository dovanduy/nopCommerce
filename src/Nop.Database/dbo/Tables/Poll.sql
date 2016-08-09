CREATE TABLE [dbo].[Poll] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [LanguageId]        INT            NOT NULL,
    [Name]              NVARCHAR (MAX) NOT NULL,
    [SystemKeyword]     NVARCHAR (MAX) NULL,
    [Published]         BIT            NOT NULL,
    [ShowOnHomePage]    BIT            NOT NULL,
    [AllowGuestsToVote] BIT            NOT NULL,
    [DisplayOrder]      INT            NOT NULL,
    [StartDateUtc]      DATETIME       NULL,
    [EndDateUtc]        DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Poll_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([Id]) ON DELETE CASCADE
);

