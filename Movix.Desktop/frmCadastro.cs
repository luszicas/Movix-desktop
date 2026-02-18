using System.Net.Http.Json;
using System.IO;

namespace Movix.Desktop
{
	public partial class frmCadastro : Form
	{
		// PORTA CORRETA (5193)
		private const string ApiBaseUrl = "http://localhost:5193/";
		private string caminhoImagemSelecionada = "";

		// --- 1. VARIÁVEL PARA SABER SE É EDIÇÃO ---
		private int _idFilmeEditando = 0;

		// CONSTRUTOR 1: Usado para NOVO FILME
		public frmCadastro()
		{
			InitializeComponent();
		}

		// CONSTRUTOR 2: Usado para EDITAR FILME (Recebe o ID)
		public frmCadastro(int idFilme)
		{
			InitializeComponent();
			_idFilmeEditando = idFilme;
		}

		// --- 2. AO ABRIR A TELA (LOAD) ---
		private async void frmCadastro_Load(object sender, EventArgs e)
		{
			// Primeiro carrega as listas de Gênero e Classificação
			await CarregarCombos();

			// Se o ID for maior que zero, significa que estamos editando!
			if (_idFilmeEditando > 0)
			{
				await CarregarFilmeParaEdicao();
			}
		}

		// --- 3. BUSCA O FILME ESPECÍFICO PARA PREENCHER A TELA ---
		private async Task CarregarFilmeParaEdicao()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(ApiBaseUrl);
				try
				{
					// Busca o filme pelo ID na API
					var filme = await client.GetFromJsonAsync<FilmeDetalheDto>($"api/filmes/{_idFilmeEditando}");

					if (filme != null)
					{
						// Preenche as caixinhas com os dados que vieram do banco
						txtTitulo.Text = filme.Titulo;
						txtAno.Text = filme.AnoLancamento.ToString();
						txtSinopse.Text = filme.Sinopse;
						txtTrailer.Text = filme.UrlTrailer;
						txtUrlCapa.Text = filme.ImagemCapaUrl;

						// Seleciona os combos corretos
						cboGenero.SelectedValue = filme.GeneroId;
						cboClassificacao.SelectedValue = filme.ClassificacaoId;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Erro ao carregar os dados do filme para edição: " + ex.Message);
				}
			}
		}

