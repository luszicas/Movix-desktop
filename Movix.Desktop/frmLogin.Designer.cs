namespace Movix.Desktop
{
    partial class frmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            txtSenha = new Guna.UI2.WinForms.Guna2TextBox();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            btnEntrar = new Guna.UI2.WinForms.Guna2Button();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // txtSenha
            // 
            txtSenha.BorderColor = Color.MidnightBlue;
            txtSenha.BorderRadius = 5;
            txtSenha.CustomizableEdges = customizableEdges1;
            txtSenha.DefaultText = "";
            txtSenha.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSenha.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSenha.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSenha.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSenha.FillColor = Color.Transparent;
            txtSenha.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSenha.Font = new Font("Segoe UI", 9F);
            txtSenha.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSenha.Location = new Point(190, 223);
            txtSenha.Margin = new Padding(3, 4, 3, 4);
            txtSenha.Name = "txtSenha";
            txtSenha.PlaceholderForeColor = Color.Gainsboro;
            txtSenha.PlaceholderText = "Senha";
            txtSenha.SelectedText = "";
            txtSenha.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtSenha.Size = new Size(200, 36);
            txtSenha.TabIndex = 0;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = SystemColors.ButtonHighlight;
            txtEmail.BorderColor = Color.MidnightBlue;
            txtEmail.BorderRadius = 5;
            txtEmail.CustomizableEdges = customizableEdges3;
            txtEmail.DefaultText = "";
            txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.FillColor = Color.Transparent;
            txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.ForeColor = Color.White;
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Location = new Point(190, 165);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Email";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtEmail.Size = new Size(200, 36);
            txtEmail.TabIndex = 1;
            txtEmail.TextChanged += txtEmail_TextChanged_1;
            // 
            // btnEntrar
            // 
            btnEntrar.BorderRadius = 5;
            btnEntrar.CustomizableEdges = customizableEdges5;
            btnEntrar.DisabledState.BorderColor = Color.DarkGray;
            btnEntrar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEntrar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEntrar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEntrar.FillColor = Color.MidnightBlue;
            btnEntrar.Font = new Font("Segoe UI", 9F);
            btnEntrar.ForeColor = Color.White;
            btnEntrar.Location = new Point(231, 288);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEntrar.Size = new Size(118, 37);
            btnEntrar.TabIndex = 2;
            btnEntrar.Text = "Entrar";
            btnEntrar.Click += btnEntrar_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(200, 32);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(181, 104);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(-5, -7);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(175, 360);
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 4;
            pictureBox2.TabStop = false;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(426, 355);
            Controls.Add(pictureBox2);
            Controls.Add(btnEntrar);
            Controls.Add(pictureBox1);
            Controls.Add(txtSenha);
            Controls.Add(txtEmail);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "frmLogin";
            Text = "frmLogin";
            Load += frmLogin_Load_1;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtSenha;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2Button btnEntrar;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}