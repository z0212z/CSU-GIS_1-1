
namespace lab4_1_1.MyForms
{
    partial class FormNewFeature
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.数据目录 = new System.Windows.Forms.Label();
            this.textFolder = new System.Windows.Forms.TextBox();
            this.dgvFields = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.字段列表 = new System.Windows.Forms.GroupBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).BeginInit();
            this.字段列表.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.数据目录);
            this.groupBox1.Controls.Add(this.textFolder);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 94);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据目录及shp文件名称";
            // 
            // 数据目录
            // 
            this.数据目录.AutoSize = true;
            this.数据目录.Location = new System.Drawing.Point(38, 33);
            this.数据目录.Name = "数据目录";
            this.数据目录.Size = new System.Drawing.Size(67, 15);
            this.数据目录.TabIndex = 1;
            this.数据目录.Text = "数据目录";
            // 
            // textFolder
            // 
            this.textFolder.Location = new System.Drawing.Point(121, 24);
            this.textFolder.Name = "textFolder";
            this.textFolder.Size = new System.Drawing.Size(327, 25);
            this.textFolder.TabIndex = 0;
            // 
            // dgvFields
            // 
            this.dgvFields.AllowUserToAddRows = false;
            this.dgvFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colData});
            this.dgvFields.Location = new System.Drawing.Point(0, 15);
            this.dgvFields.Name = "dgvFields";
            this.dgvFields.RowHeadersWidth = 51;
            this.dgvFields.RowTemplate.Height = 27;
            this.dgvFields.Size = new System.Drawing.Size(770, 267);
            this.dgvFields.TabIndex = 2;
            // 
            // colName
            // 
            this.colName.HeaderText = "字段名称";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.Width = 200;
            // 
            // colData
            // 
            this.colData.HeaderText = "数据";
            this.colData.MinimumWidth = 6;
            this.colData.Name = "colData";
            this.colData.Width = 300;
            // 
            // 字段列表
            // 
            this.字段列表.Controls.Add(this.dgvFields);
            this.字段列表.Location = new System.Drawing.Point(12, 127);
            this.字段列表.Name = "字段列表";
            this.字段列表.Size = new System.Drawing.Size(776, 288);
            this.字段列表.TabIndex = 3;
            this.字段列表.TabStop = false;
            this.字段列表.Text = "字段列表";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(333, 466);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确 定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(481, 466);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "关 闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormNewFeature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 534);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.字段列表);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormNewFeature";
            this.Text = "FormNewFeature";
            this.Load += new System.EventHandler(this.FormNewFeature_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).EndInit();
            this.字段列表.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label 数据目录;
        public System.Windows.Forms.TextBox textFolder;
        private System.Windows.Forms.DataGridView dgvFields;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colData;
        private System.Windows.Forms.GroupBox 字段列表;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
    }
}