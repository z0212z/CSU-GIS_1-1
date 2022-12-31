using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_1_1.MyForms
{
    public partial class FormEditFeature : Form
    {
        IFeatureClass featureClass;
        IFeature feature;
        public FormEditFeature(IFeatureClass featureClass, IFeature feature)
        {
            InitializeComponent();
            this.featureClass = featureClass;
            this.feature = feature;
        }

        private void FormEditFeature_Load(object sender, EventArgs e)
        {
            this.DisplayFeatureClassInfo();
        }

        public void DisplayFeatureClassInfo()
        {
            int j = 0;
            try
            {
                IFields fields = feature.Fields;
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (fields.Field[i].Type == esriFieldType.esriFieldTypeOID
                        || fields.Field[i].Type == esriFieldType.esriFieldTypeGeometry)
                        continue;
                    Debug.WriteLine(string.Format(" >> {0}:{1}", fields.Field[i].AliasName,feature.Value[i]));
                    this.dgvFields.Rows.Add(fields.Field[i].AliasName, feature.Value[i]);
                    this.dgvFields.Rows[j].Tag = i;
                    j++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[异常]" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int i;
                IField field;
                foreach(DataGridViewRow row in this.dgvFields.Rows)
                {
                    i = (int)row.Tag;
                    field = feature.Fields.Field[i];
                    if (field.Type == esriFieldType.esriFieldTypeString)
                        feature.Value[i] = row.Cells[1].Value.ToString();
                    else if(field.Type == esriFieldType.esriFieldTypeInteger
                        || field.Type == esriFieldType.esriFieldTypeSmallInteger)
                        feature.Value[i] = int.Parse(row.Cells[1].Value.ToString());
                    else if (field.Type == esriFieldType.esriFieldTypeSingle
                        || field.Type == esriFieldType.esriFieldTypeDouble)
                        feature.Value[i] = Single.Parse(row.Cells[1].Value.ToString());
                    else
                        feature.Value[i] = row.Cells[1].Value;
                }
                feature.Store();

                MessageBox.Show("要素修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show("[异常]" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvFields_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
