  
  
  
CREATE PROCEDURE [dbo].[spSelectSingleLog] (  
 @LogID int  
)  
  
AS  
  
SET NOCOUNT ON  
  
SELECT  
 L.[LogId],  
 0 AS [EventID],  
 [Priority],  
 [Severity],  
 [Title],  
 [Timestamp],  
 [MachineName],  
 [AppDomainName],  
 [ProcessID],  
 [ProcessName],  
 [ThreadName],  
 [Win32ThreadId],  
 [Message],  
 [SessionId],  
 [CallType],  
 [ProviderId] AS [SupplierId],  
 [Status],  
 LD.Value As  [AdditionalInfo],  
 [Timetaken]
FROM  
 [Log]  (NOLOCK) L
 INNER JOIN  LogData (NOLOCK) LD ON L.LogID = LD.LogId AND LD.[Key] = 'AdditionalInfo'
WHERE  
 L.[LogID] = @LogID

