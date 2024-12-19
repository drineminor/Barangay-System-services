using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Final_thesis.staffServices;

namespace Final_thesis
{
    public partial class historyRecords : Form
    {
        public string Username { get; set; } // Public property for username

        public historyRecords(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;  // Set the Username property
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            hisDD.Visible = !hisDD.Visible;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelDropdown.Visible = !panelDropdown.Visible;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CRUD CRUD = new CRUD(Username);
            CRUD.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
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

        private void button5_Click(object sender, EventArgs e)
        {
            historyRecords historyRecords = new historyRecords(Username);
            historyRecords.Show();
            this.Hide();
        }

        private void historyRecords_Load(object sender, EventArgs e)
        {
            label1.Parent = pictureBox2;
            label1.BackColor = Color.Transparent;

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

        private void button1_Click(object sender, EventArgs e)
        {
            adminProfile adminProfile = new adminProfile(Username);
            adminProfile.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CRUD CRUD = new CRUD(Username);
            CRUD.Show();
            this.Hide();
        }
    }
}
