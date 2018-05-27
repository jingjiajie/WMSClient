namespace WMS.UI
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.labelPsaaword = new System.Windows.Forms.Label();
            this.labelusername = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.buttonClosing = new System.Windows.Forms.Button();
            this.buttonEnter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelClickCount = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxWarehouse = new System.Windows.Forms.ComboBox();
            this.comboBoxAccountBook = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPsaaword
            // 
            this.labelPsaaword.AutoSize = true;
            this.labelPsaaword.BackColor = System.Drawing.Color.Transparent;
            this.labelPsaaword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPsaaword.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelPsaaword.ForeColor = System.Drawing.Color.White;
            this.labelPsaaword.Location = new System.Drawing.Point(0, 70);
            this.labelPsaaword.Margin = new System.Windows.Forms.Padding(0);
            this.labelPsaaword.Name = "labelPsaaword";
            this.labelPsaaword.Size = new System.Drawing.Size(170, 70);
            this.labelPsaaword.TabIndex = 7;
            this.labelPsaaword.Text = "密   码：";
            this.labelPsaaword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseDown);
            this.labelPsaaword.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseMove);
            this.labelPsaaword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseUp);
            // 
            // labelusername
            // 
            this.labelusername.AutoSize = true;
            this.labelusername.BackColor = System.Drawing.Color.Transparent;
            this.labelusername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelusername.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelusername.ForeColor = System.Drawing.Color.White;
            this.labelusername.Location = new System.Drawing.Point(0, 0);
            this.labelusername.Margin = new System.Windows.Forms.Padding(0);
            this.labelusername.Name = "labelusername";
            this.labelusername.Size = new System.Drawing.Size(170, 70);
            this.labelusername.TabIndex = 6;
            this.labelusername.Text = "用户名：";
            this.labelusername.Click += new System.EventHandler(this.labelusername_Click);
            this.labelusername.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseDown);
            this.labelusername.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseMove);
            this.labelusername.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseUp);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPassword.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBoxPassword.Location = new System.Drawing.Point(176, 78);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(348, 43);
            this.textBoxPassword.TabIndex = 1;
            this.textBoxPassword.UseSystemPasswordChar = true;
            this.textBoxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPassword_KeyPress);
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUsername.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.textBoxUsername.Location = new System.Drawing.Point(176, 8);
            this.textBoxUsername.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(348, 43);
            this.textBoxUsername.TabIndex = 0;
            this.textBoxUsername.TextChanged += new System.EventHandler(this.textBoxUsername_TextChanged);
            this.textBoxUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxUsername_KeyPress);
            this.textBoxUsername.Leave += new System.EventHandler(this.textBoxUsername_Leave);
            // 
            // buttonClosing
            // 
            this.buttonClosing.BackgroundImage = global::WMS.UI.Properties.Resources.bottonB4_q;
            this.buttonClosing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonClosing.FlatAppearance.BorderSize = 0;
            this.buttonClosing.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonClosing.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonClosing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClosing.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.buttonClosing.Location = new System.Drawing.Point(464, 450);
            this.buttonClosing.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.buttonClosing.Name = "buttonClosing";
            this.buttonClosing.Size = new System.Drawing.Size(176, 58);
            this.buttonClosing.TabIndex = 5;
            this.buttonClosing.Text = "退出";
            this.buttonClosing.UseVisualStyleBackColor = true;
            this.buttonClosing.Click += new System.EventHandler(this.buttonClosing_Click);
            this.buttonClosing.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonClosing_MouseDown);
            this.buttonClosing.MouseEnter += new System.EventHandler(this.buttonClosing_MouseEnter);
            this.buttonClosing.MouseLeave += new System.EventHandler(this.buttonClosing_MouseLeave);
            // 
            // buttonEnter
            // 
            this.buttonEnter.BackColor = System.Drawing.Color.Transparent;
            this.buttonEnter.BackgroundImage = global::WMS.UI.Properties.Resources.bottonB2_s;
            this.buttonEnter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonEnter.FlatAppearance.BorderColor = System.Drawing.Color.MintCream;
            this.buttonEnter.FlatAppearance.BorderSize = 0;
            this.buttonEnter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.buttonEnter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.buttonEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEnter.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.buttonEnter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonEnter.Location = new System.Drawing.Point(184, 450);
            this.buttonEnter.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.buttonEnter.Name = "buttonEnter";
            this.buttonEnter.Size = new System.Drawing.Size(176, 58);
            this.buttonEnter.TabIndex = 4;
            this.buttonEnter.Text = "确认";
            this.buttonEnter.UseVisualStyleBackColor = false;
            this.buttonEnter.Click += new System.EventHandler(this.buttonEnter_Click);
            this.buttonEnter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonEnter_MouseDown);
            this.buttonEnter.MouseEnter += new System.EventHandler(this.buttonEnter_MouseEnter);
            this.buttonEnter.MouseLeave += new System.EventHandler(this.buttonEnter_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 20F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(44, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(716, 72);
            this.label1.TabIndex = 7;
            this.label1.Text = "安途丰达WMS仓库管理系统";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseUp);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(260, 515);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(306, 35);
            this.labelStatus.TabIndex = 6;
            this.labelStatus.Text = "正在登陆，请耐心等待...";
            this.labelStatus.Visible = false;
            // 
            // labelClickCount
            // 
            this.labelClickCount.AutoSize = true;
            this.labelClickCount.BackColor = System.Drawing.Color.Transparent;
            this.labelClickCount.Location = new System.Drawing.Point(668, 362);
            this.labelClickCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelClickCount.Name = "labelClickCount";
            this.labelClickCount.Size = new System.Drawing.Size(22, 24);
            this.labelClickCount.TabIndex = 12;
            this.labelClickCount.Text = "1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelStatus);
            this.panel1.Controls.Add(this.buttonClosing);
            this.panel1.Controls.Add(this.buttonEnter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(150, 50);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 550);
            this.panel1.TabIndex = 13;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseUp);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.labelusername, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxWarehouse, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.labelPsaaword, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxAccountBook, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBoxUsername, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxPassword, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(124, 136);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(530, 280);
            this.tableLayoutPanel2.TabIndex = 16;
            // 
            // comboBoxWarehouse
            // 
            this.comboBoxWarehouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWarehouse.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.comboBoxWarehouse.FormattingEnabled = true;
            this.comboBoxWarehouse.Location = new System.Drawing.Point(178, 223);
            this.comboBoxWarehouse.Margin = new System.Windows.Forms.Padding(8, 13, 8, 13);
            this.comboBoxWarehouse.Name = "comboBoxWarehouse";
            this.comboBoxWarehouse.Size = new System.Drawing.Size(344, 43);
            this.comboBoxWarehouse.TabIndex = 3;
            // 
            // comboBoxAccountBook
            // 
            this.comboBoxAccountBook.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxAccountBook.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAccountBook.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.comboBoxAccountBook.FormattingEnabled = true;
            this.comboBoxAccountBook.Location = new System.Drawing.Point(178, 153);
            this.comboBoxAccountBook.Margin = new System.Windows.Forms.Padding(8, 13, 8, 13);
            this.comboBoxAccountBook.Name = "comboBoxAccountBook";
            this.comboBoxAccountBook.Size = new System.Drawing.Size(344, 43);
            this.comboBoxAccountBook.TabIndex = 2;
            this.comboBoxAccountBook.SelectedIndexChanged += new System.EventHandler(this.comboBoxAccountBook_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 140);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 70);
            this.label2.TabIndex = 12;
            this.label2.Text = "账   套：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 210);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 70);
            this.label3.TabIndex = 13;
            this.label3.Text = "仓   库：";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 800F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 550F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1100, 650);
            this.tableLayoutPanel1.TabIndex = 14;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            this.tableLayoutPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseDown);
            this.tableLayoutPanel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseMove);
            this.tableLayoutPanel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseUp);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 550);
            this.panel2.TabIndex = 14;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseUp);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(950, 50);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(150, 550);
            this.panel3.TabIndex = 15;
            this.panel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseDown);
            this.panel3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseMove);
            this.panel3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseUp);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1100, 650);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.labelClickCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欢迎使用安途丰达WMS仓库管理系统";
            this.Activated += new System.EventHandler(this.FormLogin_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogin_FormClosing);
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.Shown += new System.EventHandler(this.FormLogin_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormLogin_KeyPress);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormLogin_MouseUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPsaaword;
        private System.Windows.Forms.Label labelusername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Button buttonClosing;
        private System.Windows.Forms.Button buttonEnter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelClickCount;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBoxAccountBook;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxWarehouse;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}