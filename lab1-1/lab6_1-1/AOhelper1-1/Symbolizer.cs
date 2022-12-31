/*************************************************
* 文件: Symbolizer.cs
* 说明：符号管理类
* 作者：中南大学李光强（QQ：41733233）
* 时间：2022/11/30/
* 声明：请尊重作者版权，使用此文件时，请保留此信息
***********************************************/
using ESRI.ArcGIS.Display;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_1_1.AOhelper1_1
{
    /// <summary>
    /// 符号管理类
    /// </summary>
    public class Symbolizer
    {
        /// <summary>
        /// 创建点符号
        /// </summary>
        /// <param name="markerStyle">符号类型</param>
        /// <param name="color">颜色</param>
        /// <param name="size">大小</param>
        /// <returns></returns>
        public static ISymbol CreatePointSymbol(esriSimpleMarkerStyle markerStyle, IColor color, double size)
        {
            ISimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbol();
            markerSymbol.Style = markerStyle;
            markerSymbol.Color = color;
            markerSymbol.Size = size;
            return markerSymbol as ISymbol;
        }

        /// <summary>
        /// 创建线符号
        /// </summary>
        /// <param name="lineStyle">线符号类型</param>
        /// <param name="color">颜色</param>
        /// <param name="width">线宽</param>
        /// <returns></returns>
        public static ISymbol CreateLineSymbol(esriSimpleLineStyle lineStyle, IColor color, double width)
        {
            ISimpleLineSymbol lineSymbol = new SimpleLineSymbol();
            lineSymbol.Style = lineStyle;
            lineSymbol.Color = color;
            lineSymbol.Width = width;

            return lineSymbol as ISymbol;
        }

        /// <summary>
        /// 创建面符号
        /// </summary>
        /// <param name="fillStyle">符号类型</param>
        /// <param name="color">颜色</param>
        /// <param name="outLineSymbol">轮廓线符号，默认无</param>
        /// <returns></returns>
        /// <remarks>如果设置填充透明色，可以IRGBColor定义颜色变量color，设置color.NullColor = true; color.Transparency = 0;</remarks>
        public static ISymbol CreatePolygonSymbol(esriSimpleFillStyle fillStyle, IColor color, ISimpleLineSymbol outLineSymbol = null)
        {
            ISimpleFillSymbol fillSymbol = new SimpleFillSymbol();
            fillSymbol.Style = fillStyle;
            fillSymbol.Color = color;
            if (outLineSymbol != null)
                fillSymbol.Outline = outLineSymbol;

            return fillSymbol as ISymbol;
        }

        /// <summary>
        /// C#颜色转ESRI颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static IColor ToIColor(System.Drawing.Color color)
        {
            IColor esriColor = new RgbColorClass();
            string rgbString = color.B.ToString("X2") + color.G.ToString("X2") + color.R.ToString("X2");
            esriColor.RGB = Convert.ToInt32(rgbString, 16);
            return esriColor;
        }

        /// <summary>
        /// ESRI颜色转C#颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(IColor color)
        {
            IRgbColor esriColor = color as IRgbColor;
            Color clr = Color.FromArgb(esriColor.Red, esriColor.RGB, esriColor.Blue);
            return clr;
        }
    }
}
