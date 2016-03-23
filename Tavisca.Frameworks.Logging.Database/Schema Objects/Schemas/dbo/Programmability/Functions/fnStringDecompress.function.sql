CREATE FUNCTION [dbo].[fnStringDecompress]
(@data NVARCHAR (MAX), @compressionMethod INT)
RETURNS NVARCHAR (MAX)
AS
 EXTERNAL NAME [SqlFunctions].[Library].[StringDecompress]

