CREATE FUNCTION [dbo].[fnFindToken] (@data TEXT, @name varchar(100)) RETURNS VARCHAR(500)	
	BEGIN
			DECLARE @x int
			DECLARE @y int
			SET @x = (SELECT CHARINDEX(@name, @data) + LEN(@name) + 1)
			SET @y = (SELECT charindex( '&', @data, @x) - @x)
	RETURN (
	SELECT substring( @data, @x, @y) 	
	)
	END