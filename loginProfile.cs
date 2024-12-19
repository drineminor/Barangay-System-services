using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Final_thesis.staffServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace Final_thesis
{
    public partial class loginProfile : Form
    {
        private string OriginalUsername;
        private string OriginalEmail;
        private string OriginalContact;
        private byte[] OriginalProfileImage; // Store the original profile picture as byte array

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
        private readonly string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=finalthesis.accdb";
        public string Username { get; set; } // Public property for username
       


        public loginProfile(string username)
        {
            InitializeComponent();
            LoadProfileData(username);
           
        
    }
        private void LoadProfileData(string username)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT firstname, lastname, username, emailadd, contact, profileimage FROM account WHERE username = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", username);
                        conn.Open();

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fullName = $"{reader["firstname"]} {reader["lastname"]}";
                                tbfullname.Text = fullName;
                                tbusername.Text = reader["username"].ToString();
                                tbemail.Text = reader["emailadd"].ToString();
                                tbcontact.Text = reader["contact"].ToString();
                                tbname.Text = tbusername.Text;

                                OriginalUsername = tbusername.Text;
                                OriginalEmail = tbemail.Text;
                                OriginalContact = tbcontact.Text;

                                if (reader["profileimage"] != DBNull.Value)
                                {
                                    OriginalProfileImage = (byte[])reader["profileimage"];
                                    using (var ms = new MemoryStream(OriginalProfileImage))
                                    {
                                        pbProfilePhoto.Image = Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    pbProfilePhoto.Image = null;
                                }
                            }
                            else
                            {
                                MessageBox.Show($"No data found for the username: {username}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading profile data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ToggleEditing(bool isEditing)
        {
            tbfullname.ReadOnly = true;
            tbusername.ReadOnly = !isEditing;
            tbemail.ReadOnly = !isEditing;
            tbcontact.ReadOnly = !isEditing;

            button3.Visible = isEditing;
            image.Visible = isEditing;
            button2.Visible = !isEditing;
            pictureBox3.Visible = !isEditing;
            up.Visible = isEditing;
        }

        private string GetLoggedInusername()
        {
            // Temporary user ID. Replace with actual logic for fetching the logged-in user ID.
            return "";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelDropdown.Visible = !panelDropdown.Visible;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffAbout staffAbout = new staffAbout(Username);
            staffAbout.Show();
            this.Hide();
        }

        private void loginProfile_Load(object sender, EventArgs e)
        {

            // Ensure profile data loads on form load
           
            tbfullname.TextAlign = HorizontalAlignment.Center; // Center the text in the TextBox
            tbusername.TextAlign = HorizontalAlignment.Center; // Center the text in the TextBox
            tbemail.TextAlign = HorizontalAlignment.Center; // Center the text in the TextBox
            tbcontact.TextAlign = HorizontalAlignment.Center; // Center the text in the TextBox
            tbname.TextAlign = HorizontalAlignment.Center; // Center the text in the TextBox

            MakePictureBoxCircular(pbProfilePhoto); // Make pbProfilePhoto circular

            panel1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel1.Width, panel1.Height, 22, 22));
            panel2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel2.Height, 22, 22));
            panel3.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel3.Height, 22, 22));
            panel4.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width, panel4.Height, 22, 22));
            panel5.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel5.Width, panel5.Height, 22, 22));
            button2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button2.Width, button2.Height, 30, 30));
            button3.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button3.Width, button3.Height, 30, 30));
            up.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, up.Width, up.Height, 30, 30));


            // Transparent labels
            label1.Parent = pictureBox2;
            label1.BackColor = Color.Transparent;
            pictureBox1.Parent = pictureBox2;
            pictureBox1.BackColor = Color.Transparent;
            image.Parent = pictureBox2;
            image.BackColor = Color.Transparent;

            pictureBox3.Parent = pictureBox2;
            pictureBox3.BackColor = Color.Transparent;
            fn.Parent = pictureBox2;
            fn.BackColor = Color.Transparent;
            us.Parent = pictureBox2;
            us.BackColor = Color.Transparent;
            ea.Parent = pictureBox2;
            ea.BackColor = Color.Transparent;
            p.Parent = pictureBox2;
            p.BackColor = Color.Transparent;

            label8.Parent = pictureBox2;
            label8.BackColor = Color.Transparent;
            linkLabel1.Parent = pictureBox2;
            linkLabel1.BackColor = Color.Transparent;
            linkLabel2.Parent = pictureBox2;
            linkLabel2.BackColor = Color.Transparent;
            linkLabel4.Parent = pictureBox2;
            linkLabel4.BackColor = Color.Transparent;
            linkLabel5.Parent = pictureBox2;
            linkLabel5.BackColor = Color.Transparent;

            pictureBox4.Parent = pictureBox2;
            pictureBox4.BackColor = Color.Transparent;
        }

        private void MakePictureBoxCircular(PictureBox pb)
        {
            pb.Width = 250;
            pb.Height = 250;

            // Create a circular region for the PictureBox
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, pb.Width, pb.Height);
                pb.Region = new Region(path);
            }
        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            ToggleEditing(true); // Enable editing mode
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                DialogResult result = MessageBox.Show("Do you want to save the changes?", "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveProfileData();
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields before saving.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void SaveProfileData()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "UPDATE account SET username = ?, emailadd = ?, contact = ?, profileimage = ? WHERE username = ?";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Match the parameter order to the query
                        cmd.Parameters.AddWithValue("?", tbusername.Text.Trim()); // New username
                        cmd.Parameters.AddWithValue("?", tbemail.Text.Trim());    // New email
                        cmd.Parameters.AddWithValue("?", tbcontact.Text.Trim());  // New contact

                        // Handle the profile image
                        if (pbProfilePhoto.Image != null)
                        {
                            byte[] imageBytes = ImageToByteArray(pbProfilePhoto.Image);
                            cmd.Parameters.AddWithValue("?", imageBytes); // New profile image
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("?", DBNull.Value); // No profile image
                        }

                        // Add the original username for WHERE clause
                        cmd.Parameters.AddWithValue("?", OriginalUsername.Trim()); // Original username (WHERE clause)

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Profile data updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProfileData(tbusername.Text); // Reload updated data
                            ToggleEditing(false);
                        }
                        else
                        {
                            MessageBox.Show("No changes were made to the profile.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving profile data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private bool ValidateInputs()
        {
            return !string.IsNullOrWhiteSpace(tbusername.Text) &&
                   !string.IsNullOrWhiteSpace(tbemail.Text) &&
                   !string.IsNullOrWhiteSpace(tbcontact.Text);
        }


        private void tbusername_TextChanged(object sender, EventArgs e)
        {
            // Update tbname to match tbusername as the user types
            tbname.Text = tbusername.Text;
        }


        private void up_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbProfilePhoto.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Save the logout history
                SaveLogoutHistory(Username);

                // Redirect to the login form
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveLogoutHistory(string staffId)
        {
            try
            {
                // Validate staffId
                if (string.IsNullOrEmpty(staffId))
                {
                    MessageBox.Show("Error: Staff ID is required for saving logout history.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Log the staffId to ensure it's being passed correctly
                MessageBox.Show($"Saving logout history for Staff ID: {staffId}");

                // Define connection string and query
                string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=finalthesis.accdb";
                string timeOutQuery = "UPDATE staffHistory SET Time_OUT = ? WHERE staff_id = ? AND Time_OUT IS NULL"; // Ensure Time_OUT is null

                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    using (OleDbCommand cmd = new OleDbCommand(timeOutQuery, conn))
                    {
                        // Open connection
                        conn.Open();

                        // Add parameters to query
                        cmd.Parameters.AddWithValue("?", DateTime.Now);  // Time_OUT is the current date and time
                        cmd.Parameters.AddWithValue("?", staffId);       // The staff ID passed to the function

                        // Execute the query and capture the rows affected
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Handle result
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Logout history saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No matching record found for the staff ID.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception message
                MessageBox.Show($"Error saving logout history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffServices staffServices = new staffServices(Username);
            staffServices.Show();
            this.Hide();

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffHistory staffHistory = new staffHistory(Username);
            staffHistory.Show();
            this.Hide();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffHelp staffHelp = new staffHelp(Username);
            staffHelp.Show();
            this.Hide();
        }
    }   }