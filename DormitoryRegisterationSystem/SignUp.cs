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

namespace DormitoryRegisterationSystem
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e)//When checked shows the password
        {
            this.passTextBox.UseSystemPasswordChar = !this.showPasswordCheckBox.Checked;
        }

        private void exitPicBox_Click(object sender, EventArgs e)// Exits application
        {
            Application.Exit();
        }

        SignIn signIn = new SignIn();
        private void backPicBox_Click(object sender, EventArgs e)// Goes back to sign in page
        {            
            signIn.Show();
            this.Hide();
        }
        // Connection string for connecting to database
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-G1HGNHG3\SQLEXPRESS;Initial Catalog=DormitoryRegisterationSystem;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void signUpButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "" || emailTextBox.Text == "" || passTextBox.Text == "")
            {
                MessageBox.Show("Missing Information!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    // Validate email format
                    if (!emailTextBox.Text.Contains("@") || !emailTextBox.Text.EndsWith(".com"))
                    {
                        MessageBox.Show("Email must contain '@' and end with '.com'!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    con.Open();// Open the connection for adding user to database
                    string query = "insert into UsersTbl (Name, Email, Password) values (@Name, @Email, @Pass)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", nameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);
                    cmd.Parameters.AddWithValue("@Pass", passTextBox.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Successfully registered", "Registration Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();// Close the connection after adding  
                    signIn.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }
        }
        
        private void resetButton_Click(object sender, EventArgs e)// Empty the fields
        {
            nameTextBox.Clear();
            emailTextBox.Clear();
            passTextBox.Clear();
        }
    }
}
