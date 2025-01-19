START TRANSACTION;

CREATE TABLE pages (
  slug CHAR(40) NOT NULL,
  title CHAR(80) NOT NULL,
  name CHAR(80) NOT NULL,
  keywords CHAR(160) NOT NULL,
  description CHAR(160) NOT NULL,
  body TEXT NOT NULL,
  PRIMARY KEY (slug)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

INSERT INTO pages (slug, title, name, keywords, description, body)
SELECT slug, title, name, keywords, description, body FROM posts;

INSERT INTO pages (slug, title, name, keywords, description, body)
SELECT slug, title, categoryName AS name, keywords, description, body FROM post_categories;

ALTER TABLE posts
	DROP COLUMN name,
	DROP COLUMN title;
	DROP COLUMN keywords,
	DROP COLUMN description,
	DROP COLUMN body;

ALTER TABLE post_categories
	DROP COLUMN categoryName,
	DROP COLUMN title;
	DROP COLUMN keywords,
	DROP COLUMN description,
	DROP COLUMN body;

ALTER TABLE posts
	ADD CONSTRAINT fk_posts_pages
	FOREIGN KEY (slug) REFERENCES pages (slug)
	ON DELETE CASCADE
	ON UPDATE CASCADE;

ALTER TABLE post_categories
	ADD CONSTRAINT fk_post_categories_pages
	FOREIGN KEY (slug) REFERENCES pages (slug)
	ON DELETE CASCADE
	ON UPDATE CASCADE;

INSERT INTO database_version (version, updated_on)
VALUES ('007', NOW());

COMMIT;
