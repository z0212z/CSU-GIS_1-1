using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using lab4_1_1.AOhelper1_1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_1_1.MyForms
{
    public partial class FormNewFeature : Form
    {
        IFeatureClass featureClass;
        IFeatureLayer layer;
        IGeometry geom;

        public FormNewFeature(IFeatureLayer layer,IGeometry geom)
        {
            InitializeComponent();
            this.featureClass = layer.FeatureClass;
            this.layer = layer;
            this.geom = geom;
        }

        private void FormNewFeature_Load(object sender, EventArgs e)
        {
            this.textFolder.Text = LayerHelper.GetDataSource(layer);
            this.DisplayFeatureClassInfo();
        }

        public void DisplayFeatureClassInfo()
        {
            this.featureClass = layer.FeatureClass;
            int j = 0;
            try
            {
                this.textFolder.Text = LayerHelper.GetDataSource(layer);
                IFields fields = featureClass.Fields;
                for(int i=0;i<fields.FieldCount;i++)
                {
                    if (fields.Field[i].Type == esriFieldType.esriFieldTypeOID
                        || fields.Field[i].Type == esriFieldType.esriFieldTypeGeometry)
                        continue;
                    this.dgvFields.Rows.Add(fields.Field[i].AliasName, null);
                    this.dgvFields.Rows[j].Tag = fields.Field[i].Name;
                    j++;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("[异常]" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            foreach (DataGridViewRow row in this.dgvFields.Rows)
            {
                data.Add(row.Tag.ToString(), row.Cells[1].Value);
            }
            data.Add(featureClass.ShapeFieldName, geom);

            try
            {
                AOhelper1_1.FeatureClass.AddFeature(this.featureClass, data);
                MessageBox.Show("要素添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("[异常]"+ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
