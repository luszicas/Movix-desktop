using System.Net.Http.Json;

namespace Movix.Desktop
{
	public partial class frmCadastroUsuario : Form
	{
		private const string ApiBaseUrl = "http://localhost:5193/";
		private string _idUsuarioEditando = "";

		public frmCadastroUsuario()
		{
			InitializeComponent();
		}

		public frmCadastroUsuario(string idUsuario)
		{
			InitializeComponent();
			_idUsuarioEditando = idUsuario;
		}

        private async void frmCadastroUsuario_Load(object sender, EventArgs e)
        {
            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add("Admin");
            guna2ComboBox1.Items.Add("Gerente");
            guna2ComboBox1.Items.Add("Usuario"); // REMOVIDO O ACENTO AQUI
            guna2ComboBox1.SelectedIndex = -1;

            if (!string.IsNullOrEmpty(_idUsuarioEditando))
            {
                await CarregarDadosParaEdicao();
                txtSenha.PlaceholderText = "Deixe em branco para não alterar";
            }
        }

        private async Task CarregarDadosParaEdicao()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(ApiBaseUrl);
				try
				{
					var usuario = await client.GetFromJsonAsync<UsuarioDto>($"api/usuarios/{_idUsuarioEditando}");
					if (usuario != null)
					{
						txtEmail.Text = usuario.Email;
						txtUsername.Text = usuario.Username;
						guna2ComboBox1.SelectedItem = usuario.Perfil;
					}
				}
				catch (Exception ex) { MessageBox.Show("Erro ao carregar: " + ex.Message); }
			}
		}

		private async void btnSalvar_Click(object sender, EventArgs e)
		{
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Preencha os campos obrigatórios!");
                return;
            }

            var dados = new
            {
                Email = txtEmail.Text,
                Username = txtUsername.Text,
                Senha = txtSenha.Text,
                // Garante que envie "Usuario" se não houver nada selecionado
                Perfil = guna2ComboBox1.SelectedItem?.ToString() ?? "Usuario"
            };

            using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(ApiBaseUrl);
				try
				{
					HttpResponseMessage response;

					if (string.IsNullOrEmpty(_idUsuarioEditando))
						response = await client.PostAsJsonAsync("api/usuarios", dados);
					else
						response = await client.PutAsJsonAsync($"api/usuarios/{_idUsuarioEditando}", dados);

					// Forçamos a verificação do sucesso
					if (response.IsSuccessStatusCode)
					{
						MessageBox.Show("Usuário salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
						this.DialogResult = DialogResult.OK; // Define o resultado do formulário
						this.Close(); // Fecha a tela
					}
					else
					{
						var erro = await response.Content.ReadAsStringAsync();
						MessageBox.Show("Erro ao salvar: " + erro);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Erro de conexão: " + ex.Message);
				}
			}
		}
       

        private void txtEmail_TextChanged(object sender, EventArgs e) { }
		private void txtUsername_TextChanged(object sender, EventArgs e) { }
		private void txtSenha_TextChanged(object sender, EventArgs e) { }
		private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
	}
}