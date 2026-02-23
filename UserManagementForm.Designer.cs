namespace WinFormApiGMPKlik.Forms
{
    partial class UserManagementForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dgvUsers = new DataGridView();
            groupBoxFilter = new GroupBox();
            cmbSortBy = new ComboBox();
            label13 = new Label();
            chkSortDescending = new CheckBox();
            dtpCreatedTo = new DateTimePicker();
            label12 = new Label();
            dtpCreatedFrom = new DateTimePicker();
            label11 = new Label();
            cmbRole = new ComboBox();
            label10 = new Label();
            cmbIsDeleted = new ComboBox();
            label9 = new Label();
            cmbIsActive = new ComboBox();
            label8 = new Label();
            cmbBranch = new ComboBox();
            label7 = new Label();
            txtSearch = new TextBox();
            label2 = new Label();
            btnClearFilter = new Button();
            btnSearch = new Button();
            groupBoxDetail = new GroupBox();
            lblDeletedInfo = new Label();
            lblWarning = new Label();
            lblUserRoles = new Label();
            label6 = new Label();
            chkEditIsActive = new CheckBox();
            txtEditPhone = new TextBox();
            label5 = new Label();
            txtEditName = new TextBox();
            label4 = new Label();
            txtEditEmail = new TextBox();
            label3 = new Label();
            txtEditUsername = new TextBox();
            label14 = new Label();
            panelPagination = new Panel();
            lblTotalRecords = new Label();
            cmbPageSize = new ComboBox();
            lblPageSize = new Label();
            btnLast = new Button();
            btnNext = new Button();
            btnPrevious = new Button();
            btnFirst = new Button();
            lblPageInfo = new Label();
            panelActions = new Panel();
            btnExport = new Button();
            btnResetPassword = new Button();
            btnRestore = new Button();
            btnAssignRole = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();
            btnClose = new Button();
            panelHeader = new Panel();
            lblConnectionStatus = new Label();
            lblCurrentUser = new Label();
            label1 = new Label();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            progressBar = new ToolStripProgressBar();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            groupBoxFilter.SuspendLayout();
            groupBoxDetail.SuspendLayout();
            panelPagination.SuspendLayout();
            panelActions.SuspendLayout();
            panelHeader.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvUsers.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUsers.Location = new Point(12, 180);
            dgvUsers.MultiSelect = false;
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new Size(1160, 300);
            dgvUsers.TabIndex = 0;
            dgvUsers.CellDoubleClick += dgvUsers_CellDoubleClick;
            dgvUsers.SelectionChanged += dgvUsers_SelectionChanged;
            // 
            // groupBoxFilter
            // 
            groupBoxFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxFilter.Controls.Add(cmbSortBy);
            groupBoxFilter.Controls.Add(label13);
            groupBoxFilter.Controls.Add(chkSortDescending);
            groupBoxFilter.Controls.Add(dtpCreatedTo);
            groupBoxFilter.Controls.Add(label12);
            groupBoxFilter.Controls.Add(dtpCreatedFrom);
            groupBoxFilter.Controls.Add(label11);
            groupBoxFilter.Controls.Add(cmbRole);
            groupBoxFilter.Controls.Add(label10);
            groupBoxFilter.Controls.Add(cmbIsDeleted);
            groupBoxFilter.Controls.Add(label9);
            groupBoxFilter.Controls.Add(cmbIsActive);
            groupBoxFilter.Controls.Add(label8);
            groupBoxFilter.Controls.Add(cmbBranch);
            groupBoxFilter.Controls.Add(label7);
            groupBoxFilter.Controls.Add(txtSearch);
            groupBoxFilter.Controls.Add(label2);
            groupBoxFilter.Controls.Add(btnClearFilter);
            groupBoxFilter.Controls.Add(btnSearch);
            groupBoxFilter.Location = new Point(12, 50);
            groupBoxFilter.Name = "groupBoxFilter";
            groupBoxFilter.Size = new Size(1160, 120);
            groupBoxFilter.TabIndex = 1;
            groupBoxFilter.TabStop = false;
            groupBoxFilter.Text = "Filter && Pencarian";
            // 
            // cmbSortBy
            // 
            cmbSortBy.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSortBy.FormattingEnabled = true;
            cmbSortBy.Items.AddRange(new object[] { "CreatedAt", "Username", "Email", "FullName", "IsActive" });
            cmbSortBy.Location = new Point(890, 55);
            cmbSortBy.Name = "cmbSortBy";
            cmbSortBy.Size = new Size(120, 23);
            cmbSortBy.TabIndex = 18;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(835, 58);
            label13.Name = "label13";
            label13.Size = new Size(47, 15);
            label13.TabIndex = 17;
            label13.Text = "Sort By:";
            // 
            // chkSortDescending
            // 
            chkSortDescending.AutoSize = true;
            chkSortDescending.Checked = true;
            chkSortDescending.CheckState = CheckState.Checked;
            chkSortDescending.Location = new Point(1020, 57);
            chkSortDescending.Name = "chkSortDescending";
            chkSortDescending.Size = new Size(88, 19);
            chkSortDescending.TabIndex = 16;
            chkSortDescending.Text = "Descending";
            chkSortDescending.UseVisualStyleBackColor = true;
            // 
            // dtpCreatedTo
            // 
            dtpCreatedTo.Format = DateTimePickerFormat.Short;
            dtpCreatedTo.Location = new Point(680, 85);
            dtpCreatedTo.Name = "dtpCreatedTo";
            dtpCreatedTo.ShowCheckBox = true;
            dtpCreatedTo.Size = new Size(120, 23);
            dtpCreatedTo.TabIndex = 15;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(605, 88);
            label12.Name = "label12";
            label12.Size = new Size(65, 15);
            label12.TabIndex = 14;
            label12.Text = "SampaiTgl:";
            // 
            // dtpCreatedFrom
            // 
            dtpCreatedFrom.Format = DateTimePickerFormat.Short;
            dtpCreatedFrom.Location = new Point(680, 55);
            dtpCreatedFrom.Name = "dtpCreatedFrom";
            dtpCreatedFrom.ShowCheckBox = true;
            dtpCreatedFrom.Size = new Size(120, 23);
            dtpCreatedFrom.TabIndex = 13;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(605, 58);
            label11.Name = "label11";
            label11.Size = new Size(50, 15);
            label11.TabIndex = 12;
            label11.Text = "Dari Tgl:";
            // 
            // cmbRole
            // 
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.FormattingEnabled = true;
            cmbRole.Items.AddRange(new object[] { "All", "SuperAdmin", "Admin", "Manager", "User", "Tamu" });
            cmbRole.Location = new Point(450, 85);
            cmbRole.Name = "cmbRole";
            cmbRole.Size = new Size(130, 23);
            cmbRole.TabIndex = 11;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(405, 88);
            label10.Name = "label10";
            label10.Size = new Size(33, 15);
            label10.TabIndex = 10;
            label10.Text = "Role:";
            // 
            // cmbIsDeleted
            // 
            cmbIsDeleted.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIsDeleted.FormattingEnabled = true;
            cmbIsDeleted.Items.AddRange(new object[] { "All", "Active Only", "Deleted Only" });
            cmbIsDeleted.Location = new Point(450, 55);
            cmbIsDeleted.Name = "cmbIsDeleted";
            cmbIsDeleted.Size = new Size(130, 23);
            cmbIsDeleted.TabIndex = 9;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(405, 58);
            label9.Name = "label9";
            label9.Size = new Size(42, 15);
            label9.TabIndex = 8;
            label9.Text = "Status:";
            // 
            // cmbIsActive
            // 
            cmbIsActive.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbIsActive.FormattingEnabled = true;
            cmbIsActive.Items.AddRange(new object[] { "All", "Active Only", "Inactive Only" });
            cmbIsActive.Location = new Point(260, 85);
            cmbIsActive.Name = "cmbIsActive";
            cmbIsActive.Size = new Size(130, 23);
            cmbIsActive.TabIndex = 7;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(210, 88);
            label8.Name = "label8";
            label8.Size = new Size(35, 15);
            label8.TabIndex = 6;
            label8.Text = "Aktif:";
            // 
            // cmbBranch
            // 
            cmbBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBranch.FormattingEnabled = true;
            cmbBranch.Location = new Point(260, 55);
            cmbBranch.Name = "cmbBranch";
            cmbBranch.Size = new Size(130, 23);
            cmbBranch.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(210, 58);
            label7.Name = "label7";
            label7.Size = new Size(51, 15);
            label7.TabIndex = 4;
            label7.Text = "Cabang:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(70, 25);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Cari nama, email, username, telepon...";
            txtSearch.Size = new Size(320, 23);
            txtSearch.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 28);
            label2.Name = "label2";
            label2.Size = new Size(31, 15);
            label2.TabIndex = 0;
            label2.Text = "Cari:";
            // 
            // btnClearFilter
            // 
            btnClearFilter.Location = new Point(1055, 83);
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new Size(90, 25);
            btnClearFilter.TabIndex = 3;
            btnClearFilter.Text = "Clear Filter";
            btnClearFilter.UseVisualStyleBackColor = true;
            btnClearFilter.Click += btnClearFilter_Click;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.SteelBlue;
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(1055, 23);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(90, 28);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "🔍 Cari";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // groupBoxDetail
            // 
            groupBoxDetail.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxDetail.Controls.Add(lblDeletedInfo);
            groupBoxDetail.Controls.Add(lblWarning);
            groupBoxDetail.Controls.Add(lblUserRoles);
            groupBoxDetail.Controls.Add(label6);
            groupBoxDetail.Controls.Add(chkEditIsActive);
            groupBoxDetail.Controls.Add(txtEditPhone);
            groupBoxDetail.Controls.Add(label5);
            groupBoxDetail.Controls.Add(txtEditName);
            groupBoxDetail.Controls.Add(label4);
            groupBoxDetail.Controls.Add(txtEditEmail);
            groupBoxDetail.Controls.Add(label3);
            groupBoxDetail.Controls.Add(txtEditUsername);
            groupBoxDetail.Controls.Add(label14);
            groupBoxDetail.Location = new Point(12, 486);
            groupBoxDetail.Name = "groupBoxDetail";
            groupBoxDetail.Size = new Size(800, 150);
            groupBoxDetail.TabIndex = 2;
            groupBoxDetail.TabStop = false;
            groupBoxDetail.Text = "Detail User (Edit)";
            // 
            // lblDeletedInfo
            // 
            lblDeletedInfo.AutoSize = true;
            lblDeletedInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDeletedInfo.ForeColor = Color.Red;
            lblDeletedInfo.Location = new Point(550, 120);
            lblDeletedInfo.Name = "lblDeletedInfo";
            lblDeletedInfo.Size = new Size(174, 15);
            lblDeletedInfo.TabIndex = 12;
            lblDeletedInfo.Text = "⚠️ USER INI SUDAH DIHAPUS";
            lblDeletedInfo.Visible = false;
            // 
            // lblWarning
            // 
            lblWarning.AutoSize = true;
            lblWarning.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblWarning.ForeColor = Color.OrangeRed;
            lblWarning.Location = new Point(15, 120);
            lblWarning.Name = "lblWarning";
            lblWarning.Size = new Size(168, 15);
            lblWarning.TabIndex = 11;
            lblWarning.Text = "⚠️ SUPERADMIN - Protected";
            lblWarning.Visible = false;
            // 
            // lblUserRoles
            // 
            lblUserRoles.AutoSize = true;
            lblUserRoles.Location = new Point(550, 90);
            lblUserRoles.Name = "lblUserRoles";
            lblUserRoles.Size = new Size(12, 15);
            lblUserRoles.TabIndex = 10;
            lblUserRoles.Text = "-";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(480, 90);
            label6.Name = "label6";
            label6.Size = new Size(33, 15);
            label6.TabIndex = 9;
            label6.Text = "Role:";
            // 
            // chkEditIsActive
            // 
            chkEditIsActive.AutoSize = true;
            chkEditIsActive.Location = new Point(100, 118);
            chkEditIsActive.Name = "chkEditIsActive";
            chkEditIsActive.Size = new Size(51, 19);
            chkEditIsActive.TabIndex = 8;
            chkEditIsActive.Text = "Aktif";
            chkEditIsActive.UseVisualStyleBackColor = true;
            // 
            // txtEditPhone
            // 
            txtEditPhone.Location = new Point(100, 85);
            txtEditPhone.Name = "txtEditPhone";
            txtEditPhone.Size = new Size(180, 23);
            txtEditPhone.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(15, 88);
            label5.Name = "label5";
            label5.Size = new Size(52, 15);
            label5.TabIndex = 6;
            label5.Text = "Telepon:";
            // 
            // txtEditName
            // 
            txtEditName.Location = new Point(320, 55);
            txtEditName.Name = "txtEditName";
            txtEditName.Size = new Size(250, 23);
            txtEditName.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(260, 58);
            label4.Name = "label4";
            label4.Size = new Size(42, 15);
            label4.TabIndex = 4;
            label4.Text = "Nama:";
            // 
            // txtEditEmail
            // 
            txtEditEmail.Location = new Point(320, 25);
            txtEditEmail.Name = "txtEditEmail";
            txtEditEmail.ReadOnly = true;
            txtEditEmail.Size = new Size(250, 23);
            txtEditEmail.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(260, 28);
            label3.Name = "label3";
            label3.Size = new Size(39, 15);
            label3.TabIndex = 2;
            label3.Text = "Email:";
            // 
            // txtEditUsername
            // 
            txtEditUsername.Location = new Point(100, 25);
            txtEditUsername.Name = "txtEditUsername";
            txtEditUsername.ReadOnly = true;
            txtEditUsername.Size = new Size(150, 23);
            txtEditUsername.TabIndex = 1;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(15, 28);
            label14.Name = "label14";
            label14.Size = new Size(63, 15);
            label14.TabIndex = 0;
            label14.Text = "Username:";
            // 
            // panelPagination
            // 
            panelPagination.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelPagination.Controls.Add(lblTotalRecords);
            panelPagination.Controls.Add(cmbPageSize);
            panelPagination.Controls.Add(lblPageSize);
            panelPagination.Controls.Add(btnLast);
            panelPagination.Controls.Add(btnNext);
            panelPagination.Controls.Add(btnPrevious);
            panelPagination.Controls.Add(btnFirst);
            panelPagination.Controls.Add(lblPageInfo);
            panelPagination.Location = new Point(12, 642);
            panelPagination.Name = "panelPagination";
            panelPagination.Size = new Size(1160, 40);
            panelPagination.TabIndex = 3;
            // 
            // lblTotalRecords
            // 
            lblTotalRecords.AutoSize = true;
            lblTotalRecords.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalRecords.Location = new Point(450, 12);
            lblTotalRecords.Name = "lblTotalRecords";
            lblTotalRecords.Size = new Size(92, 15);
            lblTotalRecords.TabIndex = 7;
            lblTotalRecords.Text = "Total: 0 records";
            // 
            // cmbPageSize
            // 
            cmbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPageSize.FormattingEnabled = true;
            cmbPageSize.Items.AddRange(new object[] { "10", "20", "50", "100" });
            cmbPageSize.Location = new Point(750, 8);
            cmbPageSize.Name = "cmbPageSize";
            cmbPageSize.Size = new Size(60, 23);
            cmbPageSize.TabIndex = 6;
            cmbPageSize.SelectedIndexChanged += cmbPageSize_SelectedIndexChanged;
            // 
            // lblPageSize
            // 
            lblPageSize.AutoSize = true;
            lblPageSize.Location = new Point(680, 12);
            lblPageSize.Name = "lblPageSize";
            lblPageSize.Size = new Size(57, 15);
            lblPageSize.TabIndex = 5;
            lblPageSize.Text = "Data/Hal:";
            // 
            // btnLast
            // 
            btnLast.Location = new Point(320, 8);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(40, 25);
            btnLast.TabIndex = 4;
            btnLast.Text = ">>";
            btnLast.UseVisualStyleBackColor = true;
            btnLast.Click += btnLast_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(275, 8);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(40, 25);
            btnNext.TabIndex = 3;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnPrevious
            // 
            btnPrevious.Location = new Point(55, 8);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(40, 25);
            btnPrevious.TabIndex = 1;
            btnPrevious.Text = "<";
            btnPrevious.UseVisualStyleBackColor = true;
            btnPrevious.Click += btnPrevious_Click;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(10, 8);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(40, 25);
            btnFirst.TabIndex = 0;
            btnFirst.Text = "<<";
            btnFirst.UseVisualStyleBackColor = true;
            btnFirst.Click += btnFirst_Click;
            // 
            // lblPageInfo
            // 
            lblPageInfo.AutoSize = true;
            lblPageInfo.Location = new Point(105, 12);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(96, 15);
            lblPageInfo.TabIndex = 2;
            lblPageInfo.Text = "Halaman 1 dari 1";
            // 
            // panelActions
            // 
            panelActions.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            panelActions.Controls.Add(btnExport);
            panelActions.Controls.Add(btnResetPassword);
            panelActions.Controls.Add(btnRestore);
            panelActions.Controls.Add(btnAssignRole);
            panelActions.Controls.Add(btnEdit);
            panelActions.Controls.Add(btnDelete);
            panelActions.Location = new Point(818, 486);
            panelActions.Name = "panelActions";
            panelActions.Size = new Size(354, 150);
            panelActions.TabIndex = 4;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(240, 50);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(110, 35);
            btnExport.TabIndex = 5;
            btnExport.Text = "📥 Export Excel";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // btnResetPassword
            // 
            btnResetPassword.Location = new Point(240, 5);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(110, 35);
            btnResetPassword.TabIndex = 4;
            btnResetPassword.Text = "🔑 Reset Pass";
            btnResetPassword.UseVisualStyleBackColor = true;
            btnResetPassword.Click += btnResetPassword_Click;
            // 
            // btnRestore
            // 
            btnRestore.BackColor = Color.MediumSeaGreen;
            btnRestore.ForeColor = Color.White;
            btnRestore.Location = new Point(125, 95);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(110, 35);
            btnRestore.TabIndex = 3;
            btnRestore.Text = "♻️ Restore";
            btnRestore.UseVisualStyleBackColor = false;
            btnRestore.Visible = false;
            btnRestore.Click += btnRestore_Click;
            // 
            // btnAssignRole
            // 
            btnAssignRole.Location = new Point(10, 5);
            btnAssignRole.Name = "btnAssignRole";
            btnAssignRole.Size = new Size(110, 35);
            btnAssignRole.TabIndex = 0;
            btnAssignRole.Text = "👥 Ubah Role";
            btnAssignRole.UseVisualStyleBackColor = true;
            btnAssignRole.Click += btnAssignRole_Click;
            // 
            // btnEdit
            // 
            btnEdit.BackColor = Color.SteelBlue;
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(10, 50);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(110, 35);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "💾 Simpan";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.Crimson;
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(10, 95);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(110, 35);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "🗑️ Hapus";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.Location = new Point(1072, 12);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 30);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "🔄 Refresh";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Location = new Point(1072, 645);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 35);
            btnClose.TabIndex = 6;
            btnClose.Text = "❌ Tutup";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // panelHeader
            // 
            panelHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panelHeader.BackColor = Color.WhiteSmoke;
            panelHeader.Controls.Add(lblConnectionStatus);
            panelHeader.Controls.Add(lblCurrentUser);
            panelHeader.Controls.Add(label1);
            panelHeader.Controls.Add(btnRefresh);
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1184, 45);
            panelHeader.TabIndex = 7;
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblConnectionStatus.AutoSize = true;
            lblConnectionStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblConnectionStatus.ForeColor = Color.Green;
            lblConnectionStatus.Location = new Point(900, 15);
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(83, 15);
            lblConnectionStatus.TabIndex = 9;
            lblConnectionStatus.Text = "\U0001f7e2 Terhubung";
            // 
            // lblCurrentUser
            // 
            lblCurrentUser.AutoSize = true;
            lblCurrentUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCurrentUser.ForeColor = Color.SteelBlue;
            lblCurrentUser.Location = new Point(120, 15);
            lblCurrentUser.Name = "lblCurrentUser";
            lblCurrentUser.Size = new Size(12, 15);
            lblCurrentUser.TabIndex = 8;
            lblCurrentUser.Text = "-";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(92, 15);
            label1.TabIndex = 7;
            label1.Text = "User yang login:";
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, progressBar });
            statusStrip.Location = new Point(0, 689);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1184, 22);
            statusStrip.TabIndex = 8;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(39, 17);
            toolStripStatusLabel.Text = "Ready";
            // 
            // progressBar
            // 
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(200, 16);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.Visible = false;
            // 
            // UserManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 711);
            Controls.Add(statusStrip);
            Controls.Add(panelHeader);
            Controls.Add(btnClose);
            Controls.Add(panelActions);
            Controls.Add(panelPagination);
            Controls.Add(groupBoxDetail);
            Controls.Add(groupBoxFilter);
            Controls.Add(dgvUsers);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1200, 750);
            Name = "UserManagementForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Manajemen User - GMPKlik (Advanced)";
            Load += UserManagementForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            groupBoxFilter.ResumeLayout(false);
            groupBoxFilter.PerformLayout();
            groupBoxDetail.ResumeLayout(false);
            groupBoxDetail.PerformLayout();
            panelPagination.ResumeLayout(false);
            panelPagination.PerformLayout();
            panelActions.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.GroupBox groupBoxFilter;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBoxDetail;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label lblUserRoles;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkEditIsActive;
        private System.Windows.Forms.TextBox txtEditPhone;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEditName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEditEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelPagination;
        private System.Windows.Forms.ComboBox cmbPageSize;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.Button btnAssignRole;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblCurrentUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBranch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbIsActive;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbIsDeleted;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpCreatedTo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dtpCreatedFrom;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbSortBy;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkSortDescending;
        private System.Windows.Forms.TextBox txtEditUsername;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblDeletedInfo;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.Button btnExport;
    }
}