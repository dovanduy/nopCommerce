CREATE FULLTEXT INDEX ON [dbo].[Product]
    ([Name] LANGUAGE 1033, [ShortDescription] LANGUAGE 1033, [FullDescription] LANGUAGE 1033)
    KEY INDEX [PK__Product__3214EC076842AB5D]
    ON [nopCommerceFullTextCatalog];


GO
CREATE FULLTEXT INDEX ON [dbo].[ProductTag]
    ([Name] LANGUAGE 1033)
    KEY INDEX [PK__ProductT__3214EC07D184BA52]
    ON [nopCommerceFullTextCatalog];


GO
CREATE FULLTEXT INDEX ON [dbo].[LocalizedProperty]
    ([LocaleValue] LANGUAGE 1033)
    KEY INDEX [PK__Localize__3214EC077B9F28D6]
    ON [nopCommerceFullTextCatalog];

