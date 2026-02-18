using System.Net.Http.Json;

namespace Movix.Desktop
{
    public partial class frmRegistrar : Form
    {
        // Altere a porta se a sua API estiver rodando em outra
        private const string ApiBaseUrl = "http://localhost:5193/";

        public frmRegistrar()
        {
            InitializeComponent();
        }

        // --- BOTÃO DE CADASTRAR (Ação de Registro) ---


        // --- BOTÃO VOLTAR (O seu btnCriar que volta pro login) ---


        // Eventos vazios para o Designer não bugar
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtSenha_TextChanged(object sender, EventArgs e) { }
        private void txtConfirmarSenha_TextChanged(object sender, EventArgs e) { }

		private async void btnRegistrar_Click(object sender, EventArgs e)
		{
			// 1. Validação de campos vazios
			if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
			{
				MessageBox.Show("Por favor, preencha o e-mail e a senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// 2. Validação de confirmação de senha (igual ao seu site)
			if (txtSenha.Text != txtConfirmarSenha.Text)
			{
				MessageBox.Show("As senhas não coincidem!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// 3. Monta o objeto para enviar à API
			var dadosRegistro = new
			{
				Email = txtEmail.Text,
				Senha = txtSenha.Text
			};

			// 4. Envio para o AuthController na rota de registrar
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(ApiBaseUrl);
				try
				{
					// Bate na rota "api/auth/registrar" que criamos na API
					var response = await client.PostAsJsonAsync("api/auth/registrar", dadosRegistro);

					if (response.IsSuccessStatusCode)
					{
						MessageBox.Show("Conta criada com sucesso! Faça login agora.", "Movix", MessageBoxButtons.OK, MessageBoxIcon.Information);
						this.Close(); // Fecha e volta para o Login
					}
					else
					{
						var erro = await response.Content.ReadAsStringAsync();
						MessageBox.Show("Erro ao registrar: " + erro, "Ops!");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Não foi possível conectar à API: " + ex.Message);
				}
			}
		}
	}
}