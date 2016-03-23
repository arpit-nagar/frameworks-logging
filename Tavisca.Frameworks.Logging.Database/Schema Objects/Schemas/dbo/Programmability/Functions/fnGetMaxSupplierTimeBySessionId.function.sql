CREATE FUNCTION [dbo].[fnGetMaxSupplierTimeBySessionId]
(
	@SessionId nvarchar(64)
)
RETURNS float
AS
BEGIN
	DECLARE @SupplierMaxTime float
	SET @SupplierMaxTime = (SELECT MAX([TimeTaken])
			FROM [Log] WHERE 
			[SessionId]=@SessionId
			AND ProviderId is Not NULL
			AND ProviderId != 0)
	RETURN @SupplierMaxTime
	END