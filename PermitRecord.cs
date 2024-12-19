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
    public partial class PermitRecord : Form
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
        public PermitRecord(string loggedInUsername)
        {
            InitializeComponent();
            dgvPermit.Columns.Clear();
            dgvPermit.Columns.Add("name_of_business", "Business Name");
            dgvPermit.Columns.Add("firstname", "First Name");
            dgvPermit.Columns.Add("lastname", "Last Name");
            dgvPermit.Columns.Add("middle_initial", "Middle Initial");
            dgvPermit.Columns.Add("address", "Address");
            dgvPermit.Columns.Add("age", "Age");
            dgvPermit.Columns.Add("business_location", "Business Location");
            dgvPermit.Columns.Add("dit", "Date Issued");
            dgvPermit.Columns.Add("record_count", "Record Count"); // Count of permits per resident
        }
        public void LoadPermitRecords(string businessName = null)
        {
            // SQL query to fetch all permit records grouped by name_of_business
            string query = @"
    SELECT p.name_of_business, p.firstname, p.lastname, p.middle_initial, p.age, p.address, 
           p.business_location, MAX(p.date_issued) AS date_issued, 
           COUNT(p.name_of_business) AS record_count
    FROM permit p
";

            // Add filtering by business name if provided
            if (!string.IsNullOrEmpty(businessName))
            {
                query += " WHERE p.name_of_business LIKE ?";
            }

            query += " GROUP BY p.name_of_business, p.firstname, p.lastname, p.middle_initial, p.age, p.address, p.business_location";

            // Clear existing rows in the DataGridView (if any)
            dgvPermit.Rows.Clear();

            try
            {
                // Open a connection to the database
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, conn);

                    // If a business name is provided, add it as a parameter to the query
                    if (!string.IsNullOrEmpty(businessName))
                    {
                        dataAdapter.SelectCommand.Parameters.AddWithValue("?", $"%{businessName}%");
                    }

                    DataTable dt = new DataTable();
                    dataAdapter.Fill(dt); // Fill the DataTable with data

                    // Loop through the DataTable and add rows to the DataGridView
                    foreach (DataRow row in dt.Rows)
                    {
                        dgvPermit.Rows.Add(
                            row["name_of_business"].ToString(),
                            row["firstname"].ToString(),
                            row["lastname"].ToString(),
                            row["middle_initial"].ToString(),
                            row["age"].ToString(),
                            row["address"].ToString(),
                            row["business_location"].ToString(),
                            Convert.ToDateTime(row["date_issued"]).ToString("yyyy-MM-dd"), // Display the date issued
                            row["record_count"].ToString() // Show the count of permits for the business
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading permit records:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

    private void PermitRecord_Load(object sender, EventArgs e)
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
            LoadPermitRecords(Username);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pnlServices.Visible = true;
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

        private void button1_Click(object sender, EventArgs e)
        {
            loginProfile loginProfile = new loginProfile(Username);
            loginProfile.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            staffHistory staffHistory = new staffHistory(Username);
            staffHistory.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            IndigencyRecords indigencyRecords = new IndigencyRecords(Username);
            indigencyRecords.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ResidencyRecord residencyRecord = new ResidencyRecord(Username);
           residencyRecord.Show();
            this.Hide();
        }
    }
}
