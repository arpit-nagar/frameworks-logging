CREATE TABLE [dbo].[LogRequestResponse] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [LogId]    INT            NOT NULL,
    [Request]  NVARCHAR (MAX) NULL,
    [Response] NVARCHAR (MAX) NULL
);

