-- Remove temp table
BEGIN
    -- Product
    IF OBJECT_ID('tempdb..#Product') IS NOT NULL DROP TABLE #Product;
    IF OBJECT_ID('tempdb..#ProductVariation') IS NOT NULL DROP TABLE #ProductVariation;
    -- Order
    IF OBJECT_ID('tempdb..#Order') IS NOT NULL DROP TABLE #Order;
    IF OBJECT_ID('tempdb..#OrderDetail') IS NOT NULL DROP TABLE #OrderDetail;
END

-- Khoi tao column
-- BEGIN
--    -- Order
--    BEGIN
--        IF COL_LENGTH('tbl_OrderDetail', 'TotalCostOfGood') IS NOT NULL
--            ALTER TABLE tbl_OrderDetail
--            DROP COLUMN TotalCostOfGood;

--        IF COL_LENGTH('tbl_OrderDetail', 'CostOfGood') IS NOT NULL
--        BEGIN
--            IF OBJECT_ID('DF_tbl_OrderDetail_CostOfGood', 'D') IS NOT NULL
--                ALTER TABLE tbl_OrderDetail
--                DROP CONSTRAINT [DF_tbl_OrderDetail_CostOfGood];

--            ALTER TABLE tbl_OrderDetail
--            DROP COLUMN CostOfGood;
--        END

--        ALTER TABLE tbl_OrderDetail 
--        ADD [CostOfGood] MONEY NOT NULL 
--        CONSTRAINT [DF_tbl_OrderDetail_CostOfGood] DEFAULT 0
--        WITH VALUES;

--        ALTER TABLE tbl_OrderDetail
--        ADD TotalCostOfGood AS (ISNULL(Quantity, 0) * CostOfGood);

--        IF COL_LENGTH('tbl_Order', 'TotalQuantity') IS NOT NULL
--        BEGIN
--            IF OBJECT_ID('DF_tbl_Order_TotalQuantity', 'D') IS NOT NULL
--                ALTER TABLE tbl_Order
--                DROP CONSTRAINT [DF_tbl_Order_TotalQuantity];

--            ALTER TABLE tbl_Order
--            DROP COLUMN TotalQuantity;
--        END

--        ALTER TABLE tbl_Order
--        ADD [TotalQuantity] INT NOT NULL 
--        CONSTRAINT [DF_tbl_Order_TotalQuantity] DEFAULT 0
--        WITH VALUES;

--        IF COL_LENGTH('tbl_Order', 'TotalCostOfGood') IS NOT NULL
--        BEGIN
--            IF OBJECT_ID('DF_tbl_Order_TotalCostOfGood', 'D') IS NOT NULL
--                ALTER TABLE tbl_Order
--                DROP CONSTRAINT [DF_tbl_Order_TotalCostOfGood];

--            ALTER TABLE tbl_Order
--            DROP COLUMN TotalCostOfGood;
--        END

--        ALTER TABLE tbl_Order
--        ADD [TotalCostOfGood] MONEY NOT NULL 
--        CONSTRAINT [DF_tbl_Order_TotalCostOfGood] DEFAULT 0
--        WITH VALUES;
--    END
-- END

