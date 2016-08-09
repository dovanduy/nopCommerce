CREATE TABLE [dbo].[Topic] (
    [Id]                        INT            IDENTITY (1, 1) NOT NULL,
    [SystemName]                NVARCHAR (MAX) NULL,
    [IncludeInSitemap]          BIT            NOT NULL,
    [IncludeInTopMenu]          BIT            NOT NULL,
    [IncludeInFooterColumn1]    BIT            NOT NULL,
    [IncludeInFooterColumn2]    BIT            NOT NULL,
    [IncludeInFooterColumn3]    BIT            NOT NULL,
    [DisplayOrder]              INT            NOT NULL,
    [AccessibleWhenStoreClosed] BIT            NOT NULL,
    [IsPasswordProtected]       BIT            NOT NULL,
    [Password]                  NVARCHAR (MAX) NULL,
    [Title]                     NVARCHAR (MAX) NULL,
    [Body]                      NVARCHAR (MAX) NULL,
    [Published]                 BIT            NOT NULL,
    [TopicTemplateId]           INT            NOT NULL,
    [MetaKeywords]              NVARCHAR (MAX) NULL,
    [MetaDescription]           NVARCHAR (MAX) NULL,
    [MetaTitle]                 NVARCHAR (MAX) NULL,
    [SubjectToAcl]              BIT            NOT NULL,
    [LimitedToStores]           BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

