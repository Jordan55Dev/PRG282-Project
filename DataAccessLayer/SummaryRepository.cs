using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace PRG282Project.DataAccessLayer
{
    public class SummaryRepository
    {
        private string databaseConn = "Server=LAPTOP-TCIGMIL3\\SQLEXPRESS;Initial Catalog=Students;Integrated Security=True";

        public (int totalStudents, double averageAge) GetSummaryFromDatabase()
        {
            int totalStudents = 0;
            double averageAge = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(databaseConn))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) AS TotalStudents, AVG(age) AS AverageAge FROM StudentInfo";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalStudents = reader["TotalStudents"] != DBNull.Value ? Convert.ToInt32(reader["TotalStudents"]) : 0;
                                averageAge = reader["AverageAge"] != DBNull.Value ? Convert.ToDouble(reader["AverageAge"]) : 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading summary from database: " + ex.Message);
            }

            return (totalStudents, averageAge);
        }

        public (int totalStudents, double averageAge) GetSummaryFromTextFile()
        {
            string filePath = "students.txt";
            int totalStudents = 0;
            double averageAge = 0;

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length == 0)
                {
                    return (0, 0);
                }

                totalStudents = lines.Length;
                double totalAge = 0;

                foreach (string line in lines)
                {
                    string[] fields = line.Split(',');

                    if (fields.Length > 2 && int.TryParse(fields[2], out int age))
                    {
                        totalAge += age;
                    }
                }

                averageAge = totalStudents > 0 ? totalAge / totalStudents : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading summary from text file: " + ex.Message);
            }

            return (totalStudents, averageAge);
        }

        public void SaveSummaryToFile(int totalStudents, double averageAge)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "summary.txt");

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Total Students: " + totalStudents);
                    writer.WriteLine("Average Age: " + averageAge.ToString("F2"));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving summary to file: " + ex.Message);
            }
        }

    }
}