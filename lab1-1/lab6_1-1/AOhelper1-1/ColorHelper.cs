using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace lab4_1_1.AOhelper1_1
{
    public class ColorHelper
    {
        //通过R,G,B值来构建一个RGBColor对象
        public static IRgbColor GetRGBColor(int r, int g, int b)
        {           
            IRgbColor pColor;
            pColor = new RgbColorClass();

            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
            
        }

        //通过R,G,B，A值来构建一个RGBColor对象
        public static IRgbColor GetRGBColor(int red, int green, int blue, byte alpha)
        {
            //创建RgbColor对象
            IRgbColor rGB = new RgbColorClass();
            //设置R、G、B及透明度
            rGB.Red = red;
            rGB.Green = green;
            rGB.Blue = blue;
            rGB.Transparency = alpha;
            return rGB;
        }

        //通过H,S,V值来构建一个HSVColor对象
        public static IHsvColor GetHsvColor(int h, int s, int v)
        {
            //创建HsvColor对象
            IHsvColor pHsvColor = new HsvColorClass();
            //设置属性值
            pHsvColor.Hue = h;
            pHsvColor.Saturation = s;
            pHsvColor.Value = v;
            return pHsvColor;
        }

        //生成算法色带     
        public static IColorRamp GetAlgorithmicColorRamp(int nCount,
            Color pColorFrom,
            Color pColorTo)
        { 
            IAlgorithmicColorRamp pColorRamp = new AlgorithmicColorRampClass();
            pColorRamp.FromColor = GetRGBColor(pColorFrom.R, pColorFrom.G, pColorFrom.B);
            pColorRamp.ToColor = GetRGBColor(pColorTo.R, pColorTo.G, pColorTo.B);
            pColorRamp.Size = nCount;

            bool ok = true;
            pColorRamp.CreateRamp(out ok);

            return pColorRamp;
        }

        //生成随机色带     
        public static IColorRamp GetRandomColorRamp(int nCount)
        {
            IRandomColorRamp pColorRamp = new RandomColorRampClass();
            
            pColorRamp.StartHue = 0;
            pColorRamp.EndHue = 360;

            pColorRamp.MinSaturation = 15;
            pColorRamp.MaxSaturation = 30;

            pColorRamp.MinValue = 99;
            pColorRamp.MaxValue = 100;   

            pColorRamp.Size = nCount * 2;

            bool ok = true;
            pColorRamp.CreateRamp(out ok);
            return pColorRamp;
        }  

    }
}
