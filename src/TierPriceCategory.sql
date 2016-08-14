USE [nopCommerceFork]
GO

/****** Object:  Table [dbo].[OrderNote]    Script Date: 13-8-2016 23:08:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TierPriceCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TierPriceCategoryDesc] [nvarchar](400) NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) 

GO


alter table tierprice add TierPriceCategoryId int null


ALTER TABLE [dbo].[TierPrice]  WITH CHECK ADD  CONSTRAINT [TierPrice_TierPriceCategory] FOREIGN KEY([TierPriceCategoryId])
REFERENCES [dbo].[TierPriceCategory] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TierPrice] CHECK CONSTRAINT [TierPrice_TierPriceCategory]
GO


INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
VALUES(1, 'Admin.Catalog.Products.TierPrices.Fields.TierPriceCategory', 'Tier price category')

INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
VALUES(1, 'Admin.Catalog.Products.TierPrices.Fields.TierPriceCategory', 'Staffelprijs categorie')


INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
VALUES(1, 'Admin.Configuration.Settings.AllSettings.Fields.StoreName.AllTierPriceCategories', 'All tier price categories')

INSERT INTO [LocaleStringResource] ([LanguageId], [ResourceName], [ResourceValue])
VALUES(1, 'Admin.Configuration.Settings.AllSettings.Fields.StoreName.AllTierPriceCategories', 'Alle staffelprijs categorieen')

