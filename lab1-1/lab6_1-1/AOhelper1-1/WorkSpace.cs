/*************************************************
* 文件: WorkSpace.cs
* 说明：工作区管理类
* 作者：中南大学李光强（QQ：41733233）
* 时间：2022/11/16/
* 声明：请尊重作者版权，使用此文件时，请保留此信息
***********************************************/

using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_1_1.AOhelper1_1
{
    /// <summary>
    /// 工作区管理类
    /// </summary>
    public class WorkSpace
    {
        #region 打开工作区
        /// <summary>
        /// 打开shapefile工作区
        /// </summary>
        /// <param name="folder">工作区文件夹</param>
        /// <returns></returns>
        public static IWorkspace OpenshpfileWorkspace(string folder)
        {
            IWorkspaceFactory wsf = new ShapefileWorkspaceFactory();
            return wsf.OpenFromFile(folder, 0);
        }

        /// <summary>
        /// 打开个人空间数据库
        /// </summary>
        /// <param name="mdbPath">mdb数据库路径</param>
        /// <returns></returns>
        public static IWorkspace OpenAccessWorkspace(string mdbPath)
        {
            IWorkspaceFactory wsf = new AccessWorkspaceFactory();
            return wsf.OpenFromFile(mdbPath, 0);
        }

        /// <summary>
        /// 打开SDE空间数据库
        /// </summary>
        /// <param name="Server">服务器</param>
        /// <param name="Instance">实例</param>
        /// <param name="User">用户</param>
        /// <param name="Password">密码</param>
        /// <param name="Database">数据库</param>
        /// <param name="Version">版本</param>
        /// <returns></returns>
        public static IWorkspace OpenSDEWorkspace(
            string Server,
            string Instance,
            string User,
            string Password,
            string Database,
            string Version)
        {
            IWorkspace ws = null;
            IPropertySet pPropSet = new PropertySet();
            IWorkspaceFactory pSdeFact = new AccessWorkspaceFactory();
            pPropSet.SetProperty("SERVER", Server);
            pPropSet.SetProperty("INSTANCE", Instance);
            pPropSet.SetProperty("USER", User);
            pPropSet.SetProperty("PASSWORD", Password);
            pPropSet.SetProperty("DATABASE", Database);
            pPropSet.SetProperty("VERSION", Version);
            try
            {
                ws = pSdeFact.Open(pPropSet, 0);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return ws;
        }
        #endregion

        #region 获取要素类
        /// <summary>
        /// 获取指定目录下的shp文件的要素类
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="fc">要素类名称</param>
        /// <returns></returns>
        public static IFeatureClass GetShpFeatureClass(string folder,string fc)
        {
            try
            {
                IFeatureWorkspace fws = OpenshpfileWorkspace(folder) as IFeatureWorkspace;
                return getFeatureClass(fws, fc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录下的shp文件的要素类
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="fc">要素类名称</param>
        /// <returns></returns>
        public static IFeatureClass GetMdbFeatureClass(string mdbPath, string fc)
        {
            try
            {
                IFeatureWorkspace fws = OpenAccessWorkspace(mdbPath) as IFeatureWorkspace;
                return getFeatureClass(fws, fc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录下的shp文件的要素类列表
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="fc">要素类名称</param>
        /// <returns></returns>
        public static IList<IFeatureClass> ListShpFeatureClass(string folder)
        {
            try
            {
                IFeatureWorkspace fws = OpenshpfileWorkspace(folder) as IFeatureWorkspace;
                return ListFeatureClass(fws);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 私有函数

        /// <summary>
        /// 获取指定名称的要素类列表
        /// </summary>
        /// <param name="fws"></param>
        /// <param name="fc"></param>
        /// <returns></returns>
        private static IList<IFeatureClass> ListFeatureClass(IFeatureWorkspace fws)
        {
            List<IFeatureClass> list = new List<IFeatureClass>();
            IWorkspace ws = (IWorkspace)fws;
            IEnumDatasetName datasetName = ws.DatasetNames[esriDatasetType.esriDTFeatureClass];
            IDatasetName dn = datasetName.Next();
            while (dn != null)
            {
                list.Add(fws.OpenFeatureClass(dn.Name));
                dn = datasetName.Next();
            }
            return list;
        }

        /// <summary>
        /// 获取指定名称的要素类
        /// </summary>
        /// <param name="fws"></param>
        /// <param name="fc"></param>
        /// <returns>没有指定，返回空</returns>
        private static IFeatureClass getFeatureClass(IFeatureWorkspace fws,string fc)
        {
            IWorkspace ws = (IWorkspace)fws;
            IEnumDatasetName datasetName = ws.DatasetNames[esriDatasetType.esriDTFeatureClass];
            IDatasetName dn = datasetName.Next();
            while(dn!=null)
            {
                if (dn.Name == fc)
                    return fws.OpenFeatureClass(dn.Name);
                dn = datasetName.Next();
            }
            return null;
        }
        #endregion
    }
}
