CREATE PROCEDURE ClearStock
AS
BEGIN
    -- Get stock last
    SELECT
        MAX(ID) AS IDLAST
    INTO #stockLast
    FROM
        tbl_StockManager AS STM
    GROUP BY
        STM.SKU
    ;

    -- Remove stock which is not last
    DELETE tbl_StockManager
    WHERE NOT Exists (
        SELECT
            NULL AS DUMMY
        FROM
            #stockLast as STL
        WHERE
            ID = IDLAST
    );

    -- Update quantity current >= 0
    BEGIN
        DECLARE @ID INT, @Type INT, @Quantity FLOAT, @QuantityCurrent FLOAT;
        DECLARE stock_cursor CURSOR FOR
            SELECT
                STM.ID
            ,   ISNULL(STM.Type, 1) AS Type
            ,   ISNULL(STM.Quantity, 0) AS Quantity
            ,   ISNULL(STM.QuantityCurrent, 0) AS QuantityCurrent
            FROM
                tbl_StockManager AS STM
            INNER JOIN #stockLast as STL
            ON STM.ID = STL.IDLAST
        ;

        OPEN stock_cursor;

        FETCH NEXT FROM stock_cursor
        INTO @ID, @Type, @Quantity, @QuantityCurrent;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            IF @Type = 1
            BEGIN
                SET @QuantityCurrent = @QuantityCurrent + @Quantity;
            END
            ELSE
            BEGIN
                SET @QuantityCurrent = @QuantityCurrent - @Quantity;
            END

            IF @QuantityCurrent < 0
            BEGIN
                UPDATE tbl_StockManager
                SET
                    QuantityCurrent = @Quantity
                ,   Status = 21
                ,   NoteID = N'Loại bỏ số lượng âm'
                ,   ModifiedDate = GETDATE()
                ,   ModifiedBy = N'admin'
                WHERE ID = @ID
                ;
            END

            FETCH NEXT FROM stock_cursor
            INTO @ID, @Type, @Quantity, @QuantityCurrent;
        END

        CLOSE stock_cursor;
        DEALLOCATE stock_cursor;
    END
END
