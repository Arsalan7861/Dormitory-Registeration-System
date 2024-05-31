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
    public partial class EditStudents : Form
    {
        public EditStudents()
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
        //Connection string for connection to database
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-G1HGNHG3\SQLEXPRESS;Initial Catalog=DormitoryRegisterationSystem;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void Populate()// Loads the data from database and shows it in data grid view
        {
            con.Open();
            string query = "select * from StudentsTbl";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            var ds = new DataSet();
            adapter.Fill(ds);
            studentsDataGridView.DataSource = ds.Tables[0];
            con.Close();
        }

        private void ClearSelection()// Clears row selection in data grid view
        {
            if (studentsDataGridView.Rows.Count > 0)
            {
                studentsDataGridView.ClearSelection();
            }
        }

        private void ClearSelectionDataGrid()// Clears the selection of the row and prevents showing it in the fields
        {
            // Clear any existing event handlers to avoid duplicate invocations
            studentsDataGridView.SelectionChanged -= studentsDataGridView_SelectionChanged;
            Populate();
            ClearSelection();
            // Re-subscribe to the SelectionChanged event
            studentsDataGridView.SelectionChanged += studentsDataGridView_SelectionChanged;
        }

        private void EditStudents_Load(object sender, EventArgs e)// Loads the data from database when page is loaded
        {
            ClearSelectionDataGrid();
        }

        private int key = 0;// to store id of student from database
        private void studentsDataGridView_SelectionChanged(object sender, EventArgs e)// Shows selected rows' data in text boxes
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

        private void Refresh()// Refreshes the fields, clears the selection of the row and the id of the selected student
        {
            nameTextBox.Clear();
            ageTextBox.Clear();
            phoneTextBox.Clear();
            emailTextBox.Clear();
            countryComboBox.Text = "";
            mPaymentTextBox.Clear();
            studentsDataGridView.ClearSelection();
            key = 0;
        }

        private void resetButton_Click(object sender, EventArgs e)// Refresh method is called
        {
            Refresh();
        }

        private void deleteButton_Click(object sender, EventArgs e)// Deletes the selected student from database according to id
        {
            if (key == 0)
            {
                MessageBox.Show("Select the student to delete!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    con.Open();// Starting the connection
                    // Check if the student exists in the Payment table
                    string paymentCheckQuery = "SELECT COUNT(*) FROM PaymentTbl WHERE [Student Id] = @StudentId";
                    SqlCommand paymentCheckCmd = new SqlCommand(paymentCheckQuery, con);
                    paymentCheckCmd.Parameters.AddWithValue("@StudentId", key);
                    int paymentCount = (int)paymentCheckCmd.ExecuteScalar();

                    if (paymentCount > 0)
                    {
                        // Student exists in Payment table, show warning
                        MessageBox.Show("Cannot delete student as they have payments recorded.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        con.Close();// Closing the connection after finishing the process
                        Refresh();
                    }
                    else
                    {
                        // Student does not exist in Payment table, proceed with deletion
                        string deleteQuery = "DELETE FROM StudentsTbl WHERE [Student Id] = @StudentId";
                        SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
                        deleteCmd.Parameters.AddWithValue("@StudentId", key);
                        deleteCmd.ExecuteNonQuery();
                        MessageBox.Show("Student deleted successfully", "Successfully Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        con.Close();// Closing the connection after finishing the process
                        Refresh();// Refreshing the fields
                        ClearSelectionDataGrid(); // Clearing the selection
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();                    
                }
            }
        }

        private void updateButton_Click(object sender, EventArgs e)// Updating the information of selected student and adds it to database
        {
            if (nameTextBox.Text == "" || ageTextBox.Text == "" || phoneTextBox.Text == "" || emailTextBox.Text == "" || countryComboBox.Text == "" || mPaymentTextBox.Text == "")// Controling if the fields are empty or not
            {
                MessageBox.Show("Missing Information!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    if (key == 0)//Controls if student is selected to update or not                    
                    {
                        MessageBox.Show("Select the Student to update!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                    }
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
                    con.Open();// Starts the connection
                    string query = "update StudentsTbl set Name='" + nameTextBox.Text + "', Age='" + ageTextBox.Text + "', Phone='" + phoneTextBox.Text + "', Email='" + emailTextBox.Text + "', Country='" + countryComboBox.Text + "', [Monthly Payment]='" + mPaymentTextBox.Text + "' where [Student Id]=" + key + ";";// Updates the data
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student updated successfully", "Successfully Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();// Closing the conncetion after updating
                    Refresh();
                    ClearSelectionDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();                    
                }
            }
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

        private void studentsDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Auto size columns and rows
            studentsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            studentsDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Wrap text in cells and set alignment
            foreach (DataGridViewColumn column in studentsDataGridView.Columns)
            {
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // or DataGridViewAutoSizeColumnMode.Fill
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; // or any other alignment
            }

            // Clear any initial selection
            studentsDataGridView.ClearSelection();
        }
    }
}
