/*************************************************
* 文件: FeatureHelper.cs
* 说明：要素助手类
* 作者：中南大学李光强（QQ：41733233）
* 时间：2022/11/17/
* 声明：请尊重作者版权，使用此文件时，请保留此信息
***********************************************/


using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace lab4_1_1.AOhelper1_1
{
    /// <summary>
    /// 要素助手类
    /// </summary>
    public class FeatureHelper
    {
        /// <summary>
        /// 将要素导出为Json
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static string ToJson(IFeature feature)
        {
            string json = "{";
            IFields fields = feature.Fields;
            IField field;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                field = fields.Field[i];
                json += "\"" + field.Name + "\":";
                if (field.Type == esriFieldType.esriFieldTypeOID
                    || field.Type == esriFieldType.esriFieldTypeSmallInteger
                    || field.Type == esriFieldType.esriFieldTypeInteger
                    || field.Type == esriFieldType.esriFieldTypeSingle
                    || field.Type == esriFieldType.esriFieldTypeDouble)
                    json += String.Format("{0}", feature.Value[i]);
                else if (field.Type == esriFieldType.esriFieldTypeDate)
                    json += "\"" + ((DateTime)feature.Value[i]).ToString("yyyy-MM-dd") + "\"";
                else if (field.Type == esriFieldType.esriFieldTypeString
                    || field.Type == esriFieldType.esriFieldTypeXML
                    || field.Type == esriFieldType.esriFieldTypeGUID
                    || field.Type == esriFieldType.esriFieldTypeGlobalID
                    || field.Type == esriFieldType.esriFieldTypeBlob)
                    json += "\"" + feature.Value[i] + "\"";
                else
                    json += "\"\"";
            }

            return json;
        }

        /// <summary>
        /// 根据要素的字段及类型创建数据表DataTable
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable(IFeature feature)
        {
            try
            {
                DataTable dt = new DataTable();
                IFields fields = feature.Fields;
                int i = 0;
                for (i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.Field[i];
                    if (field.Type == esriFieldType.esriFieldTypeOID)
                        dt.Columns.Add(field.Name, typeof(long));
                    else if (field.Type == esriFieldType.esriFieldTypeSmallInteger
                        || field.Type == esriFieldType.esriFieldTypeInteger)
                        dt.Columns.Add(field.Name, typeof(int));
                    else if (field.Type == esriFieldType.esriFieldTypeSingle)
                        dt.Columns.Add(field.Name, typeof(Single));
                    else if (field.Type == esriFieldType.esriFieldTypeDouble)
                        dt.Columns.Add(field.Name, typeof(Double));
                    else if (field.Type == esriFieldType.esriFieldTypeDate)
                        dt.Columns.Add(field.Name, typeof(DateTime));
                    else if (field.Type == esriFieldType.esriFieldTypeString
                        || field.Type == esriFieldType.esriFieldTypeXML
                        || field.Type == esriFieldType.esriFieldTypeGeometry)
                        dt.Columns.Add(field.Name, typeof(String));
                    else
                        dt.Columns.Add(field.Name, typeof(String));
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将要素转换为DataRow
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static DataRow ToDataRow(IFeature feature, DataRow row)
        {
            try
            {
                for (int i = 0; i < feature.Fields.FieldCount; i++)
                {
                    if (feature.Fields.Field[i].Type == esriFieldType.esriFieldTypeGeometry)
                        row[i] = "Shape";
                    else
                        row[i] = feature.Value[i];
                }

                return row;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
