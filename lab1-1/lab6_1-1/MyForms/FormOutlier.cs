using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using lab4_1_1.AOhelper1_1;
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
    public partial class FormOutlier : Form
    {
        IFeatureLayer layer;
        AxMapControl axMap;
        List<IFeature> outliers;
        public FormOutlier(AxMapControl axMap)
        {
            this.axMap = axMap;
            layer = axMap.get_Layer(0) as IFeatureLayer;
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer.SelectedIndex < 0)
            {
                MessageBox.Show("请选择要搜索的点图层!”，”提示");
                return;
            }
            if (this.cmbField.SelectedIndex < 0)
            {
                MessageBox.Show("请选择要分析的要素类字段! ", "提示");
                return;
            }
            double size = 0;
            if (double.TryParse(this.txtsize.Text, out size) == false)
            {
                MessageBox.Show("窗口大小尺寸值不是合法的数字!", "提示");
                return;
            }
            int numPoint = 0;
            if (int.TryParse(this.txtMinPointNum.Text, out numPoint) == false)
            {
                MessageBox.Show("窗口邻近最小点数阈值不是合法的整数!", "提示");
                return;
            }
            if (numPoint < 0)
            {
                MessageBox.Show("窗邻近最小点数阈值不是合法的非负整数!", "提示");
                return;
            }

            int fieldIndex = this.layer.FeatureClass.Fields.FindField(this.cmbField.Text);
            try
            {
                Outlier outlier = new Outlier(
                    this.layer.FeatureClass,
                    size,
                    fieldIndex,
                    Outlier.Method.Ignore,
                    numPoint,
                    (int)this.nudstdTimes.Value);
                this.outliers = outlier.Search(this.tslTip, this.tslprogress);
                this.dgvoutliers.Rows.Clear();
                foreach (IFeature feat in outliers)
                {
                    this.dgvoutliers.Rows.Add((object)feat.OID);
                    Debug.WriteLine(">>> outlier fid:" + feat.OID.ToString());
                    this.axMap.Map.SelectFeature(this.layer, feat);
                }
                this.dgvoutliers.Refresh();
                this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" [异常] :" +ex. Message,"错误");
            }
            finally
            {
                this.tslTip.Text ="就绪";
            }
        }

        private void dgvoutliers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            IFeature ft = this.outliers[e.RowIndex];
            this.axMap.CenterAt((IPoint)ft.Shape);
        }

        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
