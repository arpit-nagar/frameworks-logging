CREATE FUNCTION [dbo].[fnRegexExtract]
(@data NVARCHAR (MAX), @pattern NVARCHAR (MAX))
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SqlFunctions].[Library].[RegexExtract]

