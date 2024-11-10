using PRG282Project.DataLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PRG282Project.BusinessLogicLayer
{

    public class StudentService
    {
        private StudentRepository studentRepository = new StudentRepository();

        public bool AddStudent(Student student)
        {
            if (!ValidateStudentData(student))
                return false;

            try
            {
                // Add student to the database
                studentRepository.AddStudentToDatabase(student);

                // Add student to the text file
                studentRepository.AddStudentToFile(student);

                return true; // Indicate success
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                throw new Exception("Could not add the student: " + ex.Message);
            }
        }

        public DataTable LoadStudents()
        {
            return studentRepository.LoadStudents();
        }

        public Student SearchStudentById(string studentId)
        {
            var students = LoadStudents();
            foreach (DataRow row in students.Rows)
            {
                if (row["StudentId"].ToString().Equals(studentId, StringComparison.OrdinalIgnoreCase))
                {
                    return new Student
                    {
                        ID = row["StudentId"].ToString(),
                        Name = row["Name"].ToString(),
                        Age = int.Parse(row["Age"].ToString()),
                        Course = row["Course"].ToString()
                    };
                }
            }
            return null; // Not found
        }

        public bool UpdateStudent(Student student)
        {
            if (!ValidateStudentData(student))
                throw new Exception("Validation failed.");

            studentRepository.UpdateStudent(student);
            return true;
        }

        public bool DeleteStudent(string studentId)
        {
            return studentRepository.DeleteStudent(studentId);
        }

        private bool ValidateStudentData(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.ID) ||
                string.IsNullOrWhiteSpace(student.Name) ||
                string.IsNullOrWhiteSpace(student.Course))
            {
                throw new Exception("Please fill in all the fields!");
            }

            if (student.Age < 18)
            {
                throw new Exception("Age must be 18 or older!");
            }

            return true;
        }
    }
}