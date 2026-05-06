CREATE PROCEDURE sp_GetBatchReport
    @start_date DATE,
    @end_date DATE
AS
BEGIN
    SELECT 
        b.batch_number,
        p.name AS product_name,
        b.start_time,
        b.end_time,
        b.status,
        b.actual_quantity_kg,
        qc.decision AS quality_decision
    FROM production_batches b
    LEFT JOIN production_orders o ON o.id = b.order_id
    LEFT JOIN recipes r ON r.id = o.recipe_id
    LEFT JOIN products p ON p.id = r.product_id
    LEFT JOIN quality_control qc ON qc.batch_id = b.id AND qc.sample_type = 'готовая продукция'
    WHERE CAST(b.start_time AS DATE) BETWEEN @start_date AND @end_date
    ORDER BY b.start_time DESC;
END;
GO

-- Пример вызова
EXEC sp_GetBatchReport '2025-03-01', '2025-03-31';