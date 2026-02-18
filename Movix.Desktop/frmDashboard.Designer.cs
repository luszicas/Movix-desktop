namespace Movix.Desktop
{
    partial class frmDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            btnGerenciarFilmes = new Guna.UI2.WinForms.Guna2Button();
            btnGerenciarUsuarios = new Guna.UI2.WinForms.Guna2Button();
            guna2Panel1.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Panel1
            // 
            guna2Panel1.Controls.Add(btnGerenciarUsuarios);
            guna2Panel1.Controls.Add(btnGerenciarFilmes);
            guna2Panel1.CustomizableEdges = customizableEdges5;
            guna2Panel1.Location = new Point(187, 105);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Panel1.Size = new Size(367, 258);
            guna2Panel1.TabIndex = 0;
            // 
            // btnGerenciarFilmes
            // 
            btnGerenciarFilmes.CustomizableEdges = customizableEdges3;
            btnGerenciarFilmes.DisabledState.BorderColor = Color.DarkGray;
            btnGerenciarFilmes.DisabledState.CustomBorderColor = Color.DarkGray;
            btnGerenciarFilmes.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnGerenciarFilmes.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnGerenciarFilmes.Font = new Font("Segoe UI", 9F);
            btnGerenciarFilmes.ForeColor = Color.White;
            btnGerenciarFilmes.Location = new Point(14, 183);
            btnGerenciarFilmes.Name = "btnGerenciarFilmes";
            btnGerenciarFilmes.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnGerenciarFilmes.Size = new Size(141, 56);
            btnGerenciarFilmes.TabIndex = 0;
            btnGerenciarFilmes.Text = "Filmes";
            btnGerenciarFilmes.Click += btnGerenciarFilmes_Click;
            // 
            // btnGerenciarUsuarios
            // 
            btnGerenciarUsuarios.CustomizableEdges = customizableEdges1;
            btnGerenciarUsuarios.DisabledState.BorderColor = Color.DarkGray;
            btnGerenciarUsuarios.DisabledState.CustomBorderColor = Color.DarkGray;
            btnGerenciarUsuarios.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnGerenciarUsuarios.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnGerenciarUsuarios.Font = new Font("Segoe UI", 9F);
            btnGerenciarUsuarios.ForeColor = Color.White;
            btnGerenciarUsuarios.Location = new Point(75, 73);
            btnGerenciarUsuarios.Name = "btnGerenciarUsuarios";
            btnGerenciarUsuarios.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnGerenciarUsuarios.Size = new Size(225, 56);
            btnGerenciarUsuarios.TabIndex = 1;
            btnGerenciarUsuarios.Text = "USuarios";
            btnGerenciarUsuarios.Click += btnGerenciarUsuarios_Click;
            // 
            // frmDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(800, 450);
            Controls.Add(guna2Panel1);
            Name = "frmDashboard";
            Text = "frmDashboard";
            guna2Panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button btnUsuarios;
        private Guna.UI2.WinForms.Guna2Button btnGerenciarFilmes;
        private Guna.UI2.WinForms.Guna2Button btnGerenciarUsuarios;
    }
}