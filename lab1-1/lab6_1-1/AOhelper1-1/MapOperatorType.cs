/*************************************************
* 文件: MapOperatorType.cs
* 说明：地图操作枚举类型
* 作者：中南大学李光强（QQ：41733233）
* 时间：2022/11/30/
* 声明：请尊重作者版权，使用此文件时，请保留此信息
***********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_1_1.AOhelper1_1
{
    /// <summary>
    /// 地图操作枚举类型
    /// </summary>
    public enum MapOperatorType
    {
        /// <summary>
        /// 默认（无操作）
        /// </summary>
        Default,
        /// <summary>
        /// 画点
        /// </summary>
        DrawPoint,
        /// <summary>
        /// 画线
        /// </summary>
        DrawPolyline,
        /// <summary>
        /// 画多边形
        /// </summary>
        DrawPolygon,

        /// <summary>
        /// 画矩形
        /// </summary>
        DrawRectangle,

        /// <summary>
        /// 从地图上创建点要素
        /// </summary>
        CreatePoint,
        /// <summary>
        /// 从地图上创建线要素
        /// </summary>
        CreatePolyline,
        /// <summary>
        /// 从地图上创建面要素
        /// </summary>
        CreatePolygon,

        /// <summary>
        /// 标识/显示要素信息
        /// </summary>
        IdentifyFeature,

        /// <summary>
        /// 点选要素
        /// </summary>
        SelectFeatureByLocation,
        /// <summary>
        /// 线选要素
        /// </summary>
        SelectFeatureByPolyline,
        /// <summary>
        /// 多边形选择要素
        /// </summary>
        SelectFeatureByPolygon,
        /// <summary>
        /// 框选要素
        /// </summary>
        SelectFeatureByRectangle,

        /// <summary>
        /// 点选编辑要素
        /// </summary>
        EditFeatureByLocation,
        /// <summary>
        /// 框选编辑要素
        /// </summary>
        EditFeatureByRectangle,

        /// <summary>
        /// 点选删除要素
        /// </summary>
        DeleteFeatureByLocation,
        /// <summary>
        /// 框选删除要素
        /// </summary>
        DeleteFeatureByRectangle,
        /// <summary>
        /// 多边形选择删除要素
        /// </summary>
        DeleteFeatureByPolygon,
        /// <summary>
        /// 点高程计算
        /// </summary>
        AnalystPoint,
        /// <summary>
        /// 高亮显示
        /// </summary>
        SelectHighlight,
        /// <summary>
        /// 框选浏览
        /// </summary>
        ZoomRectangle,
        /// <summary>
        /// 拖动浏览
        /// </summary>
        Pan
    }
}
