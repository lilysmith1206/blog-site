START TRANSACTION;

-- Rename post_hierarchy table to post_category to better fit usage.
RENAME TABLE post_hierarchy TO post_categories;

-- Insert new database version (002) into database
INSERT INTO database_version (version, updated_on)
VALUES ('002', NOW());
	
-- Commit if no errors
COMMIT;
