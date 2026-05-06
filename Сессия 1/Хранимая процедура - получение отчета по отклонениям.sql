CREATE PROCEDURE sp_GetDeviationsReport
    @start_date DATE,
    @end_date DATE
AS
BEGIN
    SELECT 
        b.batch_number,
        ps.step_name,
        ps.planned_temp_c,
        ps.actual_temp_c,
        ps.planned_duration_min,
        ps.actual_duration_min,
        ps.planned_pressure_bar,
        ps.actual_pressure_bar,
        ps.deviation_flag,
        ps.operator_comment,
        ps.started_at
    FROM production_steps ps
    INNER JOIN production_batches b ON b.id = ps.batch_id
    WHERE ps.deviation_flag = 1
    AND CAST(ps.started_at AS DATE) BETWEEN @start_date AND @end_date
    ORDER BY ps.started_at DESC;
END;
GO

-- Пример вызова
EXEC sp_GetDeviationsReport '2025-03-01', '2025-03-31';