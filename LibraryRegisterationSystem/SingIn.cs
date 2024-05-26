using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryRegisterationSystem
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void exitPicBox_Click(object sender, EventArgs e)// Exit the application method
        {
            Application.Exit();
        }

        private void signInButton_Click(object sender, EventArgs e)// Sign in button 
        {
            //Checking the user name and password
            if (emailTextBox.Text == "admin@yahoo.com" && passTextBox.Text == "1234")
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect email or Password!!", "Incorrect", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
            }
        }

        private void resetButton_Click(object sender, EventArgs e)// Reseting the fields
        {
            emailTextBox.Text = "";
            passTextBox.Text = "";
        }
    }
}
