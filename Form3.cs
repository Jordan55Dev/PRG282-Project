using PRG282Project.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PRG282Project
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.CellClick += dataGridView1_CellClick; // Attach CellClick event
        }

        private StudentService studentService = new StudentService();

        private void Form3_Load(object sender, EventArgs e)
        {
            LoadData(); // Load the data when the form loads
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                studentID.Text = row.Cells["StudentId"].Value.ToString();
                Name.Text = row.Cells["Name"].Value.ToString();
                Age.Text = row.Cells["Age"].Value.ToString();
                Course.Text = row.Cells["Course"].Value.ToString();
            }
        }

        private void ClearStudentFields()
        {
            studentID.Clear();
            Name.Clear();
            Age.Clear();
            Course.Clear();
        }

        private void LoadData()
        {
            try
            {
                dataGridView1.DataSource = studentService.LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var student = new Student
            {
                ID = studentID.Text,
                Name = Name.Text,
                Age = int.Parse(Age.Text),
                Course = Course.Text
            };

            try
            {
                studentService.UpdateStudent(student);
                MessageBox.Show("Student updated successfully!", "Success");
                ClearStudentFields();
                LoadData(); // Reload data to show the updated information
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update the student: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchId = studentID.Text.Trim(); // Get the student ID from the input field
            bool found = false; // Flag to check if the student is found

            // Check if the input is empty
            if (string.IsNullOrWhiteSpace(searchId))
            {
                MessageBox.Show("Please enter a Student ID to search.", "Input Error");
                return; // Exit the method if input is invalid
            }

            // Loop through the DataGridView rows to find the matching Student ID
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Check if the cell exists before accessing its value
                if (row.Cells["StudentId"] != null && row.Cells["StudentId"].Value != null)
                {
                    if (row.Cells["StudentId"].Value.ToString().Equals(searchId, StringComparison.OrdinalIgnoreCase))
                    {
                        // If found, select the row and scroll to it
                        dataGridView1.ClearSelection(); // Clear any existing selections
                        row.Selected = true; // Select the found row
                        dataGridView1.CurrentCell = row.Cells[0]; // Set the current cell to the first cell of the found row
                                                                  // Fill the input fields with the relevant data from the selected row
                        studentID.Text = row.Cells["StudentId"].Value.ToString();
                        Name.Text = row.Cells["Name"].Value.ToString();
                        Age.Text = row.Cells["Age"].Value.ToString();
                        Course.Text = row.Cells["Course"].Value.ToString();
                        found = true; // Set found flag to true
                        break; // Exit the loop once found
                    }
                }
            }

            // If not found, show a message
            if (!found)
            {
                // Display a message to the user, indicating that the student ID was not found.
                MessageBox.Show("Student ID not found.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SummaryForm form4 = new SummaryForm();
            form4.Show();
            this.Hide();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Check if a student ID is selected
            if (string.IsNullOrWhiteSpace(studentID.Text))
            {
                MessageBox.Show("Please select a student to delete.", "Selection Error");
                return;
            }

            string studentIdToDelete = studentID.Text;
            string databaseConn = "Server=HANNO\\SQLEXPRESS;Initial Catalog=Students;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(databaseConn))
                {
                    conn.Open();
                    string query = @"DELETE FROM StudentInfo WHERE StudentId = @studentID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentID", studentIdToDelete);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student deleted successfully!", "Success");
                            ClearStudentFields(); // Clear the input fields
                        }
                        else
                        {
                            MessageBox.Show("Student ID not found. Deletion failed.", "Deletion Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not delete the student record: " + ex.Message);
            }

            // Reload data to reflect the deletion
            LoadData();
        }
    }
}
