START TRANSACTION;

CREATE TABLE posts_temp (
    id INT(11) NOT NULL AUTO_INCREMENT,
    slug CHAR(40) NOT NULL,
    title CHAR(80) NOT NULL,
    parent_id INT DEFAULT NULL,
    date_modified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    name CHAR(80) NOT NULL,
    keywords CHAR(160) NOT NULL,
    description CHAR(160) NOT NULL,
    body VARCHAR(60000) NOT NULL,
    date_created DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    is_draft tinyint(1) NOT NULL,
    PRIMARY KEY (id),
    UNIQUE KEY (slug),
    KEY key_post_parent_category (parent_id),
    CONSTRAINT foreign_key_post_parent_category FOREIGN KEY (parent_id) REFERENCES post_categories (categoryId) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1;

INSERT INTO posts_temp (slug, title, parent_id, date_modified, name, keywords, description, body, date_created, is_draft)
SELECT slug, title, parentId, date_modified, name, keywords, description, body, date_created, is_draft FROM posts;

DROP TABLE posts;
RENAME TABLE posts_temp TO posts;

INSERT INTO database_version (version, updated_on)
VALUES ('006', NOW());

COMMIT;