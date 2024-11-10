using PRG282Project.BusinessLogicLayer;
using System;
using System.Windows.Forms;

namespace PRG282Project
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            var studentService = new StudentService();
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
        

                if (studentService.AddStudent(student))
                {
                    MessageBox.Show("Student added successfully!", "Success");
                    ClearStudentFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearStudentFields()
        {
            studentID.Clear();
            Name.Clear();
            Age.Clear();
            Course.Clear();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            Application.ExitThread();
        }
    }
}