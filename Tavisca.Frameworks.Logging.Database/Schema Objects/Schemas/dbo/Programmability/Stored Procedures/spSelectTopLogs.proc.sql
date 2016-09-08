CREATE PROCEDURE [dbo].[spSelectTopLogs] (  
 @num int  
)  
  
AS  
  
SET NOCOUNT ON  
  
  
SELECT   
 [LogID],  
 [Title],  
 [Timestamp],  
 [MachineName],  
 [SessionId],  
 [CallType],  
 ProviderId as [SupplierId],  
 [Status],  
 [TimeTaken] 
FROM  
 [Log]  
where Logid > (select max(logid)-@num from log )  
  
order by LogId desc

