using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection.Emit;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static Final_thesis.staffServices;



namespace Final_thesis
{
    public partial class adminProfile : Form
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

        public adminProfile(string loggedInUsername)

        {
            InitializeComponent();
            Username = loggedInUsername;  // Set the Username property
            
        }
            
        private string GetLoggedInUserId()
        {
            return "";
        }

        private void pictureBox3_Click(object sender, EventArgs e) 
        {
            hisDD.Visible = !hisDD.Visible;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelDropdown.Visible = !panelDropdown.Visible;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void adminProfile_Load(object sender, EventArgs e)
        {
            LoadProfileData();
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
            button7.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button7.Width, button7.Height, 30, 30));
            button2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, button2.Width, button2.Height, 30, 30));
            up.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, up.Width, up.Height, 30, 30));
            pictureBox5.Parent = pictureBox2;
            pictureBox5.BackColor = Color.Transparent;

            image.Parent = pictureBox2;
            image.BackColor = Color.Transparent;
            fn.Parent = pictureBox2;
            fn.BackColor = Color.Transparent;
            us.Parent = pictureBox2;
            us.BackColor = Color.Transparent;
            ea.Parent = pictureBox2;
            ea.BackColor = Color.Transparent;
            p.Parent = pictureBox2;
            p.BackColor = Color.Transparent;
            label1.Parent = pictureBox2;
            label1.BackColor = Color.Transparent;
            pictureBox1.Parent = pictureBox2;
            pictureBox1.BackColor = Color.Transparent;
            label2.Parent = pictureBox2;
            label2.BackColor = Color.Transparent;
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
            pictureBox3.Parent = pictureBox2;
            pictureBox3.BackColor = Color.Transparent;
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
        private void LoadProfileData()
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "SELECT firstname, lastname, username, emailadd, contact, profileimage FROM account WHERE username = ?";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("?", Username);

                        conn.Open();
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate profile fields with original values
                                string fullName = $"{reader["firstname"]} {reader["lastname"]}";
                                tbfullname.Text = fullName;
                                tbusername.Text = reader["username"].ToString();
                                tbemail.Text = reader["emailadd"].ToString();
                                tbcontact.Text = reader["contact"].ToString();
                                tbname.Text = tbusername.Text;

                                // Save original values for possible reversion
                                OriginalUsername = tbusername.Text;
                                OriginalEmail = tbemail.Text;
                                OriginalContact = tbcontact.Text;

                                // Store original profile image (in case of cancellation)
                                if (reader["profileimage"] != DBNull.Value)
                                {
                                    OriginalProfileImage = (byte[])reader["profileimage"];
                                    using (var ms = new System.IO.MemoryStream(OriginalProfileImage))
                                    {
                                        pbProfilePhoto.Image = System.Drawing.Image.FromStream(ms); // Updated line
                                    }
                                }
                                else
                                {
                                    pbProfilePhoto.Image = null; // No image available
                                }
                            }
                            else
                            {
                                MessageBox.Show($"No data found for the username: {Username}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ToggleEditing(bool isEditing)
        {
            // Make fields read-only or editable based on the isEditing flag
            tbfullname.ReadOnly = true; // Full Name should not be editable
            tbusername.ReadOnly = !isEditing;
            tbemail.ReadOnly = !isEditing;
            tbcontact.ReadOnly = !isEditing;

            // Toggle visibility of Save/Cancel buttons
            button2.Visible = isEditing;
            image.Visible = isEditing;

            // Toggle visibility of Edit button
            button7.Visible = !isEditing;
            pictureBox5.Visible = !isEditing;

            // Allow the user to change the profile picture only in editing mode
            up.Visible = isEditing;
        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            CRUD CRUD = new CRUD(Username);
            CRUD.Show();
            this.Hide();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            adminAbout adminAbout = new adminAbout(Username);
            adminAbout.Show();
            this.Hide();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            adminHelp adminHelp = new adminHelp(Username);
            adminHelp.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            historyRecords historyRecords = new historyRecords(Username);
            historyRecords.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
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
                bool isChanged = false;
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    // Start building the query dynamically based on which fields are changed
                    string query = "UPDATE account SET ";
                    List<OleDbParameter> parameters = new List<OleDbParameter>();

                    // Check if fields are changed and build the query
                    if (tbusername.Text.Trim() != OriginalUsername)
                    {
                        query += "username = @username, ";
                        parameters.Add(new OleDbParameter("@username", tbusername.Text.Trim()));
                        isChanged = true;
                    }

                    if (tbemail.Text.Trim() != OriginalEmail)
                    {
                        query += "emailadd = @email, ";
                        parameters.Add(new OleDbParameter("@email", tbemail.Text.Trim()));
                        isChanged = true;
                    }

                    if (tbcontact.Text.Trim() != OriginalContact)
                    {
                        query += "contact = @contact, ";
                        parameters.Add(new OleDbParameter("@contact", tbcontact.Text.Trim()));
                        isChanged = true;
                    }

                    // Handle profile picture change
                    if (pbProfilePhoto.Image != null && !ImageEquals(OriginalProfileImage, pbProfilePhoto.Image))
                    {
                        using (var ms = new System.IO.MemoryStream())
                        {
                            pbProfilePhoto.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Save as JPEG
                            query += "profileimage = @profileimage, ";
                            parameters.Add(new OleDbParameter("@profileimage", ms.ToArray()));
                            isChanged = true;
                        }
                    }

                    if (!isChanged)
                    {
                        MessageBox.Show("No changes were made.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return; // No changes to save
                    }

                    query = query.TrimEnd(',', ' ') + " WHERE username = @oldUsername";
                    parameters.Add(new OleDbParameter("@oldUsername", Username));  // Use the old username for the WHERE clause

                    // Execute the update query
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ToggleEditing(false); // Disable editing mode after saving
                        }
                        else
                        {
                            MessageBox.Show("No changes were made.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving profile data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Compare image byte arrays for equality
        private bool ImageEquals(byte[] originalImage, System.Drawing.Image newImage)
        {
            if (originalImage == null || newImage == null)
                return false;

            using (var msOriginal = new System.IO.MemoryStream(originalImage))
            using (var msNew = new System.IO.MemoryStream())
            {
                newImage.Save(msNew, System.Drawing.Imaging.ImageFormat.Jpeg);
                return msOriginal.ToArray().SequenceEqual(msNew.ToArray());
            }
        }


        private bool ValidateInputs()
        {
            // Validate that none of the required fields are empty or whitespace
            return !string.IsNullOrWhiteSpace(tbusername.Text) &&
                   !string.IsNullOrWhiteSpace(tbemail.Text) &&
                   !string.IsNullOrWhiteSpace(tbcontact.Text);
        }

        private void tbusername_TextChanged(object sender, EventArgs e)
        {
            // Update tbname to match tbusername as the user types
            tbname.Text = tbusername.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ToggleEditing(true); // Enable editing mode
        }

        private void up_Click(object sender, EventArgs e)
        {
            // Open file dialog to select new profile picture
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load the selected image into the picture box
                pbProfilePhoto.Image = System.Drawing.Image.FromFile(openFileDialog.FileName);
            }
        }
    }
}
