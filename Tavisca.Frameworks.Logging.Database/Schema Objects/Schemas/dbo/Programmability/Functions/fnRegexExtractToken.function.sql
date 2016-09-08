CREATE FUNCTION [dbo].[fnRegexExtractToken]
(@data NVARCHAR (MAX), @pattern NVARCHAR (MAX), @token NVARCHAR (MAX))
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SqlFunctions].[Library].[RegexExtractToken]

