CREATE FUNCTION [dbo].[fnStringCompress]
(@data NVARCHAR (MAX), @compressionMethod INT)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SqlFunctions].[Library].[StringCompress]

