using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_thesis
{
    public partial class attendane : Form
    {
        public string Username { get; set; } // Public property for username
        private OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=finalthesis.accdb");


        public attendane(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;  // Set the Username property

        }






        private void attendane_Load(object sender, EventArgs e)
        {
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
       

        // Event handler for the login button click


        private void hisDD_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panelDropdown_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.ShowDialog();
            this.Hide();    
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            hisDD.Visible = !hisDD.Visible;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelDropdown.Visible = !panelDropdown.Visible;
        }
    }


}




        
    





