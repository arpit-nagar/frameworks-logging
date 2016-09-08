




CREATE PROCEDURE [dbo].[spSelectTopExceptionLogs] (
	@num int
)

AS

SET NOCOUNT ON


SELECT 
	[LogID],
	[Timestamp],
	[ProcessName],
	[Type],
	[Message]	
FROM
	[ExceptionLog]
where LogID > (select max(LogID)-@num from exceptionlog )

order by LogID desc