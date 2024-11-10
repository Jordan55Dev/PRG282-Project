using PRG282Project.BusinessLogicLayer;
using System;
using System.Data;
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

        private void button1_Click(object sender, EventArgs e)
        {
            string searchId = studentID.Text.Trim(); // Get the student ID from the input field

            // Check if the input is empty
            if (string.IsNullOrWhiteSpace(searchId))
            {
                MessageBox.Show("Please enter a Student ID to search.", "Input Error");
                return; // Exit the method if input is invalid
            }

            try
            {
                // Use the StudentService to search for the student by ID
                Student foundStudent = studentService.SearchStudentById(searchId);

                if (foundStudent != null)
                {
                    // If found, fill the input fields with the relevant data
                    studentID.Text = foundStudent.ID;
                    Name.Text = foundStudent.Name;
                    Age.Text = foundStudent.Age.ToString();
                    Course.Text = foundStudent.Course;
                    dataGridView1.ClearSelection(); // Clear any existing selections
                    dataGridView1.CurrentCell = dataGridView1.Rows.Cast<DataGridViewRow>()
                        .FirstOrDefault(row => row.Cells["StudentId"].Value.ToString() == foundStudent.ID)?.Cells[0]; // Set the current cell to the found row
                }
                else
                {
                    // If not found, show a message
                    MessageBox.Show("Student ID not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching for student: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var student = new Student
            {
                ID = studentID.Text,
                Name = Name.Text,
                Course = Course.Text
            };

            // Validate student data before parsing Age
            try
            {
                // Check if Age is provided and is a valid integer
                if (string.IsNullOrWhiteSpace(Age.Text) || !int.TryParse(Age.Text, out int age))
                {
                    throw new Exception("Please enter a valid age!");
                }

                student.Age = age; // Assign the parsed age to the student object


                if (studentService.UpdateStudent(student))
                {
                    MessageBox.Show("Student updated successfully!", "Success");
                    ClearStudentFields();
                    LoadData(); // Reload data to show the updated information
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

            try
            {
                // Use the StudentService to delete the student
                if (studentService.DeleteStudent(studentIdToDelete))
                {
                    MessageBox.Show("Student deleted successfully!", "Success");
                    ClearStudentFields(); // Clear the input fields
                }
                else
                {
                    MessageBox.Show("Student ID not found. Deletion failed.", "Deletion Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not delete the student record: " + ex.Message);
            }

            // Reload data to reflect the deletion
            LoadData();
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

    }
}