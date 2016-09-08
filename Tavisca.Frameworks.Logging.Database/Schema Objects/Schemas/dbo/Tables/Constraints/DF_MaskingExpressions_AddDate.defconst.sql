ALTER TABLE [dbo].[MaskingExpressions]
    ADD CONSTRAINT [DF_MaskingExpressions_AddDate] DEFAULT (getdate()) FOR [AddDate];

