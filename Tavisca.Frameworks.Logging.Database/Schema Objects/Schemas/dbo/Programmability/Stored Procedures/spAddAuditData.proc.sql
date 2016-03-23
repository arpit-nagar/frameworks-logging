CREATE PROCEDURE [dbo].[spAddAuditData]
    (
	    @LogId BIGINT,

        @Key NVARCHAR(MAX),
        
        @Value NVARCHAR(MAX)
        
    )
AS 
    BEGIN
    INSERT INTO [AuditData]
           ([LogId]
           ,[Key]
           ,[Value])
     VALUES
           (@LogId
           ,@Key
           ,@Value)
    END