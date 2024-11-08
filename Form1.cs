using System;
using System.Drawing;
using System.Windows.Forms;

namespace PRG282Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Navigation Bar
            var navBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(60, 60, 60)
            };
            this.Controls.Add(navBar);

            var navFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10, 0, 0, 0)
            };
            navBar.Controls.Add(navFlowPanel);

            var btnAdd = CreateNavButton("Add Student", button1_Click);
            var btnFind = CreateNavButton("Find Student", button2_Click);
            var btnSummary = CreateNavButton("Summary", (s, e) => MessageBox.Show("Summary Report clicked!"));

            navFlowPanel.Controls.Add(btnAdd);
            navFlowPanel.Controls.Add(btnFind);
            navFlowPanel.Controls.Add(btnSummary);

            // Title Banner
            var titleBanner = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(45, 85, 135)
            };
            this.Controls.Add(titleBanner);

            var lblTitle = new Label
            {
                Text = "Student Registration Form",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            titleBanner.Controls.Add(lblTitle);

            // Footer
            var footerPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(220, 220, 220)
            };
            this.Controls.Add(footerPanel);

            var lblFooter = new Label
            {
                Text = "© 2024 Your Company Name. All Rights Reserved.",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9),
                TextAlign = ContentAlignment.MiddleCenter
            };
            footerPanel.Controls.Add(lblFooter);
        }

        private Button CreateNavButton(string text, EventHandler onClick)
        {
            var button = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                Height = 40,
                Width = 100,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                Margin = new Padding(5)
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += onClick;
            return button;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Hide();
        }
    }
}
