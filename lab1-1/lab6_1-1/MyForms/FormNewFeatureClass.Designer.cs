
namespace lab4_1_1.MyForms
{
    partial class FormNewFeatureClass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewFeatureClass));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbSR = new System.Windows.Forms.ComboBox();
            this.cmbGeometryType = new System.Windows.Forms.ComboBox();
            this.textShp = new System.Windows.Forms.TextBox();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.空间坐标 = new System.Windows.Forms.Label();
            this.几何类型 = new System.Windows.Forms.Label();
            this.shp文件 = new System.Windows.Forms.Label();
            this.数据目录 = new System.Windows.Forms.Label();
            this.textFolder = new System.Windows.Forms.TextBox();
            this.dgvFields = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.字段列表 = new System.Windows.Forms.GroupBox();
            this.btnDelFeild = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).BeginInit();
            this.字段列表.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbSR);
            this.groupBox1.Controls.Add(this.cmbGeometryType);
            this.groupBox1.Controls.Add(this.textShp);
            this.groupBox1.Controls.Add(this.btnBrowser);
            this.groupBox1.Controls.Add(this.空间坐标);
            this.groupBox1.Controls.Add(this.几何类型);
            this.groupBox1.Controls.Add(this.shp文件);
            this.groupBox1.Controls.Add(this.数据目录);
            this.groupBox1.Controls.Add(this.textFolder);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 161);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据目录及shp文件名称";
            // 
            // cmbSR
            // 
            this.cmbSR.FormattingEnabled = true;
            this.cmbSR.Items.AddRange(new object[] {
            "EPSG:0",
            "EPSG:4326",
            "EPSG:3857"});
            this.cmbSR.Location = new System.Drawing.Point(115, 105);
            this.cmbSR.Name = "cmbSR";
            this.cmbSR.Size = new System.Drawing.Size(121, 23);
            this.cmbSR.TabIndex = 8;
            this.cmbSR.SelectedIndexChanged += new System.EventHandler(this.cmbSR_SelectedIndexChanged);
            // 
            // cmbGeometryType
            // 
            this.cmbGeometryType.FormattingEnabled = true;
            this.cmbGeometryType.Items.AddRange(new object[] {
            "点",
            "线",
            "面"});
            this.cmbGeometryType.Location = new System.Drawing.Point(357, 56);
            this.cmbGeometryType.Name = "cmbGeometryType";
            this.cmbGeometryType.Size = new System.Drawing.Size(121, 23);
            this.cmbGeometryType.TabIndex = 7;
            // 
            // textShp
            // 
            this.textShp.Location = new System.Drawing.Point(109, 63);
            this.textShp.Name = "textShp";
            this.textShp.Size = new System.Drawing.Size(100, 25);
            this.textShp.TabIndex = 6;
            // 
            // btnBrowser
            // 
            this.btnBrowser.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowser.Image")));
            this.btnBrowser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowser.Location = new System.Drawing.Point(455, 25);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(64, 23);
            this.btnBrowser.TabIndex = 5;
            this.btnBrowser.Text = "浏览";
            this.btnBrowser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
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
            this.textFolder.Size = new System.Drawing.Size(327, 25);
            this.textFolder.TabIndex = 0;
            this.textFolder.Click += new System.EventHandler(this.Folder_Click);
            this.textFolder.TextChanged += new System.EventHandler(this.textFolder_TextChanged);
            this.textFolder.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textFolder_KeyPress);
            // 
            // dgvFields
            // 
            this.dgvFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colAlias,
            this.colType,
            this.colLength});
            this.dgvFields.Location = new System.Drawing.Point(0, 15);
            this.dgvFields.Name = "dgvFields";
            this.dgvFields.RowHeadersWidth = 51;
            this.dgvFields.RowTemplate.Height = 27;
            this.dgvFields.Size = new System.Drawing.Size(580, 243);
            this.dgvFields.TabIndex = 1;
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
            this.colType.Items.AddRange(new object[] {
            "整数",
            "数字",
            "日期",
            "文本"});
            this.colType.MinimumWidth = 6;
            this.colType.Name = "colType";
            this.colType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colType.Width = 125;
            // 
            // colLength
            // 
            this.colLength.HeaderText = "字段长度";
            this.colLength.MinimumWidth = 6;
            this.colLength.Name = "colLength";
            this.colLength.Width = 125;
            // 
            // 字段列表
            // 
            this.字段列表.Controls.Add(this.dgvFields);
            this.字段列表.Location = new System.Drawing.Point(13, 181);
            this.字段列表.Name = "字段列表";
            this.字段列表.Size = new System.Drawing.Size(580, 264);
            this.字段列表.TabIndex = 2;
            this.字段列表.TabStop = false;
            this.字段列表.Text = "字段列表";
            // 
            // btnDelFeild
            // 
            this.btnDelFeild.Location = new System.Drawing.Point(32, 452);
            this.btnDelFeild.Name = "btnDelFeild";
            this.btnDelFeild.Size = new System.Drawing.Size(75, 23);
            this.btnDelFeild.TabIndex = 3;
            this.btnDelFeild.Text = "删除字段";
            this.btnDelFeild.UseVisualStyleBackColor = true;
            this.btnDelFeild.Click += new System.EventHandler(this.btnDelFeild_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(193, 451);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确 定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(330, 451);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关 闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormNewFeatureClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 503);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnDelFeild);
            this.Controls.Add(this.字段列表);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormNewFeatureClass";
            this.Text = "创建SHP文件";
            this.Load += new System.EventHandler(this.FormNewFeatureClass_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).EndInit();
            this.字段列表.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvFields;
        private System.Windows.Forms.GroupBox 字段列表;
        private System.Windows.Forms.Button btnDelFeild;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.TextBox textShp;
        private System.Windows.Forms.Label 空间坐标;
        private System.Windows.Forms.Label 几何类型;
        private System.Windows.Forms.Label shp文件;
        private System.Windows.Forms.Label 数据目录;
        public System.Windows.Forms.TextBox textFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlias;
        private System.Windows.Forms.DataGridViewComboBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLength;
        private System.Windows.Forms.ComboBox cmbSR;
        public System.Windows.Forms.ComboBox cmbGeometryType;
        private System.Windows.Forms.Button btnBrowser;
    }
}