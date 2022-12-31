/*************************************************
 * 文件: SelectionSet.cs
 * 说明：要素选择集助手类
 * 作者：中南大学李光强（QQ：41733233）
 * 时间：2022/11/17/
 * 声明：请尊重作者版权，使用此文件时，请保留此信息
 ***********************************************/

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_1_1.AOhelper1_1
{
    /// <summary>
    /// 选择集类
    /// </summary>
    public class SelectionSet
    {
        /// <summary>
        /// 获取当前地图的要素选择集
        /// </summary>
        /// <param name="map">地图控件</param>
        /// <returns></returns>
        public static List<IFeature> ListSelectedFeatures(IMap map)
        {
            List<IFeature> list = new List<IFeature>();

            IEnumFeature pEnumFeat = (IEnumFeature)map.FeatureSelection;
            pEnumFeat.Reset();
            try
            {
                IFeature pfeat = pEnumFeat.Next();
                while (pfeat != null)
                {
                    list.Add(pfeat);
                    pfeat = pEnumFeat.Next();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定图层的要素选择集
        /// </summary>
        /// <param name="layer">图层</param>
        /// <returns></returns>
        public static List<IFeature> ListSelectedFeatures(ILayer layer)
        {
            List<IFeature> list = new List<IFeature>();
            try
            {
                ICursor pCursor = null;
                IFeatureCursor pFeatCur = null;
                if (layer == null) return null;
                IFeatureSelection fs = layer as IFeatureSelection;
                ISelectionSet pSelSet = fs.SelectionSet;
                if (pSelSet.Count == 0) return null;
                pSelSet.Search(null, false, out pCursor);
                pFeatCur = pCursor as IFeatureCursor;
                IFeature feature = pFeatCur.NextFeature();
                while (feature != null)
                {
                    list.Add(feature);
                    feature = pFeatCur.NextFeature();
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 使用多边形与其相交的要素
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="polygon"></param>
        public static void SelectFeatures(ILayer layer, IPolygon polygon)
        {
            ISpatialFilter filter = new SpatialFilter();
            filter.Geometry = polygon;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureSelection set = layer as IFeatureSelection;
            set.SelectFeatures(filter, esriSelectionResultEnum.esriSelectionResultNew, false);
            set.SelectionSet.Refresh();
        }

    }
}

