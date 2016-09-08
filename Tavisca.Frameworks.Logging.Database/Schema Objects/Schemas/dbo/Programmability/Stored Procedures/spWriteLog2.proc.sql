CREATE PROCEDURE [dbo].[spWriteLog2]  
( 
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
 @AdditionalInfo AdditionalDetailsType READONLY,   
 @TimeTaken float, 
 @Dk   nvarchar(32)='',
 @accountid NVARCHAR (32)='',
 @usersessionid  NVARCHAR (256),
 @useridentifier NVARCHAR (256),
 @application  NVARCHAR (256), 
 @contextidentifier NVARCHAR (128),
 @ipaddress nvarchar(256),
 @tracingToken UNIQUEIDENTIFIER = NULL,
 @LogID int OUTPUT
)  
AS   
  
 INSERT INTO [Log] (
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
  SessionId,  
  CallType,  
  ProviderId,  
  Status,
  usersessionid,
  useridentifier,
  ApplicationName,
  contextidentifier,
  ipaddress,
  TracingToken
 )  
 VALUES ( 
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
  @SessionId,  
  @CallType ,  
  @SupplierId,  
  @Status,     
  @usersessionid,
  @useridentifier,
  @application,
  @contextidentifier,
  @ipaddress,
  @tracingToken )  
  
 SET @LogID = @@IDENTITY  
 
 INSERT INTO LogData (LogId,[Key],[Value])
 SELECT @LogID,[Key],[Value] FROM @AdditionalInfo
 
 INSERT INTO LogRequestResponse(LogId,Request,Response)
 VALUES(@LogID,@XmlRequest,@XmlResponse)
 
 RETURN @LogID
GO

