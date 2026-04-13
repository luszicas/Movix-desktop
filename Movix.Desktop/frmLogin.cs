using System.Net.Http.Json;
using Movix.Application.DTOs;

namespace Movix.Desktop
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

       

        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            var loginData = new LoginDto
            {
                Email = txtEmail.Text,
                Senha = txtSenha.Text
            };

            using (var client = new HttpClient())
            {
                // Porta e Protocolo (HTTP) corretos
                client.BaseAddress = new Uri("http://localhost:5193");

                try
                {
                    var response = await client.PostAsJsonAsync("api/auth/login", loginData);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Login realizado com sucesso!");

                        // Navegação
                        this.Hide();
                        var dashboard = new frmDashboard(); // <-- MUDOU AQUI
                        dashboard.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        // --- AQUI ESTÁ A MUDANÇA PARA DESCOBRIR O ERRO ---
                        var statusCode = response.StatusCode; // Ex: 401, 404, 500
                        var mensagemErro = await response.Content.ReadAsStringAsync(); // Lê o texto que a API devolveu

                        MessageBox.Show($"Login falhou!\n\nCódigo: {statusCode}\nDetalhes: {mensagemErro}", "Erro de Login");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro de conexão (API desligada?): " + ex.Message);
                }
            }
        }

      

        private void txtEmail_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load_1(object sender, EventArgs e)
        {

        }

      

		
	}
}