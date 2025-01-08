START TRANSACTION;

-- Create new database_version table
CREATE TABLE IF NOT EXISTS database_version (
    version CHAR(3),
    updated_on DATETIME
);

-- Insert current database version (000) into database
INSERT INTO database_version (version, updated_on)
VALUES ('000', NOW());

COMMIT;
