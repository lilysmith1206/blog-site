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

    -- Create new database_version table
    CREATE TABLE database_version (
      database_version CHAR(3),
      updated_on DATETIME
    );

    -- Insert current database version (004) into database
    INSERT INTO database_version (database_version, updated_on)
    VALUES ('004', NOW());
	
    -- Commit if no errors
    COMMIT;
    SELECT 'Transaction committed successfully.' AS message;
END;
//

DELIMITER ;

CALL SafeMigration();

DROP PROCEDURE SafeMigration;