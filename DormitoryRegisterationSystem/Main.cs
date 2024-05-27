using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DormitoryRegisterationSystem
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }    

        private void exitPicBox_Click_1(object sender, EventArgs e)// Exit the application method
        {
            Application.Exit();
        }

        private void backPicBox_Click(object sender, EventArgs e)// Goes back to the sign in page.
        {
            SignIn signIn = new SignIn();
            signIn.Show();
            this.Hide();
        }

        private void registerStudentButton_Click(object sender, EventArgs e)// When clicked goes to registeration page.
        {
            RegisterStudent reg = new RegisterStudent();
            reg.Show();
            this.Hide();
        }

        private void StudentsButton_Click(object sender, EventArgs e)// When clicked goes to students page.
        {
            Students std = new Students();
            std.Show();
            this.Hide();
        }

        private void editStudentsButton_Click(object sender, EventArgs e)// When clicked shows the page of students that we can edit.
        {
            EditStudents editStudents = new EditStudents();
            editStudents.Show();
            this.Hide();
        }

        private void paymentButton_Click(object sender, EventArgs e)// When clicked goes to payments page.
        {
            Payment payment = new Payment();
            payment.Show();
            this.Hide();
        }
    }
}
