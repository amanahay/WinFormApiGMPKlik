namespace WinFormApiGMPKlik.Forms
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelHeader = new Panel();
            lblSuperAdminBadge = new Label();
            lblTitle = new Label();
            panelMenu = new Panel();
            btnLogout = new Button();
            btnPengaturan = new Button();
            btnDaftarMandiri = new Button();
            btnUserManagement = new Button();
            panelInfo = new Panel();
            lblCountdown = new Label();
            lblEmailInfo = new Label();
            lblRoleInfo = new Label();
            lblUserInfo = new Label();
            lblStatus = new Label();
            panelContent = new Panel();
            label1 = new Label();
            panelHeader.SuspendLayout();
            panelMenu.SuspendLayout();
            panelInfo.SuspendLayout();
            panelContent.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.SteelBlue;
            panelHeader.Controls.Add(lblSuperAdminBadge);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(884, 60);
            panelHeader.TabIndex = 0;
            // 
            // lblSuperAdminBadge
            // 
            lblSuperAdminBadge.AutoSize = true;
            lblSuperAdminBadge.BackColor = Color.Gold;
            lblSuperAdminBadge.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSuperAdminBadge.ForeColor = Color.Black;
            lblSuperAdminBadge.Location = new Point(700, 22);
            lblSuperAdminBadge.Name = "lblSuperAdminBadge";
            lblSuperAdminBadge.Padding = new Padding(5);
            lblSuperAdminBadge.Size = new Size(98, 25);
            lblSuperAdminBadge.TabIndex = 1;
            lblSuperAdminBadge.Text = "SUPER ADMIN";
            lblSuperAdminBadge.Visible = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(275, 37);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "GMPKlik Dashboard";
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.WhiteSmoke;
            panelMenu.Controls.Add(btnLogout);
            panelMenu.Controls.Add(btnPengaturan);
            panelMenu.Controls.Add(btnDaftarMandiri);
            panelMenu.Controls.Add(btnUserManagement);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 60);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(200, 451);
            panelMenu.TabIndex = 1;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.Crimson;
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(20, 280);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(160, 40);
            btnLogout.TabIndex = 3;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnPengaturan
            // 
            btnPengaturan.Location = new Point(20, 130);
            btnPengaturan.Name = "btnPengaturan";
            btnPengaturan.Size = new Size(160, 40);
            btnPengaturan.TabIndex = 2;
            btnPengaturan.Text = "Pengaturan Koneksi";
            btnPengaturan.UseVisualStyleBackColor = true;
            btnPengaturan.Click += btnPengaturan_Click;
            // 
            // btnDaftarMandiri
            // 
            btnDaftarMandiri.Location = new Point(20, 80);
            btnDaftarMandiri.Name = "btnDaftarMandiri";
            btnDaftarMandiri.Size = new Size(160, 40);
            btnDaftarMandiri.TabIndex = 1;
            btnDaftarMandiri.Text = "Daftar Mandiri";
            btnDaftarMandiri.UseVisualStyleBackColor = true;
            btnDaftarMandiri.Click += btnDaftarMandiri_Click;
            // 
            // btnUserManagement
            // 
            btnUserManagement.Location = new Point(20, 30);
            btnUserManagement.Name = "btnUserManagement";
            btnUserManagement.Size = new Size(160, 40);
            btnUserManagement.TabIndex = 0;
            btnUserManagement.Text = "Manajemen User";
            btnUserManagement.UseVisualStyleBackColor = true;
            btnUserManagement.Click += btnUserManagement_Click;
            // 
            // panelInfo
            // 
            panelInfo.BackColor = Color.White;
            panelInfo.Controls.Add(lblCountdown);
            panelInfo.Controls.Add(lblEmailInfo);
            panelInfo.Controls.Add(lblRoleInfo);
            panelInfo.Controls.Add(lblUserInfo);
            panelInfo.Dock = DockStyle.Top;
            panelInfo.Location = new Point(200, 60);
            panelInfo.Name = "panelInfo";
            panelInfo.Size = new Size(684, 120);
            panelInfo.TabIndex = 2;
            // 
            // lblCountdown
            // 
            lblCountdown.AutoSize = true;
            lblCountdown.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblCountdown.ForeColor = Color.SteelBlue;
            lblCountdown.Location = new Point(20, 80);
            lblCountdown.Name = "lblCountdown";
            lblCountdown.Size = new Size(256, 25);
            lblCountdown.TabIndex = 3;
            lblCountdown.Text = "Session Expires In: 00:00:00";
            // 
            // lblEmailInfo
            // 
            lblEmailInfo.AutoSize = true;
            lblEmailInfo.Location = new Point(20, 55);
            lblEmailInfo.Name = "lblEmailInfo";
            lblEmailInfo.Size = new Size(47, 15);
            lblEmailInfo.TabIndex = 2;
            lblEmailInfo.Text = "Email: -";
            // 
            // lblRoleInfo
            // 
            lblRoleInfo.AutoSize = true;
            lblRoleInfo.Location = new Point(20, 35);
            lblRoleInfo.Name = "lblRoleInfo";
            lblRoleInfo.Size = new Size(41, 15);
            lblRoleInfo.TabIndex = 1;
            lblRoleInfo.Text = "Role: -";
            // 
            // lblUserInfo
            // 
            lblUserInfo.AutoSize = true;
            lblUserInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUserInfo.Location = new Point(20, 15);
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.Size = new Size(44, 15);
            lblUserInfo.TabIndex = 0;
            lblUserInfo.Text = "User: -";
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Location = new Point(200, 491);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(684, 20);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Status: Ready";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panelContent
            // 
            panelContent.Controls.Add(label1);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(200, 180);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(684, 311);
            panelContent.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.ForeColor = Color.Gray;
            label1.Location = new Point(200, 130);
            label1.Name = "label1";
            label1.Size = new Size(278, 21);
            label1.TabIndex = 0;
            label1.Text = "Selamat datang di GMPKlik Dashboard";
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 511);
            Controls.Add(panelContent);
            Controls.Add(lblStatus);
            Controls.Add(panelInfo);
            Controls.Add(panelMenu);
            Controls.Add(panelHeader);
            KeyPreview = true;
            MinimumSize = new Size(900, 550);
            Name = "DashboardForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dashboard - GMPKlik";
            WindowState = FormWindowState.Maximized;
            FormClosing += DashboardForm_FormClosing;
            Load += DashboardForm_Load;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelMenu.ResumeLayout(false);
            panelInfo.ResumeLayout(false);
            panelInfo.PerformLayout();
            panelContent.ResumeLayout(false);
            panelContent.PerformLayout();
            ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button btnUserManagement;
        private System.Windows.Forms.Button btnDaftarMandiri;
        private System.Windows.Forms.Button btnPengaturan;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Label lblRoleInfo;
        private System.Windows.Forms.Label lblEmailInfo;
        private System.Windows.Forms.Label lblCountdown;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblSuperAdminBadge;
        private System.Windows.Forms.Label label1;
    }
}