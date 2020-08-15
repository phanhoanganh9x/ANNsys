-- Remove temp table
BEGIN
    -- Product
    IF OBJECT_ID('tempdb..#Product') IS NOT NULL DROP TABLE #Product;
    IF OBJECT_ID('tempdb..#ProductVariation') IS NOT NULL DROP TABLE #ProductVariation;
    -- Refund
    IF OBJECT_ID('tempdb..#Refunds') IS NOT NULL DROP TABLE #Refunds;
    IF OBJECT_ID('tempdb..#RefundDetails') IS NOT NULL DROP TABLE #RefundDetails;
END

-- Khoi tao column
 --BEGIN
 --   -- Refund
 --   BEGIN
 --       IF COL_LENGTH('tbl_RefundGoodsDetails', 'TotalCostOfGood') IS NOT NULL
 --           ALTER TABLE tbl_RefundGoodsDetails
 --           DROP COLUMN TotalCostOfGood;

 --       IF COL_LENGTH('tbl_RefundGoodsDetails', 'CostOfGood') IS NOT NULL
 --       BEGIN
 --           IF OBJECT_ID('DF_tbl_RefundGoodsDetails_CostOfGood', 'D') IS NOT NULL
 --               ALTER TABLE tbl_RefundGoodsDetails
 --               DROP CONSTRAINT [DF_tbl_RefundGoodsDetails_CostOfGood];

 --           ALTER TABLE tbl_RefundGoodsDetails
 --           DROP COLUMN CostOfGood;
 --       END

 --       ALTER TABLE tbl_RefundGoodsDetails 
 --       ADD [CostOfGood] MONEY NOT NULL 
 --       CONSTRAINT [DF_tbl_RefundGoodsDetails_CostOfGood] DEFAULT 0
 --       WITH VALUES;

 --       ALTER TABLE tbl_RefundGoodsDetails 
 --       ADD TotalCostOfGood AS (ISNULL(Quantity, 0) * CostOfGood);

 --       IF COL_LENGTH('tbl_RefundGoods', 'TotalQuantity') IS NOT NULL
 --       BEGIN
 --           IF OBJECT_ID('DF_tbl_RefundGoods_TotalQuantity', 'D') IS NOT NULL
 --               ALTER TABLE tbl_RefundGoods
 --               DROP CONSTRAINT [DF_tbl_RefundGoods_TotalQuantity];

 --           ALTER TABLE tbl_RefundGoods
 --           DROP COLUMN TotalQuantity;
 --       END

 --       ALTER TABLE tbl_RefundGoods
 --       ADD [TotalQuantity] INT NOT NULL 
 --       CONSTRAINT [DF_tbl_RefundGoods_TotalQuantity] DEFAULT 0
 --       WITH VALUES;

 --       IF COL_LENGTH('tbl_RefundGoods', 'TotalCostOfGood') IS NOT NULL
 --       BEGIN
 --           IF OBJECT_ID('DF_tbl_RefundGoods_TotalCostOfGood', 'D') IS NOT NULL
 --               ALTER TABLE tbl_RefundGoods
 --               DROP CONSTRAINT [DF_tbl_RefundGoods_TotalCostOfGood];

 --           ALTER TABLE tbl_RefundGoods
 --           DROP COLUMN TotalCostOfGood;
 --       END

 --       ALTER TABLE tbl_RefundGoods
 --       ADD [TotalCostOfGood] MONEY NOT NULL 
 --       CONSTRAINT [DF_tbl_RefundGoods_TotalCostOfGood] DEFAULT 0
 --       WITH VALUES;
 --   END
 --END

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

    -- Lay du lieu Refund
    BEGIN
        SELECT
            RF.*
        INTO #Refunds
        FROM
            tbl_RefundGoods AS RF
        ORDER BY
            RF.ID DESC
        ;

        SELECT
            RFD.*
        INTO #RefundDetails
        FROM
            tbl_RefundGoodsDetails AS RFD
        INNER JOIN #Refunds AS RF
            ON RFD.RefundGoodsID = RF.ID
        ORDER BY
            RFD.RefundGoodsID DESC
        ,   RFD.SKU
        ;
    END

    DECLARE @RefundGoodsID INT,
        @ProductStyle INT,
        @SKU NVARCHAR(255),
        @Quantity INT,
        @COGS MONEY;

    -- Cap nhat Refund
    BEGIN
        -- Cap nhat Refund Detail
        BEGIN
            DECLARE refund_detail_cursor CURSOR FOR
                SELECT
                    RFD.RefundGoodsID
                ,   RFD.SKU
                ,   RFD.ProductType
                FROM
                    #RefundDetails AS RFD
                GROUP BY
                    RFD.RefundGoodsID
                ,   RFD.SKU
                ,   RFD.ProductType
            ;

            SET @RefundGoodsID = NULL;
            SET @ProductStyle = NULL;
            SET @SKU = NULL;
            SET @COGS = NULL;

            OPEN refund_detail_cursor;

            FETCH NEXT FROM refund_detail_cursor
            INTO @RefundGoodsID, @SKU, @ProductStyle
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

                UPDATE tbl_RefundGoodsDetails
                SET
                    CostOfGood = ISNULL(@COGS, 0)
                WHERE
                    RefundGoodsID = @RefundGoodsID
                AND SKU = @SKU
                ;

                FETCH NEXT FROM refund_detail_cursor
                INTO @RefundGoodsID, @SKU, @ProductStyle
                ;
            END

            CLOSE refund_detail_cursor;
            DEALLOCATE refund_detail_cursor;
        END

        -- Cap nhat tong Cost Of Good Sold của Refund
        BEGIN
            DECLARE refund_cursor CURSOR FOR
                SELECT
                    RF.ID
                FROM
                    #Refunds AS RF
            ;

            SET @RefundGoodsID = NULL;
            SET @Quantity = NULL;
            SET @COGS = NULL;

            OPEN refund_cursor;

            FETCH NEXT FROM refund_cursor
            INTO @RefundGoodsID
            ;

            WHILE @@FETCH_STATUS = 0
            BEGIN
                SELECT
                    @Quantity = SUM(ISNULL(RFD.Quantity,0))
                ,   @COGS = SUM(ISNULL(RFD.TotalCostOfGood, 0))
                FROM
                    tbl_RefundGoodsDetails AS RFD
                WHERE
                    RFD.RefundGoodsID = @RefundGoodsID
                GROUP BY
                    RFD.RefundGoodsID
                ;

                UPDATE tbl_RefundGoods
                SET
                    TotalQuantity = ISNULL(@Quantity, 0)
                ,   TotalCostOfGood = ISNULL(@COGS, 0)
                WHERE
                    ID = @RefundGoodsID
                ;

                FETCH NEXT FROM refund_cursor
                INTO @RefundGoodsID
                ;
            END

            CLOSE refund_cursor;
            DEALLOCATE refund_cursor;
        END
    END

    -- Xem ke qua thuc hien
    BEGIN
        SELECT
            RB.ID
        ,   RB.TotalQuantity AS QuantityBefore
        ,   RA.TotalQuantity AS QuantityAfter
        ,   RB.TotalCostOfGood AS COGSBefore
        ,   RA.TotalCostOfGood AS COGSAfter
        FROM
            #Refunds AS RB
        INNER JOIN tbl_RefundGoods AS RA
            ON RB.ID = RA.ID
        ORDER BY
            RB.ID
        ;

        SELECT
            RFDB.ID
        ,   RFDB.SKU
        ,   RFDB.CostOfGood AS COGSBefore
        ,   RFDA.CostOfGood AS COGSAfter
        ,   IIF(RFDB.ProductType = 1, P.CostOfGood, PV.CostOfGood) AS COGS 
        FROM
            #RefundDetails AS RFDB
        INNER JOIN tbl_RefundGoodsDetails AS RFDA
            ON RFDB.ID = RFDA.ID
        LEFT JOIN #Product AS P
            ON RFDB.ProductType = 1
            AND RFDB.SKU = P.ProductSKU
        LEFT JOIN #ProductVariation AS PV
            ON RFDB.ProductType = 2
            AND RFDB.SKU = PV.SKU
        ORDER BY
            RFDB.OrderID
        ,   RFDB.ID DESC
        ;
    END
END