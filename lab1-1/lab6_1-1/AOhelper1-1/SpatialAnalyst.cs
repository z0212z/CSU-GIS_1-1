using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_1_1.AOhelper1_1
{
    class SpatialAnalyst
    {
        ILayer selectedLayer;
        public SpatialAnalyst(ILayer layer)
        {
            this.selectedLayer = layer;
        }
        public void pointAnalyst(IPoint pPoint)
        {
            double dbElevation = -1, dbAspect = -1, dbSlope = -1;//定义变量用于存储高程、坡度和坡向
            ILayer pLayer = selectedLayer; //获得DEM图层
            if (pLayer == null) return;
            IRasterLayer pRasterLayer = pLayer as IRasterLayer;//QI至IRasterLayer接口
            if (pRasterLayer == null) return;
            ESRI.ArcGIS.Analyst3D.IRasterSurface pRasterSurf =
            new ESRI.ArcGIS.Analyst3D.RasterSurface();//创建RasterSurface对象
            pRasterSurf.PutRaster(pRasterLayer.Raster, 0);//设置栅格对象的表面数据
            ISurface pSurface = pRasterSurf as ISurface;//转换为ISurface接口
            if (pSurface == null) return;
            dbElevation = pSurface.GetElevation(pPoint);//获得点位高程
            dbAspect = pSurface.GetAspectDegrees(pPoint);//获得点位坡向
            dbSlope = pSurface.GetSlopeDegrees(pPoint);//获得点位坡度
            string sz = string.Format("高度【{0:F3}】米\n坡度【{1:F3}】度 \n坡向【{2:F3}】度"
            , dbElevation, dbSlope, dbAspect);
            MessageBox.Show(sz, "地形分析统计-坡度统计"
            , MessageBoxButtons.OKCancel
            ,MessageBoxIcon.Information);
        }

        public static bool Get3DInfoByPolyline(ILayer layer
            , IPolyline polyline
            , out IList<double> elevs
            , out IList<double> aspects
            , out IList<double> slopes
            , out string msg)
        {
            
            elevs = new List<double>();
            aspects = new List<double>();
            slopes = new List<double>();
            msg = "";
            double dbElevation = -1, dbAspect = -1, dbSlope = -1;//定义变量用于存储高程、坡度和坡向
            ILayer pLayer = layer; //获得DEM图层
            if (pLayer == null) return false;
            IRasterLayer pRasterLayer = pLayer as IRasterLayer;//QI至IRasterLayer接口
            if (pRasterLayer == null) return false;
            ESRI.ArcGIS.Analyst3D.IRasterSurface pRasterSurf =
            new ESRI.ArcGIS.Analyst3D.RasterSurface();//创建RasterSurface对象
            pRasterSurf.PutRaster(pRasterLayer.Raster, 0);//设置栅格对象的表面数据
            ISurface pSurface = pRasterSurf as ISurface;//转换为ISurface接口
            if (pSurface == null) return false;
            IPointCollection pc = polyline as IPointCollection;
            IPoint linept;
            for (int i = 0; i < pc.PointCount; i++)
            {
                linept = pc.Point[i];
                dbElevation = pSurface.GetElevation(linept);//获得点位高程
                elevs.Add(dbElevation);
                dbAspect = pSurface.GetAspectDegrees(linept);//获得点位坡向
                aspects.Add(dbAspect);
                dbSlope = pSurface.GetSlopeDegrees(linept);//获得点位坡度
                slopes.Add(dbSlope);
            }
            return true;
        }
    }
}
