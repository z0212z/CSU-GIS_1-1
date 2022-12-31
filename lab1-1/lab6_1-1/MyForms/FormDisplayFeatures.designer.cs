
namespace lab4_1_1.MyForms
{
    partial class FormDisplayFeatures
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
            this.数据列表 = new System.Windows.Forms.GroupBox();
            this.dgvFields = new System.Windows.Forms.DataGridView();
            this.tsdCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.数据列表.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).BeginInit();
            this.SuspendLayout();
            // 
            // 数据列表
            // 
            this.数据列表.Controls.Add(this.dgvFields);
            this.数据列表.Location = new System.Drawing.Point(12, 12);
            this.数据列表.Name = "数据列表";
            this.数据列表.Size = new System.Drawing.Size(776, 363);
            this.数据列表.TabIndex = 5;
            this.数据列表.TabStop = false;
            this.数据列表.Text = "数据列表";
            // 
            // dgvFields
            // 
            this.dgvFields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFields.Location = new System.Drawing.Point(0, 15);
            this.dgvFields.Name = "dgvFields";
            this.dgvFields.RowHeadersWidth = 51;
            this.dgvFields.RowTemplate.Height = 27;
            this.dgvFields.Size = new System.Drawing.Size(770, 342);
            this.dgvFields.TabIndex = 2;
            // 
            // tsdCount
            // 
            this.tsdCount.Location = new System.Drawing.Point(90, 426);
            this.tsdCount.Name = "tsdCount";
            this.tsdCount.Size = new System.Drawing.Size(100, 25);
            this.tsdCount.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 429);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "要素数量";
            // 
            // FormDisplayFeatures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tsdCount);
            this.Controls.Add(this.数据列表);
            this.Name = "FormDisplayFeatures";
            this.Text = "FormDisplayFeatures";
            this.Load += new System.EventHandler(this.FormDisplayFeatures_Load);
            this.数据列表.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFields)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox 数据列表;
        private System.Windows.Forms.DataGridView dgvFields;
        private System.Windows.Forms.TextBox tsdCount;
        private System.Windows.Forms.Label label1;
    }
}