BEGIN
    SET NOCOUNT ON; 

    -- Lay du lieu Product
    BEGIN
        SELECT
            P.*
        INTO #Product
        FROM
            tbl_Product AS P
        ORDER BY
            P.ProductSKU
        ;

        SELECT
            PV.*
        INTO #ProductVariation
        FROM
            tbl_ProductVariable AS PV
        ORDER BY
            PV.SKU
        ;
    END

    -- Lay du lieu Order
    BEGIN
        SELECT
            O.*
        INTO #Order
        FROM
            tbl_Order AS O
        ORDER BY
            O.ID DESC
        ;

        SELECT
            OD.*
        INTO #OrderDetail
        FROM
            tbl_OrderDetail AS OD
        INNER JOIN #Order AS O
            ON OD.OrderID = O.ID
        ORDER BY
            OD.OrderID DESC
        ,   OD.SKU
        ;
    END

    DECLARE @OrderID INT,
        @ProductStyle INT,
        @SKU NVARCHAR(255),
        @Quantity INT,
        @COGS MONEY;

    -- Cap nhat Order
    BEGIN
        -- Cap nhat Order Detail
        BEGIN
            DECLARE order_detail_cursor CURSOR FOR
                SELECT
                    OD.OrderID
                ,   OD.SKU
                ,   OD.ProductType
                FROM
                    #OrderDetail AS OD
                GROUP BY
                    OD.OrderID
                ,   OD.SKU
                ,   OD.ProductType
            ;

            SET @OrderID = NULL;
            SET @ProductStyle = NULL;
            SET @SKU = NULL;
            SET @COGS = NULL;

            OPEN order_detail_cursor;

            FETCH NEXT FROM order_detail_cursor
            INTO @OrderID, @SKU, @ProductStyle
            ;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                IF @ProductStyle = 1
                BEGIN
                    SET @COGS = (
                        SELECT
                            ISNULL(P.CostOfGood, 0)
                        FROM
                            #Product AS P
                        WHERE
                            P.ProductSKU = @SKU
                    );
                END
                ELSE
                BEGIN
                    SET @COGS = (
                        SELECT
                            ISNULL(PV.CostOfGood, 0)
                        FROM
                            #ProductVariation AS PV
                        WHERE
                            PV.SKU = @SKU
                    );
                END

                UPDATE tbl_OrderDetail
                SET
                    CostOfGood = ISNULL(@COGS, 0)
                WHERE
                    @OrderID = @OrderID
                AND SKU = @SKU
                ;

                FETCH NEXT FROM order_detail_cursor
                INTO @OrderID, @SKU, @ProductStyle
                ;
            END

            CLOSE order_detail_cursor;
            DEALLOCATE order_detail_cursor;
        END

        -- Cap nhat Order
        BEGIN
            DECLARE order_cursor CURSOR FOR
                SELECT
                    O.ID
                FROM
                    #Order AS O
            ;

            SET @OrderID = NULL;
            SET @Quantity = NULL;
            SET @COGS = NULL;

            OPEN order_cursor;

            FETCH NEXT FROM order_cursor
            INTO @OrderID
            ;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                SELECT
                    @Quantity = SUM(ISNULL(OD.Quantity, 0))
                ,   @COGS = SUM(ISNULL(OD.TotalCostOfGood, 0))
                FROM
                    tbl_OrderDetail AS OD
                WHERE
                    OD.OrderID = @OrderID
                GROUP BY
                    OD.OrderID
                ;
                
                UPDATE tbl_Order
                SET
                    TotalQuantity = ISNULL(@Quantity, 0)
                ,   TotalCostOfGood = ISNULL(@COGS, 0)
                WHERE
                    ID = @OrderID
                ;

                FETCH NEXT FROM order_cursor
                INTO @OrderID
                ;
            END

            CLOSE order_cursor;
            DEALLOCATE order_cursor;
        END
    END

    -- Xem ke qua thuc hien
    BEGIN
        SELECT
            OB.ID
        ,   OB.TotalQuantity AS QuantityBefore
        ,   OA.TotalQuantity AS QuantityAfter
        ,   OB.TotalCostOfGood AS COGSBefore
        ,   OA.TotalCostOfGood AS COGSAfter
        FROM
            #Order AS OB
        INNER JOIN tbl_Order AS OA
            ON OB.ID = OA.ID
        ORDER BY
            OB.ID
        ;

        SELECT
            ODB.ID
        ,   ODB.SKU
        ,   ODB.CostOfGood AS COGSBefore
        ,   ODA.CostOfGood AS COGSAfter
        ,   IIF(ODB.ProductType = 1, P.CostOfGood, PV.CostOfGood) AS COGS 
        FROM
            #OrderDetail AS ODB
        INNER JOIN tbl_OrderDetail AS ODA
            ON ODB.ID = ODA.ID
        LEFT JOIN #Product AS P
            ON ODB.ProductType = 1
            AND ODB.SKU = P.ProductSKU
        LEFT JOIN #ProductVariation AS PV
            ON ODB.ProductType = 2
            AND ODB.SKU = PV.SKU
        ORDER BY
            ODB.OrderID
        ,   ODB.ID DESC
        ;
    END
END