using System;
using System.Windows.Forms;
using TravelModel.domain;
using TravelServices.service;

namespace TravelClient.gui
{
    public partial class LoginForm : Form
    {
        private readonly IService _loginService;

        public LoginForm(IService loginService)
        {
            
            _loginService = loginService; 
            InitializeComponent();
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
               
                MainForm mainForm = new MainForm(_loginService);

                Agent agent = _loginService.Login(username, password, mainForm);

                mainForm.SetLoggedAgent(agent);
                
                mainForm.Show();

                // Ascundem formularul de login
                this.Hide();
            }
            catch (Exception ex)
            {
                // Gestionăm erorile de autentificare
                MessageBox.Show($"Login failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        

    }
}