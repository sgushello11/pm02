CREATE TRIGGER trg_quality_approve_update
ON quality_control
AFTER UPDATE
AS
BEGIN
    UPDATE pb
    SET pb.status = 'completed'
    FROM production_batches pb
    INNER JOIN inserted i ON i.batch_id = pb.id
    WHERE i.decision = 'approved'
    AND pb.status = 'quality_pending';
END;
GO