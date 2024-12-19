using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_thesis
{
    public partial class staffAbout : Form
    {
        public string Username { get; set; } // Public property for username
        public staffAbout(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;  // Set the Username property
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void staffAbout_Load(object sender, EventArgs e)
        {
            label8.Parent = pictureBox2;
            label8.BackColor = System.Drawing.Color.Transparent;
            linkLabel1.Parent = pictureBox2;
            linkLabel1.BackColor = System.Drawing.Color.Transparent;
            linkLabel2.Parent = pictureBox2;
            linkLabel2.BackColor = System.Drawing.Color.Transparent;
            linkLabel4.Parent = pictureBox2;
            linkLabel4.BackColor = System.Drawing.Color.Transparent;
            linkLabel5.Parent = pictureBox2;
            linkLabel5.BackColor = System.Drawing.Color.Transparent;
            label2.Parent = pictureBox2;
            label2.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.Parent = pictureBox2;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.Parent = pictureBox2;
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            label1.Parent = pictureBox2;
            label1.BackColor = System.Drawing.Color.Transparent;
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

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelDropdown.Visible = !panelDropdown.Visible;
        }
    }
}
