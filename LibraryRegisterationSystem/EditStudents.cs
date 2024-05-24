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

namespace LibraryRegisterationSystem
{
    public partial class EditStudents : Form
    {
        public EditStudents()
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
        private void populate()
        {
            con.Open();
            string query = "select * from StudentsTbl";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            var ds = new DataSet();
            adapter.Fill(ds);
            studentsDataGridView.DataSource = ds.Tables[0];
            con.Close();
        }

        private void EditStudents_Load(object sender, EventArgs e)
        {
            populate();
        }

        private int key = 0;
        private void studentsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (studentsDataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = studentsDataGridView.SelectedRows[0];
                key = Convert.ToInt32(selectedRow.Cells[0].Value.ToString());
                nameTextBox.Text = selectedRow.Cells[1].Value.ToString();
                ageTextBox.Text = selectedRow.Cells[2].Value.ToString();
                phoneTextBox.Text = selectedRow.Cells[3].Value.ToString();
                emailTextBox.Text = selectedRow.Cells[4].Value.ToString();
                countryComboBox.Text = selectedRow.Cells[5].Value.ToString();
                mPaymentTextBox.Text = selectedRow.Cells[6].Value.ToString();
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

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Select the student to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "delete from StudentsTbl where Id=" + key + ";";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student deleted successfully", "Successfully Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refresh();
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (key == 0 || nameTextBox.Text == "" || ageTextBox.Text == "" || phoneTextBox.Text == "" || emailTextBox.Text == "" || countryComboBox.Text == "" || mPaymentTextBox.Text == "")
            {
                MessageBox.Show("Missing Information!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "update StudentsTbl set Name='"+ nameTextBox.Text+"', Age='"+ageTextBox.Text+"', Phone='"+phoneTextBox.Text+"', Email='"+emailTextBox.Text+"', Country='"+countryComboBox.Text+"', [Monthly Payment]='"+mPaymentTextBox.Text+"' where Id="+key+";";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student updated successfully", "Successfully Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refresh();
                    con.Close();
                    populate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
    }
}
