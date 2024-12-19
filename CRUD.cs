using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb; // For connecting to Access databases
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // For handling file and memory streams
using System.Drawing.Imaging;
using static Final_thesis.staffServices; // For working with images and saving them in different formats


namespace Final_thesis
{
    public partial class CRUD : Form
    {
        OleDbConnection conn;// OleDbConnection: Manages database connection.
        OleDbCommand cmd; // OleDbCommand: Executes SQL commands (e.g., INSERT, UPDATE).
        OleDbDataAdapter adapter;// OleDbDataAdapter: Connects database and DataTable, retrieves and updates data.
        DataTable dt; // DataTable: Stores data in-memory, can be bound to controls like DataGridView.
        private bool isImageUploaded = false;
        public string Username { get; set; } // Public property for username
       
        public CRUD(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;  // Set the Username property
            
        }
       
            void GetUsers()
            {
                // Establish the connection string to connect to the Access database
                conn = new OleDbConnection("Provider= Microsoft.ACE.OleDb.12.0;Data Source=finalthesis.accdb");
                // Initialize the DataTable to hold user data
                dt = new DataTable();
                // Set up an adapter to run the query and fetch the user data
                adapter = new OleDbDataAdapter("SELECT  staff_id, firstname, lastname, username, emailadd, contact, Pass, role, profileimage FROM account", conn);
                // Open the connection
                conn.Open();
                // Fill the DataTable with the result of the query
                adapter.Fill(dt);
                // Bind the DataTable to the DataGridView to display user information
                dgvUser.DataSource = dt;
                // Close the database connection
                conn.Close();
            }
        


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CRUD_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox2;
            label1.BackColor = Color.Transparent;
            label3.Parent = pictureBox2;
            label3.BackColor = Color.Transparent;
            label10.Parent = pictureBox2;
            label10.BackColor = Color.Transparent;
            label4.Parent = pictureBox2;
            label4.BackColor = Color.Transparent;
            label5.Parent = pictureBox2;
            label5.BackColor = Color.Transparent;
            label6.Parent = pictureBox2;
            label6.BackColor = Color.Transparent;
            label7.Parent = pictureBox2;
            label7.BackColor = Color.Transparent;
            label9.Parent = pictureBox2;
            label9.BackColor = Color.Transparent;
            label11.Parent = pictureBox2;
            label11.BackColor = Color.Transparent;
            label12.Parent = pictureBox2;
            label12.BackColor = Color.Transparent;
            label13.Parent = pictureBox2;
            label13.BackColor = Color.Transparent;
            pictureBox5.Parent = pictureBox2;
            pictureBox5.BackColor = Color.Transparent;
            pictureBox6.Parent = pictureBox2;
            pictureBox6.BackColor = Color.Transparent;
            pictureBox7.Parent = pictureBox2;
            pictureBox7.BackColor = Color.Transparent;
            pictureBox8.Parent = pictureBox2;
            pictureBox8.BackColor = Color.Transparent;
            pictureBox9.Parent = pictureBox2;
                pictureBox9.BackColor = Color.Transparent;
            pictureBox10.Parent = pictureBox2;
                pictureBox10.BackColor = Color.Transparent;
            pictureBox11.Parent = pictureBox2;
            pictureBox11.BackColor = Color.Transparent;
            pictureBox12.Parent = pictureBox2;
            pictureBox12.BackColor = Color.Transparent;

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
            GetUsers();
           
        }

        private void btnupload_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog to allow users to select an image file
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the file filter to only show image files
            openFileDialog.Filter = "Image Files|*.jpg;*.jfif;*.jpeg;*.png;*.bmp";

            // Show the dialog and check if the user selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load the selected image into the PictureBox (pbImage)
                pbImage.Image = new Bitmap(openFileDialog.FileName);


