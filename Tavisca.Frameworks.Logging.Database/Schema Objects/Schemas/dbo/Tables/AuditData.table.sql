CREATE TABLE [dbo].[AuditData] (
    [Id]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [LogId] BIGINT         NOT NULL,
    [Key]   NVARCHAR (MAX) NOT NULL,
    [Value] NVARCHAR (MAX) NULL
);

