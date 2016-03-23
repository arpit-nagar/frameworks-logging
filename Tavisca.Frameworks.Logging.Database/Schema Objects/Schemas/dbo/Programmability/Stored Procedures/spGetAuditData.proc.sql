CREATE PROCEDURE [dbo].[spGetAuditData]
    (
	    @LogId BIGINT           
    )
AS 
    BEGIN
    SELECT [KEY] ,Value
     FROM
		AuditData
	WHERE
	LogId = @LogID
	   END