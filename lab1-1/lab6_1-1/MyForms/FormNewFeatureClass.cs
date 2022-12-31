using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using lab4_1_1.AOhelper1_1;
using System;
using System.IO;
using System.Windows.Forms;

namespace lab4_1_1.MyForms
{
    public partial class FormNewFeatureClass : Form
    {
        public IFeatureClass featureClass;
        public FormNewFeatureClass()
        {
            InitializeComponent();
        }

        private void btnDelFeild_Click(object sender, EventArgs e)
        {
            DataGridViewRow row;
            for(int i=0;i<this.dgvFields.SelectedRows.Count;i++)
            {
                row = this.dgvFields.SelectedRows[i];
                this.dgvFields.Rows.Remove(row);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textFolder.Text))
            {
                MessageBox.Show("请选择数据目录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(this.textShp.Text))
            {
                MessageBox.Show("请输入SHAPE文件名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(this.cmbGeometryType.Text))
            {
                MessageBox.Show("请选择几何类型！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (string.IsNullOrEmpty(this.cmbSR.Text))
            {
                MessageBox.Show("请选择空间参考！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            IFieldsEdit myfields = new Fields() as IFieldsEdit;
            foreach(DataGridViewRow row in this.dgvFields.Rows)
            {
                if (row.Cells[0].Value == null
                    || string.IsNullOrEmpty(row.Cells[0].Value.ToString())
                    || row.Cells[2].Value == null
                    || string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                    continue;

                IFieldEdit field = new Field() as IFieldEdit;
                field.Name_2 = row.Cells[0].Value.ToString();

                if (row.Cells[1].Value == null || string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                    field.AliasName_2 = field.Name;
                else
                    field.AliasName_2 = row.Cells[1].Value.ToString().Trim();

                field.Type_2 = Util.ToFieldType(row.Cells[2].Value.ToString());

                if(field.Type==esriFieldType.esriFieldTypeString)
                {
                    if (row.Cells[3].Value == null)
                        field.Length_2 = 50;
                    else if (string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                        field.Length_2 = 50;
                    else
                    {
                        int len = 50;
                        if (!int.TryParse(row.Cells[3].Value.ToString(), out len))
                            len = 50;
                        field.Length_2 = len;
                    }
                }
                myfields.AddField(field);
            }
            try
            {
                string fileName = this.textShp.Text.Trim();
                if (fileName.ToLower().EndsWith(".shp") == false)
                    fileName = fileName + ".shp";

                string filePath = this.textFolder.Text + "\\" + fileName;
                if(File.Exists(filePath))
                {
                    if (MessageBox.Show("SHP文件已存在，是否覆盖？", "提示"
                        , MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                }

                esriGeometryType geomType = Util.ToGeometryType(this.cmbGeometryType.Text);

                int wkid = 0;
                int.TryParse(this.cmbSR.Text.Replace("EPSG:", ""), out wkid);
                ISpatialReference2 sr = null;
                if (wkid == 4326)
                    sr = SpatialReference.CreateGeographicCoordinate(wkid) as ISpatialReference2;
                else if(wkid==3857)
                    sr = SpatialReference.CreateProjectedCoordinate(wkid) as ISpatialReference2;
                else
                    sr = SpatialReference.CreateUnKnownSpatialReference() as ISpatialReference2;

                IFeatureClassDescription description = null;
                IFields fields = FieldHelper.CreateFields(out description, myfields, geomType, sr);

                this.featureClass = lab4_1_1.AOhelper1_1.FeatureClass.CreateShpFile(
                    this.textFolder.Text,
                    this.textShp.Text,
                    fields,
                    description.ShapeFieldName,
                    true);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("[异常]" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Folder_Click(object sender, EventArgs e)
        {

        }

        private void textFolder_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;
            this.textFolder.Text = fbd.SelectedPath;
        }

        private void textFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbSR_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FormNewFeatureClass_Load(object sender, EventArgs e)
        {

        }
    }
}
