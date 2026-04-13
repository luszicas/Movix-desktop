namespace Movix.Desktop
{
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
        }

        // Evento de clique do botão "Gerenciar Filmes"
        private void btnGerenciarFilmes_Click(object sender, EventArgs e)
        {
            var menuFilmes = new frmMenu();
            menuFilmes.ShowDialog();
        }

        // Evento de clique do botão "Gerenciar Usuários"
        private void btnGerenciarUsuarios_Click(object sender, EventArgs e)
        {
            // Você ainda vai criar esse formulário depois
            var menuUsuarios = new frmMenuUsuarios();
            menuUsuarios.ShowDialog();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}