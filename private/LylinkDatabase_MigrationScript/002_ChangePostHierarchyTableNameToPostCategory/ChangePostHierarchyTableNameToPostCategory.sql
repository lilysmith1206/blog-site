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

	-- Rename post_hierarchy table to post_category to better fit usage.
	RENAME TABLE post_hierarchy TO post_categories;
	
    -- Commit if no errors
    COMMIT;
    SELECT 'Transaction committed successfully.' AS message;
END;
//

DELIMITER ;

CALL SafeMigration();

DROP PROCEDURE SafeMigration;
