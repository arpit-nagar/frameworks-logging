  
  
  
  
CREATE PROCEDURE [dbo].[spSelectXmlRequest] (  
 @LogID int  
)  
  
AS  
  
SET NOCOUNT ON  
  
  
  
SELECT   
  dbo.fnStringDecompress([Request],1) as XmlRequest  
  
FROM  
 LogRequestResponse  
  
WHERE [LogID]=@LogID