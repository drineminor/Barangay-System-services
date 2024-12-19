using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using System;
using Application = Microsoft.Office.Interop.Word.Application; // Avoid conflict with System.Windows.Forms.Application
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;

using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using static Final_thesis.staffHistory;
using DocumentFormat.OpenXml.Office.Word;


namespace Final_thesis
{
    public partial class staffServices : Form
    {

        private staffHistory staffHistoryForm;
        // Connection string to the database
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=finalthesis.accdb";



        private string photoFilePath;
        private System.Windows.Forms.PictureBox pbpoto;
        private object resident_id;

        public static staffHistory StaffHistoryFormInstance { get; private set; }
        public string Username { get; set; } // Public property for username

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

        public staffServices(string loggedInUsername)
        {
            InitializeComponent();
            tbs.KeyDown += Tbs_KeyDown;
            Username = loggedInUsername;  // Set the Username property

        }
        private void ShowStaffHistoryForm()
        {
            if (staffHistoryForm == null || staffHistoryForm.IsDisposed)
            {
                staffHistoryForm = new staffHistory(Username); // Initialize a new instance
            }
            staffHistoryForm.Show(); // Show the form
            staffHistoryForm.Focus(); // Focus if already open
        }

        private void Tbs_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key is pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Call the search function
                SearchResident();
                e.Handled = true;
                e.SuppressKeyPress = true; // Prevent the "ding" sound
            }
        }


        private void SearchResident()
        {
            // Get the ID entered in the search TextBox (tbs)
            string searchId = tbs.Text.Trim();
            // Validate the search ID
            if (string.IsNullOrEmpty(searchId))
            {
                MessageBox.Show("Please enter an ID to search.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Connect to the database
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to fetch resident data based on the ID
                    string query = "SELECT Firstname, Lastname, Middle_initial, age, address, purpose, picture FROM resident WHERE resident_id = ?";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Add the search ID as a parameter
                        command.Parameters.AddWithValue("?", searchId);

                        // Execute the query and fetch the data
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the TextBoxes with the fetched data
                                tbfn.Text = reader["Firstname"].ToString();
                                tbln.Text = reader["Lastname"].ToString();
                                tbmi.Text = reader["Middle_initial"].ToString();
                                tbage.Text = reader["age"].ToString();
                                tbadd.Text = reader["address"].ToString();



                            }
                            else
                            {
                                // If no record is found
                                MessageBox.Show("No resident found with the entered ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                MessageBox.Show("An error occurred while fetching the data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void staffServices_Load(object sender, EventArgs e)
        {
            clearance.Visible = false;
            bpm.Visible = false;
            pbpt.Visible = false;
            btnpt.Visible = false;
            txtprp.Visible = false;
            prp.Visible = false;
            residency.Visible = false;
            indigency.Visible = false;



            // Initialize the PictureBox
            pbpoto = new PictureBox
            {
                Name = "pbpoto",
                Width = 100, // Set desired width
                Height = 100, // Set desired height
                Location = new System.Drawing.Point(50, 50), // Set location on the form
                BorderStyle = BorderStyle.FixedSingle, // Optional: Visual border
                SizeMode = PictureBoxSizeMode.StretchImage // To stretch or resize images
            };

            // Add the PictureBox to the form's controls
            this.Controls.Add(pbpoto);

            btnClearance.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnClearance.Width, btnClearance.Height, 30, 30));
            btnBack.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnBack.Width, btnBack.Height, 30, 30));
            btnIndigency.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnIndigency.Width, btnIndigency.Height, 30, 30));
            btnPermit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnPermit.Width, btnPermit.Height, 30, 30));
            btnResidency.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnResidency.Width, btnResidency.Height, 30, 30));
            indigency.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, indigency.Width, indigency.Height, 30, 30));
            residency.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, residency.Width, residency.Height, 30, 30));


            btnsave.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnsave.Width, btnsave.Height, 30, 30));
            btnSava.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSava.Width, btnSava.Height, 30, 30));
            btnbck.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnbck.Width, btnbck.Height, 30, 30));

            label8.Parent = pictureBox1;
            label8.BackColor = System.Drawing.Color.Transparent;
            linkLabel1.Parent = pictureBox1;
            linkLabel1.BackColor = System.Drawing.Color.Transparent;
            linkLabel2.Parent = pictureBox1;
            linkLabel2.BackColor = System.Drawing.Color.Transparent;
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
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearance.Visible = true;


            btnsave.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clearance.Visible = false;
            pbpt.Visible = false;
            btnpt.Visible = false;
            txtprp.Visible = false;
            prp.Visible = false;
            indigency.Visible = false;

            tbfn.Clear();
            tbln.Clear();
            tbmi.Clear();
            tbage.Clear();
            tbadd.Clear();
            tbs.Clear();

        }

        private void btnIndigency_Click(object sender, EventArgs e)
        {


            clearance.Visible = true;
            pbpt.Visible = true;
            btnpt.Visible = true;
            txtprp.Visible = true;
            prp.Visible = true;
            indigency.Visible = true;
            btnsave.Visible = false;


        }

        private void btnPermit_Click(object sender, EventArgs e)
        {
            bpm.Visible = true;
        }

        private void btnResidency_Click(object sender, EventArgs e)
        {
            clearance.Visible = true;
            residency.Visible = true;
        }

        private void btnphoto_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelDropdown.Visible = !panelDropdown.Visible;
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

        private void btnbck_Click(object sender, EventArgs e)
        {
            bpm.Visible = false;
        }


        private void SaveData()
        {
            // Connection string to the database
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=finalthesis.accdb;";

            // SQL query to insert data
            string query = "INSERT INTO permit (name_of_business, firstname, lastname, middle_initial, address, age, business_location, dit) " +
                           "VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

            // Establishing connection
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Adding parameters to prevent SQL injection
                        command.Parameters.AddWithValue("?", tnnb.Text);
                        command.Parameters.AddWithValue("?", tbpfn.Text);
                        command.Parameters.AddWithValue("?", tbpln.Text);
                        command.Parameters.AddWithValue("?", tbpmi.Text);
                        command.Parameters.AddWithValue("?", tbpadd.Text);
                        command.Parameters.AddWithValue("?", int.Parse(tbpage.Text)); // Convert age to integer
                        command.Parameters.AddWithValue("?", tbbl.Text);
                        command.Parameters.AddWithValue("?", datetp.Value); // DateTimePicker value

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data saved successfully!");
                        }
                        else
                        {
                            MessageBox.Show("No data was saved.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Get input values from the textboxes
                string name_of_business = tnnb.Text; // Permit Number
                string firstname = tbpfn.Text;
                string lastname = tbpln.Text;
                string middle_initial = tbpmi.Text;
                string age = tbpage.Text;
                string address = tbpadd.Text;
                string business_location = tbbl.Text;
                string dit = datetp.Value.ToString("yyyy-MM-dd"); // Get the Date from DateTimePicker control

                string captainName = "Wilfredo Danting"; // Replace with your Barangay Captain's name

                // Check if required fields are empty
                if (string.IsNullOrEmpty(name_of_business) || string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(middle_initial) ||
                    string.IsNullOrEmpty(lastname) || string.IsNullOrEmpty(age) || string.IsNullOrEmpty(address) ||
                    string.IsNullOrEmpty(business_location) || string.IsNullOrEmpty(dit))
                {
                    MessageBox.Show("Please fill in all the required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save data into the database
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "INSERT INTO permit (name_of_business, firstname, lastname, middle_initial, age, address, business_location, dit) " +
                                   "VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = name_of_business;           // Text (String)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = firstname;             // Text (String)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = lastname;              // Text (String)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = middle_initial;        // Text (String)
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(age);         // Integer (Age)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = address;               // Text (String)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = business_location;      // Text (String)
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Parse(dit);        // DateTime (Date)

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Generate Word document
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                var doc = wordApp.Documents.Add();

                // Set margins for the document
                doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(2);

                // Add content to the document
                var range = doc.Content;

                // Header Section
                range.Text = "                                                         Republic of the Philippines\n" +
                                 "                                                             Province of Pampanga\n" +
                                 "                                                             Municipality of Apalit\n" +
                                 "                                                             Barangay San Vicente\n";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 12;
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                range.InsertParagraphAfter();

                // Title
                range.InsertAfter("                                             OFFICE OF THE BARANGAY CAPTAIN\n" +
                                  "                                                   BARANGAY BUSINESS PERMIT\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                range.Font.Size = 16;
                range.Font.Bold = 2;
                range.InsertParagraphAfter();

                // Body
                range.InsertAfter("TO WHOM IT MAY CONCERN:\n");
                range.Font.Bold = 0;
                range.Font.Size = 14;
                range.InsertParagraphAfter();

                range.InsertAfter($"            This is to certify that {firstname} {middle_initial}. {lastname}, {age} years old, " +
                                 $"and a resident of Barangay {address}, is applying for a business permit for the business located at {business_location}.\n");
                range.InsertParagraphAfter();

                range.InsertAfter("             The business is in good standing and complies with the rules and regulations set by the Barangay.\n");
                range.InsertParagraphAfter();

                range.InsertAfter($"           ISSUED this {dit} at Barangay San Vicente, Apalit, Pampanga " +
                                 "upon request of the interested party for whatever legal purposes it may serve.\n\n\n");
                range.InsertParagraphAfter();

                // Signature Section
                range.InsertAfter($"                                                                                                  {captainName}\n" +
                                  $"                                                                                                   Barangay Captain\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                range.InsertParagraphAfter();

                // Footer Section
                range.InsertAfter($"Permit Number: {name_of_business}\nDate Issued: {dit}\nDoc. Stamp: Paid\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                range.InsertParagraphAfter();

                // Save and Open the Document
                string fileName = $"{name_of_business}_{firstname}_{lastname}_Business_Permit.docx";
                string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName;
                doc.SaveAs2(savePath);
                doc.Close();
                wordApp.Quit();

                MessageBox.Show($"Business Permit saved successfully to:\n{savePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optional: Open the document
                System.Diagnostics.Process.Start(savePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while generating the document or saving the record:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect form data from textboxes
                string Firstname = tbfn.Text;
                string Lastname = tbln.Text;
                string Middle_initial = tbmi.Text;
                string age = tbage.Text;
                string address = tbadd.Text;
                string Dt = dtp.Text;  // Assuming dtp is a DateTimePicker or TextBox for the date
                string captainName = "Wilfredo danting"; // Replace with your Barangay Captain's name

                // Check if required fields are empty
                if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Middle_initial) || string.IsNullOrEmpty(Lastname) ||
                    string.IsNullOrEmpty(age) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(Dt))
                {
                    MessageBox.Show("Please fill in all the required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the resident_id from the search (it was populated during the search)
                string resident_id = tbs.Text.Trim(); // Assuming tbs is where the resident ID was entered

                // Validate resident_id
                if (string.IsNullOrEmpty(resident_id))
                {
                    MessageBox.Show("No resident selected. Please search for a resident first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert data into the ClearanceRecord table in the database
                string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=finalthesis.accdb";
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "INSERT INTO ClearanceRecord (resident_id, fname, lname, m_initial, age, address, date_issued) " +
                                   "VALUES (?, ?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(resident_id);  // Assuming resident_id is numeric (integer)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = Firstname;               // Text (String)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = Lastname;                // Text (String)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = Middle_initial;          // Text (String)
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(age);           // Integer (Age)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = address;                 // Text (String)
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Parse(Dt);          // DateTime (Date)

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Initialize Word Interop for the clearance document
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                var doc = wordApp.Documents.Add();

                // Set margins for the document
                doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(2);

                // Add content to the document
                var range = doc.Content;

                // Header Section
                range.Text = "                                                         Republic of the Philippines\n" +
                             "                                                             Province of Pampanga\n" +
                             "                                                             Municipality of Apalit\n" +
                             "                                                             Barangay San Vicente\n";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 12;
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                range.InsertParagraphAfter();

                // Title
                range.InsertAfter("                                             OFFICE OF THE BARANGAY CAPTAIN\n" +
                                  "                                                        BARANGAY CLEARANCE\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                range.Font.Size = 16;
                range.Font.Bold = 2;
                range.InsertParagraphAfter();

                // Body
                range.InsertAfter("TO WHOM IT MAY CONCERN:\n");
                range.Font.Bold = 0;
                range.Font.Size = 14;
                range.InsertParagraphAfter();

                range.InsertAfter($"            This is to certify that {Firstname} {Middle_initial}. {Lastname}, {age} years old, " +
                                 $"and a resident of Barangay {address}, is known to be of good moral character " +
                                 "and a law-abiding citizen in the community.\n");
                range.InsertParagraphAfter();

                range.InsertAfter("             To certify further, that he/she has no derogatory and/or criminal records " +
                                 "filed in this barangay.\n");
                range.InsertParagraphAfter();

                range.InsertAfter($"           ISSUED this {Dt} at Barangay San Vicente, Apalit, Pampanga " +
                                 "upon request of the interested party for whatever legal purposes it may serve.\n\n\n");
                range.InsertParagraphAfter();

                // Signature Section
                range.InsertAfter($"                                                                                                  {captainName}\n" +
                                  $"                                                                                                   Barangay Captain\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                range.InsertParagraphAfter();

                // Footer Section
                range.InsertAfter($"O.R. No.: ________________\nDate Issued: {Dt}\nDoc. Stamp: Paid\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                range.InsertParagraphAfter();

                // Save and Open the Document
                string fileName = $"{Firstname}_{Middle_initial}_{Lastname}_Barangay_Clearance.docx";
                string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName;
                doc.SaveAs2(savePath);
                doc.Close();
                wordApp.Quit();

                MessageBox.Show($"Barangay clearance saved successfully to:\n{savePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optional: Open the document
                System.Diagnostics.Process.Start(savePath);

                // Now, update the dgvHistory in the staffHistory form with the new record.
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while generating the document or saving the record:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    





        private void sve_Click(object sender, EventArgs e)
        {

        }
        private void btnpoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pbpt.Image = System.Drawing.Image.FromFile(ofd.FileName);
                    photoFilePath = ofd.FileName; // Store the file path for debugging if needed
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading the image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

            private void bck_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
           


        }

        private void tbsc_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void tbsearch_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void tbs_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void indigency_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect form data from textboxes
                string Firstname = tbfn.Text.Trim();
                string Lastname = tbln.Text.Trim();
                string Middle_initial = tbmi.Text.Trim();
                string age = tbage.Text.Trim();
                string address = tbadd.Text.Trim();
                string purpose = txtprp.Text.Trim();
                string Dt = dtp.Text.Trim(); // Assuming dtp is a DateTimePicker or TextBox for the date
                string captainName = "Wilfredo Danting"; // Replace with your Barangay Captain's name

                // Check if required fields are empty
                if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Middle_initial) || string.IsNullOrEmpty(Lastname) ||
                    string.IsNullOrEmpty(age) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(Dt))
                {
                    MessageBox.Show("Please fill in all the required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the resident_id from the search (it was populated during the search)
                string resident_id = tbs.Text.Trim(); // Assuming tbs is where the resident ID was entered

                // Validate resident_id
                if (string.IsNullOrEmpty(resident_id))
                {
                    MessageBox.Show("No resident selected. Please search for a resident first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert data into the IndigencyRecord table in the database
                string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=finalthesis.accdb";
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "INSERT INTO IndigencyRecord (resident_id, fname, lname, m_initial, age, address, date_issued, purpose) " +
                                   "VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Ensure data types match the database schema
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(resident_id);  // Numeric (Integer)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = Firstname;             // Text
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = Lastname;              // Text
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = Middle_initial;        // Text
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(age);         // Numeric (Integer)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = address;               // Text
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Parse(Dt);        // Date/Time
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = purpose;               // Text

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Generate the Word Document
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                var doc = wordApp.Documents.Add();

                // Set document margins
                doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(2);

                // Add the Header
                foreach (Microsoft.Office.Interop.Word.Section section in doc.Sections)
                {
                    var headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    headerRange.Text = "Republic of the Philippines\n" +
                                       "Province of Pampanga\n" +
                                       "Municipality of Apalit\n" +
                                       "Barangay San Vicente";
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 12;
                    headerRange.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                }

                // Add Title
                var range = doc.Content;
                range.Text = "\nCERTIFICATE OF INDIGENCY\n";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 14;
                range.Font.Bold = 1;
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                range.InsertParagraphAfter();

                // Add Body Content
                range.InsertAfter($"\n\nTo Whom It May Concern,\n\n" +
                                  $"This is to certify that {Firstname} {Middle_initial}. {Lastname}, {age} years old, " +
                                  $"and a resident of {address}, is a recognized indigent of Barangay San Vicente.\n\n" +
                                  $"This certification is issued for the purpose of {purpose}.\n\n" +
                                  $"Issued on {Dt} at Barangay San Vicente, Apalit, Pampanga.\n\n\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                range.Font.Size = 12;
                range.Font.Bold = 0;

                // Add Photo
                if (pbpt.Image != null)
                {
                    string tempImagePath = Path.Combine(Path.GetTempPath(), "resident_photo.jpg");
                    pbpt.Image.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    // Insert the image
                    var imageRange = range.Duplicate;
                    imageRange.Collapse(WdCollapseDirection.wdCollapseStart);
                    var shape = imageRange.InlineShapes.AddPicture(tempImagePath);
                    shape.Width = 100;  // Adjust size as needed
                    shape.Height = 100;
                    File.Delete(tempImagePath); // Clean up the temp file
                }

                // Add Signature Section
                range.InsertAfter($"                                                                                                  {captainName}\n" +
                                  $"                                                                                                  Barangay Captain\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                // Save the Document
                string fileName = $"{Firstname}_{Lastname}_IndigencyCertificate.docx";
                string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                doc.SaveAs2(savePath);
                doc.Close();
                wordApp.Quit();

                MessageBox.Show($"Certificate saved successfully at:\n{savePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optional: Open the document
                System.Diagnostics.Process.Start(savePath);
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Data format error: {ex.Message}\nPlease ensure all numeric fields (age, resident_id) contain valid numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }





        }

        private void btnpt_Click(object sender, EventArgs e)
        {
            try
            {
                // Open a file dialog for selecting an image
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All Files (*.*)|*.*",
                    Title = "Select a Photo"
                };

                // Show the dialog and check if the user selected a file
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Load the selected image into the PictureBox
                    pbpt.Image = System.Drawing.Image.FromFile(ofd.FileName);

                    // Optionally resize the PictureBox image to fit the box
                    pbpt.SizeMode = PictureBoxSizeMode.StretchImage; 

                    MessageBox.Show("Photo loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the photo:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void residency_Click(object sender, EventArgs e)
        {
            try
            {
                // Collect form data from textboxes
                string Firstname = tbfn.Text;
                string Lastname = tbln.Text;
                string Middle_initial = tbmi.Text;
                string age = tbage.Text;
                string address = tbadd.Text;
                string Dt = dtp.Text;  // Assuming dtp is a DateTimePicker or TextBox for the date
                string captainName = "Wilfredo danting"; // Replace with your Barangay Captain's name

                // Check if required fields are empty
                if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Middle_initial) || string.IsNullOrEmpty(Lastname) ||
                    string.IsNullOrEmpty(age) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(Dt))
                {
                    MessageBox.Show("Please fill in all the required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the resident_id from the search (it was populated during the search)
                string resident_id = tbs.Text.Trim(); // Assuming tbs is where the resident ID was entered

                // Validate resident_id
                if (string.IsNullOrEmpty(resident_id))
                {
                    MessageBox.Show("No resident selected. Please search for a resident first.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Insert data into the ClearanceRecord table in the database
                string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=finalthesis.accdb";
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    string query = "INSERT INTO ResidencyRecord (resident_id, fname, lname, m_initial, age, address, date_issued) " +
                                   "VALUES (?, ?, ?, ?, ?, ?, ?)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(resident_id);  // Assuming resident_id is numeric (integer)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = Firstname;               // Text (String)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = Lastname;                // Text (String)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = Middle_initial;          // Text (String)
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(age);           // Integer (Age)
                        cmd.Parameters.Add("?", OleDbType.VarWChar).Value = address;                 // Text (String)
                        cmd.Parameters.Add("?", OleDbType.Date).Value = DateTime.Parse(Dt);          // DateTime (Date)

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Initialize Word Interop for the clearance document
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                var doc = wordApp.Documents.Add();

                // Set margins for the document
                doc.PageSetup.TopMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(2);
                doc.PageSetup.RightMargin = wordApp.CentimetersToPoints(2);

                // Add content to the document
                var range = doc.Content;

                // Header Section
                range.Text = "                                                         Republic of the Philippines\n" +
                             "                                                             Province of Pampanga\n" +
                             "                                                             Municipality of Apalit\n" +
                             "                                                             Barangay San Vicente\n";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 12;
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                range.InsertParagraphAfter();

                // Title
                range.InsertAfter("                                             OFFICE OF THE BARANGAY CAPTAIN\n" +
                                  "                                                        BARANGAY RESIDENCY\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                range.Font.Size = 16;
                range.Font.Bold = 2;
                range.InsertParagraphAfter();

                // Body
                range.InsertAfter("TO WHOM IT MAY CONCERN:\n");
                range.Font.Bold = 0;
                range.Font.Size = 14;
                range.InsertParagraphAfter();

                range.InsertAfter($"            This is to certify that {Firstname} {Middle_initial}. {Lastname}, {age} years old, " +
                                 $"and a resident of Barangay {address}, is known to be of good moral character " +
                                 "and a law-abiding citizen in the community.\n");
                range.InsertParagraphAfter();

                range.InsertAfter("             To certify further, that he/she has no derogatory and/or criminal records " +
                                 "filed in this barangay.\n");
                range.InsertParagraphAfter();

                range.InsertAfter($"           ISSUED this {Dt} at Barangay San Vicente, Apalit, Pampanga " +
                                 "upon request of the interested party for whatever legal purposes it may serve.\n\n\n");
                range.InsertParagraphAfter();

                // Signature Section
                range.InsertAfter($"                                                                                                  {captainName}\n" +
                                  $"                                                                                                   Barangay Captain\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                range.InsertParagraphAfter();

                // Footer Section
                range.InsertAfter($"O.R. No.: ________________\nDate Issued: {Dt}\nDoc. Stamp: Paid\n");
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                range.InsertParagraphAfter();

                // Save and Open the Document
                string fileName = $"{Firstname}_{Middle_initial}_{Lastname}_Barangay_Residency.docx";
                string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fileName;
                doc.SaveAs2(savePath);
                doc.Close();
                wordApp.Quit();

                MessageBox.Show($"Barangay Residency saved successfully to:\n{savePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Optional: Open the document
                System.Diagnostics.Process.Start(savePath);

                // Now, update the dgvHistory in the staffHistory form with the new record.

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while generating the document or saving the record:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void panelDropdown_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffHistory staffHistory = new staffHistory(Username);
            staffHistory.Show();
            this.Hide();
        }

        private void linkLabel5_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffHelp staffHelp = new staffHelp(Username);
            staffHelp.Show();
            this.Hide();
        }

        private void linkLabel4_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffAbout staffAbout = new staffAbout(Username);
            staffAbout.Show();
            this.Hide();
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        // Upload Photo Button

    }
}
    

