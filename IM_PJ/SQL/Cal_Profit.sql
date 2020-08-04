DECLARE @FromDate AS DATETIME,
    @ToDate AS DATETIME, 
    @CustomerID AS INT;

SET @FromDate = CAST('2020-06-01' AS DATETIME);
SET @ToDate = CAST('2020-06-30' AS DATETIME);
--SET @CustomerID = 15486;
SET @CustomerID = NULL;

WITH #Product AS (
    SELECT
        P.ID
    ,   P.ProductSKU AS SKU
    ,   P.CostOfGood
    FROM
        tbl_Product AS P
),
#Variation AS (
    SELECT
        V.ID
    ,   V.SKU
    ,   V.CostOfGood
    FROM
        tbl_ProductVariable AS V
),
#Order AS (
    SELECT
        O.ID AS OrderID
    ,   SUM(ISNULL(OD.Quantity, 0)) AS Quantity
    ,   CAST(ISNULL(O.TotalPriceNotDiscount, '0') AS Money) AS TotalPrice
    ,   CAST(ISNULL(O.TotalDiscount, 0) AS Money) AS TotalDiscount
    ,   CAST(ISNULL(O.FeeShipping, '0') AS Money) AS TotalShippingFee
    ,   CAST(ISNULL(O.OtherFeeValue, 0) AS Money) AS TotalOtherFee
    ,   CAST(ISNULL(O.CouponValue, 0) AS Money) AS TotalCoupone
    ,   SUM(ISNULL(OD.Quantity, 0) * IIF(OD.ProductType = 1, ISNULL(P.CostOfGood, 0), ISNULL(V.CostOfGood, 0))) AS TotalCostOfGood
    ,   O.RefundsGoodsID
    FROM
        tbl_Order AS O
    INNER JOIN tbl_OrderDetail AS OD
        ON O.ID = OD.OrderID
    LEFT JOIN #Product AS P
        ON OD.ProductType = 1
        AND OD.ProductID = P.ID
    LEFT JOIN #Variation AS V
        ON OD.ProductType = 2
        AND OD.ProductVariableID = V.ID
    WHERE
        O.ExcuteStatus = 2
        AND O.PaymentStatus != 1
        AND O.DateDone IS NOT NULL
        AND CONVERT(NVARCHAR(10), O.DateDone, 121) BETWEEN CONVERT(NVARCHAR(10), @FromDate, 121) AND CONVERT(NVARCHAR(10), @ToDate, 121)
        AND (@CustomerID IS NULL OR (@CustomerID IS NOT NULL AND O.CustomerID = @CustomerID))
    GROUP BY
        O.ID
    ,   O.TotalPriceNotDiscount
    ,   O.TotalDiscount
    ,   O.FeeShipping
    ,   O.OtherFeeValue
    ,   O.CouponValue
    ,   O.RefundsGoodsID
),
#Refund AS (
    SELECT
        O.OrderID
    ,   R.ID AS RefundID
    ,   SUM(ISNULL(RD.Quantity, 0)) AS TotalQuantity
    ,   CAST(ISNULL(R.TotalPrice, '0') AS Money) AS TotalPrice
    ,   CAST(ISNULL(R.TotalRefundFee, '0') AS Money) AS TotalRefundFee
    ,   SUM(ISNULL(RD.Quantity, 0) * IIF(RD.ProductType = 1, ISNULL(P.CostOfGood, 0), ISNULL(V.CostOfGood, 0))) AS TotalCostOfGood
    FROM
        tbl_RefundGoods AS R
    INNER JOIN #Order AS O
        ON O.RefundsGoodsID IS NOT NULL
        AND R.ID = O.RefundsGoodsID
    INNER JOIN tbl_RefundGoodsDetails AS RD
        ON R.ID = RD.RefundGoodsID
    LEFT JOIN #Product AS P
        ON RD.ProductType = 1
        AND RD.SKU = P.SKU
    LEFT JOIN #Variation AS V
        ON RD.ProductType = 2
        AND RD.SKU = V.SKU
    GROUP BY
        O.OrderID
    ,   R.ID
    ,   R.TotalPrice
    ,   R.TotalRefundFee
)
SELECT
    O.OrderID
,   O.Quantity AS SaleQuantity
,   O.TotalPrice AS SalePrice
,   O.TotalDiscount AS SaleDiscount
,   O.TotalShippingFee AS SaleShippingFee
,   O.TotalOtherFee AS SaleOtherFee
,   O.TotalCoupone AS SaleCoupone
,   O.TotalCostOfGood AS SaleCOGS
,   (O.TotalPrice - O.TotalCostOfGood) AS SaleProfit 
,   R.RefundID
,   ISNULL(R.TotalQuantity, 0) AS RefundQuantity
,   ISNULL(R.TotalPrice, 0) AS RefundPrice
,   ISNULL(R.TotalRefundFee, 0) AS RefundFee
,   ISNULL(R.TotalCostOfGood, 0) AS RefundCOGS
,   (ISNULL(R.TotalPrice, 0) - ISNULL(R.TotalCostOfGood, 0)) AS RefundProfit 
,   (O.TotalPrice - O.TotalCostOfGood) - O.TotalDiscount - O.TotalCoupone - (ISNULL(R.TotalPrice, 0) - ISNULL(R.TotalCostOfGood, 0)) AS Profit
FROM
    #Order AS O
LEFT JOIN #Refund AS R
    ON O.OrderID = R.OrderID
ORDER BY
    O.OrderID DESC