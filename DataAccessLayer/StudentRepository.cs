using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace PRG282Project.DataLayer
{

    public class StudentRepository
    {
        private string databaseConn = "Server=LAPTOP-TCIGMIL3\\SQLEXPRESS;Initial Catalog=Students;Integrated Security=True";
        private string filePath = "students.txt";

        public void AddStudentToDatabase(Student student)
        {
            using (SqlConnection conn = new SqlConnection(databaseConn))
            {
                conn.Open();
                string query = @"INSERT INTO StudentInfo(StudentId, Name, Age, Course) VALUES (@studentID, @name, @age, @course)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@studentID", SqlDbType.VarChar).Value = student.ID;
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = student.Name;
                    cmd.Parameters.Add("@age", SqlDbType.Int).Value = student.Age;
                    cmd.Parameters.Add("@course", SqlDbType.VarChar).Value = student.Course;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddStudentToFile(Student student)
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(student.ToString());
            }
        }

        public DataTable LoadStudents()
        {
            string query = @"SELECT * FROM StudentInfo";
            using (SqlConnection connection = new SqlConnection(databaseConn))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        public void UpdateStudent(Student student)
        {
            using (SqlConnection conn = new SqlConnection(databaseConn))
            {
                conn.Open();
                string query = @"UPDATE StudentInfo SET Name = @name, Age = @age, Course = @course WHERE StudentId = @studentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@studentID", student.ID);
                    cmd.Parameters.AddWithValue("@name", student.Name);
                    cmd.Parameters.AddWithValue("@age", student.Age);
                    cmd.Parameters.AddWithValue("@course", student.Course);
                    cmd.ExecuteNonQuery();
                }
            }

            UpdateTextFile(student);
        }

        private void UpdateTextFile(Student student)
        {
            // Read existing file data
            var lines = File.ReadAllLines(filePath).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                var fields = lines[i].Split(',');
                if (fields[0] == student.ID)
                {
                    lines[i] = $"{student.ID},{student.Name},{student.Age},{student.Course}";
                    break;
                }
            }
            // Write updated data back to file
            File.WriteAllLines(filePath, lines);
        }

        public bool DeleteStudent(string studentId)
        {
            // Delete from the database
            using (SqlConnection conn = new SqlConnection(databaseConn))
            {
                conn.Open();
                string query = @"DELETE FROM StudentInfo WHERE StudentId = @studentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@studentID", studentId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // If a row was deleted from the database, delete from the text file
                        DeleteStudentFromFile(studentId);
                        return true; // Return true if a row was deleted
                    }
                    return false; // Return false if no row was deleted
                }
            }
        }

        private void DeleteStudentFromFile(string studentId)
        {
            // Read all lines from the file
            var lines = File.ReadAllLines(filePath).ToList();

            // Remove the line that matches the student ID
            lines.RemoveAll(line => line.StartsWith(studentId + ","));

            // Write the remaining lines back to the file
            File.WriteAllLines(filePath, lines);
        }
    }
}
