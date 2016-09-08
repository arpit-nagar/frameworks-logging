CREATE PROCEDURE [dbo].[spWriteExceptionLog]
(
	@EventID [int]=0 ,
	@Priority [int],
	@Severity [nvarchar](32) ,
	@Title [nvarchar](256) ,
	@Timestamp [datetime] ,
	@MachineName [nvarchar](32) ,
	@AppDomainName [nvarchar](256) ,
	@ProcessID [nvarchar](256) ,
	@ProcessName [nvarchar](256),
	@ThreadName [nvarchar](256) ,
	@Win32ThreadId [nvarchar](128) ,
	@Type [nvarchar] (150) ,
	@Message [varchar](4096),
	@Source [nvarchar] (64) ,
	@TargetSite [nvarchar] (128) ,
	@StackTrace [varchar] (max) ,
	@ThreadIdentity [nvarchar] (64),
	@AdditionalInfo [ntext] ,
	@InnerExceptions [ntext] ,		
	@SessionId nvarchar(512) ,  
	@Dk [nvarchar](32)='',
	 @accountid NVARCHAR (32)='',
 @usersessionid  NVARCHAR (256),
 @useridentifier NVARCHAR (256),
 @application  NVARCHAR (256), 
 @contextidentifier NVARCHAR (128),
 @ipaddress nvarchar(256),
 @LogID int OUTPUT
)
AS 

	INSERT INTO [ExceptionLog] (		
	
	[Priority],
	[Severity] ,
	[Title] ,
	[Timestamp] ,
	[MachineName],
	[AppDomainName],
	[ProcessID] ,
	[ProcessName],
	[ThreadName] ,
	[Win32ThreadId] ,
	[Type] ,
	[Message],
	[Source],
	[TargetSite] ,
	[StackTrace] ,
	[ThreadIdentity],
	--[AdditionalInfo] ,
	[InnerExceptions] ,
	[SessionId],
	--[Dk],	
	--[accountid],
  [usersessionid],
  [useridentifier],
  [ApplicationName],
  [contextidentifier],
  [ipaddress]
	)
	VALUES (		
		@Priority, 
		@Severity, 
		@Title, 
		@Timestamp,
		@MachineName, 
		@AppDomainName,
		@ProcessID,
		@ProcessName,
		@ThreadName,
		@Win32ThreadId,
		@Type ,
	@Message,
	@Source,
	@TargetSite,
	@StackTrace ,
	@ThreadIdentity,
	--@AdditionalInfo ,
	@InnerExceptions,
	@SessionId,
	--@Dk,
	--@accountid,
  @usersessionid,
  @useridentifier,
  @application,
  @contextidentifier,
  @ipaddress)
		
	SET @LogID = @@IDENTITY
	
	--	IF @SessionId IS NOT NULL AND @SessionId <> ''
	--INSERT dbo.ExceptionSessionLog(LogId,SessionId) 
	
	INSERT INTO ExceptionData(LogId,[Key],[Value])
	VALUES(@LogID,'AdditionalInfo',@AdditionalInfo )
	
	select @LogId,Convert(UNIQUEIDENTIFIER,token) from [dbo].[fnSplitExcluder](@SessionId,',')
	
	RETURN @LogID