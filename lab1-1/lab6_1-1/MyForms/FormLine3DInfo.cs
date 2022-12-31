using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
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
    public partial class FormLine3DInfo : Form
    {
        AxMapControl axMap;
        ILayer layer;
        public FormLine3DInfo(AxMapControl axMap)
        {
            this.axMap = axMap;
            this.layer = axMap.get_Layer(1);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.cmbLayer.SelectedIndex < 0)
            {
                MessageBox.Show("请选择DEM栅格图层!”，”提示");
                return;
            }
            ISelection selection = this.axMap.Map.FeatureSelection;
            IEnumFeature feats = selection as IEnumFeature;
            IFeature feat = feats.Next();
            if (feat==null)
            {
                MessageBox.Show("请在地图上选择要分析的线要素! ", "提示");
                return;
            }
            if (!(feat.Shape is IPolyline))
            {
                MessageBox.Show("当前选中的要素不是线要素!", "提示");
                return;
            }
            this.tslTip.Text = "开始生成剖面图，请稍后...";
            this.statusStrip1.Refresh();

            IList<double> elevs = new List<double>();
            IList<double> aspects = new List<double>();
            IList<double> slopes = new List<double>();
            IPolyline polyline = feat.Shape as IPolyline;
            string msg = "";

            //计算各顶点的三维数据
            if (SpatialAnalyst.Get3DInfoByPolyline(this.layer
                , polyline
                , out elevs
                , out aspects
                , out slopes
                , out msg) == true)
            {
                List<string> vertics = new List<string>();
                IPointCollection pc = polyline as IPointCollection;
                double x, y;
                for (int i = 0; i < pc.PointCount; i++)
                {
                    x = pc.Point[i].X;
                    y = pc.Point[i].Y;

                    vertics.Add(string.Format("{0}\n{1}"
                        , (x % 10000).ToString("0.00")
                        , (y % 10000).ToString("0.00")
                        ));
                }
                chart1.Series[0].Points.DataBindXY(vertics, elevs);
            }
            else
                MessageBox.Show("生成剖面错误：" + msg, "错误");
            this.tslTip.Text = "就绪";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
