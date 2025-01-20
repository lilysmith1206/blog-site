START TRANSACTION;

CREATE TABLE post_sorting_methods (
  id int(11) NOT NULL AUTO_INCREMENT,
  sorting_name CHAR(80) NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

INSERT INTO post_sorting_methods (id, sorting_name)
VALUES
	(1, 'ByDateCreatedAscending'),
	(2, 'ByDateCreatedDescending'),
	(3, 'ByDateModifiedAscending'),
	(4, 'ByDateModifiedDescending');

ALTER TABLE post_categories ADD COLUMN post_sorting_method_id INT(11);
	
ALTER TABLE post_categories
	ADD CONSTRAINT fk_category_post_sorting_method
	FOREIGN KEY (post_sorting_method_id) REFERENCES post_sorting_methods (id)
	ON DELETE CASCADE
	ON UPDATE CASCADE;
	
UPDATE post_categories SET post_sorting_method_id = 2 WHERE use_date_created_for_sorting = 1;
UPDATE post_categories SET post_sorting_method_id = 1 WHERE use_date_created_for_sorting = 0;

ALTER TABLE post_categories DROP COLUMN use_date_created_for_sorting;

INSERT INTO database_version (version, updated_on)
VALUES ('008', NOW());

COMMIT;
