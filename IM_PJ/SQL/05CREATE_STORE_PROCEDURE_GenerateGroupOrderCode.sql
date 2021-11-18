CREATE PROCEDURE [dbo].[usp_GenerateGroupOrderCode]
    @NewCode NVARCHAR(6) OUTPUT
AS
BEGIN
    DECLARE @IdSeq INT;

    SET @IdSeq = NEXT VALUE FOR [dbo].[GroupOrderSeq];
    SET @NewCode = CONCAT('GP', RIGHT('0000' + CAST(@IdSeq AS NVARCHAR(4)), 4));
END
GO