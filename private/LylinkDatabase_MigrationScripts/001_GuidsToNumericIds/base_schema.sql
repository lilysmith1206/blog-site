-- MariaDB dump 10.19  Distrib 10.6.15-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: lylinkdb
-- ------------------------------------------------------
-- Server version	10.6.15-MariaDB

--
-- Table structure for table annotations
--

DROP TABLE IF EXISTS annotations;
CREATE TABLE annotations (
  id varchar(40) NOT NULL,
  slug varchar(40) DEFAULT NULL,
  editor_name varchar(80) DEFAULT NULL,
  annotation_content varchar(10000) DEFAULT NULL,
  PRIMARY KEY (id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Table structure for table database_version
--
DROP TABLE IF EXISTS database_version;
CREATE TABLE database_version (
  version char(3) DEFAULT NULL,
  updated_on datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Table structure for table post_hierarchy
--

DROP TABLE IF EXISTS post_hierarchy;
CREATE TABLE post_hierarchy (
  categoryId char(40) NOT NULL,
  parentId char(40) DEFAULT NULL,
  name char(80) DEFAULT NULL,
  slug char(40) DEFAULT NULL,
  title char(80) DEFAULT NULL,
  keywords char(80) DEFAULT NULL,
  description char(80) DEFAULT NULL,
  body varchar(60000) DEFAULT NULL,
  use_date_created_for_sorting tinyint(1) DEFAULT NULL,
  PRIMARY KEY (categoryId)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Table structure for table posts
--

DROP TABLE IF EXISTS posts;
CREATE TABLE posts (
  slug char(40) NOT NULL,
  title char(80) DEFAULT NULL,
  parentId char(40) DEFAULT NULL,
  date_modified datetime DEFAULT current_timestamp(),
  name char(80) DEFAULT NULL,
  keywords char(160) DEFAULT NULL,
  description char(160) DEFAULT NULL,
  body varchar(60000) DEFAULT NULL,
  date_created datetime DEFAULT current_timestamp(),
  PRIMARY KEY (slug),
  CONSTRAINT fk_parentId FOREIGN KEY (parentId) REFERENCES post_hierarchy (categoryId)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;
