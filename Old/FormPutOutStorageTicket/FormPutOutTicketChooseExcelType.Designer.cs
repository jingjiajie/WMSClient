namespace WMS.UI
{
    partial class FormPutOutTicketChooseExcelType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPutOutTicketChooseExcelType));
            this.buttonCover = new System.Windows.Forms.Button();
            this.buttonNormal = new System.Windows.Forms.Button();
            this.buttonMoBiSi = new System.Windows.Forms.Button();
            this.buttonZhongDu = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonZhongDuFlow = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCover
            // 
            this.buttonCover.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonCover.BackgroundImage")));
            this.buttonCover.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonCover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCover.FlatAppearance.BorderSize = 0;
            this.buttonCover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCover.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonCover.Location = new System.Drawing.Point(153, 4);
            this.buttonCover.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCover.Name = "buttonCover";
            this.buttonCover.Size = new System.Drawing.Size(141, 91);
            this.buttonCover.TabIndex = 1;
            this.buttonCover.Text = "顺义套单";
            this.buttonCover.UseVisualStyleBackColor = true;
            this.buttonCover.Click += new System.EventHandler(this.buttonInner_Click);
            this.buttonCover.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonCover_MouseDown);
            this.buttonCover.MouseEnter += new System.EventHandler(this.buttonCover_MouseEnter);
            this.buttonCover.MouseLeave += new System.EventHandler(this.buttonCover_MouseLeave);
            // 
            // buttonNormal
            // 
            this.buttonNormal.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonNormal.BackgroundImage")));
            this.buttonNormal.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonNormal.FlatAppearance.BorderSize = 0;
            this.buttonNormal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNormal.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonNormal.Location = new System.Drawing.Point(4, 4);
            this.buttonNormal.Margin = new System.Windows.Forms.Padding(4);
            this.buttonNormal.Name = "buttonNormal";
            this.buttonNormal.Size = new System.Drawing.Size(141, 91);
            this.buttonNormal.TabIndex = 0;
            this.buttonNormal.Text = "普通出库单";
            this.buttonNormal.UseVisualStyleBackColor = true;
            this.buttonNormal.Click += new System.EventHandler(this.buttonCover_Click);
            this.buttonNormal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonNormal_MouseDown);
            this.buttonNormal.MouseEnter += new System.EventHandler(this.buttonNormal_MouseEnter);
            this.buttonNormal.MouseLeave += new System.EventHandler(this.buttonNormal_MouseLeave);
            // 
            // buttonMoBiSi
            // 
            this.buttonMoBiSi.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonMoBiSi.BackgroundImage")));
            this.buttonMoBiSi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonMoBiSi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMoBiSi.FlatAppearance.BorderSize = 0;
            this.buttonMoBiSi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMoBiSi.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonMoBiSi.Location = new System.Drawing.Point(302, 4);
            this.buttonMoBiSi.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMoBiSi.Name = "buttonMoBiSi";
            this.buttonMoBiSi.Size = new System.Drawing.Size(141, 91);
            this.buttonMoBiSi.TabIndex = 2;
            this.buttonMoBiSi.Text = "摩比斯出库单";
            this.buttonMoBiSi.UseVisualStyleBackColor = true;
            this.buttonMoBiSi.Click += new System.EventHandler(this.buttonMoBiSi_Click);
            this.buttonMoBiSi.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonMoBiSi_MouseDown);
            this.buttonMoBiSi.MouseEnter += new System.EventHandler(this.buttonMoBiSi_MouseEnter);
            this.buttonMoBiSi.MouseLeave += new System.EventHandler(this.buttonMoBiSi_MouseLeave);
            // 
            // buttonZhongDu
            // 
            this.buttonZhongDu.BackgroundImage = global::WMS.UI.Properties.Resources.bottonB2_q;
            this.buttonZhongDu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonZhongDu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZhongDu.FlatAppearance.BorderSize = 0;
            this.buttonZhongDu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZhongDu.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonZhongDu.Location = new System.Drawing.Point(451, 4);
            this.buttonZhongDu.Margin = new System.Windows.Forms.Padding(4);
            this.buttonZhongDu.Name = "buttonZhongDu";
            this.buttonZhongDu.Size = new System.Drawing.Size(141, 91);
            this.buttonZhongDu.TabIndex = 3;
            this.buttonZhongDu.Text = "中都出库单";
            this.buttonZhongDu.UseVisualStyleBackColor = true;
            this.buttonZhongDu.Click += new System.EventHandler(this.buttonZhongDu_Click);
            this.buttonZhongDu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonZhongDu_MouseDown);
            this.buttonZhongDu.MouseEnter += new System.EventHandler(this.buttonZhongDu_MouseEnter);
            this.buttonZhongDu.MouseLeave += new System.EventHandler(this.buttonZhongDu_MouseLeave);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.buttonNormal, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonZhongDu, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCover, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMoBiSi, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonZhongDuFlow, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(748, 99);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // buttonZhongDuFlow
            // 
            this.buttonZhongDuFlow.BackgroundImage = global::WMS.UI.Properties.Resources.bottonB2_q;
            this.buttonZhongDuFlow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonZhongDuFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZhongDuFlow.FlatAppearance.BorderSize = 0;
            this.buttonZhongDuFlow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZhongDuFlow.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.buttonZhongDuFlow.Location = new System.Drawing.Point(600, 4);
            this.buttonZhongDuFlow.Margin = new System.Windows.Forms.Padding(4);
            this.buttonZhongDuFlow.Name = "buttonZhongDuFlow";
            this.buttonZhongDuFlow.Size = new System.Drawing.Size(144, 91);
            this.buttonZhongDuFlow.TabIndex = 4;
            this.buttonZhongDuFlow.Text = "中都出库流水";
            this.buttonZhongDuFlow.UseVisualStyleBackColor = true;
            this.buttonZhongDuFlow.Click += new System.EventHandler(this.buttonZhongDuFlow_Click);
            this.buttonZhongDuFlow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonZhongDuFlow_MouseDown);
            this.buttonZhongDuFlow.MouseEnter += new System.EventHandler(this.buttonZhongDuFlow_MouseEnter);
            this.buttonZhongDuFlow.MouseLeave += new System.EventHandler(this.buttonZhongDuFlow_MouseLeave);
            // 
            // FormPutOutTicketChooseExcelType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 35F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 99);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPutOutTicketChooseExcelType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择报表类型";
            this.Load += new System.EventHandler(this.FormPutOutTicketChooseExcelType_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCover;
        private System.Windows.Forms.Button buttonNormal;
        private System.Windows.Forms.Button buttonMoBiSi;
        private System.Windows.Forms.Button buttonZhongDu;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonZhongDuFlow;
    }
}