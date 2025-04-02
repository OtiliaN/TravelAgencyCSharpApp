using System;
using System.Windows.Forms;
using TravelAgency.service;

namespace TravelAgency
{
    public partial class LoginForm : Form
    {
        private readonly Service _loginService;

        public LoginForm(Service loginService)
        {
            InitializeComponent();
            _loginService = loginService;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            bool isLoggedIn = _loginService.Login(username, password);

            if (isLoggedIn)
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MainForm mainForm =  new MainForm(_loginService);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}