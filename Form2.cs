using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (!ValidateStudentData()) return;
            string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\students.txt");
            var student = new Student
            {
                ID = studentID.Text,
                Name = Name.Text,
                Age = int.Parse(Age.Text),
                Course = Course.Text
            };
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, true))
                {
                    sw.WriteLine(student.ToString());
                }

                MessageBox.Show("Student added successfully!", "Success");
                ClearStudentFields();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ocurred while adding the student:" + ex.Message);
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
