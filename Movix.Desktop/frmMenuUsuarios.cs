using System.Net.Http.Json;

namespace Movix.Desktop
{
	public partial class frmMenuUsuarios : Form
	{
		private const string ApiBaseUrl = "http://localhost:5193/";

		public frmMenuUsuarios()
		{
			InitializeComponent();
		}

		private async void frmMenuUsuarios_Load(object sender, EventArgs e)
		{
			await CarregarUsuarios();
		}

		private async Task CarregarUsuarios()
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(ApiBaseUrl);
				try
				{
					var usuarios = await client.GetFromJsonAsync<List<UsuarioDto>>("api/usuarios");
					if (usuarios != null)
					{
						guna2DataGridView1.DataSource = null;
						guna2DataGridView1.DataSource = usuarios;

						if (guna2DataGridView1.Columns.Contains("Id"))
							guna2DataGridView1.Columns["Id"].Visible = false;

						guna2DataGridView1.Columns["Username"].HeaderText = "Usuário";
						guna2DataGridView1.Columns["Email"].HeaderText = "E-mail";
						guna2DataGridView1.Columns["Perfil"].HeaderText = "Nível de Acesso";
						guna2DataGridView1.Columns["IsActive"].HeaderText = "Status";
						guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
					}
				}
				catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
			}
		}

		private void btnNovoUsuario_Click(object sender, EventArgs e)
		{
			new frmCadastroUsuario().ShowDialog();
			_ = CarregarUsuarios();
		}

		private void bntEditar_Click(object sender, EventArgs e)
		{
			if (guna2DataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("Selecione uma linha inteira clicando na seta ao lado.");
				return;
			}

			// Pega o ID da primeira célula da linha selecionada (que é o Id invisível)
			var id = guna2DataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();

			if (!string.IsNullOrEmpty(id))
			{
				var frm = new frmCadastroUsuario(id);
				frm.ShowDialog();
				_ = CarregarUsuarios(); // Recarrega a lista depois de editar
			}
		}

		private async void btnExcluir_Click(object sender, EventArgs e)
		{
			if (guna2DataGridView1.SelectedRows.Count == 0) return;
			var id = guna2DataGridView1.SelectedRows[0].Cells["Id"].Value.ToString();
			var confirm = MessageBox.Show("Alterar status?", "Confirmar", MessageBoxButtons.YesNo);
			if (confirm == DialogResult.Yes)
			{
				using (var client = new HttpClient())
				{
					client.BaseAddress = new Uri(ApiBaseUrl);
					var resp = await client.DeleteAsync($"api/usuarios/{id}");
					if (resp.IsSuccessStatusCode) await CarregarUsuarios();
				}
			}
		}

		// FORMATAÇÃO VISUAL
		private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "IsActive" && e.Value != null)
			{
				bool ativo = (bool)e.Value;
				if (!ativo)
				{
					guna2DataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Gray;
					e.Value = "Inativo";
				}
				else
				{
					guna2DataGridView1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
					e.Value = "Ativo";
				}
				e.FormattingApplied = true;
			}
		}

		// SILENCIA O ERRO DA FOTO
		private void guna2DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			e.ThrowException = false;
		}
	}

	// ÚNICA DEFINIÇÃO DO DTO NO PROJETO
	public class UsuarioDto
	{
		public string Id { get; set; } = string.Empty;
		public string Username { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Perfil { get; set; } = string.Empty;
		public bool IsActive { get; set; }
	}
}