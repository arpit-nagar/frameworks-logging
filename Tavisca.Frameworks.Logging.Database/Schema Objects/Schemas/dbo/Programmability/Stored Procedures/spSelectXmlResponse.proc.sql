  
  
  
  
CREATE PROCEDURE [dbo].[spSelectXmlResponse] (  
 @LogID int  
)  
  
AS  
  
SET NOCOUNT ON  
  
  
  
SELECT   
  dbo.fnStringDecompress(Response,1)as XmlResponse  
  
FROM  
 LogRequestResponse  
  
WHERE [LogID]=@LogID