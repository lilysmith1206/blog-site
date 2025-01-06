DELIMITER //

CREATE PROCEDURE SafeMigration()
BEGIN
    DECLARE exit HANDLER FOR SQLEXCEPTION
    BEGIN
        -- Rollback on any SQL error
		GET DIAGNOSTICS CONDITION 1 @err_msg = MESSAGE_TEXT;
		ROLLBACK;
		SELECT @err_msg AS "error message:";
    END;

    START TRANSACTION;

	-- Add tinyint is_draft column to posts table
	ALTER TABLE posts ADD COLUMN is_draft TINYINT(1) DEFAULT 0;
	
    -- Commit if no errors
    COMMIT;
    SELECT 'Transaction committed successfully.' AS message;
END;
//

DELIMITER ;

CALL SafeMigration();

DROP PROCEDURE SafeMigration;