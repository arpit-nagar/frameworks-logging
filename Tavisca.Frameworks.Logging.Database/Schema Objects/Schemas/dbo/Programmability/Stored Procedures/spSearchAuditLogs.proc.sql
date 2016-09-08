CREATE PROCEDURE [dbo].[spSearchAuditLogs]
    (
	    @Id BIGINT,

        @ChainId NVARCHAR(1024),

        @ReferenceNumber NVARCHAR(1024),

        @IPAddress NVARCHAR(1024),

        @Username NVARCHAR(1024),

        @StartDate DATETIME,
        
        @EndDate DATETIME,

        @Applicaion NVARCHAR(1024),

        @Module NVARCHAR(1024),
        
        @EventType NVARCHAR(1024),

        @Status NVARCHAR(1024),

        @Tags NVARCHAR(2048),
        
        @PageSize BIGINT,
        
        @PageNumber BIGINT,
        
        @RequiredModules NVARCHAR(2048),
        
        @ExemptedModules NVARCHAR(2048)
        
    )
AS 
    BEGIN
    
    Set NoCount on    
  
	IF @Tags IS NULL OR @Tags = ''  
	SET @Tags = ''''  
	
	DECLARE @RequiredMod TABLE ([token] varchar(1000))
		INSERT INTO @RequiredMod ( [token] )
		SELECT [token] FROM [dbo].fnSplitAsString(@RequiredModules,',')

	DECLARE @ExemptedMod TABLE ([token] varchar(1000))
		INSERT INTO @ExemptedMod ( [token] )
		SELECT [token] FROM [dbo].fnSplitAsString(@ExemptedModules,',')


	
;WITH CTE AS  
(  
SELECT   
     ROW_NUMBER() OVER     
 (    
  Order by AT.[TimeStamp] Desc   
 ) AS RowNumber   
 	  ,AT.[Id]
      ,AT.[TimeStamp]
      ,AT.[ChainId]
      ,AT.[ReferenceNumber]
      ,AT.[IPAddress]
      ,AT.[Username]
      ,AT.[Application]
      ,AT.[Module]
      ,AT.[Status]
      ,AT.[Tags]
      ,AT.EventType
      ,Count(AT.[Id]) OVER() as [TotalCount]  
	FROM 
	  [AuditTrail] AS AT (NOLOCK)
	WHERE
	  (ISNULL(@RequiredModules,'')='' OR    Exists (SELECT [token] FROM @RequiredMod R Where AT.Module Like '%' + R.token +'%' ))
	  AND
	  (ISNULL(@ExemptedModules,'')='' OR NOT Exists (SELECT [token] FROM @ExemptedMod R Where AT.Module Like '%' + R.token +'%' ))
	  AND
	  (ISNULL(@Id,0)=0 OR (AT.Id = @ID ))
	  AND
	  @StartDate <= AT.TimeStamp 
	  AND
	  AT.TimeStamp <= @EndDate
	  AND
	  (ISNULL(@ChainId,'')='' OR (AT.ChainId = @ChainId))
	  AND
	  (ISNULL(@ReferenceNumber,'')='' OR (AT.ReferenceNumber = @ReferenceNumber))
	  AND
	  (ISNULL(@EventType,'')='' OR (AT.EventType LIKE '%'+@EventType+'%'))	  
	  AND
	  (ISNULL(@Applicaion,'')='' OR (AT.Application LIKE '%'+@Applicaion+'%'))
	  AND
	  (ISNULL(@Module,'')='' OR (AT.Module LIKE '%'+@Module+'%'))
	  AND
	  (ISNULL(@Username,'')='' OR (AT.Username LIKE '%'+@Username+'%'))
	  AND
	  (ISNULL(@IPAddress,'')='' OR (AT.IPAddress LIKE '%'+@IPAddress+'%'))
	  AND
	  (ISNULL(@Status,'')='' OR (AT.Status  = @Status))
	  AND 
	  (ISNULL(@Tags,'''')='''' OR (AT.Tags LIKE '%' + @Tags + '%'))  

)  
	SELECT top(@PageSize)  
  
	   [Id]
	  ,[TimeStamp]
	  ,[ChainId]
	  ,[ReferenceNumber]
	  ,[IPAddress]
	  ,[Username]
	  ,[Application]
	  ,[Module]
	  ,[Status]
	  ,[EventType]
	  ,[Tags]
	  ,[TotalCount]
	FROM   
		CTE   
	WHERE   
		RowNumber > (@PageNumber-1)*@PageSize    
    END