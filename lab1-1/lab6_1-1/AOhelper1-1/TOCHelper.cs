/*************************************************
* 文件: TOCHelper.cs
* 说明：TOC助手类
* 作者：中南大学李光强（QQ：41733233）
* 时间：2022/11/17/
* 声明：请尊重作者版权，使用此文件时，请保留此信息
***********************************************/

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_1_1.AOhelper1_1
{
    /// <summary>
    /// TOC助手类
    /// </summary>
    public class TOCHelper
    {
        TOCControl toc;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toc">toc控件</param>
        public TOCHelper(TOCControl toc)
        {
            this.toc = toc;
        }

        /// <summary>
        /// 从TOC中获取当前选中的图层
        /// </summary>
        /// <returns></returns>
        public ILayer GetSelectedLayer()
        {
            try
            {
                ITOCControl2 toc2 = this.toc as ITOCControl2;

                ILayer layer = null;
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemLayer;
                IBasicMap map = null;
                Object obj1 = null, obj2 = null;
                toc2.GetSelectedItem(ref item, ref map, ref layer, ref obj1, ref obj2);
                return layer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
