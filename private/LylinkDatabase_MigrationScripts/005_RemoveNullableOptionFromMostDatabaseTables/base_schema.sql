CREATE TABLE annotations (
  id varchar(40) NOT NULL,
  slug varchar(40) DEFAULT NULL,
  editor_name varchar(80) DEFAULT NULL,
  annotation_content varchar(10000) DEFAULT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

CREATE TABLE database_version (
  version char(3) DEFAULT NULL,
  updated_on datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE post_categories (
  categoryId int(11) NOT NULL AUTO_INCREMENT,
  parentId int(11) DEFAULT NULL,
  categoryName char(80) DEFAULT NULL,
  slug char(40) DEFAULT NULL,
  title char(80) DEFAULT NULL,
  keywords char(80) DEFAULT NULL,
  description char(80) DEFAULT NULL,
  body varchar(60000) DEFAULT NULL,
  use_date_created_for_sorting tinyint(1) DEFAULT NULL,
  PRIMARY KEY (categoryId),
  KEY fk_post_hierarchy_parent (parentId),
  CONSTRAINT fk_post_hierarchy_parent FOREIGN KEY (parentId) REFERENCES post_categories (categoryId) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

CREATE TABLE posts (
  slug char(40) NOT NULL,
  title char(80) DEFAULT NULL,
  parentId int(11) DEFAULT NULL,
  date_modified datetime DEFAULT current_timestamp(),
  name char(80) DEFAULT NULL,
  keywords char(160) DEFAULT NULL,
  description char(160) DEFAULT NULL,
  body varchar(60000) DEFAULT NULL,
  date_created datetime DEFAULT current_timestamp(),
  is_draft tinyint(1) DEFAULT 0,
  PRIMARY KEY (slug),
  KEY fk_posts_parent (parentId),
  CONSTRAINT fk_posts_parent FOREIGN KEY (parentId) REFERENCES post_categories (categoryId) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

CREATE TABLE visit_analytics (
  id int(11) NOT NULL AUTO_INCREMENT,
  visitor_id char(128) DEFAULT NULL,
  slug_visited text DEFAULT NULL,
  slug_given char(40) DEFAULT NULL,
  visited_on datetime DEFAULT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB AUTO_INCREMENT=853 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
