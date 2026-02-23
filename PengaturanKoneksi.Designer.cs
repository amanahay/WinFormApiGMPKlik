namespace WinFormApiGMPKlik
{
    partial class PengaturanKoneksi
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
            this.txtApiUrl = new System.Windows.Forms.TextBox();
            this.lblApiUrl = new System.Windows.Forms.Label();
            this.btnSimpan = new System.Windows.Forms.Button();
            this.btnTes = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtApiUrl
            // 
            this.txtApiUrl.Location = new System.Drawing.Point(15, 45);
            this.txtApiUrl.Name = "txtApiUrl";
            this.txtApiUrl.Size = new System.Drawing.Size(360, 23);
            this.txtApiUrl.TabIndex = 0;
            // 
            // lblApiUrl
            // 
            this.lblApiUrl.AutoSize = true;
            this.lblApiUrl.Location = new System.Drawing.Point(15, 25);
            this.lblApiUrl.Name = "lblApiUrl";
            this.lblApiUrl.Size = new System.Drawing.Size(102, 15);
            this.lblApiUrl.TabIndex = 1;
            this.lblApiUrl.Text = "Alamat Server API:";
            // 
            // btnSimpan
            // 
            this.btnSimpan.Location = new System.Drawing.Point(300, 85);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(75, 30);
            this.btnSimpan.TabIndex = 2;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.UseVisualStyleBackColor = true;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // btnTes
            // 
            this.btnTes.Location = new System.Drawing.Point(219, 85);
            this.btnTes.Name = "btnTes";
            this.btnTes.Size = new System.Drawing.Size(75, 30);
            this.btnTes.TabIndex = 3;
            this.btnTes.Text = "Tes Koneksi";
            this.btnTes.UseVisualStyleBackColor = true;
            this.btnTes.Click += new System.EventHandler(this.btnTes_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(15, 130);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(42, 15);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtApiUrl);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.lblApiUrl);
            this.groupBox1.Controls.Add(this.btnTes);
            this.groupBox1.Controls.Add(this.btnSimpan);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 160);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Konfigurasi Server";
            // 
            // PengaturanKoneksi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 191);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PengaturanKoneksi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pengaturan Koneksi API";
            this.Load += new System.EventHandler(this.PengaturanKoneksi_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TextBox txtApiUrl;
        private System.Windows.Forms.Label lblApiUrl;
        private System.Windows.Forms.Button btnSimpan;
        private System.Windows.Forms.Button btnTes;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}