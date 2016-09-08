CREATE PROCEDURE [dbo].[spAddAuditLog]
    (
	    @ChainId NVARCHAR(1024),

        @ReferenceNumber NVARCHAR(1024),

        @IPAddress NVARCHAR(1024),

        @Username NVARCHAR(1024),

        @TimeStamp DateTime,

        @Applicaion NVARCHAR(1024),
        
        @EventType NVARCHAR(50),

        @Module NVARCHAR(1024),

        @Status NVARCHAR(1024),

        @Tags NVARCHAR(2048)
        
    )
AS 
    BEGIN
    
		INSERT INTO [AuditTrail]
           ([TimeStamp]
           ,[ChainId]
           ,[ReferenceNumber]
           ,[IPAddress]
           ,[Username]
           ,[Application]
           ,[Module]
           ,[Status]
           ,[EventType]
           ,[Tags])
     VALUES
           (@TimeStamp
           ,@ChainId
           ,@ReferenceNumber
           ,@IPAddress
           ,@Username
           ,@Applicaion
           ,@Module
           ,@Status
           ,@EventType
           ,@Tags)
    END
Select SCOPE_IDENTITY() as Id