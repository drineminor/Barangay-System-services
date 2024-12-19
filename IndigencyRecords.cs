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

namespace Final_thesis
{
    public partial class IndigencyRecords : Form
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
        public IndigencyRecords(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;  // Set the Username property
            InitializeIndigencyDataGridView(); // Initialize the DataGridView columns
        }
        private void InitializeIndigencyDataGridView()
        {
            dgvIndigency.Columns.Clear();
            dgvIndigency.Columns.Add("resident_id", "Resident ID");
            dgvIndigency.Columns.Add("name", "Name");
            dgvIndigency.Columns.Add("age", "Age");
            dgvIndigency.Columns.Add("address", "Address");
            dgvIndigency.Columns.Add("purpose", "Purpose");
            dgvIndigency.Columns.Add("date_issued", "Date Issued");
            dgvIndigency.Columns.Add("record_count", "Record Count");
        }
        public void LoadClearanceRecords(string residentId = null)
        {
            // SQL query to get all records grouped by resident_id and showing the latest date_issued
            string query = @"
            SELECT 
                c.resident_id, 
                c.fname, 
                c.lname, 
                c.m_initial, 
                c.age, 
                c.address, 
                c.purpose,
                COUNT(c.resident_id) AS record_count, 
                MAX(c.date_issued) AS date_issued
            FROM IndigencyRecord c
        ";

            // Add filtering by resident_id if provided
            if (!string.IsNullOrEmpty(residentId))
            {
                query += " WHERE c.resident_id = ?";
            }

            // Grouping to avoid duplication
            query += " GROUP BY c.resident_id, c.fname, c.lname, c.m_initial, c.age, c.address, c.purpose";

            // Clear existing rows in DataGridView
            dgvIndigency.Rows.Clear();

            try
            {
                // Open a connection to the database
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    OleDbCommand cmd = new OleDbCommand(query, conn);

                    // Add the parameter for filtering if a resident_id is provided
                    if (!string.IsNullOrEmpty(residentId))
                    {
                        cmd.Parameters.AddWithValue("?", residentId);
                    }

                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt); // Fill the DataTable with data

                    // Loop through the DataTable and add rows to the DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        dgvIndigency.Rows.Add(
                            row["resident_id"].ToString(),
                            $"{row["fname"]} {row["m_initial"]}. {row["lname"]}",
                            row["age"].ToString(),
                            row["address"].ToString(),
                            row["purpose"].ToString(),
                            Convert.ToDateTime(row["date_issued"]).ToString("yyyy-MM-dd"),
                            row["record_count"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading records:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

    private void IndigencyRecords_Load(object sender, EventArgs e)
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelDropdown.Visible = !panelDropdown.Visible;
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

        private void button3_Click(object sender, EventArgs e)
        {
            IndigencyRecords indigencyRecords = new IndigencyRecords(Username);
            indigencyRecords.Show();
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

        private void button7_Click(object sender, EventArgs e)
        {
            pnlServices.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PermitRecord permitRecord = new PermitRecord(Username);
            permitRecord.Show();
            this.Hide();
        }
    }
}
   