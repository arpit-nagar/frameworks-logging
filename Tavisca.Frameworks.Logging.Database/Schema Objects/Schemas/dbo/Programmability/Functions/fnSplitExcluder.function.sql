CREATE  FUNCTION [dbo].[fnSplitExcluder](@list VARCHAR(8000), @delimChar varchar(1))
	RETURNS @tokens TABLE ([token] varchar(1000))
                                
AS 

BEGIN
	DECLARE @token AS varchar(1000)
    	DECLARE	@delimPos AS int

	SET @token = 0
	IF (RIGHT(@list, 1) <> @delimChar)
		SET @list = @list + @delimChar

	WHILE LEN(@list) <> 0
	BEGIN			
		SET @delimPos = CHARINDEX(@delimChar, @list)
		SET @token = SUBSTRING(@list, 0, @delimPos)
		SET @list = SUBSTRING(@list, @delimPos + 1, LEN(@list) - @delimPos + 1)
		
		INSERT @tokens VALUES (@token)
	END
	RETURN

END
