using System.Net.Http.Json;
using System.IO;

namespace Movix.Desktop
{
    public partial class frmCadastro : Form
    {
        private const string ApiBaseUrl = "http://localhost:5193/";
        private string caminhoImagemSelecionada = "";
        private int _idFilmeEditando = 0;

        public frmCadastro()
        {
            InitializeComponent();
        }

        public frmCadastro(int idFilme)
        {
            InitializeComponent();
            _idFilmeEditando = idFilme;
        }

        private async void frmCadastro_Load(object sender, EventArgs e)
        {
            await CarregarCombos();

            if (_idFilmeEditando > 0)
            {
                await CarregarFilmeParaEdicao();
            }
        }

        private async Task CarregarFilmeParaEdicao()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                try
                {
                    var filme = await client.GetFromJsonAsync<FilmeDetalheDto>($"api/filmes/{_idFilmeEditando}");

                    if (filme != null)
                    {
                        txtTitulo.Text = filme.Titulo;
                        txtAno.Text = filme.AnoLancamento.ToString();
                        txtSinopse.Text = filme.Sinopse;
                        txtTrailer.Text = filme.UrlTrailer;
                        txtUrlCapa.Text = filme.ImagemCapaUrl;

                        cboGenero.SelectedValue = filme.GeneroId;
                        cboClassificacao.SelectedValue = filme.ClassificacaoId;

                        // Se já tem URL na edição, esconde o seletor de arquivo local
                        if (!string.IsNullOrEmpty(filme.ImagemCapaUrl))
                        {
                            txtCaminhoImagem.Visible = false;
                            btnEscolherArquivo.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar os dados: " + ex.Message);
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
                    var generos = await client.GetFromJsonAsync<List<ItemCombo>>("api/generos");
                    if (generos != null)
                    {
                        cboGenero.DataSource = generos;
                        cboGenero.DisplayMember = "Nome";
                        cboGenero.ValueMember = "Id";
                        cboGenero.SelectedIndex = -1;
                    }

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
                    MessageBox.Show("Erro ao carregar listas: " + ex.Message);
                }
            }
        }

        // --- LÓGICA DE ESCONDER O CAMPO DE URL ---
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

                    // SE ESCOLHEU ARQUIVO: Esconde o campo de URL
                    txtUrlCapa.Visible = false;
                    txtUrlCapa.Text = "";
                }
            }
        }

        // --- LÓGICA DE ESCONDER O CAMPO DE ARQUIVO ---
        private void txtUrlCapa_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUrlCapa.Text))
            {
                // SE DIGITOU URL: Esconde o botão e o texto do arquivo local
                caminhoImagemSelecionada = "";
                txtCaminhoImagem.Text = "";

                txtCaminhoImagem.Visible = false;
                btnEscolherArquivo.Visible = false;
            }
            else
            {
                // SE APAGOU A URL: Mostra o seletor de arquivo de volta
                txtCaminhoImagem.Visible = true;
                btnEscolherArquivo.Visible = true;
            }
        }

        // --- LÓGICA DE RESET (Opcional: Caso queira limpar tudo) ---
        private void btnLimparImagem_Click(object sender, EventArgs e)
        {
            caminhoImagemSelecionada = "";
            txtCaminhoImagem.Text = "";
            txtUrlCapa.Text = "";

            // Mostra tudo de novo
            txtUrlCapa.Visible = true;
            txtCaminhoImagem.Visible = true;
            btnEscolherArquivo.Visible = true;
        }

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
                        if (_idFilmeEditando == 0)
                            response = await client.PostAsync("api/filmes", content);
                        else
                            response = await client.PutAsync($"api/filmes/{_idFilmeEditando}", content);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show(_idFilmeEditando == 0 ? "Filme cadastrado!" : "Filme atualizado!");
                            this.Close();
                        }
                        else
                        {
                            var erro = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Erro ao salvar: {erro}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro de conexão: " + ex.Message);
                    }
                }
            }
        }

        // Eventos vazios mantidos para não quebrar o seu Designer
        private void txtTitulo_TextChanged(object sender, EventArgs e) { }
        private void txtSinopse_TextChanged(object sender, EventArgs e) { }
        private void txtAno_TextChanged(object sender, EventArgs e) { }
        private void txtCaminhoImagem_TextChanged(object sender, EventArgs e) { }
        private void txtTrailer_TextChanged(object sender, EventArgs e) { }
        private void cboGenero_SelectedIndexChanged(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void pictureBox2_Click(object sender, EventArgs e) { }
    }

    // Classes auxiliares (DTOs)
    public class ItemCombo
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

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