CREATE PROCEDURE [dbo].[spSelectExceptions] (
	@LogID int,
	@AppDomainName nvarchar(256),
	@Type nvarchar(150),
	@MachineName nvarchar(32),
	@TimeStampFrom DateTime=NULL,
	@TimeStampTo DateTime=NULl,
	@SubString nvarchar(200),
	@SubStringfrom nvarchar(50)
		
)

AS

SET NOCOUNT ON


SELECT 
	[LogID],
	[AppDomainName],
	[Type],
	[Timestamp],
	[MachineName],
	[Message]
	
FROM
	[ExceptionLog] nolock
WHERE

(isnull(@LogID,'')=' ' or [LogID]=@LogID)
and
(isnull(@AppDomainName,'')='' or [AppDomainName] like '%'+@AppDomainName+'%')
and
(isnull(@Type,'')='' or [Type] like '%'+@Type+'%')
and
(isnull(@MachineName,'')='' or [MachineName] like '%'+@MachineName+'%')
and
((isnull(@TimeStampFrom,'')='') or (isnull(@TimeStampTo,'')='') or ([Timestamp] BETWEEN @TimeStampFrom AND @TimeStampTo))
and
((isnull(@SubString,'')='' or
 (@SubStringfrom='Message' and [Message] like '%'+@SubString+'%'))or
(@SubStringfrom='StackTrace' and [StackTrace] like '%'+@SubString+'%')or
(@SubStringfrom='InnerExceptions' and [InnerExceptions] like '%'+@SubString+'%') or 
(@SubStringfrom='AppDomainName' and [AppDomainName] like '%'+@SubString+'%'))

ORDER BY LogID DESC

