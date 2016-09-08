CREATE FUNCTION [dbo].[fnBinaryDecompress]
(@data VARBINARY (MAX), @compressionMethod INT)
RETURNS VARBINARY (MAX)
AS
 EXTERNAL NAME [SqlFunctions].[Library].[BinaryDecompress]

