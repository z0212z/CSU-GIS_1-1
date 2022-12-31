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
    public partial class FormIdentify : Form
    {
        IFeature feature;
        public FormIdentify(IFeature feature)
        {
            InitializeComponent();
            this.feature = feature;
        }

        private void FormIdentify_Load(object sender, EventArgs e)
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
                    Debug.WriteLine(string.Format(" >> {0}:{1}", fields.Field[i].AliasName, feature.Value[i]));
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
