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
    public partial class Students : Form
    {
        public Students()
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
        private void Students_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            searchTextBox.Text = "";
        }
    }
}
