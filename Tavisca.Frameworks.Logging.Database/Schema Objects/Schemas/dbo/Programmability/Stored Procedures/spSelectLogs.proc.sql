

     
    
CREATE PROCEDURE [dbo].[spSelectLogs] (  
 @LogID int,  
 @SessionId nvarchar(64),  
 @MachineName nvarchar(32),  
 @TimeStampFrom DateTime = NULL,  
 @TimeStampTo DateTime = NULL,  
 @SupplierID int,  
 @Status nvarchar(32),  
 @CallType nvarchar(32),  
 @SubString nvarchar(200),  
 @SubStringfrom nvarchar(50)  
    
)  
  
AS  
  
SET NOCOUNT ON  
  
  
SELECT top 20 
 L.[LogID],  
 [Title],  
 [ProviderId],  
 [CallType],  
 [Timestamp],  
 [MachineName],  
 [Status],
 [SessionId],
 TimeTaken
   
FROM  
 [Log]  L 
 INNER JOIN [LogRequestResponse] LRR ON L.LogID = LRR.LogId 
WHERE  
  
(isnull(@LogID,'')=' ' or L.[LogID]=@LogID)  
and  
(isnull(@SessionId,'')='' or [SessionId]=@SessionId)  
and  
(isnull(@MachineName,'')='' or [MachineName]=@MachineName)  
and  
(@TimeStampFrom IS NULL OR  @TimeStampTo IS NULL or ([Timestamp] BETWEEN @TimeStampFrom AND @TimeStampTo))  
and  
(isnull(@SupplierID,'')='' or [ProviderId]=@SupplierID)  
and  
(isnull(@Status,'')='' or [Status]=@Status)  
and  
(isnull(@CallType,'')='' or [CallType]=@CallType)  
and  
((isnull(@SubString,'')='' or  
@SubStringfrom='Select' or  
 (@SubStringfrom='XmlResponse' and dbo.fnStringDecompress([Response],1) like '%'+@SubString+'%'))or  
(@SubStringfrom='XmlRequest' and dbo.fnStringDecompress([Request],1) like '%'+@SubString+'%') or   
(@SubStringfrom='Title' and [Title] like '%'+@SubString+'%'))  
  
 ORDER BY L.LogID DESC