using System;
using System.Windows.Forms;
using PRG282Project.BusinessLogicLayer;

namespace PRG282Project
{
    public partial class SummaryForm : Form
    {
        private SummaryService summaryService = new SummaryService();

        public SummaryForm()
        {
            InitializeComponent();
        }

        private void LoadSummary()
        {
            try
            {
                // Fetch data from the database
                var (totalStudents, averageAge) = summaryService.GetSummaryFromDatabase();

                // Display the results in the labels
                textBox1.Text = "Total Students: " + totalStudents;
                textBox2.Text = "Average Age: " + averageAge.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading summary: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadSummary();
        }

        private void SummaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
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
