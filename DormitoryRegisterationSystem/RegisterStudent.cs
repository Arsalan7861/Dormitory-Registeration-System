using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DormitoryRegisterationSystem
{
    public partial class RegisterStudent : Form
    {
        public RegisterStudent()
        {
            InitializeComponent();
        }

        private void exitPicBox_Click(object sender, EventArgs e)// Exits the application
        {
            Application.Exit();
        }

        private void backPicBox_Click(object sender, EventArgs e)// Goes back to the main menu
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }
        // Connection string for connecting to database
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-G1HGNHG3\SQLEXPRESS;Initial Catalog=DormitoryRegisterationSystem;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void registerButton_Click(object sender, EventArgs e)// Button when clicked adds the student into the databse
        {
            if (nameTextBox.Text == "" || ageTextBox.Text == "" || phoneTextBox.Text == "" || emailTextBox.Text == "" || countryComboBox.Text == "" || mPaymentTextBox.Text == "")//Checking the fields if it is empty
            {
                MessageBox.Show("Missing Information!!", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    // Validate numeric fields
                    if (!int.TryParse(ageTextBox.Text, out int age) || !long.TryParse(phoneTextBox.Text, out long phone) || !decimal.TryParse(mPaymentTextBox.Text, out decimal monthlyPayment))
                    {
                        MessageBox.Show("Age, Phone, Monthly Payment must be a numeric value!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // Validate email format
                    if (!emailTextBox.Text.Contains("@") || !emailTextBox.Text.EndsWith(".com"))
                    {
                        MessageBox.Show("Email must contain '@' and end with '.com'!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // Validate country
                    if (!countryComboBox.Items.Contains(countryComboBox.Text))
                    {
                        MessageBox.Show("Country must be one of the listed items!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    con.Open();// Adding entered infotrmation into database.
                    string query = "INSERT INTO StudentsTbl (Name, Age, Phone, Email, Country, [Monthly Payment]) VALUES (@Name, @Age, @Phone, @Email, @Country, @MonthlyPayment)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", nameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Age", ageTextBox.Text);
                    cmd.Parameters.AddWithValue("@Phone", phoneTextBox.Text);
                    cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);
                    cmd.Parameters.AddWithValue("@Country", countryComboBox.Text);
                    cmd.Parameters.AddWithValue("@MonthlyPayment", mPaymentTextBox.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Successfully registered", "Registration Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();// Closing the connection after finishing process of adding
                    Refresh();// Refreshing the field method
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }
        }

        private void Refresh()// Emptying the fields
        {
            nameTextBox.Clear();
            ageTextBox.Clear();
            phoneTextBox.Clear();
            emailTextBox.Clear();
            countryComboBox.Text = "";
            mPaymentTextBox.Clear();
        }

        private void resetButton_Click(object sender, EventArgs e)// Reseting the fields
        {
            Refresh();
        }

        private void countryComboBox_Validating(object sender, CancelEventArgs e)// User can only select items that are in combo box
        {
            ComboBox comboBox = sender as ComboBox;
            if (!comboBox.Items.Contains(comboBox.Text))
            {
                MessageBox.Show("Please select a valid country from the list.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true; // Prevents the user from leaving the ComboBox
            }
        }
    }
}
