CREATE PROCEDURE [dbo].[spWriteLog]  
(  
 @EventID int=0,   
 @Priority int,   
 @Severity nvarchar(32),   
 @Title nvarchar(256),   
 @Timestamp datetime,  
 @MachineName nvarchar(32),   
 @AppDomainName nvarchar(512),  
 @ProcessID nvarchar(256),  
 @ProcessName nvarchar(512),  
 @ThreadName nvarchar(512),  
 @Win32ThreadId nvarchar(128),  
 @Message nvarchar(2048),  
 @XmlRequest nvarchar(MAX),  
 @XmlResponse nvarchar(MAX),  
 @SessionId uniqueidentifier ,  
 @CallType nvarchar (32),  
 @SupplierId int ,  
 @Status nvarchar (32),   
 @AdditionalInfo nvarchar(MAX),   
 @TimeTaken float, 
 @Dk   nvarchar(32)='',
 @accountid NVARCHAR (32)='',
 @usersessionid  NVARCHAR (256),
 @useridentifier NVARCHAR (256),
 @application  NVARCHAR (256), 
 @contextidentifier NVARCHAR (128),
 @ipaddress nvarchar(256),
 @LogID int OUTPUT
)  
AS   
  
 INSERT INTO [Log] (  
  --EventID,  
  Priority,  
  Severity,  
  Title,  
  [Timestamp], 
  [TimeTaken],
  MachineName,  
  AppDomainName,  
  ProcessID,  
  ProcessName,  
  ThreadName,  
  Win32ThreadId,  
  Message,  
  --XmlRequest,  
  --XmlResponse,  
  SessionId,  
  CallType,  
  ProviderId,  
  Status,   
  --AdditionalInfo ,
  --Dk,
  --accountid,
  usersessionid,
  useridentifier,
  ApplicationName,
  contextidentifier,
  ipaddress

 )  
 VALUES (  
  --@EventID,   
  @Priority,   
  @Severity,   
  @Title,   
  @Timestamp,  
  @TimeTaken,
  @MachineName,   
  @AppDomainName,  
  @ProcessID,  
  @ProcessName,  
  @ThreadName,  
  @Win32ThreadId,  
  @Message,  
  --@XmlRequest,  
  --@XmlResponse,  
  @SessionId,  
  @CallType ,  
  @SupplierId,  
  @Status,     
  --@AdditionalInfo,
  --@Dk,
  --@accountid,
  @usersessionid,
  @useridentifier,
  @application,
  @contextidentifier,
  @ipaddress )  
  
 SET @LogID = @@IDENTITY  
 
 INSERT INTO LogData (LogId,[Key],[Value])
 VALUES (@LogID,'AdditionalInfo',@AdditionalInfo)
 
 INSERT INTO LogRequestResponse(LogId,Request,Response)
 VALUES(@LogID,@XmlRequest,@XmlResponse)
 
 RETURN @LogID