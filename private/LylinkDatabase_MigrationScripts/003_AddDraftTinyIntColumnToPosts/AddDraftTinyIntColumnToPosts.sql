START TRANSACTION;

-- Add tinyint is_draft column to posts table
ALTER TABLE posts ADD COLUMN is_draft TINYINT(1) DEFAULT 0;
	
-- Insert new database version (001) into database
INSERT INTO database_version (version, updated_on)
VALUES ('003', NOW());

-- Commit if no errors
COMMIT;
