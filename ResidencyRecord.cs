using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Final_thesis
{
    public partial class ResidencyRecord : Form
    {
        public string Username { get; set; } // Public property for username
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=finalthesis.accdb";
        private static staffHistory instance; // Singleton instance of staffHistory
        public static staffHistory GetInstance(string loggedInUsername)
        {
            // Ensure only one instance of staffHistory exists
            if (instance == null || instance.IsDisposed)
            {
                instance = new staffHistory(loggedInUsername);
            }
            return instance;
        }
        public ResidencyRecord(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;  // Set the Username property

            dgvResidency.Columns.Clear();
            dgvResidency.Columns.Add("resident_id", "Resident ID");
            dgvResidency.Columns.Add("name", "Name");
            dgvResidency.Columns.Add("age", "Age");
            dgvResidency.Columns.Add("address", "Address");
            dgvResidency.Columns.Add("date_issued", "Date Issued");  // Add the Date Issued column
            dgvResidency.Columns.Add("record_count", "Record");  // Add the Record Count column
        }
        public void LoadClearanceRecords()
        {
            // SQL query to get all records from the ClearanceRecord table, grouped by resident_id and showing the latest date_issued
            string query = @"
        SELECT c.resident_id, c.fname, c.lname, c.m_initial, c.age, c.address, 
               COUNT(c.resident_id) AS record_count, 
               MAX(d.date_issued) AS date_issued
        FROM ResidencyRecord c
        INNER JOIN ResidencyRecord d ON c.resident_id = d.resident_id
        GROUP BY c.resident_id, c.fname, c.lname, c.m_initial, c.age, c.address
    ";

            // Clear existing rows in DataGridView (if any)
            dgvResidency.Rows.Clear();

            try
            {
                // Open a connection to the database
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);  // Fill the DataTable with data

                    // Loop through the DataTable and add rows to the DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        dgvResidency.Rows.Add(
                            row["resident_id"].ToString(),
                            $"{row["fname"]} {row["m_initial"]}. {row["lname"]}",
                            row["age"].ToString(),
                            row["address"].ToString(),
                            Convert.ToDateTime(row["date_issued"]).ToString("yyyy-MM-dd"),  // Display the selected date_issued

                            row["record_count"].ToString()  // This will show the count of clearances for the resident
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading records:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void LoadClearanceRecords(string residentId = null)
        {
            // SQL query to get all records from the ClearanceRecord table, grouped by resident_id and showing the latest date_issued
            string query = @"
        SELECT c.resident_id, c.fname, c.lname, c.m_initial, c.age, c.address,
               COUNT(c.resident_id) AS record_count, 
               MAX(d.date_issued) AS date_issued
        FROM ResidencyRecord c
        INNER JOIN ResidencyRecord d ON c.resident_id = d.resident_id
    ";

            // Add filtering by resident_id if provided
            if (!string.IsNullOrEmpty(residentId))
            {
                query += " WHERE c.resident_id = ?";
            }

            query += " GROUP BY c.resident_id, c.fname, c.lname, c.m_initial, c.age, c.address";

            // Clear existing rows in DataGridView (if any)
            dgvResidency.Rows.Clear();

            try
            {
                // Open a connection to the database
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);

                    // If a resident_id is provided, add it as a parameter to the query
                    if (!string.IsNullOrEmpty(residentId))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("?", residentId);
                    }

                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt);  // Fill the DataTable with data

                    // Loop through the DataTable and add rows to the DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        dgvResidency.Rows.Add(
                            row["resident_id"].ToString(),
                            $"{row["fname"]} {row["m_initial"]}. {row["lname"]}",
                            row["age"].ToString(),
                            row["address"].ToString(),
                            Convert.ToDateTime(row["date_issued"]).ToString("yyyy-MM-dd"),  // Display the selected date_issued

                            row["record_count"].ToString()  // This will show the count of clearances for the resident
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading records:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResidencyRecord_Load(object sender, EventArgs e)
        {
            bc.Parent = pictureBox1;
            bc.BackColor = System.Drawing.Color.Transparent;
            linkLabel1.Parent = pictureBox1;
            linkLabel1.BackColor = System.Drawing.Color.Transparent;
            LinkLabelHistory.Parent = pictureBox1;
            LinkLabelHistory.BackColor = System.Drawing.Color.Transparent;
            sy.Parent = pictureBox1;
            sy.BackColor = System.Drawing.Color.Transparent;
            linkLabel4.Parent = pictureBox1;
            linkLabel4.BackColor = System.Drawing.Color.Transparent;
            linkLabel5.Parent = pictureBox1;
            linkLabel5.BackColor = System.Drawing.Color.Transparent;
            label1.Parent = pictureBox1;
            label1.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.Parent = pictureBox1;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            pictureBox2.Parent = pictureBox1;
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
            LoadClearanceRecords();  // Load records when the form is loaded
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pnlServices.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IndigencyRecords indigencyRecords = new IndigencyRecords(Username);
            indigencyRecords.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            staffHistory staffHistory = new staffHistory(Username);
            staffHistory.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ResidencyRecord residencyRecord = new ResidencyRecord(Username);
            residencyRecord.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffServices staffServices = new staffServices(Username);
            staffServices.Show();
            this.Hide();
        }

        private void LinkLabelHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffHistory staffHistory = new staffHistory(Username);
            staffHistory.Show();
            this.Hide();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffAbout staffAbout = new staffAbout(Username);
            staffAbout.Show();
            this.Hide();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffHelp staffHelp = new staffHelp(Username);
            staffHelp.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loginProfile loginProfile = new loginProfile(Username);
            loginProfile.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PermitRecord permitRecord = new PermitRecord(Username);
            permitRecord.Show();
            this.Hide();
        }
    }
}
