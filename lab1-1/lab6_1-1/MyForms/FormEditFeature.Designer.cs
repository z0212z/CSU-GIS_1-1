
namespace lab4_1_1.MyForms
{
    partial class FormEditFeature
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
            this.字段列表 = new System.Windows.Forms.GroupBox();
            this.dgvFields = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.字段列表.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).BeginInit();
            this.SuspendLayout();
            // 
            // 字段列表
            // 
            this.字段列表.Controls.Add(this.dgvFields);
            this.字段列表.Location = new System.Drawing.Point(12, 6);
            this.字段列表.Name = "字段列表";
            this.字段列表.Size = new System.Drawing.Size(776, 363);
            this.字段列表.TabIndex = 4;
            this.字段列表.TabStop = false;
            this.字段列表.Text = "字段列表";
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
            this.dgvFields.Size = new System.Drawing.Size(770, 342);
            this.dgvFields.TabIndex = 2;
            this.dgvFields.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFields_CellContentClick);
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
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(308, 392);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确 定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(450, 392);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "关 闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormEditFeature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.字段列表);
            this.Name = "FormEditFeature";
            this.Text = "FormEditFeature";
            this.Load += new System.EventHandler(this.FormEditFeature_Load);
            this.字段列表.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox 字段列表;
        private System.Windows.Forms.DataGridView dgvFields;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colData;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
    }
}