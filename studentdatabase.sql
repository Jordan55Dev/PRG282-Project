-- Create the students database
CREATE DATABASE Students;
GO

-- Switch to the students database
USE Students;
GO

-- Create the StudentInfo table within the students database
CREATE TABLE StudentInfo (
    StudentId INT PRIMARY KEY,
    name VARCHAR(100),
    age INT,
    course VARCHAR(100)
);

