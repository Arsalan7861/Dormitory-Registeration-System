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
    public partial class Students : Form
    {
        public Students()
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
        // Giving the connection string to connect to database
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-G1HGNHG3\SQLEXPRESS;Initial Catalog=DormitoryRegisterationSystem;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        private void Populate()// Loading data from the database to data grid view
        {
            con.Open();
            string query = "select * from StudentsTbl";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            var ds = new DataSet();
            adapter.Fill(ds);
            studentsDataGridView.DataSource = ds.Tables[0];
            con.Close();
        }
        private void Students_Load(object sender, EventArgs e)// When the page is loaded shows the students data
        {
            Populate();
        }

        private void refreshButton_Click(object sender, EventArgs e)// Emptying the search field
        {
            searchTextBox.Clear();
        }

        private void searchButton_Click(object sender, EventArgs e)// Searching from database
        {
            try
            {
                con.Open();//Starting the connection
                string query = "SELECT * FROM StudentsTbl WHERE Name LIKE @Name";// Selecting from database according to name
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.SelectCommand.Parameters.AddWithValue("@Name", $"%{searchTextBox.Text}%");
                var ds = new DataSet();
                adapter.Fill(ds);
                studentsDataGridView.DataSource = ds.Tables[0];
                con.Close();// Closing the connection after finishing the process
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
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
