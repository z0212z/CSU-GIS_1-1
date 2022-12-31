/*************************************************
 * 文件: FieldHelper.cs
 * 说明：要素字段助手类
 * 作者：中南大学李光强（QQ：41733233）
 * 时间：2022/11/16/
 * 声明：请尊重作者版权，使用此文件时，请保留此信息
 ***********************************************/


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
    /// ESRI字段助手 
    /// </summary>
    public class FieldHelper
    {
        /// <summary>
        /// 生成默认字段，包括FID和Shape字段
        /// </summary>
        /// <param name="geomType">几何类型</param>
        /// <param name="sr">空间参考系</param>
        /// <remarks></remarks>
        /// <returns></returns>
        public static IFields CreateDefaultFields(
            out IFeatureClassDescription description,
            esriGeometryType geomType,
            ISpatialReference sr=null)
        {
            description = new FeatureClassDescription() as IFeatureClassDescription;
            IObjectClassDescription pObjectDescription = (IObjectClassDescription)description;

            IFields fields = pObjectDescription.RequiredFields;
            int shapeFieldIndex = fields.FindField(description.ShapeFieldName);
            IField field = fields.get_Field(shapeFieldIndex);
            IGeometryDef geometryDef = field.GeometryDef;
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;

            geometryDefEdit.GeometryType_2 = geomType;
            if (sr == null)
                geometryDefEdit.SpatialReference_2 = new UnknownCoordinateSystem() as ISpatialReference2;
            else
                geometryDefEdit.SpatialReference_2 = sr;
            return fields;
        }

        /// <summary>
        /// 创建字段列表
        /// </summary>
        /// <param name="description">要素类描述</param>
        /// <param name="fields">自定义字段集</param>
        /// <param name="geomType">几何类型</param>
        /// <param name="sr">空间参考</param>
        /// <returns></returns>
        public static IFields CreateFields(
             out IFeatureClassDescription description
            , IFields fields
            , esriGeometryType geomType
            , ISpatialReference sr = null)
        {
            try
            {
                IFieldsEdit flds = CreateDefaultFields(out description, geomType, sr) as IFieldsEdit;

                if (fields == null)
                    return flds;

                for (int i = 0; i < fields.FieldCount; i++)
                    flds.AddField(fields.Field[i]);

                return flds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将字段类型转换为汉字
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String TypeToHZ(esriFieldType type)
        {
            if (type == esriFieldType.esriFieldTypeInteger || type == esriFieldType.esriFieldTypeSmallInteger)
                return "整数";
            else if (type == esriFieldType.esriFieldTypeSingle || type == esriFieldType.esriFieldTypeDouble)
                return "数字";
            else if (type == esriFieldType.esriFieldTypeDate)
                return "日期";
            else if (type == esriFieldType.esriFieldTypeString)
                return "文本";
            else if (type == esriFieldType.esriFieldTypeOID)
                return "整数";
            else if (type == esriFieldType.esriFieldTypeGeometry)
                return "图形";
            else if (type == esriFieldType.esriFieldTypeXML)
                return "XML";
            else
                return type.ToString();
        }
    }
}
