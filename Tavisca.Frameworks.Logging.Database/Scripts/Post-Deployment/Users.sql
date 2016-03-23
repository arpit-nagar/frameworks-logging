

DECLARE @Users NVARCHAR(MAX)
SET @Users = N'$(DatabaseUsers)'

DECLARE @User VARCHAR(512),@Cnt INT,@MaxCnt INT,@Cmd NVARCHAR(512)

SELECT *,ROW_NUMBER() OVER(ORDER BY token) AS Id INTO #TempUsers FROM dbo.fnSplitAsString(@Users,',')

SET @Cnt = 1
SELECT @MaxCnt = MAX(Id) FROM #TempUsers

WHILE @Cnt <= @MaxCnt 
BEGIN 
	
	SELECT @User = ISNULL(token,'') FROM #TempUsers WHERE Id = @Cnt 
	
	IF @User <> ''
	BEGIN 
		IF  NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @User)
		BEGIN 
			SET @Cmd = 'CREATE USER [' + @User + '] FOR LOGIN [' + @User + ']'
			EXEC sp_executesql @Cmd
		END
		
		EXEC sp_addrolemember N'db_accessadmin',@User
		EXEC sp_addrolemember N'db_datareader',@User
		EXEC sp_addrolemember N'db_datawriter',@User	
		
		SET @Cmd =  'GRANT EXECUTE ON  SCHEMA::[dbo]  TO [' + @User + '];'
		EXEC sp_executesql @Cmd
		
		SET @Cmd =  'GRANT VIEW DEFINITION ON  SCHEMA::[dbo]  TO [' + @User + '];'
		EXEC sp_executesql @Cmd
		
		SET @Cmd =  'GRANT SHOWPLAN TO [' + @User + '];'
		EXEC sp_executesql @Cmd
	END
	SET @Cnt = @Cnt + 1;
	
END 


DROP TABLE #TempUsers
 

GO

