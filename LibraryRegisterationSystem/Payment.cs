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
    public partial class Payment : Form
    {
        public Payment()
        {
            InitializeComponent();
        }

        private void exitPicBox_Click(object sender, EventArgs e)// Exits the application
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)// Goes back to the main menu
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void Refresh()// Emptying the fields, clears selection of the row and the id
        {
            paymentComboBox.Text = "";
            studentIDTextBox.Text = "";
            amountTextBox.Text = "";
            ClearSelection();
            key = 0;
        }
        private void resetButton_Click(object sender, EventArgs e)// Reseting the fields
        {
            Refresh();
        }
        // Connection string to connect to database
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-G1HGNHG3\SQLEXPRESS;Initial Catalog=DormitoryRegisterationSystem;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
      
        private void payButton_Click(object sender, EventArgs e)// Saves the payments to database
        {
            if (paymentComboBox.Text == "" || studentIDTextBox.Text == "" || amountTextBox.Text == "" )// Checking the fields if they are empty or not
            {
                MessageBox.Show("Missing Information!!", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {                    
                    //Validate numeric values
                    if (!int.TryParse(studentIDTextBox.Text, out int studentId) || !decimal.TryParse(amountTextBox.Text, out decimal amount))
                    {
                        MessageBox.Show("Student ID and Amount must be a numeric value!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    // Check if StudentId exists in StudentsTbl
                    con.Open();
                    string checkStudentQuery = "SELECT COUNT(*) FROM StudentsTbl WHERE [Student Id] = @StudentId";
                    SqlCommand checkStudentCmd = new SqlCommand(checkStudentQuery, con);
                    checkStudentCmd.Parameters.AddWithValue("@StudentId", studentId);

                    int studentExists = (int)checkStudentCmd.ExecuteScalar();

                    if (studentExists == 0)
                    {
                        MessageBox.Show("Student does not exist!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        con.Close();
                        return;
                    }

                    string query = "INSERT INTO PaymentTbl ([Payment Month], [Student Id], Amount) VALUES (@PMonth, @SId, @Amount)";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@PMonth", paymentComboBox.Text);
                    cmd.Parameters.AddWithValue("@SId", studentIDTextBox.Text);
                    cmd.Parameters.AddWithValue("@Amount", amountTextBox.Text);                    
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Payment Has Been Done Successfully", "Payment Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();
                    Refresh();
                    ClearSelectionDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }
        private void Populate()// Loads the data from database
        {
            con.Open();
            string query = "select * from PaymentTbl";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            var ds = new DataSet();
            adapter.Fill(ds);
            paymentsDataGridView.DataSource = ds.Tables[0];
            con.Close();
        }

        private void ClearSelection()// Clears the selection of row
        {
            if (paymentsDataGridView.Rows.Count > 0)
            {
                paymentsDataGridView.ClearSelection();
            }
        }

        private void ClearSelectionDataGrid()// Clears the selection of the row and prevents showing it in the fields
        {
            // Clear any existing event handlers to avoid duplicate invocations
            paymentsDataGridView.SelectionChanged -= paymentsDataGridView_SelectionChanged;            
            Populate();            
            ClearSelection();
            // Re-subscribe to the SelectionChanged event
            paymentsDataGridView.SelectionChanged += paymentsDataGridView_SelectionChanged;
        }
        private void Payment_Load(object sender, EventArgs e)// When page is load the data is shown in data grid view from database
        {
            ClearSelectionDataGrid();
        }

        int key = 0;
        private void paymentsDataGridView_SelectionChanged(object sender, EventArgs e)// Shows selected rows' data in text boxes
        {
            if (paymentsDataGridView.SelectedRows.Count > 0 && paymentsDataGridView != null)
            {
                var selectedRow = paymentsDataGridView.SelectedRows[0];
                key = Convert.ToInt32(selectedRow.Cells[0].Value.ToString());
                paymentComboBox.Text = selectedRow.Cells[1].Value.ToString();
                studentIDTextBox.Text = selectedRow.Cells[2].Value.ToString();
                amountTextBox.Text = selectedRow.Cells[3].Value.ToString();                
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)// Deletes selected payment from database according to payment id
        {
            if (key == 0)
            {
                MessageBox.Show("Select the payment to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "delete from PaymentTbl where PId=" + key + ";";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Payment deleted successfully", "Successfully Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);                   
                    con.Close();
                    Refresh();
                    ClearSelectionDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private void updateButton_Click(object sender, EventArgs e)// Updates selected payment's data
        {
            if (key == 0 || paymentComboBox.Text == "" || studentIDTextBox.Text == "" || amountTextBox.Text == "")
            {
                MessageBox.Show("Missing Information!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    con.Open();
                    string query = "update PaymentTbl set [Payment Month]='" + paymentComboBox.Text + "', [Student Id]='" + studentIDTextBox.Text + "', Amount='" + amountTextBox.Text + "' where PId=" + key + ";";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Payment updated successfully", "Successfully Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);                   
                    con.Close();
                    Refresh();
                    ClearSelectionDataGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)// Refreshes search field
        {
            paymentsSearchTextBox.Text = "";
        }

        private void searchButton_Click(object sender, EventArgs e)// Searches according to Student Id from database
        {
            try
            {
                con.Open();
                string query = "SELECT * FROM PaymentTbl WHERE [Student Id] LIKE @SId";// Selecting from database according to name
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.SelectCommand.Parameters.AddWithValue("@SId", $"%{paymentsSearchTextBox.Text}%");
                var ds = new DataSet();
                paymentsDataGridView.SelectionChanged -= paymentsDataGridView_SelectionChanged;
                adapter.Fill(ds);
                paymentsDataGridView.DataSource = ds.Tables[0];
                paymentsDataGridView.ClearSelection();
                paymentsDataGridView.SelectionChanged += paymentsDataGridView_SelectionChanged;
                con.Close();                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }        
    }
}