		private async Task CarregarCombos()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(ApiBaseUrl);
				try
				{
					// Busca Gêneros
					var generos = await client.GetFromJsonAsync<List<ItemCombo>>("api/generos");
					if (generos != null)
					{
						cboGenero.DataSource = generos;
						cboGenero.DisplayMember = "Nome";
						cboGenero.ValueMember = "Id";
						cboGenero.SelectedIndex = -1;
					}

					// Busca Classificações
					var classificacoes = await client.GetFromJsonAsync<List<ItemCombo>>("api/classificacoes");
					if (classificacoes != null)
					{
						cboClassificacao.DataSource = classificacoes;
						cboClassificacao.DisplayMember = "Nome";
						cboClassificacao.ValueMember = "Id";
						cboClassificacao.SelectedIndex = -1;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Erro ao carregar listas (Verifique se a API está rodando): " + ex.Message);
				}
			}
		}

		private void btnEscolherArquivo_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog dialog = new OpenFileDialog())
			{
				dialog.Title = "Selecione a Capa";
				dialog.Filter = "Imagens|*.jpg;*.png;*.webp;*.jpeg";
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					caminhoImagemSelecionada = dialog.FileName;
					txtCaminhoImagem.Text = dialog.SafeFileName;
					txtUrlCapa.Text = "";
				}
			}
		}

		// --- 4. BOTÃO SALVAR (AGORA COM POST OU PUT) ---
		private async void btnSalvar_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtTitulo.Text))
			{
				MessageBox.Show("O Título é obrigatório.");
				return;
			}

			if (cboGenero.SelectedValue == null || cboClassificacao.SelectedValue == null)
			{
				MessageBox.Show("Por favor, selecione um Gênero e uma Classificação.");
				return;
			}

			int generoId = (int)cboGenero.SelectedValue;
			int classId = (int)cboClassificacao.SelectedValue;
			int.TryParse(txtAno.Text, out int ano);

			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(ApiBaseUrl);

				using (var content = new MultipartFormDataContent())
				{
					// Se for edição, envia o ID junto
					if (_idFilmeEditando > 0)
					{
						content.Add(new StringContent(_idFilmeEditando.ToString()), "Id");
					}

					content.Add(new StringContent(txtTitulo.Text), "Titulo");
					content.Add(new StringContent(ano.ToString()), "AnoLancamento");
					content.Add(new StringContent(txtSinopse.Text), "Sinopse");
					content.Add(new StringContent(generoId.ToString()), "GeneroId");
					content.Add(new StringContent(classId.ToString()), "ClassificacaoId");
					content.Add(new StringContent(txtTrailer.Text ?? ""), "UrlTrailer");

					if (!string.IsNullOrEmpty(caminhoImagemSelecionada) && File.Exists(caminhoImagemSelecionada))
					{
						var bytes = File.ReadAllBytes(caminhoImagemSelecionada);
						var imagemContent = new ByteArrayContent(bytes);
						content.Add(imagemContent, "ArquivoCapa", Path.GetFileName(caminhoImagemSelecionada));
					}
					else
					{
						content.Add(new StringContent(txtUrlCapa.Text ?? ""), "ImagemCapaUrl");
					}

					try
					{
						HttpResponseMessage response;

						// A MÁGICA ACONTECE AQUI:
						if (_idFilmeEditando == 0)
						{
							// Se for 0, é filme novo (POST)
							response = await client.PostAsync("api/filmes", content);
						}
						else
						{
							// Se tiver ID, é edição (PUT)
							response = await client.PutAsync($"api/filmes/{_idFilmeEditando}", content);
						}

						if (response.IsSuccessStatusCode)
						{
							MessageBox.Show(_idFilmeEditando == 0 ? "Filme cadastrado com sucesso!" : "Filme atualizado com sucesso!");
							this.Close();
						}
						else
						{
							var erro = await response.Content.ReadAsStringAsync();
							MessageBox.Show($"Erro ao salvar: {response.StatusCode}\n{erro}");
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show("Erro de conexão: " + ex.Message);
					}
				}
			}
		}

		// Eventos vazios para não quebrar o Designer
		private void txtTitulo_TextChanged(object sender, EventArgs e) { }
		private void txtSinopse_TextChanged(object sender, EventArgs e) { }
		private void txtAno_TextChanged(object sender, EventArgs e) { }
		private void txtUrlCapa_TextChanged(object sender, EventArgs e) { }
		private void txtCaminhoImagem_TextChanged(object sender, EventArgs e) { }
		private void txtTrailer_TextChanged(object sender, EventArgs e) { }
		private void txtGenero_TextChanged(object sender, EventArgs e) { }
		private void txtClassificacao_TextChanged(object sender, EventArgs e) { }
		private void pictureBox1_Click(object sender, EventArgs e) { }
		private void cboGenero_SelectedIndexChanged(object sender, EventArgs e) { }
		private void pictureBox2_Click(object sender, EventArgs e) { }
	}

	// CLASSES AUXILIARES NO FINAL DO ARQUIVO
	public class ItemCombo
	{
		public int Id { get; set; }
		public string Nome { get; set; } = string.Empty;
	}

	// Classe nova para receber os dados do filme para edição
	public class FilmeDetalheDto
	{
		public int Id { get; set; }
		public string Titulo { get; set; } = string.Empty;
		public string Sinopse { get; set; } = string.Empty;
		public int AnoLancamento { get; set; }
		public string ImagemCapaUrl { get; set; } = string.Empty;
		public string UrlTrailer { get; set; } = string.Empty;
		public int GeneroId { get; set; }
		public int ClassificacaoId { get; set; }
	}
}