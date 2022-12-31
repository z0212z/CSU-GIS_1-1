/*************************************************
 * 文件: SpatialReference.cs
 * 说明：空间参考类
 * 作者：中南大学李光强（QQ：41733233）
 * 时间：2022/11/17/
 * 声明：请尊重作者版权，使用此文件时，请保留此信息
 ***********************************************/

using ESRI.ArcGIS.Carto;
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
    /// 空间参考系类
    /// </summary>
    public class SpatialReference
    {
        #region 构建空间参考
        /// <summary>
        /// 根据prj文件创建空间参考
        /// </summary>
        /// <param name="strProFile">空间参照文件</param>
        /// <returns></returns>
        public static ISpatialReference CreateSpatialReference(string strProFile)
        {
            ISpatialReferenceFactory pSpatialReferenceFactory = new SpatialReferenceEnvironment();
            ISpatialReference pSpatialReference = pSpatialReferenceFactory.CreateESRISpatialReferenceFromPRJFile(strProFile);
            return pSpatialReference;
        }

        /// <summary>
        /// 创建地理坐标系
        /// </summary>
        /// <param name="gcType">esriSRProjCS4Type</param>
        /// <returns></returns>
        public static ISpatialReference CreateGeographicCoordinate(esriSRGeoCSType gcsType)
        {
            ISpatialReferenceFactory pSpatialReferenceFactory = new SpatialReferenceEnvironment();
            ISpatialReference pSpatialReference = pSpatialReferenceFactory.CreateGeographicCoordinateSystem((int)gcsType);
            return pSpatialReference;
        }

        /// <summary>
        /// 创建地理坐标系
        /// </summary>
        /// <param name="WKID">EPSG ID</param>
        /// <returns></returns>
        public static ISpatialReference CreateGeographicCoordinate(int WKID)
        {
            ISpatialReferenceFactory pSpatialReferenceFactory = new SpatialReferenceEnvironment();
            ISpatialReference pSpatialReference = pSpatialReferenceFactory.CreateGeographicCoordinateSystem(WKID);
            return pSpatialReference;
        }

        /// <summary>
        /// 创建投影坐标系
        /// </summary>
        /// <param name="pcType">esriSRProjCS4Type</param>
        /// <returns></returns>
        public static ISpatialReference CreateProjectedCoordinate(esriSRProjCS4Type pcsType)
        {
            ISpatialReferenceFactory2 pSpatialReferenceFactory = new SpatialReferenceEnvironment() as ISpatialReferenceFactory2;
            ISpatialReference pSpatialReference = pSpatialReferenceFactory.CreateProjectedCoordinateSystem((int)pcsType);
            return pSpatialReference;
        }

        /// <summary>
        /// 创建投影坐标系
        /// </summary>
        /// <param name="wkid">EPSG ID</param>
        /// <returns></returns>
        public static ISpatialReference CreateProjectedCoordinate(int wkid)
        {
            ISpatialReferenceFactory2 pSpatialReferenceFactory = new SpatialReferenceEnvironment() as ISpatialReferenceFactory2;
            ISpatialReference pSpatialReference = pSpatialReferenceFactory.CreateProjectedCoordinateSystem(wkid);
            return pSpatialReference;
        }

        /// <summary>
        /// 获取空投影
        /// </summary>
        /// <returns></returns>
        public static ISpatialReference CreateUnKnownSpatialReference()
        {
            ISpatialReference pSpatialReference = new UnknownCoordinateSystem() as ISpatialReference;
            pSpatialReference.SetDomain(0, 99999999, 0, 99999999);//设置空间范围
            return pSpatialReference;
        }
        #endregion

        #region 获取空间参考
        /// <summary>
        /// 获取要素集空间参考
        /// </summary>
        /// <param name="pFeatureDataset">要素集</param>
        /// <returns></returns>
        public static ISpatialReference GetSpatialReference(IFeatureDataset pFeatureDataset)
        {
            IGeoDataset pGeoDataset = pFeatureDataset as IGeoDataset;
            ISpatialReference pSpatialReference = pGeoDataset.SpatialReference;
            return pSpatialReference;
        }

        /// <summary>
        /// 获取要素层空间参考
        /// </summary>
        /// <param name="pFeatureLayer">要素层</param>
        /// <returns></returns>
        public static ISpatialReference GetSpatialReferenc(IFeatureLayer pFeatureLayer)
        {
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            IGeoDataset pGeoDataset = pFeatureClass as IGeoDataset;
            ISpatialReference pSpatialReference = pGeoDataset.SpatialReference;
            return pSpatialReference;
        }

        /// <summary>
        /// 获取要素类空间参考
        /// </summary>
        /// <param name="pFeatureClass">要素类</param>
        /// <returns></returns>
        public static ISpatialReference GetSpatialReference(IFeatureClass pFeatureClass)
        {
            IGeoDataset pGeoDataset = pFeatureClass as IGeoDataset;
            ISpatialReference pSpatialReference = pGeoDataset.SpatialReference;
            return pSpatialReference;
        }
        #endregion
    }
}

