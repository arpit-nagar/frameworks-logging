CREATE FUNCTION [dbo].[fnBinaryCompress]
(@data VARBINARY (MAX), @compressionMethod INT)
RETURNS VARBINARY (MAX)
AS
 EXTERNAL NAME [SqlFunctions].[Library].[BinaryCompress]

