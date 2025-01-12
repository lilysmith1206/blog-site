START TRANSACTION;

-- Create new database_version table
CREATE TABLE IF NOT EXISTS visit_analytics (
    id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    visitor_id CHAR(128),
    slug_visited CHAR(40),
    slug_given CHAR(40),
    visited_on DATETIME
);

-- Insert new database version (001) into database
INSERT INTO database_version (version, updated_on)
VALUES ('004', NOW());

-- Commit if no errors
COMMIT;
