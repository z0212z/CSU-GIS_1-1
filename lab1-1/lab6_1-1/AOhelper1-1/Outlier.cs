using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_1_1.AOhelper1_1
{
    class Outlier
    {
        public enum Method { Ignore, IsOutlier }
        public List<IFeature> outliers;
        IFeatureClass featureClass;
        double size;
        int fieldIndex;
        Method method = Method.Ignore;
        int minPointsNum;
        int multiTimes;
        public Outlier(
        IFeatureClass featureClass,
        double size,
        int fieldIndex,
        Method method = Method.Ignore,
        int minPointsNum = 5,
        int multiTimes = 3)
        {
            this.featureClass = featureClass;
            this.size = size;
            this.minPointsNum = minPointsNum;
            this.fieldIndex = fieldIndex;
            this.multiTimes = multiTimes;
            this.method = method;
        }
        public List<IFeature> Search(
            ToolStripStatusLabel tipLabel = null,
            ToolStripProgressBar progressBar = null)
        {
            try
            {
                if (this.featureClass == null)
                    throw new Exception("要素类为空");
                int featureCount = this.featureClass.FeatureCount(null);
                //处理状态栏、 提示栏和进度条
                StatusStrip statusBar = null;
                if (progressBar != null)
                    statusBar = (StatusStrip)progressBar.GetCurrentParent();
                if (tipLabel != null)
                    tipLabel.Text = "正在用滑动窗法搜索异常点...";
                if (progressBar != null)
                {
                    progressBar.Value = 0;
                    progressBar.Maximum = featureCount;
                    statusBar.Refresh();
                }

                this.outliers = new List<IFeature>();
                IFeatureCursor cursor = this.featureClass.Search(null, false);
                IFeature feat;
                while ((feat = cursor.NextFeature()) != null)
                {
                    List<double> nn = this.getNeighborH(feat);
                    //忽略窗邻近域点数小于最小阐值的点
                    if (nn.Count < this.minPointsNum)
                    {
                        //如果判断方法是定义为异常，则直接加入异常列表
                        if (this.method == Method.IsOutlier)
                            this.outliers.Add(feat);
                    }
                    else
                    {
                        //邻近域的高程和、标准差、均值
                        double sum = 0, avg, std;
                        //计算高程和
                        foreach (double d in nn)
                            sum += d;
                        avg = sum / nn.Count;
                        sum = 0;
                        foreach (double d in nn)
                            sum += (d - avg) * (d - avg);
                        std = Math.Sqrt(sum / (nn.Count - 1));

                        double h = double.Parse(feat.Value[this.fieldIndex].ToString());
                        if (Math.Abs(h - avg) > this.multiTimes * std)
                            this.outliers.Add(feat);
                    }
                    if(progressBar!=null)
                    {
                        progressBar.Value++;
                        statusBar.Refresh();
                    }
                }
                return this.outliers;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<double> getNeighborH(IFeature dist)
        {
            try
            {
                //窗口邻近域高程值集合
                List<double> elevations = new List<double>();
                ISpatialFilter sf = new SpatialFilterClass { WhereClause = "FID<>" + dist.OID };
                IPoint pt = dist.Shape as IPoint;
                //构建滑动窗口
                IEnvelope win = new EnvelopeClass
                {
                    XMin = pt.X - size / 2,
                    XMax = pt.X + size / 2,
                    YMin = pt.Y - size / 2,
                    YMax = pt.Y + size / 2
                };
                sf.Geometry = win;
                sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                IFeatureCursor cursor = this.featureClass.Search(sf, false);
                IFeature feat;
                while ((feat = cursor.NextFeature()) != null)
                    elevations.Add(double.Parse(feat.Value[this.fieldIndex].ToString()));
                return elevations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
