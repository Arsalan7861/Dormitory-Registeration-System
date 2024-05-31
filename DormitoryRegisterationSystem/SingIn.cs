using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DormitoryRegisterationSystem
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
        // Connection string for connecting to database
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-G1HGNHG3\SQLEXPRESS;Initial Catalog=DormitoryRegisterationSystem;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void signInButton_Click(object sender, EventArgs e)// Sign in button 
        {
            if (emailTextBox.Text == "" && passTextBox.Text == "")
            {
                MessageBox.Show("Missing information!!", "Incorrect", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {                    
                    con.Open();
                    string query = "SELECT COUNT(*) FROM UsersTbl WHERE Email=@Email AND Password=@Pass";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);
                    cmd.Parameters.AddWithValue("@Pass", passTextBox.Text);

                    int userCount = (int)cmd.ExecuteScalar();
                    con.Close();

                    if (userCount > 0)
                    {
                        // Email and password are correct, navigate to the main menu
                        Main mainMenu = new Main();
                        mainMenu.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Email and/or password are incorrect
                        MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Clear the text boxes to allow the user to try again
                        emailTextBox.Clear();
                        passTextBox.Clear();
                        emailTextBox.Focus(); // Set focus back to the email text box
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private void resetButton_Click(object sender, EventArgs e)// Reseting the fields
        {
            emailTextBox.Clear();
            passTextBox.Clear();
        }

        private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e)//When checked shows the password
        {
            this.passTextBox.UseSystemPasswordChar = !this.showPasswordCheckBox.Checked;
        }

        private void signUpButton_Click(object sender, EventArgs e)// Goes to sign up page
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Hide();
        }
    }
}
