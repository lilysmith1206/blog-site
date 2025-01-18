START TRANSACTION;

ALTER TABLE annotations MODIFY slug VARCHAR(40) NOT NULL;
ALTER TABLE annotations MODIFY editor_name VARCHAR(80) NOT NULL;
ALTER TABLE annotations MODIFY annotation_content VARCHAR(10000) NOT NULL;

ALTER TABLE database_version MODIFY version CHAR(3) NOT NULL;
ALTER TABLE database_version MODIFY updated_on DATETIME NOT NULL;

ALTER TABLE post_categories MODIFY parentId INT NULL;
ALTER TABLE post_categories MODIFY categoryName CHAR(80) NOT NULL;
ALTER TABLE post_categories MODIFY slug CHAR(40) NOT NULL;
ALTER TABLE post_categories MODIFY title CHAR(80) NOT NULL;
ALTER TABLE post_categories MODIFY keywords CHAR(80) NOT NULL;
ALTER TABLE post_categories MODIFY description CHAR(80) NOT NULL;
ALTER TABLE post_categories MODIFY body VARCHAR(60000) NOT NULL;
UPDATE post_categories SET use_date_created_for_sorting = 0 WHERE use_date_created_for_sorting IS NULL;
ALTER TABLE post_categories MODIFY use_date_created_for_sorting TINYINT(1) NOT NULL;

ALTER TABLE posts MODIFY title CHAR(80) NOT NULL;
ALTER TABLE posts MODIFY date_modified DATETIME NOT NULL;
ALTER TABLE posts MODIFY name CHAR(80) NOT NULL;
ALTER TABLE posts MODIFY keywords CHAR(160) NOT NULL;
ALTER TABLE posts MODIFY description CHAR(160) NOT NULL;
ALTER TABLE posts MODIFY body VARCHAR(60000) NOT NULL;
ALTER TABLE posts MODIFY date_created DATETIME NOT NULL;
UPDATE posts SET is_draft = 1 WHERE is_draft IS NULL;
ALTER TABLE posts MODIFY is_draft TINYINT(1) NOT NULL;

ALTER TABLE visit_analytics MODIFY visitor_id CHAR(128) NOT NULL;
ALTER TABLE visit_analytics MODIFY slug_visited TEXT NOT NULL;
ALTER TABLE visit_analytics MODIFY slug_given CHAR(40) NOT NULL;
ALTER TABLE visit_analytics MODIFY visited_on DATETIME NOT NULL;

-- Insert new database version (005) into database
INSERT INTO database_version (version, updated_on)
VALUES ('005', NOW());

-- Commit if no errors
COMMIT;
