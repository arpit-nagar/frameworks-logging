


CREATE PROCEDURE [dbo].[spSelectSingleExceptionLog] (
	@LogID int
)

AS

SET NOCOUNT ON

SELECT
	EL.[LogID],
	0 As [EventID],
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
	[Type],
	[Message],
	[Source],
	[TargetSite],
	[StackTrace],
	[ThreadIdentity],
	ED.[Value] AS AdditionalInfo,
	[InnerExceptions]
FROM
	[ExceptionLog] (NOLOCK) EL 
	INNER JOIN ExceptionData (NOLOCK) ED ON EL.LogID = ED.LogId AND ED.[Key] ='AdditionalInfo'
WHERE
	EL.[LogID] = @LogID

