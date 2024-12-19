using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_thesis
{
    public partial class staffHelp : Form
    {
        public string Username { get; set; } // Public property for username
        public staffHelp(string loggedInUsername)
        {
            InitializeComponent();
            Username = loggedInUsername;  // Set the Username property

        }

        private void staffHelp_Load(object sender, EventArgs e)
        {
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
            label2.Parent = pictureBox1;
            label2.BackColor = System.Drawing.Color.Transparent;
            pictureBox4.Parent = pictureBox1;
            pictureBox4.BackColor = System.Drawing.Color.Transparent;
            pictureBox2.Parent = pictureBox1;
            pictureBox2.BackColor = System.Drawing.Color.Transparent;
            label1.Parent = pictureBox1;
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

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            staffAbout staffAbout = new staffAbout(Username);
            staffAbout.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelDropdown.Visible = !panelDropdown.Visible;
        }
    }
}
