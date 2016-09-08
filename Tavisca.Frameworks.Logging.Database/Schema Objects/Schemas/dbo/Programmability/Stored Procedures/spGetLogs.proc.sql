CREATE PROCEDURE [dbo].[spGetLogs]
    (
      @Id INT ,
      @TimeStampFrom DATETIME = NULL ,
      @TimeStampTo DATETIME = NULL ,
      @MachineName NVARCHAR(32) ,
      @Message VARCHAR(2048) ,
      @SessionId UNIQUEIDENTIFIER = NULL,
      @CallType NVARCHAR(32) ,
      @SupplierId INT ,
      @Status NVARCHAR(32) ,
      @TimeMin FLOAT,
      @TimeMax FLOAT,
      @SearchText NVARCHAR(100) = NULL ,
      @PageIndex INT = 1 ,
      @PageSize INT = 10 ,
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
        
                
        SET @innerStmt = 'SELECT    ROW_NUMBER() OVER ( ORDER BY [TimeStamp] DESC ) AS ROW, LogID, EventID, Priority, Severity, Title, Timestamp, MachineName,AppDomainName, ProcessID, ProcessName, ThreadName, Win32ThreadId, Message, SessionId,Dk, CallType, SupplierId, Status, AdditionalInfo, TimeTaken,AccountId,UsersessionId,UserIdentifier,Application,ContextIdentifier,IpAddress FROM [Log] WITH ( NOLOCK ) WHERE 1=1 AND'

        IF @Id IS NOT NULL 
            SET @innerStmt = @innerStmt + '[LogID] = ' + CAST(@Id AS NVARCHAR) + ' AND '

        IF @TimeStampFrom IS NOT NULL
            AND @TimeStampTo IS NOT NULL 
            SET @innerStmt = @innerStmt + ' [Timestamp] BETWEEN ''' + CAST(@TimeStampFrom AS NVARCHAR) + ''' AND '''
                + CAST(@TimeStampTo AS NVARCHAR) + ''' AND '
         Print @innerStmt       
        IF @MachineName IS NOT NULL 
            SET @innerStmt = @innerStmt + ' [MachineName] LIKE ''%' + @MachineName + '%'' AND '

        IF @Message IS NOT NULL 
            SET @innerStmt = @innerStmt + ' [Message] LIKE ''%' + @Message + '%'' AND ' 

        IF @SessionId IS NOT NULL 
            SET @innerStmt = @innerStmt + ' [SessionId] = ''' + CAST(@SessionId AS NVARCHAR(36)) + ''' AND '
                      
        IF @CallType IS NOT NULL 
            SET @innerStmt = @innerStmt + ' [CallType] LIKE ''%' + @CallType + '%'' AND '

        IF @SupplierId IS NOT NULL 
            SET @innerStmt = @innerStmt + '[SupplierId] =' + CAST(@SupplierId AS NVARCHAR) + ' AND '
                                   
        IF @Status IS NOT NULL 
            SET @innerStmt = @innerStmt + ' [Status] LIKE ''%' + @Status + '%'' AND '


        IF @TimeMin IS NOT NULL
            AND @TimeMax IS NOT NULL 
            SET @innerStmt = @innerStmt + ' [TimeTaken] BETWEEN ''' + CAST(@TimeMin AS NVARCHAR) + ''' AND '''
                + CAST(@TimeMax AS NVARCHAR) + ''' AND '
               
               
         IF @Dk IS NOT NULL AND @Dk <> ''
            SET @innerStmt = @innerStmt + ' [Dk] = ''' + CAST(@Dk AS NVARCHAR(32)) + ''' AND '
                                                               
        IF @SearchText IS NOT NULL 
            SET @innerStmt = @innerStmt + ' ([Title] LIKE ''%' + @SearchText + '%'' OR [Message] LIKE ''%' + @SearchText
                + '%'' OR [AdditionalInfo] LIKE ''%' + @SearchText + '%'' ) AND '
                        
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
              [AppDomainName] [nvarchar](512) NOT NULL ,
              [ProcessID] [nvarchar](256) NOT NULL ,
              [ProcessName] [nvarchar](512) NOT NULL ,
              [ThreadName] [nvarchar](512) NULL ,
              [Win32ThreadId] [nvarchar](128) NULL ,
              [Message] [nvarchar](2048) NULL ,
              [SessionId] [uniqueidentifier] NULL ,
              [Dk] [nvarchar](32) NULL,
              [CallType] [nvarchar](32) NULL ,
              [SupplierId] [int] NULL ,
              [Status] [nvarchar](32) NULL ,
              [AdditionalInfo] [nvarchar](MAX) NULL ,
              [TimeTaken] [float] NULL,
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
        SELECT  ROW, LogID, EventID, Priority, Severity, Title, Timestamp, MachineName,
                AppDomainName, ProcessID, ProcessName, ThreadName, Win32ThreadId, Message, SessionId,Dk,
                CallType, SupplierId, Status, AdditionalInfo, TimeTaken,AccountId,UsersessionId,UserIdentifier
      ,Application,ContextIdentifier,IpAddress 
        FROM    @RowTable             
    END