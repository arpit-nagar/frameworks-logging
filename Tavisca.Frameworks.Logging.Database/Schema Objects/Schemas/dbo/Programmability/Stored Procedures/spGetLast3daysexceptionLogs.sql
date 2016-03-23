CREATE PROCEDURE [dbo].[spGetLast3daysexceptionLogs] 
@ProfileName VARCHAR(512),
@RecipientList VARCHAR(4000)
AS
BEGIN
	
	SET NOCOUNT ON;
    DECLARE @MailSubject VARCHAR(1028),@MailBody VARCHAR(MAX)
    
    DECLARE @LastDay DATE,@Last2ndDay DATE,@Last3rdDay DATE

		SELECT @LastDay = DATEADD(dd,-1,GETDATE())
		SELECT @Last2ndDay = DATEADD(dd,-2,GETDATE())
		SELECT @Last3rdDay = DATEADD(dd,-3,GETDATE())
  


		--SET @RecipientList = 'apatidar@tavisca.com;'--'bkulkarni@tavisca.com;apatidar@tavisca.com;adutta@tavisca.com'
		SET @ProfileName = 'Optimus_Profile'
		SET @MailSubject = 'Engines stage environment – Last 3 days exception stats'


		;WITH CTE
		AS
		(
		SELECT
		[Type]
		,SUM(LastDay) AS LastDay
		,SUM(Last2ndDay) AS Last2ndDay
		,SUM(Last3rdDay) AS Last3rdDay
		FROM
		(
		SELECT
		[type] ,
		LastDay = CASE WHEN DATEADD(D, 0, DATEDIFF(D, 0, [Timestamp])) = @LastDay THEN 1 ELSE 0 END,
		Last2ndDay = CASE WHEN DATEADD(D, 0, DATEDIFF(D, 0, [Timestamp])) = @Last2ndDay THEN 1 ELSE 0 END,
		Last3rdDay = CASE WHEN DATEADD(D, 0, DATEDIFF(D, 0, [Timestamp])) = @Last3rdDay THEN 1 ELSE 0 END
		from ExceptionLog (nolock)
		WHERE
		DATEADD(D, 0, DATEDIFF(D, 0, [Timestamp])) >= @Last3rdDay
		and [MachineName] = 'NEXUS'
		)X
		GROUP BY [Type]
		)

		SELECT * , ROW_NUMBER() over(order by LastDay desc) as Id into #temp
		FROM
		(
		SELECT [Type], LastDay, Last2ndDay as [2ndLastday], Last3rdDay as [3rdLastDay] FROM CTE
		union all
		SELECT 'Total Exceptions' as [Type], SUM(LastDay) as [LastDay], SUM(Last2ndDay) as [2ndLastday], SUM(Last3rdDay) as [3rdLastDay] FROM CTE
		)Y
		ORDER BY LastDay desc ,[Type]



		DECLARE @Cnt INT,@MaxCnt INT
		SET @Cnt = 1
		SELECT @MaxCnt = MAX(Id) FROM #Temp


		SET @MailBody = '<Body>' + 'Last 3 days stage exceptionlog details' +'</T><P></P>' +
		'<table border="2">
		<tr>
		<td>ID</td>
		<td>Type</td>
		<td>'+CAST(@LastDay AS VARCHAR(64))+'</td>
		<td>'+CAST(@Last2ndDay AS VARCHAR(64))+'</td>
		<td>'+CAST(@Last3rdDay AS VARCHAR(64))+'</td>
		</tr>'

		WHILE @Cnt <=@MaxCnt
		BEGIN

		Set @MailBody = @MailBody +
		(
		SELECT '<tr>'
		+'<td>' + CAST(Id AS VARCHAR(64)) +'</td>'
		+'<td>' + [Type] +'</td>'
		+'<td>' + CAST(LastDay AS VARCHAR(64)) +'</td>'
		+'<td>' + CAST([2ndLastday] AS VARCHAR(64)) +'</td>'
		+'<td>' + CAST([3rdLastDay] AS VARCHAR(64)) +'</td>'
		+'</tr>'
		FROM
		#Temp
		WHERE
		Id = @Cnt
		)


		SET @Cnt = @Cnt + 1;
		END

		Set @MailBody = @MailBody + '</table>'+'</T><P></P>' +'DBA Team'+'</Body>'




		EXEC msdb.dbo.sp_send_dbmail @recipients = @RecipientList ,
		@subject = @mailSubject ,
		@body = @MailBody,
		@profile_name = @ProfileName,
		@body_format = 'HTML'

		DROP TABLE #Temp
   
END