                isImageUploaded = true; // Set the flag to true when an image is uploaded
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFn.Text) ||    // First Name
      string.IsNullOrWhiteSpace(tbLn.Text) ||    // Last Name
      string.IsNullOrWhiteSpace(tbUn.Text) ||    // Username
      string.IsNullOrWhiteSpace(tbph.Text) ||     // Phone
      string.IsNullOrWhiteSpace(tbea.Text) ||     // Email
      string.IsNullOrWhiteSpace(tbpass.Text))     // Password
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Determine SQL query based on whether an image is uploaded
            string query = isImageUploaded
                ? "UPDATE account SET firstname=?, lastname=?, username=?, emailadd=?, contact=?, Pass=?, role=?, profileimage=? WHERE staff_id=?"
                : "UPDATE account SET firstname=?, lastname=?, username=?, emailadd=?, contact=?, Pass=?, role=? WHERE staff_id=?";

            // Create and configure the command
            using (OleDbCommand cmd = new OleDbCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("?", tbFn.Text);
                cmd.Parameters.AddWithValue("?", tbLn.Text);
                cmd.Parameters.AddWithValue("?", tbUn.Text);
                cmd.Parameters.AddWithValue("?", tbph.Text);
                cmd.Parameters.AddWithValue("?", tbea.Text);
                cmd.Parameters.AddWithValue("?", tbpass.Text);
                cmd.Parameters.AddWithValue("?", cbrole.SelectedItem.ToString()); // Role

                // Add image parameter if uploaded
                if (isImageUploaded && pbImage.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pbImage.Image.Save(ms, pbImage.Image.RawFormat);
                        cmd.Parameters.AddWithValue("?", ms.ToArray()); // Profile image
                    }
                }

                // Add the staff ID parameter
                cmd.Parameters.AddWithValue("?", Convert.ToInt32(tbID.Text));

                // Execute the update command
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                // Show result message
                MessageBox.Show(rowsAffected > 0 ? "User Updated Successfully" : "No user found with the provided ID.");
            }

            // Reset flag and refresh DataGridView
            isImageUploaded = false;
            GetUsers();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Check if all required fields are filled
            if (string.IsNullOrWhiteSpace(tbFn.Text) ||    // First Name
                string.IsNullOrWhiteSpace(tbLn.Text) ||    // Last Name
                string.IsNullOrWhiteSpace(tbUn.Text) ||
                 string.IsNullOrWhiteSpace(tbph.Text) ||
                  string.IsNullOrWhiteSpace(tbea.Text) ||// Username
                string.IsNullOrWhiteSpace(tbpass.Text) ||     // Password
                string.IsNullOrWhiteSpace(tbcompass.Text) ||    // Repeat Password
                 cbrole.SelectedIndex == -1 || // Ensure Role is selected
              pbImage.Image == null)                  // Check if an image is selected
            {
                MessageBox.Show("Please fill in all fields."); // Display a message if any field is empty
                return; // Exit the method if any field is missing
            }
            // Check if the passwords match
            if (tbpass.Text != tbcompass.Text)
            {
                MessageBox.Show("Passwords do not match. Please re-enter your password."); // Warn if passwords mismatch
                return; // Exit the method if passwords don't match
            }
            // SQL query to insert a new user into the 'useracc' table
            string query = "INSERT INTO account (firstname, lastname, username, emailadd, contact, Pass, role, profileimage) VALUES " +
                "(?,?,?,?,?,?,?,?)";
            // Create the command to execute the query
            cmd = new OleDbCommand(query, conn);

            // Add values from textboxes and other controls to the command parameters
            cmd.Parameters.AddWithValue("?", tbFn.Text); // First Name
            cmd.Parameters.AddWithValue("?", tbLn.Text); // Last Name
            cmd.Parameters.AddWithValue("?", tbUn.Text);   // Username
            cmd.Parameters.AddWithValue("?", tbph.Text);
            cmd.Parameters.AddWithValue("?", tbea.Text);
            cmd.Parameters.AddWithValue("?", tbpass.Text);   // Password
            cmd.Parameters.AddWithValue("?", cbrole.SelectedItem.ToString()); // Add role

            // Handle the image parameter (convert the image in PictureBox to byte array)
            using (MemoryStream ms = new MemoryStream())
            {
                pbImage.Image.Save(ms, pbImage.Image.RawFormat); // Save the image in its raw format to the memory stream
                cmd.Parameters.AddWithValue("?", ms.ToArray()); // Add the image as a byte array to the SQL command
            }
            isImageUploaded = false;
            // Open the connection, execute the command, and close the connection
            conn.Open(); // Open the connection to the database
            cmd.ExecuteNonQuery(); // Execute the insert query
            MessageBox.Show("User Inserted Successfully"); // Show success message
            conn.Close(); // Close the connection to the database

            // Refresh DataGridView to show the newly inserted user
            GetUsers();

        }


        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Populate textboxes and controls with data from the currently selected row
            tbID.Text = dgvUser.CurrentRow.Cells["staff_id"].Value.ToString(); // User ID
            tbFn.Text = dgvUser.CurrentRow.Cells["firstname"].Value.ToString(); // First Name
            tbLn.Text = dgvUser.CurrentRow.Cells["lastname"].Value.ToString(); // Last Name
            tbUn.Text = dgvUser.CurrentRow.Cells["username"].Value.ToString();
            tbea.Text = dgvUser.CurrentRow.Cells["emailadd"].Value.ToString();   // Password
            tbph.Text = dgvUser.CurrentRow.Cells["contact"].Value.ToString();   // Usernam
            tbpass.Text = dgvUser.CurrentRow.Cells["Pass"].Value.ToString();
            cbrole.SelectedItem = dgvUser.CurrentRow.Cells["role"].Value.ToString();
            // Check if the 'Photo' column contains an image
            if (dgvUser.CurrentRow.Cells["profileimage"].Value != DBNull.Value)
            {
                // Convert the byte array from the 'Photo' column to an image and display it
                byte[] imgData = (byte[])dgvUser.CurrentRow.Cells["profileimage"].Value;
                using (MemoryStream ms = new MemoryStream(imgData))
                {
                    pbImage.Image = Image.FromStream(ms); // Load the image from the memory stream
                }
            }
            else
            {
                // If the 'Photo' column is null, clear the PictureBox
                pbImage.Image = null;
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            // Clear all textboxes
            tbFn.Clear(); // Clear the First Name t extbox
            tbLn.Clear(); // Clear the Last Name textbox
            tbUn.Clear();  // Clear the Username textbox
            tbph.Clear();
            tbea.Clear();
            tbpass.Clear();  // Clear the Password textbox
            tbcompass.Clear();  // Clear the ReenterPassword textbox
            tbID.Clear(); // Clear the ID textbox
            tbS.Clear();  // Clear the Search textbox
            cbrole.SelectedIndex = -1; // Reset ComboBox




            // Clear the PictureBox
            pbImage.Image = null; // Remove any image from the PictureBox

        }

        private void btndel_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected in the DataGridView
            if (dgvUser.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a staff member to delete.");
                return;
            }

            try
            {
                // Get the ID from the selected row in the DataGridView
                int userId = Convert.ToInt32(dgvUser.SelectedRows[0].Cells["staff_id"].Value);

                // SQL query to delete the user with the selected ID
                string query = "DELETE FROM account WHERE staff_id = ?";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    // Add the ID parameter to the query
                    cmd.Parameters.AddWithValue("?", userId);

                    // Open the database connection
                    conn.Open();

                    // Execute the delete query
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Check if the deletion was successful
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Staff Deleted Successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No staff member found with the selected ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the operation
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // Ensure the connection is closed
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                // Refresh the DataGridView to reflect changes
                GetUsers();
            }
        }

        private void tbS_TextChanged(object sender, EventArgs e)
        {
            // SQL query to search for users by ID using a partial match
            string searchQuery = "SELECT * FROM account WHERE staff_id LIKE ?";

            // Create a new OleDbDataAdapter with the search query and the database connection
            adapter = new OleDbDataAdapter(searchQuery, conn);

            // Add the search parameter with the value from the search textbox
            adapter.SelectCommand.Parameters.AddWithValue("?", tbS.Text + "%"); // Add '%' for partial matching

            dt = new DataTable(); // Create a new DataTable to hold the search results

            // Open the connection, fill the DataTable with search results, and close the connection
            conn.Open(); // Open the connection to the database
            adapter.Fill(dt); // Fill the DataTable with search results
            conn.Close(); // Close the connection to the database

            // Bind the DataTable to the DataGridView
            dgvUser.DataSource = dt;

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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelDropdown.Visible = !panelDropdown.Visible;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            hisDD.Visible = !hisDD.Visible;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CRUD CRUD = new CRUD(Username);
            CRUD.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            historyRecords historyRecords = new historyRecords(Username);
            historyRecords.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adminProfile adminProfile = new adminProfile(Username);
            adminProfile.Show();
            this.Hide();

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }
    }
}

