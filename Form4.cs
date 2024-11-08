using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

using System.Windows.Forms;

namespace PRG282Project
{
    public partial class SummaryForm : Form
    {
        public SummaryForm()
        {
            InitializeComponent();
        }

        // Event handler for the Generate Report button
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            LoadSummary();  // Call LoadSummary method to generate the report
        }

        // LoadSummary method: Calls database and file methods to get summary data
        private void LoadSummary()
        {
            int totalStudents = 0;
            double averageAge = 0;

            // Fetch data from both database and text file if needed
            GetSummaryFromDatabase(ref totalStudents, ref averageAge);
            // Optionally: Uncomment below if you want to include data from the text file as well
            // GetSummaryFromTextFile(ref totalStudents, ref averageAge);

            // Display the results in the labels
            lblTotalStudents.Text = "Total Students: " + totalStudents;
            lblAverageAge.Text = "Average Age: " + averageAge.ToString("F2");
        }

        // GetSummaryFromDatabase method: Connects to the SQL database to retrieve student count and average age
        private void GetSummaryFromDatabase(ref int totalStudents, ref double averageAge)
        {
            string databaseConn = "Server=HANNO\\SQLEXPRESS;Initial Catalog=Students;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(databaseConn))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) AS TotalStudents, AVG(Age) AS AverageAge FROM StudentInfo";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalStudents = reader.GetInt32(0);  // Get total number of students
                                averageAge = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);  // Get average age
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading summary from database: " + ex.Message);
            }
        }

        // GetSummaryFromTextFile method: Reads from text file to calculate total students and average age
        private void GetSummaryFromTextFile(ref int totalStudents, ref double averageAge)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"students.txt");
            try
            {
                var lines = File.ReadAllLines(filePath);
                if (lines.Length == 0) return;

                totalStudents = lines.Length;
                double totalAge = lines.Select(line =>
                {
                    var fields = line.Split(',');
                    return int.TryParse(fields[2], out int age) ? age : 0;
                }).Sum();
                
                averageAge = totalStudents > 0 ? totalAge / totalStudents : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading summary from text file: " + ex.Message);
            }
        }
    }
}
