/*************************************************
* 文件: LayerHelper.cs
* 说明：图层管理类
* 作者：中南大学李光强（QQ：41733233）
* 时间：2022/11/30/
* 声明：请尊重作者版权，使用此文件时，请保留此信息
***********************************************/

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab4_1_1.AOhelper1_1
{
    public enum DataSourceType
    {
        Unkown,
        ShapeFile,
        PersonalGDB,
        SDE
    }
    /// <summary>
    /// 图层工具类
    /// </summary>
    public class LayerHelper
    {

        #region 获取要素类的数据源
        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="layer">矢量图层</param>
        /// <param name="dataSourceType">数据源类型</param>
        /// <param name="featureClassName">要素类名称</param>
        /// <param name="dataSource">数据源</param>
        public static void GetDataSource(IFeatureLayer layer,
        out DataSourceType dataSourceType,
        out string featureClassName,
        out string dataSource)
        {
            try
            {
                dataSourceType = getDataSourceType(layer.DataSourceType);
                IFeatureClass featureClass = layer.FeatureClass;

                featureClassName = null;
                dataSource = null;
                lab4_1_1.AOhelper1_1.FeatureClass.GetDataSource(featureClass, out featureClassName, out dataSource);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取数据源路径
        /// </summary>
        /// <param name="layer">矢量图层</param>
        public static String GetDataSource(IFeatureLayer layer)
        {
            try
            {
                DataSourceType dataSourceType = getDataSourceType(layer.DataSourceType);

                string path = lab4_1_1.AOhelper1_1.FeatureClass.GetDataSource(layer.FeatureClass);
                if (dataSourceType == DataSourceType.ShapeFile)
                    path += ".shp";

                return path;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据数据源字符串转换为枚举
        /// </summary>
        /// <param name="dt"></param>
        private static DataSourceType getDataSourceType(string dt)
        {
            if (dt == "Personal Geodatabase Feature Class")
                return DataSourceType.PersonalGDB;
            else if (dt == "SDE Feature Class")
                return DataSourceType.SDE;
            else if (dt == "Shapefile Feature Class")
                return DataSourceType.ShapeFile;
            else
                return DataSourceType.Unkown;
        }
        #endregion

        #region 获取空间参考系/几何类型
        /// <summary>
        /// 获取空间参考系
        /// </summary>
        /// <param name="featureClass"></param>
        /// <returns></returns>
        public static ISpatialReference GetSpatialReference(IFeatureLayer layer)
        {
            return lab4_1_1.AOhelper1_1.FeatureClass.GetSpatialReference(layer.FeatureClass);
        }

        /// <summary>
        /// 获取空间参考系
        /// </summary>
        /// <param name="featureClass"></param>
        /// <returns></returns>
        public static esriGeometryType GetGeometryType(IFeatureLayer layer)
        {
            return layer.FeatureClass.ShapeType;
        }

        #endregion
    }
}
