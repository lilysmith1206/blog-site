-- Start a transaction
START TRANSACTION;

DECLARE `_rollback` BOOL DEFAULT 0;
DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET `_rollback` = 1;

-- Step 1: Remove foreign key from posts to post_hierarchy
ALTER TABLE posts DROP FOREIGN KEY fk_parentId;

-- Step 2: Create temporary tables with INT keys
CREATE TABLE post_hierarchy_temp (
    categoryId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    parentId INT DEFAULT NULL,
    categoryName CHAR(80) DEFAULT NULL,
    slug CHAR(40) DEFAULT NULL,
    title CHAR(80) DEFAULT NULL,
    keywords CHAR(80) DEFAULT NULL,
    description CHAR(80) DEFAULT NULL,
    body VARCHAR(60000) DEFAULT NULL,
    use_date_created_for_sorting TINYINT(1) DEFAULT NULL
) ENGINE=InnoDB;

CREATE TABLE posts_temp (
    slug CHAR(40) NOT NULL PRIMARY KEY,
    title CHAR(80) DEFAULT NULL,
    parentId INT DEFAULT NULL,
    date_modified DATETIME DEFAULT CURRENT_TIMESTAMP,
    name CHAR(80) DEFAULT NULL,
    keywords CHAR(160) DEFAULT NULL,
    description CHAR(160) DEFAULT NULL,
    body VARCHAR(60000) DEFAULT NULL,
    date_created DATETIME DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- Step 3: Transfer base data into post_hierarchy_temp
INSERT INTO post_hierarchy_temp (slug, title, categoryName, keywords, description, body, use_date_created_for_sorting)
SELECT slug, title, name, keywords, description, body, use_date_created_for_sorting FROM post_hierarchy;

-- Step 4: Resolve parentId references in post_hierarchy_temp
UPDATE post_hierarchy_temp AS t
JOIN post_hierarchy AS o ON t.slug = o.slug
SET t.parentId = (
    SELECT temp.categoryId 
    FROM post_hierarchy_temp AS temp 
    WHERE temp.slug = (
        SELECT parent.slug
        FROM post_hierarchy AS parent
        WHERE parent.categoryId = o.parentId
    )
);

-- Step 5: Transfer data into posts_temp
INSERT INTO posts_temp (slug, title, date_modified, name, keywords, description, body, date_created)
SELECT slug, title, date_modified, name, keywords, description, body, date_created FROM posts;

-- Step 6: Resolve parentId references in posts_temp
UPDATE posts_temp AS t
JOIN posts AS o ON t.slug = o.slug
SET t.parentId = (
    SELECT temp.categoryId 
    FROM post_hierarchy_temp AS temp 
    WHERE temp.slug = (
        SELECT parent.slug
        FROM post_hierarchy AS parent
        WHERE parent.categoryId = o.parentId
    )
);

-- Step 7: Drop old tables
DROP TABLE posts;
DROP TABLE post_hierarchy;

-- Step 8: Rename temporary tables
RENAME TABLE post_hierarchy_temp TO post_hierarchy;
RENAME TABLE posts_temp TO posts;

-- Step 9: Add new foreign key constraints
ALTER TABLE post_hierarchy
    ADD CONSTRAINT fk_post_hierarchy_parent
    FOREIGN KEY (parentId) REFERENCES post_hierarchy (categoryId)
    ON UPDATE CASCADE ON DELETE SET NULL;

ALTER TABLE posts
    ADD CONSTRAINT fk_posts_parent
    FOREIGN KEY (parentId) REFERENCES post_hierarchy (categoryId)
    ON UPDATE CASCADE ON DELETE SET NULL;

IF `_rollback` THEN
    ROLLBACK;
ELSE
    COMMIT;
END IF;
