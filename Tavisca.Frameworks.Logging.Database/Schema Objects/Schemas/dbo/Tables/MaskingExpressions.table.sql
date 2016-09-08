CREATE TABLE [dbo].[MaskingExpressions] (
    [ProviderName]          NVARCHAR(16)            NOT NULL,
    [CallType]              NVARCHAR (512) NULL,
    [Regex]                 NVARCHAR (512) NULL,
    [ReplacementExpression] NVARCHAR (512) NULL,
    [AddDate]               DATETIME       NOT NULL
);

