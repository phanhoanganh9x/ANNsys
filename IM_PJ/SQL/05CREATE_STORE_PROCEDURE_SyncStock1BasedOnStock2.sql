IF OBJECT_ID('SyncStock1BasedOnStock2', 'P') IS NOT NULL DROP PROCEDURE SyncStock1BasedOnStock2;
GO

CREATE PROCEDURE SyncStock1BasedOnStock2
    @MinQuantity INT NULL = 1
AS
BEGIN
    SET NOCOUNT ON;

    -- Lay tat ca san pham trong stock 2 co so luong > 0
    BEGIN
        SELECT
            STK2.ProductID
        ,   STK2.ProductVariableID
        ,   MAX(STK2.ID) AS ID
        INTO #Stock2Last
        FROM
            StockManager2 AS STK2
        GROUP BY
            STK2.ProductID
        ,   STK2.ProductVariableID
        ;

        -- Loai bo san pham trung lap trong Stock Manager 2
        BEGIN
            DELETE FROM StockManager2
            WHERE NOT EXiSTS (
                SELECT
                    NULL AS DUMMY
                FROM
                    #Stock2Last AS L
                WHERE
                    ID = L.ID
            );
        END

        SELECT
            STK2.ProductID
        ,   STK2.ProductVariableID
        ,   (STK2.QuantityCurrent + STK2.Quantity * IIF(STK2.Type = 1, 1, -1)) AS AvailableQuantity
        INTO #Stock2
        FROM
            StockManager2 AS STK2
        INNER JOIN #Stock2Last AS L
            ON  STK2.ProductID = L.ProductID
            AND STK2.ProductVariableID = L.ProductVariableID
            AND STK2.ID = L.ID
        WHERE
            (STK2.QuantityCurrent + STK2.Quantity * IIF(STK2.Type = 1, 1, -1)) >= @MinQuantity
        ;
    END

    -- Lay san pham trong stock 1 co ton tai trong stock 2
    BEGIN
        SELECT
            STK1.ParentID
        ,   STK1.ProductVariableID
        ,   MAX(STK1.ID) AS ID
        INTO #Stock1Last
        FROM
            tbl_StockManager AS STK1
        GROUP BY
            STK1.ParentID
        ,   STK1.ProductVariableID
        ;

        -- Loai bo san pham trung lap trong Stock Manager 1
        BEGIN
            DELETE FROM tbl_StockManager
            WHERE NOT EXiSTS (
                SELECT
                    NULL AS DUMMY
                FROM
                    #Stock1Last AS L
                WHERE
                    ID = L.ID
            );
        END

        SELECT
            ISNULL(STK1.ParentID, 0) AS ProductID
        ,   ISNULL(STK1.ProductVariableID, 0) AS ProductVariableID
        ,   STK1.SKU AS SKU
        ,   ISNULL(STK1.Type, 1) AS [Type]
        ,   ISNULL(STK1.Quantity, 0) AS Quantity
        ,   ISNULL(STK1.QuantityCurrent, 0) AS QuantityCurrent
        ,   ISNULL(STK1.CreatedDate, GETDATE()) AS CreatedDate
        INTO #Stock1Filter
        FROM
            tbl_StockManager AS STK1
        INNER JOIN #Stock1Last AS L
            ON STK1.ParentID = L.ParentID
            AND STK1.ProductVariableID = L.ProductVariableID
        INNER JOIN #Stock2 AS STK2
            ON STK1.ParentID = STK2.ProductID
            AND STK1.ProductVariableID = STK2.ProductVariableID
        ;

        SELECT
            STK1.ProductID
        ,   STK1.ProductVariableID
        ,   STK1.SKU
        ,   (STK1.QuantityCurrent + STK1.Quantity * IIF(STK1.Type = 1, 1, -1)) AS AvailableQuantity
        INTO #Stock1
        FROM
            #Stock1Filter AS STK1
        ;
    END

    -- Khoi tao du lieu cap nhat kho 1
    BEGIN
        SELECT
            STK1.ProductID
        ,   STK1.ProductVariableID
        ,   STK1.SKU
        ,   STK1.AvailableQuantity AS AvailableQuantity1
        ,   STK2.AvailableQuantity AS AvailableQuantity2
        INTO #StockUpdate
        FROM
            #Stock1 AS STK1
        INNER JOIN #Stock2 AS STK2
            ON STK1.ProductID = STK2.ProductID
            AND STK1.ProductVariableID = STK2.ProductVariableID
            AND STK1.AvailableQuantity <> STK2.AvailableQuantity
        ORDER BY
            STK1.ProductID
        ,   STK1.ProductVariableID
        ;
    END

    -- Thuc hien cursor cap nhat kho 1
    BEGIN
        DECLARE
           @ProductID INT
        ,   @ProductVariableID INT
        ,   @SKU NVARCHAR(255)
        ,   @AvailableQuantity1 INT
        ,   @AvailableQuantity2 INT
        ;
        DECLARE product_cusror CURSOR FOR SELECT * FROM #StockUpdate; 

        OPEN product_cusror;

        FETCH NEXT FROM product_cusror
        INTO 
           @ProductID
        ,   @ProductVariableID
        ,   @SKU
        ,   @AvailableQuantity1
        ,   @AvailableQuantity2
        ;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            UPDATE tbl_StockManager
            SET
                [Type] = IIF((@AvailableQuantity2 - @AvailableQuantity1) > 0, 1, 2)
            ,   QuantityCurrent = @AvailableQuantity1
            ,   Quantity = ABS(@AvailableQuantity2 - @AvailableQuantity1)
            ,   [Status] = 20
            ,   [NoteID] = N'Cân bằng số lượng kho 1 bằng với kho 2'
            ,   ModifiedDate = GETDATE()
            ,   ModifiedBy = N'admin'
            WHERE
                ParentID = @ProductID
                AND ProductVariableID = @ProductVariableID
            ;

           FETCH NEXT FROM product_cusror
           INTO 
               @ProductID
           ,   @ProductVariableID
           ,   @SKU
           ,   @AvailableQuantity1
           ,   @AvailableQuantity2
           ;
        END

        CLOSE product_cusror;
        DEALLOCATE product_cusror;
    END

    -- Xem ket qua cap nhat
    BEGIN
        WITH [DATA] AS (
            SELECT
                ISNULL(STK1.ParentID, 0) AS ProductID
            ,   ISNULL(STK1.ProductVariableID, 0) AS ProductVariableID
            ,   STK1.SKU AS SKU
            ,   ISNULL(STK1.Type, 1) AS [Type]
            ,   ISNULL(STK1.Quantity, 0) AS Quantity
            ,   ISNULL(STK1.QuantityCurrent, 0) AS QuantityCurrent
            ,   ISNULL(STK1.CreatedDate, GETDATE()) AS CreatedDate
            FROM
                tbl_StockManager AS STK1
            INNER JOIN #StockUpdate AS U
                ON  STK1.ParentID = U.ProductID
                AND STK1.ProductVariableID = U.ProductVariableID
        )
        SELECT
            STK1.ProductID
        ,   STK1.ProductVariableID
        ,   STK1.SKU
        ,   OLD.AvailableQuantity1 AS AvailableQuantityOld
        ,   (STK1.QuantityCurrent + STK1.Quantity * IIF(STK1.Type = 1, 1, -1)) AS AvailableQuantityNow
        ,   OLD.AvailableQuantity2 AS AvailableQuantity2
        FROM
            [DATA] AS STK1
        INNER JOIN #StockUpdate AS OLD
            ON  STK1.ProductID = OLD.ProductID
            AND STK1.ProductVariableID = OLD.ProductVariableID
        ;
    END
END