using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRG282Project
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            if (!ValidateStudentData()) return;

            var student = new Student
            {
                ID = studentID.Text,
                Name = Name.Text,
                Age = int.Parse(Age.Text),
                Course = Course.Text
            };

            string databaseConn = "Server=HANNO\\SQLEXPRESS;Initial Catalog=Students;Integrated Security=True";
            
            try
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

                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Student updated successfully!", "Success");
                            ClearStudentFields();
                        }
                        else
                        {
                            MessageBox.Show("Student ID not found. Update failed.", "Update Error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update the database: " + ex.Message);
            }

            // Optional: Update text file if needed
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\students.txt");
            try
            {
                // Read existing file data
                var lines = File.ReadAllLines(filePath).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    // Assuming each line has the format "ID,Name,Age,Course"
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
            catch (Exception ex)
            {
                MessageBox.Show("Error updating the text file: " + ex.Message);
            }
        }

        private bool ValidateStudentData()
        {
            if (string.IsNullOrWhiteSpace(studentID.Text) ||
                string.IsNullOrWhiteSpace(Name.Text) ||
                string.IsNullOrWhiteSpace(Age.Text) ||
                string.IsNullOrWhiteSpace(Course.Text))
            {
                MessageBox.Show("Please fill in all the fields!");
                return false;
            }
            if (!int.TryParse(Age.Text, out int age))
            {
                MessageBox.Show("Please enter a valid number for age!", "Validation Error");
                this.Age.Clear();
                return false;
            }
            if (age < 18)
            {
                MessageBox.Show("Age must be 18 or older!", "Validation Error");
                Age.Clear();
                return false;
            }
            return true;
        }

        private void ClearStudentFields()
        {
            studentID.Clear();
            Name.Clear();
            Age.Clear();
            Course.Clear();
        }
    }
}
