
using System; 
using System.Windows.Forms; 
using System.IO; 
using System.Collections.Generic; 

// Defining a public class Form5 that inherits from Form
public class Form5 : Form
{
    // Private fields for the DataGridView and Delete button
    private DataGridView dataGridView;
    private Button deleteButton;

    // Constant for the file path where student data is stored
    private const string FILE_PATH = "students.txt";

    // Constructor for Form5
    public Form5()
    {
        InitializeComponent(); // Initialize the form components
        LoadStudentData(); // Load student data from the file into the DataGridView
    }

    // Method to initialize form components
    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // Form5
            // 
            this.ClientSize = new System.Drawing.Size(629, 413);
            this.Name = "Form5";
            this.ResumeLayout(false);

    }

    // Method to load student data from a file into the DataGridView
    private void LoadStudentData()
    {
        dataGridView.Columns.Clear(); // Clear existing columns
        dataGridView.Rows.Clear(); // Clear existing rows

        // Adding columns to the DataGridView
        dataGridView.Columns.Add("ID", "ID");
        dataGridView.Columns.Add("Name", "Name");
        dataGridView.Columns.Add("Age", "Age");

        try
        {
            // Check if the file exists
            if (File.Exists(FILE_PATH))
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(FILE_PATH);
                foreach (string line in lines) // Iterate through each line
                {
                    // Split the line by comma to get individual parts
                    string[] parts = line.Split(',');
                    if (parts.Length == 3) // Ensure there are exactly 3 parts
                    {
                        // Add a new row to the DataGridView with the student data
                        dataGridView.Rows.Add(parts[0], parts[1], parts[2]);
                    }
                }
            }
        }
        catch (Exception ex) // Catch any exceptions that occur
        {
            // Display an error message if loading fails
            MessageBox.Show($"Error loading student data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    // Method to save student data to the file
    private void SaveStudentData()
    {
        try
        {
            List<string> lines = new List<string>(); // List to hold each line of student data
            foreach (DataGridViewRow row in dataGridView.Rows) // Iterate through each row in the DataGridView
            {
                // Construct a CSV line from the ID, Name, and Age columns
                string line = $"{row.Cells["ID"].Value},{row.Cells["Name"].Value},{row.Cells["Age"].Value}";
                lines.Add(line); // Add the constructed line to the list
            }
            // Write all lines to the specified file path
            File.WriteAllLines(FILE_PATH, lines);
        }
        catch (Exception ex) // Catch any exceptions that occur during the save process
        {
            // Display an error message if saving fails
            MessageBox.Show($"Error saving student data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // Event handler for the delete button click event
    private void DeleteButton_Click(object sender, EventArgs e)
    {
        // Check if any rows are selected in the DataGridView
        if (dataGridView.SelectedRows.Count > 0)
        {
            // Get the index of the first selected row
            int selectedIndex = dataGridView.SelectedRows[0].Index;
            // Retrieve the student ID from the selected row
            string studentId = dataGridView.Rows[selectedIndex].Cells["ID"].Value.ToString();

            // Prompt the user for confirmation before deleting
            DialogResult result = MessageBox.Show($"Are you sure you want to delete student with ID {studentId}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // If the user confirms the deletion
            if (result == DialogResult.Yes)
            {
                // Remove the selected row from the DataGridView
                dataGridView.Rows.RemoveAt(selectedIndex);
                SaveStudentData(); // Save the updated student data to the file
                                   // Show a success message
                MessageBox.Show($"Student with ID {studentId} has been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        else // If no rows are selected
        {
            // Show a warning message to prompt the user to select a student
            MessageBox.Show("Please select a student to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}