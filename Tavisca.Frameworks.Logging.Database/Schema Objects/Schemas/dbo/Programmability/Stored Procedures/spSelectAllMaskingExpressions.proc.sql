CREATE PROCEDURE [dbo].[spSelectAllMaskingExpressions]   

AS  
SELECT 
 [ProviderName],[CallType],[Regex], [ReplacementExpression]  
FROM  
 [MaskingExpressions]