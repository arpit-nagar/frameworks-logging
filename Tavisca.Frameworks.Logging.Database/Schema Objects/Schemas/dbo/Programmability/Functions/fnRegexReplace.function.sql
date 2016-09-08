CREATE FUNCTION [dbo].[fnRegexReplace]
(@data NVARCHAR (MAX), @pattern NVARCHAR (MAX), @replacement NVARCHAR (MAX))
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SqlFunctions].[Library].[RegexReplace]

