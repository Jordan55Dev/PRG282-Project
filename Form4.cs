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
            AddFooter(); // Call method to add footer
        }

        // Method to add footer to the form
        private void AddFooter()
        {
            // Create a panel for the footer
            Panel footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,  // Adjust the height as per your needs
                BackColor = Color.LightGray  // Change the color if needed
            };

            // Add footer text (you can customize the text or add other controls)
            Label footerLabel = new Label
            {
                Text = "Student Management System - © 2024 All rights reserved",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 10, FontStyle.Regular)
            };

            // Add the label to the footer panel
            footerPanel.Controls.Add(footerLabel);

            // Add the footer panel to the form
            this.Controls.Add(footerPanel);
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
            textBox1.Text = "Total Students: " + totalStudents;
            textBox2.Text = "Average Age: " + averageAge.ToString("F2");
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

        private void button1_Click(object sender, EventArgs e)
        {
            LoadSummary();
        }
    }
}
