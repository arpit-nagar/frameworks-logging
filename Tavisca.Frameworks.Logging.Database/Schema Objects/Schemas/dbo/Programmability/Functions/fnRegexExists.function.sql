CREATE FUNCTION [dbo].[fnRegexExists]
(@data NVARCHAR (MAX), @pattern NVARCHAR (MAX))
RETURNS BIT
AS
 EXTERNAL NAME [SqlFunctions].[Library].[RegexExists]

