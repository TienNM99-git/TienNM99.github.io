using FamilyManagementSoftware.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FamilyManagementSoftware
{
    public partial class Authentication : Form
    {
        //public Authentication()
        //{
        //    InitializeComponent();
        //}

        

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
      
        private bool Authenticate(string userName, string passWord)
        {
            return DAOAccount.Instance.Login(userName, passWord);
        }
        private void Authentication_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text;
            string passWord = txtPassword.Text;
            if (Authenticate(userName, passWord))
            {
                Form form1 = new Form1();
                this.Hide();
                form1.ShowDialog();
                //this.Show();
            }
            else
            {
                MessageBox.Show("Invalid useranme or passwrod !");
            }

        }
    }
}
