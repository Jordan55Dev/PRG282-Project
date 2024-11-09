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

namespace PRG282Project
{
    public partial class SummaryForm : Form
    {
        public SummaryForm()
        {
            InitializeComponent();
           
        }

        
        

        private void LoadSummary()
        {
            int totalStudents = 0;
            double averageAge = 0;

            // Fetch data from the database
            GetSummaryFromDatabase(ref totalStudents, ref averageAge);

            // Display the results in the labels
            textBox1.Text = "Total Students: " + totalStudents;
            textBox2.Text = "Average Age: " + averageAge.ToString("F2");
        }

        private void GetSummaryFromDatabase(ref int totalStudents, ref double averageAge)
        {
            string databaseConn = "Server=HANNO\\SQLEXPRESS;Initial Catalog=Students;Integrated Security=True";

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
                                // Read the total number of students, handling possible DBNull values
                                totalStudents = reader["TotalStudents"] != DBNull.Value ? Convert.ToInt32(reader["TotalStudents"]) : 0;
                                // Read the average age, handling possible DBNull values
                                averageAge = reader["AverageAge"] != DBNull.Value ? Convert.ToDouble(reader["AverageAge"]) : 0;
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

        // Simplified GetSummaryFromTextFile method (move inside the class)
        private void GetSummaryFromTextFile(ref int totalStudents, ref double averageAge)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "students.txt");

            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // If no lines, exit early
                if (lines.Length == 0)
                {
                    return;
                }

                totalStudents = lines.Length;
                double totalAge = 0;

                foreach (string line in lines)
                {
                    string[] fields = line.Split(',');

                    // Parse age from the 3rd field, assuming the format is correct
                    if (fields.Length > 2 && int.TryParse(fields[2], out int age))
                    {
                        totalAge += age;
                    }
                }

                averageAge = totalStudents > 0 ? totalAge / totalStudents : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading summary from text file: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadSummary();
        }

        private void SummaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Use Application.Exit() to exit the application
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
