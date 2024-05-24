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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void exitPicBox_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void exitPicBox_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void backPicBox_Click(object sender, EventArgs e)
        {
            SignIn signIn = new SignIn();
            signIn.Show();
            this.Hide();
        }

        private void registerStudentButton_Click(object sender, EventArgs e)
        {
            RegisterStudent reg = new RegisterStudent();
            reg.Show();
            this.Hide();
        }

        private void StudentsButton_Click(object sender, EventArgs e)
        {
            Students std = new Students();
            std.Show();
            this.Hide();
        }

        private void editStudentsButton_Click(object sender, EventArgs e)
        {
            EditStudents editStudents = new EditStudents();
            editStudents.Show();
            this.Hide();
        }

        private void paymentButton_Click(object sender, EventArgs e)
        {
            Payment payment = new Payment();
            payment.Show();
            this.Hide();
        }
    }
}
