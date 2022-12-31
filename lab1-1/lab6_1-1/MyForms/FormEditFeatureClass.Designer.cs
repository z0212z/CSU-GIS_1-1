
namespace lab4_1_1.MyForms
{
    partial class FormEditFeatureClass
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
            this.textSR = new System.Windows.Forms.TextBox();
            this.textGeometryType = new System.Windows.Forms.TextBox();
            this.textShp = new System.Windows.Forms.TextBox();
            this.空间坐标 = new System.Windows.Forms.Label();
            this.几何类型 = new System.Windows.Forms.Label();
            this.shp文件 = new System.Windows.Forms.Label();
            this.数据目录 = new System.Windows.Forms.Label();
            this.textFolder = new System.Windows.Forms.TextBox();
            this.dgvFields = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelFeild = new System.Windows.Forms.Button();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textSR);
            this.groupBox1.Controls.Add(this.textGeometryType);
            this.groupBox1.Controls.Add(this.textShp);
            this.groupBox1.Controls.Add(this.空间坐标);
            this.groupBox1.Controls.Add(this.几何类型);
            this.groupBox1.Controls.Add(this.shp文件);
            this.groupBox1.Controls.Add(this.数据目录);
            this.groupBox1.Controls.Add(this.textFolder);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 161);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据目录及shp文件名称";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // textSR
            // 
            this.textSR.Location = new System.Drawing.Point(114, 102);
            this.textSR.Name = "textSR";
            this.textSR.Size = new System.Drawing.Size(342, 25);
            this.textSR.TabIndex = 8;
            // 
            // textGeometryType
            // 
            this.textGeometryType.Location = new System.Drawing.Point(356, 60);
            this.textGeometryType.Name = "textGeometryType";
            this.textGeometryType.Size = new System.Drawing.Size(100, 25);
            this.textGeometryType.TabIndex = 7;
            // 
            // textShp
            // 
            this.textShp.Location = new System.Drawing.Point(109, 63);
            this.textShp.Name = "textShp";
            this.textShp.Size = new System.Drawing.Size(168, 25);
            this.textShp.TabIndex = 6;
            // 
            // 空间坐标
            // 
            this.空间坐标.AutoSize = true;
            this.空间坐标.Location = new System.Drawing.Point(41, 105);
            this.空间坐标.Name = "空间坐标";
            this.空间坐标.Size = new System.Drawing.Size(67, 15);
            this.空间坐标.TabIndex = 4;
            this.空间坐标.Text = "空间坐标";
            // 
            // 几何类型
            // 
            this.几何类型.AutoSize = true;
            this.几何类型.Location = new System.Drawing.Point(283, 63);
            this.几何类型.Name = "几何类型";
            this.几何类型.Size = new System.Drawing.Size(67, 15);
            this.几何类型.TabIndex = 3;
            this.几何类型.Text = "几何类型";
            // 
            // shp文件
            // 
            this.shp文件.AutoSize = true;
            this.shp文件.Location = new System.Drawing.Point(41, 63);
            this.shp文件.Name = "shp文件";
            this.shp文件.Size = new System.Drawing.Size(61, 15);
            this.shp文件.TabIndex = 2;
            this.shp文件.Text = "shp文件";
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
            this.textFolder.Size = new System.Drawing.Size(335, 25);
            this.textFolder.TabIndex = 0;
            // 
            // dgvFields
            // 
            this.dgvFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colAlias,
            this.colType,
            this.colLength});
            this.dgvFields.Location = new System.Drawing.Point(12, 179);
            this.dgvFields.Name = "dgvFields";
            this.dgvFields.RowHeadersWidth = 51;
            this.dgvFields.RowTemplate.Height = 27;
            this.dgvFields.Size = new System.Drawing.Size(580, 243);
            this.dgvFields.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(180, 467);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确 定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(320, 467);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "关 闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelFeild
            // 
            this.btnDelFeild.Location = new System.Drawing.Point(45, 467);
            this.btnDelFeild.Name = "btnDelFeild";
            this.btnDelFeild.Size = new System.Drawing.Size(75, 23);
            this.btnDelFeild.TabIndex = 7;
            this.btnDelFeild.Text = "删除字段";
            this.btnDelFeild.UseVisualStyleBackColor = true;
            this.btnDelFeild.Click += new System.EventHandler(this.btnDelFeild_Click);
            // 
            // colName
            // 
            this.colName.HeaderText = "字段名称";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.Width = 125;
            // 
            // colAlias
            // 
            this.colAlias.HeaderText = "字段别名";
            this.colAlias.MinimumWidth = 6;
            this.colAlias.Name = "colAlias";
            this.colAlias.Width = 125;
            // 
            // colType
            // 
            this.colType.HeaderText = "字段类型";
            this.colType.MinimumWidth = 6;
            this.colType.Name = "colType";
            this.colType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colType.Width = 125;
            // 
            // colLength
            // 
            this.colLength.HeaderText = "字段长度";
            this.colLength.MinimumWidth = 6;
            this.colLength.Name = "colLength";
            this.colLength.Width = 125;
            // 
            // FormEditFeatureClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 542);
            this.Controls.Add(this.btnDelFeild);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvFields);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormEditFeatureClass";
            this.Text = "FormEditFeatureClass";
            this.Load += new System.EventHandler(this.FormEditFeatureClass_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox textShp;
        private System.Windows.Forms.Label 空间坐标;
        private System.Windows.Forms.Label 几何类型;
        private System.Windows.Forms.Label shp文件;
        private System.Windows.Forms.Label 数据目录;
        public System.Windows.Forms.TextBox textFolder;
        public System.Windows.Forms.TextBox textSR;
        public System.Windows.Forms.TextBox textGeometryType;
        private System.Windows.Forms.DataGridView dgvFields;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelFeild;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlias;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
    }
}