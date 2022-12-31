using ESRI.ArcGIS.Geodatabase;
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
    public partial class FormDisplayFeatures : Form
    {
        IFeatureClass featureClass;
        int pageSize = 100;
        public FormDisplayFeatures(IFeatureClass featureClass)
        {
            InitializeComponent();
            this.featureClass = featureClass;
        }

        private void FormDisplayFeatures_Load(object sender, EventArgs e)
        {
            /*if (this.tsdCount.Text == "全部")
                pageSize = 0;
            else
                pageSize = int.Parse(this.tsdCount.Text);*/

            this.DisplyFeature();
        }

        private void DisplyFeature()
        {
            try
            {
                DataTable dt = lab4_1_1.AOhelper1_1.FeatureClass.ToDataTable(this.featureClass);

                this.dgvFields.DataSource = dt;
                this.dgvFields.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show("[异常]" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
