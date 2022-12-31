/*************************************************
* 文件: FeatureClass.cs
* 说明：要素类管理类
* 作者：中南大学李光强（QQ：41733233）
* 时间：2022/11/16/
* 声明：请尊重作者版权，使用此文件时，请保留此信息
***********************************************/

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace lab4_1_1.AOhelper1_1
{
    /// <summary>
    /// 要素类管理类
    /// </summary>
    public class FeatureClass
    {
        #region 创建要素类
        /// <summary>
        /// 创建仅带有OID和SHAPE字段的SHP文件
        /// </summary>
        /// <param name="folder">文件夹</param>
        /// <param name="shpFileName">shp文件名称（不含路径）</param>
        /// <param name="geomType">几何类型</param>
        /// <param name="sr">坐标系</param>
        /// <returns></returns>
        public static IFeatureClass CreateShpFile(
            string folder,
            string shpFileName,
            esriGeometryType geomType,
            ISpatialReference2 sr = null,
            bool isOverWrite = false)
        {
            try
            {
                //判断shpFileName中是否包含.shp后缀名称
                string shpFileFullPath;
                if (shpFileName.ToLower().EndsWith(".shp"))
                    shpFileFullPath = folder + "\\" + shpFileName;
                else
                    shpFileFullPath = folder + "\\" + shpFileName + ".shp";

                //打开工作区
                IWorkspace ws = WorkSpace.OpenshpfileWorkspace(folder);

                //如果文件已存在               
                if (File.Exists(shpFileFullPath))
                {
                    if (isOverWrite)
                    {
                        IFeatureWorkspace fws = ws as IFeatureWorkspace;
                        IFeatureClass pFCChecker = fws.OpenFeatureClass(shpFileName);
                        if (pFCChecker != null)
                        {
                            IDataset pds = pFCChecker as IDataset;
                            pds.Delete();
                        }
                    }
                    else
                    {
                        throw new Exception("Shape文件已存在");
                    }
                }
                IFeatureClassDescription desc = null;
                IFields fields = FieldHelper.CreateDefaultFields(out desc, geomType, sr);
                return createShpFile(ws, shpFileName, fields, desc.ShapeFieldName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建SHP文件
        /// </summary>
        /// <param name="folder">文件夹</param>
        /// <param name="shpFileName">shp文件名称</param>
        /// <param name="fields">字段列表</param>
        /// <param name="geomFieldName">几何字段名称</param>
        /// <returns></returns>
        public static IFeatureClass CreateShpFile(
            string folder,
            string shpFileName,
            IFields fields,
            string geomFieldName = "Shape",
            bool isOverWrite = false)
        {
            try
            {
                string shpFileFullPath;
                if (shpFileName.ToLower().EndsWith(".shp"))
                    shpFileFullPath = folder + "\\" + shpFileName;
                else
                    shpFileFullPath = folder + "\\" + shpFileName + ".shp";

                IWorkspaceFactory wsf = new ShapefileWorkspaceFactory();
                IWorkspace ws = wsf.OpenFromFile(folder, 0);
                IFeatureWorkspace fws = ws as IFeatureWorkspace;

                //如果文件已存在               
                if (File.Exists(shpFileFullPath))
                {
                    if (isOverWrite)
                    {
                        IFeatureClass pFCChecker = fws.OpenFeatureClass(shpFileName);
                        if (pFCChecker != null)
                        {
                            IDataset pds = pFCChecker as IDataset;
                            pds.Delete();
                        }
                    }
                    else
                    {
                        throw new Exception("Shape文件已存在");
                    }
                }

                return createShpFile(ws, shpFileName, fields, geomFieldName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 创建SHP文件 
        /// </summary>
        /// <param name="shpFullFilePath"></param>
        /// <param name="pGeometryType"></param>
        /// <param name="sr"></param>
        /// <param name="isOverWrite"></param>
        /// <remarks>此函数为老版本，暂时保留，以备后用</remarks>
        public static IFeatureClass CreateShpFile(
            string shpFullFilePath
            , IFields fields
            , string geomFieldName = "Shape"
            , bool isOverWrite = false)
        {
            try
            {
                string folder = System.IO.Path.GetDirectoryName(shpFullFilePath);
                string fileName = System.IO.Path.GetFileName(shpFullFilePath);

                return CreateShpFile(folder, fileName, fields, geomFieldName, isOverWrite);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建SHP文件
        /// </summary>
        /// <param name="ws">工作区</param>
        /// <param name="shpFileName">shp文件完整名称(含路径)</param>
        /// <param name="fields">自定义字段集</param>
        /// <param name="geomFieldName">几何字段名称，默认Shape</param>
        /// <returns></returns>
        private static IFeatureClass createShpFile(
            IWorkspace ws,
            string shpFileName,
            IFields fields,
            string geomFieldName = "Shape")
        {
            try
            {
                if (fields == null)
                {
                    throw new Exception("字段列表不能为空");
                }

                IFieldChecker fieldChecker = new FieldChecker();
                IEnumFieldError enumFieldError = null;
                IFields validatedFields = null; //将传入字段 转成 validatedFields
                fieldChecker.ValidateWorkspace = ws;
                fieldChecker.Validate(fields, out enumFieldError, out validatedFields);
                IFeatureWorkspace fws = (IFeatureWorkspace)ws;

                return fws.CreateFeatureClass(shpFileName,
                    validatedFields,
                    null,//pObjectDescription.InstanceCLSID, 
                    null,//pObjectDescription.ClassExtensionCLSID, 
                    esriFeatureType.esriFTSimple,
                    geomFieldName,
                    "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 生成mdb数据库要素类
        /// </summary>
        /// <param name="mdbPath">mdb文件路径</param>
        /// <param name="featureClass">要素类名</param>
        /// <param name="fields">要素类字段</param>
        /// <param name="geomType">几何类型</param>
        /// <param name="sr">空间参考</param>
        /// <returns></returns>
        public static IFeatureClass CreateAccessFeatureClass(
            string mdbPath,
            string featureClass,
            IFields fields,
            esriGeometryType geomType,
            ISpatialReference2 sr = null)
        {
            try
            {
                IWorkspace ws = WorkSpace.OpenAccessWorkspace(mdbPath);
                IFeatureWorkspace fws = ws as IFeatureWorkspace;

                IGeometryDefEdit pGeomDef = new GeometryDef() as IGeometryDefEdit;
                pGeomDef.GeometryType_2 = geomType;

                if (sr == null)
                    pGeomDef.SpatialReference_2 = new UnknownCoordinateSystem() as ISpatialReference2;
                else
                    pGeomDef.SpatialReference_2 = sr;

                return fws.CreateFeatureClass(
                    featureClass,
                    fields,
                    null,
                    null,
                    esriFeatureType.esriFTSimple,
                    " Shape",
                    ""
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除要素类/SHP文件
        /// <summary>
        /// 删除SHP文件
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="shpFile"></param>
        public static void DeleteSHP(string folder, string shpFile)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(folder);
                shpFile = shpFile.ToLower().Replace(".shp", "");
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Name.ToLower().IndexOf(shpFile) >= 0)
                        fi.Delete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除要素类
        /// </summary>
        /// <param name="fc">要素类名称</param>
        public static void DeleteFeatureClass(IFeatureClass fc)
        {
            try
            {
                if (fc != null)
                {
                    IDataset pds = fc as IDataset;
                    pds.Delete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 字段管理
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="fields"></param>
        public static void AddFields(IFeatureClass fc, IFields fields)
        {
            try
            {
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    fc.AddField(fields.Field[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取要素数据源
        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="fc">要素类</param>
        /// <param name="featureName">要素类名称</param>
        /// <param name="dataSource">数据源</param>
        public static void GetDataSource(IFeatureClass fc,
            out string featureName,
            out string dataSource)
        {
            try
            {
                IDataset dataSet = (IDataset)fc;
                featureName = dataSet.Name;
                object names, values;

                IPropertySet propertySet = dataSet.Workspace.ConnectionProperties;
                propertySet.GetAllProperties(out names, out values);
                dataSource = ((object[])values)[0].ToString();
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
        /// <param name="dataSourceType">数据源类型</param>
        /// <param name="featureName">要素类名称</param>
        /// <param name="dataSource">数据源</param>
        public static string GetDataSource(IFeatureClass fc)
        {
            try
            {
                string featureName = null, dataSource = null;
                GetDataSource(fc, out featureName, out dataSource);
                return dataSource + "\\" + featureName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取要素类中所有数据

        /// <summary>
        /// 获取要素类中所有要素列表
        /// </summary>
        /// <param name="featureClass"></param>
        /// <returns></returns>
        public static IList<IFeature> ListFeatures(IFeatureClass featureClass)
        {
            List<IFeature> list = new List<IFeature>();

            IFeatureCursor featureCursor = featureClass.Search(null, false);
            IFeature feature = featureCursor.NextFeature();

            while (feature != null)
            {
                list.Add(feature);
                feature = featureCursor.NextFeature();
            }
            return list;
        }

        /// <summary>
        /// 将要素类转换为数据表DataTable
        /// </summary>
        /// <param name="featureClass"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(IFeatureClass featureClass)
        {
            try
            {
                DataTable dt = new DataTable();

                IFeatureCursor featureCursor = featureClass.Search(null, false);
                IFeature feature = featureCursor.NextFeature();

                if (feature != null)
                {
                    for (int i = 0; i < feature.Fields.FieldCount; i++)
                    {
                        dt.Columns.Add(feature.Fields.Field[i].Name);
                    }
                    while (feature != null)
                    {
                        DataRow dataRow = dt.NewRow();
                        for (int j = 0; j < featureCursor.Fields.FieldCount; j++)
                        {
                            if (featureCursor.Fields.Field[j].Type == esriFieldType.esriFieldTypeGeometry)
                                dataRow[j] = "Shape";
                            else
                                dataRow[j] = feature.Value[j];
                        }
                        dt.Rows.Add(dataRow);
                        feature = featureCursor.NextFeature();
                    }
                    return dt;
                }
                else
                    throw new Exception("要素类中没有要素");
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ToDataTable(IList<IFeature> features)
        {
            if (features == null)
                throw new Exception("要素列表为空");
            if (features.Count == 0)
                throw new Exception("要素列表中没有要素");

            try
            {
                IFeature feature = features[0];
                DataTable dt = FeatureHelper.CreateDataTable(feature);

                foreach (IFeature f in features)
                {
                    DataRow row = dt.NewRow();
                    row = FeatureHelper.ToDataRow(f, row);
                    dt.Rows.Add(row);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 要素管理

        /// <summary>
        /// 添加要素
        /// </summary>
        /// <param name="fc">要素类</param>
        /// <param name="data">要素属性数据Dictionary，包括几何图形数据</param>
        public static void AddFeature(IFeatureClass fc, Dictionary<string, object> data)
        {
            try
            {
                IFeature feature = fc.CreateFeature();
                foreach (KeyValuePair<string, object> kv in data)
                {
                    if (kv.Key.ToUpper() == "FID" || kv.Key.ToUpper() == "OID")
                        continue;

                    feature.Value[feature.Fields.FindField(kv.Key)] = kv.Value;
                }
                feature.Store();
            }
            catch (Exception ex)
            {
                MessageBox.Show("要素创建错误，原因：" + ex.Message
                    , "异常"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除要素
        /// </summary>
        /// <param name="fc">要素类</param>
        /// <param name="filter">筛选条件,为空时删除全部数据</param>
        public static void DeleteFeature(IFeatureClass fc, IQueryFilter filter = null)
        {
            try
            {
                ITable table = (ITable)fc;
                //如果filter=null，则删除全部要素
                table.DeleteSearchedRows(filter);
            }
            catch (Exception ex)
            {
                MessageBox.Show("要素删除错误，原因：" + ex.Message
                    , "异常"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除要素
        /// </summary>
        /// <param name="fc">要素类</param>
        /// <param name="whereClause">筛选条件从句,为空时删除全部数据</param>
        public static void DeleteFeature(IFeatureClass fc, string whereClause = null)
        {
            try
            {
                IQueryFilter filter = null;
                if (!string.IsNullOrEmpty(whereClause))
                {
                    filter = new QueryFilterClass();
                    filter.WhereClause = whereClause;
                }

                ITable table = (ITable)fc;
                //如果filter=null，则删除全部要素
                table.DeleteSearchedRows(filter);
            }
            catch (Exception ex)
            {
                MessageBox.Show("要素删除错误，原因：" + ex.Message
                    , "异常"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 从要素类生成图层
        /// <summary>
        /// 从ShapeFile要素类生成图层
        /// </summary>
        /// <param name="shpFile">shp文件的绝对路径全名(含文件夹和文件名)</param>
        /// <returns></returns>
        public static ILayer ShpToLayer(string shpFile)
        {
            string folder;
            FileInfo fi = new FileInfo(shpFile);
            folder = fi.DirectoryName;
            IWorkspace ws = WorkSpace.OpenshpfileWorkspace(folder);
            return toLayer(ws, shpFile);
        }
        /// <summary>
        /// 从ShapeFile要素类生成图层
        /// </summary>
        /// <param name="folder">shp文件所在的文件夹路径</param>
        /// <param name="shpFileName">shp文件名(不含路径)</param>
        /// <returns></returns>
        public static ILayer ShpToLayer(string folder, string shpFileName)
        {
            IWorkspace ws = WorkSpace.OpenshpfileWorkspace(folder);
            return toLayer(ws, shpFileName);
        }

        /// <summary>
        /// 从MDB空间数据库的要素类生成图层
        /// </summary>
        /// <param name="mdbPath">mdb路径名称</param>
        /// <param name="featureClass">要素类名</param>
        /// <returns></returns>
        public static ILayer MdbToLayer(string mdbPath, string featureClass)
        {
            IWorkspace ws = WorkSpace.OpenAccessWorkspace(mdbPath);
            return toLayer(ws, featureClass);
        }

        /// <summary>
        /// 从SDE空间数据库的要素类生成图层
        /// </summary>
        /// <param name="pPropertyset">连接属性</param>
        /// <param name="featureClass">要素类</param>
        /// <returns></returns>
        public static ILayer SdeToLayer(
            IPropertySet pPropertyset,
            string featureClass)
        {
            IWorkspaceFactory pWorkspaceFactory = new SdeWorkspaceFactory();
            return toLayer(pWorkspaceFactory, pPropertyset, featureClass);
        }
        #endregion

        #region 获取空间参考系/获取几何类型
        /// <summary>
        /// 获取空间参考系
        /// </summary>
        /// <param name="featureClass"></param>
        /// <returns></returns>
        public static ISpatialReference GetSpatialReference(IFeatureClass featureClass)
        {
            IDataset dataset = (IDataset)featureClass;
            // If the dataset supports IGeoDataset
            if (dataset is IGeoDataset)
            {
                IGeoDataset geoDataset = (IGeoDataset)dataset;
                return geoDataset.SpatialReference;
            }
            else
                return new UnknownCoordinateSystem() as ISpatialReference;
        }

        /// <summary>
        /// 获取几何类型
        /// </summary>
        /// <param name="featureClass"></param>
        /// <returns></returns>
        public static esriGeometryType GetGeometryType(IFeatureClass featureClass)
        {
            return featureClass.ShapeType;
        }
        #endregion

        #region 私有函数
        /// <summary>
        /// 生成图层
        /// </summary>
        /// <param name="pWorkspaceFactory"></param>
        /// <param name="pPropertyset"></param>
        /// <param name="featureClass"></param>
        /// <returns></returns>
        private static ILayer toLayer(
            IWorkspaceFactory pWorkspaceFactory,
            IPropertySet pPropertyset,
            string featureClass)
        {
            IWorkspace ws = pWorkspaceFactory.Open(pPropertyset, 0);
            IFeatureClass fc = getFeatureClass(ws, featureClass);
            return ToFeatureLayer(fc);
        }

        /// <summary>
        /// 生成图层
        /// </summary>
        /// <param name="pWorkspace"></param>
        /// <param name="featureClass"></param>
        /// <returns></returns>
        private static ILayer toLayer(
            IWorkspace pWorkspace,
            string featureClass)
        {
            IFeatureClass fc = getFeatureClass(pWorkspace, featureClass);
            return ToFeatureLayer(fc);
        }

        /// <summary>
        /// 获取指定要素名称的要素类
        /// </summary>
        /// <param name="ws">工作区</param>
        /// <param name="featureClass">要素类名称</param>
        /// <returns></returns>
        private static IFeatureClass getFeatureClass(IWorkspace ws, string featureClass)
        {
            IFeatureWorkspace pFeatWorkspace = ws as IFeatureWorkspace;
            IFeatureClass pFeatClass;
            pFeatClass = pFeatWorkspace.OpenFeatureClass(featureClass);
            return pFeatClass;
        }

        /// <summary>
        /// 从要素类创建图层
        /// </summary>
        /// <param name="fc">要素类</param>
        /// <returns></returns>
        public static ILayer ToFeatureLayer(IFeatureClass fc)
        {
            IFeatureLayer pFeatLyr;
            pFeatLyr = new FeatureLayer();
            pFeatLyr.FeatureClass = fc;
            pFeatLyr.Name = fc.AliasName;
            return pFeatLyr;
        }
        #endregion
    }
}
