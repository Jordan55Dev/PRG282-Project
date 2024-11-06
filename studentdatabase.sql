CREATE TABLE StudentInfo (
    StudentId INT PRIMARY KEY,
    name VARCHAR(100),
    age INT,
    course VARCHAR(100)
);

-- Insert some sample data
INSERT INTO StudentInfo (StudentId, name, age, course)
VALUES (1, 'John Doe', 20, 'Computer Science');

INSERT INTO StudentInfo (StudentId, name, age, course)
VALUES (2, 'Jane Smith', 22, 'Electrical Engineering');

INSERT INTO StudentInfo (StudentId, name, age, course)
VALUES (3, 'Samuel Lee', 19, 'Mechanical Engineering');