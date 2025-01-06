-- Start a transaction
START TRANSACTION;

-- Rename post_hierarchy table to post_category to better fit usage.
RENAME TABLE post_hierarchy TO post_categories;

-- Commit transaction
COMMIT;
