CREATE TABLE users (
    id          SERIAL      PRIMARY KEY,
    fullname    varchar     NOT NULL,
    active      BOOLEAN     NOT NULL DEFAULT FALSE
);

INSERT INTO users(fullname, active) VALUES('admin admin', true);