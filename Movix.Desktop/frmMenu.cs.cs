using System.Net.Http.Json;
using Movix.Application.DTOs;

namespace Movix.Desktop
{
    public partial class frmMenu : Form
    {
        // CONFIRA SE A PORTA É ESSA MESMO NO SEU LAUNCHSETTINGS.JSON
        // Se no navegador abre em 5193, aqui tem que ser 5193.
        private const string ApiBaseUrl = "http://localhost:5193/";

        public frmMenu()
        {
            InitializeComponent();
        }

        private async void frmMenu_Load(object sender, EventArgs e)
        {
            await CarregarFilmes();
        }

        private async Task CarregarFilmes()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                try
                {
                    // Busca o pacote "PagedResult" na API
                    var pacote = await client.GetFromJsonAsync<PagedResult<FilmeDto>>("api/filmes");

                    // Abre o pacote e joga a lista (.Items) no Grid
                    if (pacote != null && pacote.Items != null)
                    {
                        // O .ToList() ajuda a evitar erros de binding no Grid
                        dataGridView1.DataSource = pacote.Items.ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao buscar filmes: " + ex.Message);
                }
            }
        }

        // --- AQUI ESTÁ A CORREÇÃO DO BOTÃO ---
        private async void btnNovo_Click(object sender, EventArgs e)
        {
            // 1. Cria e abre a tela de cadastro
            var frm = new frmCadastro();
            frm.ShowDialog();

            // 2. O código espera a tela fechar e então recarrega a lista
            await CarregarFilmes();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Futuro: Edição e Exclusão
        }

		// --- BOTÃO EXCLUIR ---
		private async void btnExcluir_Click(object sender, EventArgs e)
		{
			// Verifica se tem alguma linha selecionada no Grid
			if (dataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("Por favor, selecione um filme na tabela para excluir.", "Aviso");
				return;
			}

			// Pega o ID do filme da linha selecionada (assumindo que a coluna se chama "Id" e é int)
			var idFilme = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

			// Confirmação de segurança
			var confirmacao = MessageBox.Show("Tem certeza que deseja excluir este filme?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
			if (confirmacao == DialogResult.Yes)
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(ApiBaseUrl);
					try
					{
						// Envia o comando DELETE para a API (ex: /api/filmes/36)
						var response = await client.DeleteAsync($"api/filmes/{idFilme}");

						if (response.IsSuccessStatusCode)
						{
							MessageBox.Show("Filme excluído com sucesso!");
							await CarregarFilmes(); // Recarrega a tabela para sumir com o filme deletado
						}
						else
						{
							MessageBox.Show("Erro ao excluir. Código: " + response.StatusCode);
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show("Erro de conexão: " + ex.Message);
					}
				}
			}
		}

		// --- BOTÃO EDITAR ---
		private async void btnEditar_Click(object sender, EventArgs e)
		{
			if (dataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("Por favor, selecione um filme na tabela para editar.", "Aviso");
				return;
			}

			var idFilme = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

			// Abre a tela de cadastro passando o ID para ela saber que é uma edição!
			var frm = new frmCadastro(idFilme);
			frm.ShowDialog();

			// Recarrega a tabela quando a tela de cadastro fechar
			await CarregarFilmes();
		}
	}
}