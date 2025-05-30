CREATE TABLE annotations (
  id varchar(40) NOT NULL,
  slug varchar(40) NOT NULL,
  editor_name varchar(80) NOT NULL,
  annotation_content varchar(10000) NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

CREATE TABLE database_version (
  version char(3) NOT NULL,
  updated_on datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE pages (
  slug char(40) NOT NULL,
  title char(80) NOT NULL,
  name char(80) NOT NULL,
  keywords char(160) NOT NULL,
  description char(160) NOT NULL,
  body text NOT NULL,
  PRIMARY KEY (slug)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

CREATE TABLE post_categories (
  categoryId int(11) NOT NULL AUTO_INCREMENT,
  parentId int(11) DEFAULT NULL,
  slug char(40) NOT NULL,
  use_date_created_for_sorting tinyint(1) NOT NULL,
  PRIMARY KEY (categoryId),
  KEY fk_post_hierarchy_parent (parentId),
  KEY fk_post_categories_pages (slug),
  CONSTRAINT fk_post_categories_pages FOREIGN KEY (slug) REFERENCES pages (slug) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT fk_post_hierarchy_parent FOREIGN KEY (parentId) REFERENCES post_categories (categoryId) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

CREATE TABLE posts (
  id int(11) NOT NULL AUTO_INCREMENT,
  slug char(40) NOT NULL,
  parent_id int(11) DEFAULT NULL,
  date_modified datetime NOT NULL DEFAULT current_timestamp(),
  date_created datetime NOT NULL DEFAULT current_timestamp(),
  is_draft tinyint(1) NOT NULL,
  PRIMARY KEY (id),
  UNIQUE KEY slug (slug),
  KEY key_post_parent_category (parent_id),
  CONSTRAINT fk_posts_pages FOREIGN KEY (slug) REFERENCES pages (slug) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT foreign_key_post_parent_category FOREIGN KEY (parent_id) REFERENCES post_categories (categoryId) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

CREATE TABLE visit_analytics (
  id int(11) NOT NULL AUTO_INCREMENT,
  visitor_id char(128) NOT NULL,
  slug_visited text NOT NULL,
  slug_given char(40) NOT NULL,
  visited_on datetime NOT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB AUTO_INCREMENT=853 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
