using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using static Final_thesis.staffServices;


namespace Final_thesis
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
 (
     int nLeft,
     int nTop,
     int nRight,
     int nBottom,
     int nWidthEllipse,
     int nHeightEllipse

  );
       






        OleDbConnection conn;
        OleDbCommand cmd;

        private int loginattempts = 0;
        private int loginAttempts;
        private int failedAttempts = 0; // Track failed login attempts
        private DateTime lockoutEndTime; // Store when the lockout ends
        private bool isLockedOut = false; // Check if the account is locked out
        public Form1()
        {
            InitializeComponent();
           }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnLog.Region = Region.FromHrgn(CreateRoundRectRgn(0,0,btnLog.Width,btnLog.Height, 35, 35));
            tbpass.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, tbpass.Width, tbpass.Height, 22, 22));
            tbun.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, tbun.Width, tbun.Height, 22, 22));

            tbun.TextAlign = HorizontalAlignment.Center; // Center the text in the TextBox
            tbpass.TextAlign = HorizontalAlignment.Center; // Center the text in the TextBox


            label1.Parent = pictureBox1;
            label1.BackColor = Color.Transparent;
            label2.Parent = pictureBox1;
            label2.BackColor = Color.Transparent;
            label3.Parent = pictureBox1;
            label3.BackColor = Color.Transparent;
            label4.Parent = pictureBox1;
            label4.BackColor = Color.Transparent;
            label9.Parent = pictureBox1;
            label9.BackColor = Color.Transparent;
            p5.Parent = pictureBox1;
            p5.BackColor = Color.Transparent;
            panel11.Parent = pictureBox1;
            panel11.BackColor = Color.Transparent;
            panel15.Parent = pictureBox1;
            panel15.BackColor = Color.Transparent;
            fg.Parent = pictureBox1;
            fg.BackColor = Color.Transparent;






            label8.Parent = pictureBox1;
            label8.BackColor = Color.Transparent;
            label6.Parent = pictureBox1;
            label6.BackColor = Color.Transparent;
            pictureBox4.Parent = pictureBox1;
            pictureBox4.BackColor = Color.Transparent;
            pictureBox3.Parent = pictureBox1;
            pictureBox3.BackColor = Color.Transparent;
            



        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try

            {
                if (tbun.Text == "")
                {
                    tbun.Text = "";
                    return;
                }
                tbun.ForeColor = Color.Black;
             

            }
            catch { }

        }

        private void tbpass_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbpass.Text == "")
                {
                    tbpass.Text = "";
                    return;
                }
                tbpass.ForeColor = Color.Black;
                

            }

            catch { }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Check if the user is currently locked out
            if (isLockedOut)
            {
                TimeSpan remainingLockout = lockoutEndTime - DateTime.Now;
                if (remainingLockout.TotalSeconds > 0)
                {
                    MessageBox.Show($"You are locked out! Try again in {remainingLockout.Seconds} seconds.", "Locked Out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    // Reset lockout when the lockout period expires
                    isLockedOut = false;
                    loginAttempts = 0;
                }
            }

            // Database connection and query
            string connString = "Provider=Microsoft.ACE.OleDb.12.0;Data Source=finalthesis.accdb";
            string query = "SELECT staff_id, [Role], firstname, lastname FROM account WHERE username = @username AND Pass = @password";

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", tbun.Text);
                    cmd.Parameters.AddWithValue("@password", tbpass.Text);

                    try
                    {
                        // Open the connection
                        conn.Open();

                        // Execute the query and get the result
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Reset login attempts after successful login
                                loginAttempts = 0;

                                // The user exists, retrieve details
                                string staffId = reader["staff_id"].ToString();
                                string userType = reader["Role"].ToString();
                                string fullName = $"{reader["firstname"]} {reader["lastname"]}";

                                // Save Time_IN to staffhistory table
                                SaveLoginHistory(staffId, fullName);

                                // Check the user type and open the corresponding form
                                this.Hide();

                                if (userType == "admin")
                                {
                                    adminProfile adminProfile = new adminProfile(tbun.Text);
                                    adminProfile.Show();
                                }
                                else if (userType == "staff")
                                {
                                    loginProfile loginProfile = new loginProfile(tbun.Text); // Pass the username here
                                    loginProfile.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Unrecognized user type.");
                                    this.Show(); // Show the login form again if type is unrecognized
                                }
                            }
                            else
                            {
                                // Increment failed login attempts
                                loginAttempts++;
                                MessageBox.Show($"Invalid username or password. Attempt {loginAttempts} of 3.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                if (loginAttempts >= 3)
                                {
                                    // Lock the user out for 1 minute
                                    isLockedOut = true;
                                    lockoutEndTime = DateTime.Now.AddMinutes(1);
                                    MessageBox.Show("You have been locked out for 1 minute.", "Locked Out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void SaveLoginHistory(string staffId, string fullName)
        {
            string connString = "Provider=Microsoft.ACE.OleDb.12.0;Data Source=finalthesis.accdb";
            string timeInQuery = "INSERT INTO staffHistory (staff_id, fullname, Time_IN) VALUES (?, ?, ?)";

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand(timeInQuery, conn))
                {
                    try
                    {
                        // Open the connection
                        conn.Open();

                        // Add parameters with explicit types
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = staffId;   // Use VarChar if staff_id is Text
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = fullName; // Full name should be a string
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now; // Date/Time field

                        // Execute the query
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Login history saved successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving login history: " + ex.Message);
                    }
                }
            }
        }

        private void SaveLogoutHistory(string staffId)
        {
            string connString = "Provider=Microsoft.ACE.OleDb.12.0;Data Source=finalthesis.accdb";
            string timeOutQuery = "UPDATE staffHistory SET Time_OUT = ? WHERE staff_id = ? AND Time_OUT IS NULL";

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand(timeOutQuery, conn))
                {
                    try
                    {
                        // Open the connection
                        conn.Open();

                        // Add parameters with explicit types
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now; // Set Time_OUT to current date and time
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = staffId;   // staff_id should match the logged-in user

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Logout history saved successfully!");
                        }
                        else
                        {
                            MessageBox.Show("No matching record found to update. Logout history not saved.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving logout history: " + ex.Message);
                    }
                }
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                tbpass.PasswordChar = '\0';
            }
            else
            {
                tbpass.PasswordChar = '*';
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
           

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
