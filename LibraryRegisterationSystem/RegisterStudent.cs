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

namespace LibraryRegisterationSystem
{
    public partial class RegisterStudent : Form
    {
        public RegisterStudent()
        {
            InitializeComponent();
        }

        private void exitPicBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void backPicBox_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-G1HGNHG3\SQLEXPRESS;Initial Catalog=DormitoryRegisterationSystem;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void RegisterStudent_Load(object sender, EventArgs e)
        {

        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text == "" || ageTextBox.Text == "" || phoneTextBox.Text == "" || emailTextBox.Text == "" || countryComboBox.Text == "" || mPaymentTextBox.Text == "")
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
                    con.Open();
                    string query = "INSERT INTO StudentsTbl (Name, Age, Phone, Email, Country, [Monthly Payment]) VALUES (@Name, @Age, @Phone, @Email, @Country, @MonthlyPayment)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Name", nameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Age", ageTextBox.Text);
                    cmd.Parameters.AddWithValue("@Phone", phoneTextBox.Text);
                    cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);
                    cmd.Parameters.AddWithValue("@Country", countryComboBox.Text);
                    cmd.Parameters.AddWithValue("@MonthlyPayment", mPaymentTextBox.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student Successfully registered", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    refresh();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
        private void refresh()
        {
            nameTextBox.Text = "";
            ageTextBox.Text = "";
            phoneTextBox.Text = "";
            emailTextBox.Text = "";
            countryComboBox.Text = "";
            mPaymentTextBox.Text = "";
        }
        private void resetButton_Click(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
