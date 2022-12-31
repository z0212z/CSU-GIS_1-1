/*********************************************************
* 文件：Util.cs
* 说明：常用工具类
* 作者：李光强
* 时间：2022/11/28
* ******************************************************/
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
    /// <summary>
    /// 工具类
    /// </summary>
    public class Util
    {
        /// <summary>
        /// 将C#变量类型转为字段类型
        /// </summary>
        /// <param name="type">C#类型名称</param>
        /// <returns></returns>
        public static esriFieldType ToFieldType(string type)
        {
            if (type.ToLower() == "short" || type.ToLower() == "int16")
                return esriFieldType.esriFieldTypeSmallInteger;
            else if (type.ToLower() == "int" || type.ToLower() == "int32" || type == "整数")
                return esriFieldType.esriFieldTypeInteger;
            else if (type.ToLower() == "float" || type.ToLower() == "single" || type == "数字")
                return esriFieldType.esriFieldTypeSingle;
            else if (type.ToLower() == "double")
                return esriFieldType.esriFieldTypeDouble;
            else if (type.ToLower() == "datetime" || type == "日期")
                return esriFieldType.esriFieldTypeDate;
            else if (type.ToLower() == "string" || type == "文本")
                return esriFieldType.esriFieldTypeString;
            else
                return esriFieldType.esriFieldTypeString;
        }

        /// <summary>
        /// 根据文本内容转换几何类型
        /// </summary>
        /// <param name="type">几何类型文本</param>
        /// <returns></returns>
        public static esriGeometryType ToGeometryType(string type)
        {
            if (type.ToLower().IndexOf("point") >= 0
                || type.IndexOf("点") >= 0)
                return esriGeometryType.esriGeometryPoint;
            else if (type.ToLower().IndexOf("polyline") >= 0
                || type.IndexOf("线") >= 0)
                return esriGeometryType.esriGeometryPolyline;
            else if (type.ToLower().IndexOf("polygon") >= 0
                || type.IndexOf("面") >= 0
                || type.IndexOf("多边形") >= 0)
                return esriGeometryType.esriGeometryPolygon;
            else
                return esriGeometryType.esriGeometryPoint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPointCollection"></param>
        /// <returns></returns>
        public static IPolygon CreatePolygonByPoints(IPointCollection pPointCollection)
        {
            IGeometryBridge2 pGeometryBridge2 = new GeometryEnvironmentClass();
            IPointCollection4 pPolygon = new PolygonClass();
            WKSPoint[] pWKSPoint = new WKSPoint[pPointCollection.PointCount];
            for (int i = 0; i < pPointCollection.PointCount; i++)
            {
                pWKSPoint[i].X = pPointCollection.get_Point(i).X;
                pWKSPoint[i].Y = pPointCollection.get_Point(i).Y;
            }

            pGeometryBridge2.SetWKSPoints(pPolygon, ref pWKSPoint);
            IPolygon pPoly = pPolygon as IPolygon;
            pPoly.Close();
            return pPoly;
        }

        /// <summary>
        /// Envelope转多边形
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static IPolygon PolygonFromEnvlope(IEnvelope rect)
        {
            IPointCollection pc = new Polygon() as IPointCollection;
            IPoint point = new PointClass();
            point.PutCoords(rect.XMin, rect.YMin);
            pc.AddPoint(point);

            point = new PointClass();
            point.PutCoords(rect.XMin, rect.YMax);
            pc.AddPoint(point);

            point = new PointClass();
            point.PutCoords(rect.XMax, rect.YMax);
            pc.AddPoint(point);

            point = new PointClass();
            point.PutCoords(rect.XMax, rect.YMin);
            pc.AddPoint(point);

            point = new PointClass();
            point.PutCoords(rect.XMin, rect.YMin);
            pc.AddPoint(point);

            IPolygon polygon = pc as IPolygon;
            return polygon;
        }
    }
}

