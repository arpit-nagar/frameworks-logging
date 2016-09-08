CREATE PROCEDURE [dbo].[spGetExceptions]
    (
      @Id INT ,
      @MachineName NVARCHAR(32) ,
      @Source NVARCHAR(64) ,
      @TargetSite NVARCHAR(128) ,
      @ExceptionType NVARCHAR(150) ,
      @AppDomainName NVARCHAR(256) ,
      @Message VARCHAR(4096) ,
      @TimeStampFrom DATETIME = NULL ,
      @TimeStampTo DATETIME = NULL ,
      @PageIndex INT = 1 ,
      @PageSize INT = 10 ,
      @SearchText NVARCHAR(100) = NULL ,
      @SessionId UNIQUEIDENTIFIER = NULL ,
      @Dk NVARCHAR(32) = NULL,
	  @AccountId  nvarchar(32)= NULL,
	  @UsersessionId nvarchar(256)= NULL,  
	  @UserIdentifier  nvarchar(256)= NULL,
	  @Application  nvarchar(256)= NULL,
	  @ContextIdentifier nvarchar(128)= NULL,
	  @IpAddress nvarchar(256)= NULL,
      @TotalRowCount INT OUTPUT
    )
AS 
    BEGIN

        DECLARE @StartIndex AS INT
        DECLARE @EndIndex AS INT

        SET @PageIndex = @PageIndex - 1
        SET @StartIndex = @PageIndex * @PageSize
        SET @EndIndex = ( @PageIndex + 1 ) * @PageSize

		DECLARE @innerStmt NVARCHAR(MAX)
        DECLARE @rowStmt NVARCHAR(MAX)
        DECLARE @totalCountStmt NVARCHAR(MAX)
        SET @innerStmt = 'SELECT  ROW_NUMBER() OVER ( ORDER BY [TimeStamp] DESC ) AS ROW, e.LogID,e.EventID,e.Priority,
		e.Severity,e.Title,e.Timestamp,e.MachineName,e.AppDomainName,e.ProcessID,e.ProcessName,e.ThreadName,e.Win32ThreadId,e.Type,e.Message,
		e.Source,e.TargetSite,e.StackTrace, e.ThreadIdentity,e.AdditionalInfo,e.InnerExceptions,e.Dk,s.SessionId ,
		e.AccountId,e.UsersessionId,e.UserIdentifier,e.Application,e.ContextIdentifier,e.IpAddress
		FROM [ExceptionLog] e WITH ( NOLOCK ) LEFT OUTER JOIN [ExceptionSessionLog] s WITH ( NOLOCK ) ON 
        e.LogId = s.LogId  WHERE 1=1 AND'

        IF @Id IS NOT NULL 
            SET @innerStmt = @innerStmt + ' e.LogId = ' + CAST(@Id AS NVARCHAR) + ' AND '

        IF @MachineName IS NOT NULL 
            SET @innerStmt = @innerStmt + ' e.MachineName] LIKE ''%' + @MachineName + '%'' AND '

        IF @Source IS NOT NULL 
            SET @innerStmt = @innerStmt + ' e.Source LIKE ''%' + @Source + '%'' AND '
                      
        IF @TargetSite IS NOT NULL 
            SET @innerStmt = @innerStmt + ' e.TargetSite LIKE ''%' + @TargetSite + '%'' AND '
                       
        IF @ExceptionType IS NOT NULL 
            SET @innerStmt = @innerStmt + ' e.Type LIKE ''%' + @ExceptionType + '%'' AND '
                        
        IF @AppDomainName IS NOT NULL 
            SET @innerStmt = @innerStmt + ' e.AppDomainName LIKE ''%' + @AppDomainName + '%'' AND '
                        
        IF @Message IS NOT NULL 
            SET @innerStmt = @innerStmt + ' e.Message LIKE ''%' + @Message + '%'' AND ' 
                        
        IF @TimeStampFrom IS NOT NULL
            AND @TimeStampTo IS NOT NULL 
            SET @innerStmt = @innerStmt + ' e.Timestamp BETWEEN ''' + CAST(@TimeStampFrom AS NVARCHAR) + ''' AND '''
                + CAST(@TimeStampTo AS NVARCHAR) + ''' AND '
         
        IF @SessionId IS NOT NULL 
            SET @innerStmt = @innerStmt + ' e.SessionId = ''' + CAST(@SessionId AS NVARCHAR(36)) + ''' AND '
                 
        IF @Dk IS NOT NULL AND @Dk <> ''
            SET @innerStmt = @innerStmt + ' e.Dk LIKE ''%' + @Dk + '%'' AND '  
                        
        IF @SearchText IS NOT NULL 
            SET @innerStmt = @innerStmt + ' (e.StackTrace LIKE ''%' + @SearchText + '%'' OR [AdditionalInfo] LIKE ''%'
                + @SearchText + '%'') AND '
                        
        SET @innerStmt = @innerStmt + ' 1=1 '


        SET @rowStmt = 'SELECT * FROM   ( ' + @innerStmt + ') AS DataRows WHERE  ROW > ' + CAST(@StartIndex AS NVARCHAR)
            + ' AND ROW <= ' + CAST(@EndIndex AS NVARCHAR)
        SET @totalCountStmt = 'SELECT count(*) FROM   ( ' + @innerStmt + ') as A'
		
		

		DECLARE @RowTable TABLE
            (
              ROW BIGINT NOT NULL ,
              [LogID] [int] NOT NULL ,
              [EventID] [int] NULL ,
              [Priority] [int] NOT NULL ,
              [Severity] [nvarchar](32) NOT NULL ,
              [Title] [nvarchar](256) NOT NULL ,
              [Timestamp] [datetime] NOT NULL ,
              [MachineName] [nvarchar](32) NOT NULL ,
              [AppDomainName] [nvarchar](256) NOT NULL ,
              [ProcessID] [nvarchar](256) NOT NULL ,
              [ProcessName] [nvarchar](256) NOT NULL ,
              [ThreadName] [nvarchar](256) NULL ,
              [Win32ThreadId] [nvarchar](128) NULL ,
              [Type] [nvarchar](150) NULL ,
              [Message] [varchar](4096) NULL ,
              [Source] [nvarchar](64) NULL ,
              [TargetSite] [nvarchar](128) NULL ,
              [StackTrace] [varchar](MAX) NULL ,
              [ThreadIdentity] [nvarchar](64) NULL ,
              [AdditionalInfo] [ntext] NULL ,
              [InnerExceptions] [ntext] NULL,
              [Dk] [nvarchar](32) NULL,
              [SessionId] [uniqueidentifier] NULL ,
              [AccountId] [nvarchar](32) NULL,
	          [UsersessionId] [nvarchar](256) NULL,
			  [UserIdentifier] [nvarchar](256) NULL,
				[Application] [nvarchar](256) NULL,
				[ContextIdentifier] [nvarchar](128) NULL,
				[IpAddress] [nvarchar](256) NULL
            )


        INSERT  INTO @RowTable
                EXEC sp_executesql @rowStmt        

        DECLARE @t TABLE ( ROW_COUNT BIGINT )
        INSERT  INTO @t ( ROW_COUNT )
                EXEC sp_executesql @totalCountStmt
        SET @TotalRowCount = ( SELECT   ROW_COUNT
                               FROM     @t
                             )
 
 
        SELECT  ROW, LogID, EventID, Priority, Severity, Title, Timestamp, MachineName, AppDomainName, ProcessID,
                ProcessName, ThreadName, Win32ThreadId, Type, Message, Source, TargetSite, StackTrace, ThreadIdentity,
                AdditionalInfo, InnerExceptions, SessionId, Dk,AccountId,UsersessionId,UserIdentifier
      ,Application,ContextIdentifier,IpAddress
        FROM    @RowTable
  
             
    END