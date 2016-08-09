CREATE TABLE [dbo].[ProductTag] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (400) NOT NULL,
    CONSTRAINT [PK__ProductT__3214EC07D184BA52] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ProductTag_Name]
    ON [dbo].[ProductTag]([Name] ASC);

