CREATE TABLE [dbo].[SlowBooking] (
    [LogId]                  INT              IDENTITY (1, 1) NOT NULL,
    [SupplierName]           NVARCHAR (32)    NULL,
    [TripProductId]          UNIQUEIDENTIFIER NULL,
    [TripFolderId]           UNIQUEIDENTIFIER NULL,
    [SupplierConfirmationNo] NVARCHAR (16)    NULL,
    [VendorConfirmationNo]   NVARCHAR (16)    NULL,
    [SessionId]              UNIQUEIDENTIFIER NULL,
    [SupplierId]             INT              NULL,
    [TimeTaken]              FLOAT            NULL,
    [AddDate]                DATETIME         NOT NULL,
    [Status]                 NVARCHAR (8)     NULL,
    [Message]                NVARCHAR (MAX)   NULL,
    [AdditionalInfo]         NVARCHAR (MAX)   NULL
);

