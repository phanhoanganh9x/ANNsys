-- =============================================
-- Author:      Binh-TT
-- Create date: 2018-06-04
-- Description: Delete the record two day ago
-- ==========================================

CREATE PROCEDURE ClearCronJob
    @NumberDaysKeep INT = 2
AS
BEGIN
    DELETE 
        FROM [CronJobProductStatus]
    WHERE
        [CreatedDate] < CONVERT(VARCHAR(10), DATEADD(DAY, -1 * @NumberDaysKeep, GETDATE()), 121)
END
