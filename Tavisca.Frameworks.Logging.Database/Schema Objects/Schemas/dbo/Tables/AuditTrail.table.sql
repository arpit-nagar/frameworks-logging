CREATE TABLE [dbo].[AuditTrail] (
    [Id]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [TimeStamp]       DATETIME        NOT NULL,
    [ChainId]         NVARCHAR (1024) NULL,
    [ReferenceNumber] NVARCHAR (1024) NULL,
    [EventType]       NVARCHAR (124)  NULL,
    [IPAddress]       NVARCHAR (50)   NULL,
    [Username]        NVARCHAR (1024) NULL,
    [Application]     NVARCHAR (1024) NULL,
    [Module]          NVARCHAR (1024) NULL,
    [Status]          NVARCHAR (50)   NULL,
    [Tags]            NVARCHAR (2048) NULL
);

