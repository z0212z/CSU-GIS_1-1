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
    public class FeatureLayerRenderHelper
    {
        private volatile static FeatureLayerRenderHelper _pInstance = null;
        private static readonly object _pLockHelper = new object();
        private FeatureLayerRenderHelper() { }
        public static FeatureLayerRenderHelper GetInstance()
        {
            if (_pInstance == null)
            {
                lock(_pLockHelper)
                {
                    if (_pInstance == null)
                        _pInstance = new FeatureLayerRenderHelper();
                }
            }
            return _pInstance;
        }

        //对面图层进行简单符号化
        public void SetSimpleRenderer(IGeoFeatureLayer pGeoFeatureLayer,
            esriSimpleFillStyle euFillStyle,	//填充样式
            IColor pColor,		//填充颜色
            IColor pOutLineColor,		//外边线颜色
            string szRenderLabel,		//样式名称注释
            string szDescripition)		//描述信息
        {
            if (pGeoFeatureLayer == null)
                return;

            //创建简单填充符号对象
            ISimpleFillSymbol simpleFillSymbol = new SimpleFillSymbolClass();
            //设置其样式及填充色
            simpleFillSymbol.Style = euFillStyle;
            simpleFillSymbol.Color = pColor;
            //创建边线符号对象，该对象是一个SimpleLineSymbolClass
            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;	//边线样式
            simpleLineSymbol.Color = pOutLineColor;		//边线颜色
            ISymbol symbol = simpleLineSymbol as ISymbol;
            //symbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;	//设置边线绘制方式
            simpleFillSymbol.Outline = simpleLineSymbol;

            //创建单一符号渲染对象
            ISimpleRenderer simpleRender = new SimpleRendererClass();
            //设置simpleRender用于渲染图层的符号
            simpleRender.Symbol = simpleFillSymbol as ISymbol;
            simpleRender.Label = szRenderLabel;		//图例中的注释
            simpleRender.Description = szDescripition;	//图例中的描述信息

            //将simpleRender设为矢量图层的渲染器
            pGeoFeatureLayer.Renderer = simpleRender as IFeatureRenderer;
        }
        
        //分级符号化
        public void SetClassBreakRender(IGeoFeatureLayer pGeoFeatureLayer,
            int nClassCount, 				//分级数目
            string szClassField, 			//分级字段
            esriSimpleFillStyle euFillStyle)		//简单填充样式枚举
        {
            if (pGeoFeatureLayer == null)
                return;

            ILayer pLayer = pGeoFeatureLayer as ILayer;
            ITable pTable = pLayer as ITable;
            //创建表格直方图对象
            ITableHistogram pTableHistogram = new BasicTableHistogramClass();
            //接口转换，转换为IBasicHistogram接口
            IBasicHistogram pBasicHistogram = pTableHistogram as IBasicHistogram;

            //按照数值字段分级
            pTableHistogram.Table = pTable;			//设置表格直方图的数据源
            pTableHistogram.Field = szClassField;	    //设置表格直方图的分级字段
            //用输出变量统计每个值和各个值出现的次数
            object values;
            object frequencys;
            pBasicHistogram.GetHistogram(out values, out frequencys);

            //创建分位数分级对象
            IClassifyGEN pClassify = new QuantileClass();
            //用统计结果进行分级 ，级别数目为classCount
            pClassify.Classify(values, frequencys, ref nClassCount);
            //获得分级结果,是个双精度类型数组 
            double[] classes;
            classes = pClassify.ClassBreaks as double[];
            
            //定义不同等级渲染的色带用色
            IEnumColors pEnumColors = ColorHelper.GetAlgorithmicColorRamp(
                classes.Length, 
                Color.White, 
                Color.FromArgb(32, 200, 150)).Colors;

            //创建ClassBreaksRendererClass对象
            IClassBreaksRenderer pClassBreaksRenderer = new ClassBreaksRendererClass();
            pClassBreaksRenderer.Field = szClassField;
            pClassBreaksRenderer.BreakCount = nClassCount;//分级数目
            pClassBreaksRenderer.SortClassesAscending = true;

            //利用生成的算法色带，生成简单填充符号
            //并将其设置为pClassBreaksRenderer的地图符号
            IColor pColor;
            ISimpleFillSymbol pSimpleFillSymbol;
            for (int i = 0; i < classes.Length - 1; i++)
            {                
                pColor = pEnumColors.Next();
                pSimpleFillSymbol = new SimpleFillSymbolClass();
                pSimpleFillSymbol.Color = pColor;
                pSimpleFillSymbol.Style = euFillStyle;
                pClassBreaksRenderer.set_Symbol(i, pSimpleFillSymbol as ISymbol);
                pClassBreaksRenderer.set_Break(i, classes[i]);
            }

            //将pClassBreaksRenderer设为矢量图层的渲染器
            pGeoFeatureLayer.Renderer = pClassBreaksRenderer as IFeatureRenderer;
        }

        //获取渲染字段szRenderField所有单一值及其个数
        System.Collections.IEnumerator GetUniqueValues(IFeatureClass pFeatureClass, 
            string szRenderField,   //渲染字段
            ref int nCount)         //单一值的个数
        {
            //获取要素类的游标对象
            ICursor pCursor = pFeatureClass.Search(null, false) as ICursor;
            //创建数据统计对象
            IDataStatistics pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Field = szRenderField;  //待统计的字段
            pDataStatistics.Cursor = pCursor;       //待统计的要素集的游标
            //获取渲染字段szRenderField的单一值
            System.Collections.IEnumerator pEnumerator = pDataStatistics.UniqueValues;
            //获取渲染字段szRenderField的单一值个数
            nCount = pDataStatistics.UniqueValueCount;
            if (nCount == 0)
                return null;

            return pEnumerator;
        }

        public IUniqueValueRenderer CreateUniqueValueRenderer(
            IFeatureClass pFeatureClass,     //要渲染的图层的要素类
            string szRendererField         //渲染字段
            )
        {
            int nUniqueValuesCount = 0;
            System.Collections.IEnumerator pEnumerator = GetUniqueValues(pFeatureClass, 
                szRendererField, 
                ref nUniqueValuesCount);
            if (nUniqueValuesCount == 0)
                return null;

            //获取随机颜色集合
            IRandomColorRamp pRandomColorRamp = ColorHelper.GetRandomColorRamp(nUniqueValuesCount) as IRandomColorRamp;
            IEnumColors pEnumRamp = pRandomColorRamp.Colors;
            pEnumRamp.Reset();

            //创建单一值渲染器
            IUniqueValueRenderer pUniqueValueRender = new UniqueValueRendererClass();
            //只用一个字段进行单值着色
            pUniqueValueRender.FieldCount = 1;
            //用于区分着色的字段
            pUniqueValueRender.set_Field(0, szRendererField);


            //循环增加单一值及其对应的地图符号
            IColor pColor = null;
            ISymbol pSymbol = null;
            pEnumerator.Reset();
            while (pEnumerator.MoveNext())
            {
                object pCodeValue = pEnumerator.Current;    //得到一种单一值
                pColor = pEnumRamp.Next();  //从随机颜色集合中获取一种颜色
                switch (pFeatureClass.ShapeType)
                {
                    //如果是点要素，则创建简单点符号
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                        ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
                        pMarkerSymbol.Color = pColor;
                        pSymbol = pMarkerSymbol as ISymbol;
                        break;
                    //如果是线要素，创建简单线符号
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                        ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
                        pLineSymbol.Color = pColor;
                        pSymbol = pLineSymbol as ISymbol;
                        break;
                    //如果是面要素，创建简单面符号
                    case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                        ISimpleFillSymbol pillSymbol = new SimpleFillSymbolClass();
                        pillSymbol.Color = pColor;
                        pSymbol = pillSymbol as ISymbol;
                        break;
                    default:
                        break;
                }
                //将每次得到的渲染字段单一值和地图符号放入pUniqueValueRender中
                pUniqueValueRender.AddValue(pCodeValue.ToString(), szRendererField, pSymbol);
            }
            return pUniqueValueRender;
        }

        //比率符号渲染
        public void SetProportionSymbolRender(
            IGeoFeatureLayer pGeoFeatureLayer,  //要渲染的图层
            string szProportionField,             //参考字段
            esriSimpleFillStyle euFillStyle,  //填充样式
            IColor pFillColor,               //填充Color
            ICharacterMarkerSymbol pCharacterMarkerSymbol,   //特征点符号
            esriUnits euUnits,            //参考单位
            int nLegendSymbolCount)     //要分成的级数
        {
            IFeatureLayer pFeatureLayer;
            ITable pTable;
            ICursor pCursor;
            IDataStatistics pDataStatistics;//用一个字段生成统计数据
            IStatisticsResults pStatisticsResult;//报告统计数据

            if (pGeoFeatureLayer == null)
                return;

            pFeatureLayer = pGeoFeatureLayer as IFeatureLayer;
            pTable = pGeoFeatureLayer as ITable;     //接口转换，获取属性数据表
            pCursor = pTable.Search(null, true);      //获取属性数据集合游标

            //创建数据统计对象，并进行统计
            pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Cursor = pCursor;         //设置数据统计对象的游标
            pDataStatistics.Field = szProportionField;         //确定参考字段
            pStatisticsResult = pDataStatistics.Statistics;   //得到统计结果
            if (pStatisticsResult != null)
            {
                IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
                pFillSymbol.Color = pFillColor;

                //创建比例符号渲染器对象
                IProportionalSymbolRenderer pProportionalRenderer = new 
                    ProportionalSymbolRendererClass();
                
                //设置比例符号渲染对象的长度单位，具体如下：
                //（1）如果数据代表真实世界中的长度值，则ValueUnit可以是esriInches、esriPoints、
                //      esriFeet、esriYards、esriMiles、esriNauticalMiles、esriMillimeters、
                //      esriCentimeters、esriMeters、esriKilometers、esriDecimalDegrees等值
                //（2）如果数据值不代表长度，如人口数量、仓库储量等属性数据，
                //      那么ValueUnit的值设置为unknown
                pProportionalRenderer.ValueUnit = euUnits;

                pProportionalRenderer.Field = szProportionField;//包含数据值的属性字段名
                pProportionalRenderer.FlanneryCompensation = false;//是不是在TOC中显示legend
                pProportionalRenderer.MinDataValue = pStatisticsResult.Minimum;//数据最小值
                pProportionalRenderer.MaxDataValue = pStatisticsResult.Maximum;//数据最大值
                pProportionalRenderer.BackgroundSymbol = pFillSymbol;
                pProportionalRenderer.MinSymbol = pCharacterMarkerSymbol as ISymbol;
                pProportionalRenderer.LegendSymbolCount = nLegendSymbolCount;//要分成的级数
                pProportionalRenderer.CreateLegendSymbols();
                pGeoFeatureLayer.Renderer = pProportionalRenderer as IFeatureRenderer;
            }
        }


        //柱状图渲染
        public void CreateBarChartSymbol(
            IGeoFeatureLayer pGeoFeatureLayer,//要渲染的图层
            string[] szRenderField, //柱状图表示的字段
            IColor[] pFillSymbolColor,//这些字段分别需要渲染的颜色 
            double dbBarWidth, //每个柱子的宽度
            IColor pBgColor,//背景色
            double dbMarkerSize //整个柱状图的大小（单位：磅）
            )
        {
            if (pGeoFeatureLayer == null)
                return;
            pGeoFeatureLayer.ScaleSymbols = true;

            //创建统计图渲染对象，即ChartRendererClass对象
            IChartRenderer pChartRenderer = new ChartRendererClass();
            
            #region 循环增加柱状图表示的字段
            //接口转换至IRendererFields
            IRendererFields pRendererFields = pChartRenderer as IRendererFields;
            for (int i = 0; i < szRenderField.Length; i++)
            {
                //增加柱状图表示的字段
                pRendererFields.AddField(szRenderField[i], szRenderField[i]);
            }
            #endregion

            IFeatureLayer pFeatureLayer = pGeoFeatureLayer as IFeatureLayer;
            ITable pTable = pFeatureLayer as ITable;//转换为ITable接口
            ICursor pCursor = pTable.Search(null, true);//获取属性表游标
            IRowBuffer pRowBuffer = pCursor.NextRow();//获取第一行

            #region 获取要素最大值，确定柱状图的最大高度
            double dbMaxValue = 0.0;
            while (pRowBuffer != null)
            {
                for (int i = 0; i < szRenderField.Length; i++)
                {
                    int nIdx = pTable.FindField(szRenderField[i]);
                    double dbFieldValue = double.Parse(pRowBuffer.get_Value(nIdx).ToString());
                    if (dbFieldValue > dbMaxValue)
                    {
                        dbMaxValue = dbFieldValue;
                    }
                }
                pRowBuffer = pCursor.NextRow();
            }
            #endregion

            //创建柱状图符号
            IBarChartSymbol pBarChartSymbol = new BarChartSymbolClass();
            pBarChartSymbol.Width = dbBarWidth;//柱状图的宽度
            //转换至IChartSymbol接口
            IChartSymbol pChartSymbol = pBarChartSymbol as IChartSymbol;
            pChartSymbol.MaxValue = dbMaxValue;//设置柱状图符号的最大值
            //转换至IMarkerSymbol接口
            IMarkerSymbol pMarkerSymbol = pBarChartSymbol as IMarkerSymbol;
            pMarkerSymbol.Size = dbMarkerSize;//设置柱状图符号大小
            
            #region 利用ISymbolArray接口添加符号
            //接口转换至ISymbolArray
            ISymbolArray pSymbolArray = pBarChartSymbol as ISymbolArray;
            //创建填充符号数组，对其循环赋值
            IFillSymbol[] pFillSymbols = new IFillSymbol[szRenderField.Length];
            for (int i = 0; i < szRenderField.Length; i++)
            {
                //设置不同颜色的柱子
                pFillSymbols[i] = new SimpleFillSymbolClass();
                pFillSymbols[i].Color = pFillSymbolColor[i];
                //增加符号
                pSymbolArray.AddSymbol(pFillSymbols[i] as ISymbol);
            }
            #endregion

            //设置柱状图符号
            pChartRenderer.ChartSymbol = pBarChartSymbol as IChartSymbol;            
            //设置底图样式
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pBgColor;
            pChartRenderer.BaseSymbol = pFillSymbol as ISymbol;
            //假如那个位置放不下柱状图，是否用线段连接指示位置
            pChartRenderer.UseOverposter = false;
            //创建图例
            pChartRenderer.CreateLegend();

            pGeoFeatureLayer.Renderer = pChartRenderer as IFeatureRenderer;
        }
        
        // 创建堆叠柱状图表（stacked）
        public void CreateStackedChartSymbol(
            IGeoFeatureLayer pGeoFeatureLayer, //要渲染的图层
            string[] szRenderField, //堆叠图表示的字段
            IColor[] pFillsymbolColor, //这些字段分别需要渲染的颜色 
            double dbBarWidth, //每个柱子的宽度
            IColor pBgColor,//背景色
            double dbMarkerSize //整个堆叠柱状图的大小（单位：磅）
            )
        {
            if (pGeoFeatureLayer == null)
                return;
            pGeoFeatureLayer.ScaleSymbols = true;
            
            IFeatureLayer pFeatureLayer = pGeoFeatureLayer as IFeatureLayer;
            ITable pTable = pFeatureLayer as ITable;
            ICursor pCursor = pTable.Search(null, true);
            IRowBuffer pRowBuffer = pCursor.NextRow();
            IChartRenderer pChartRenderer = new ChartRendererClass();

            #region 循环增加堆叠图表示的字段
            IRendererFields pRendererFields = pChartRenderer as IRendererFields;
            for (int i = 0; i < szRenderField.Length; i++)
            {
                pRendererFields.AddField(szRenderField[i], szRenderField[i]);
            }
            #endregion
            
            #region 获取要素最大值
            double dbMaxValue = 0.0;
            while (pRowBuffer != null)
            {
                for (int i = 0; i < szRenderField.Length; i++)
                {
                    int nIdx = pTable.FindField(szRenderField[i]);
                    double dbFieldValue = double.Parse(pRowBuffer.get_Value(nIdx).ToString());
                    if (dbFieldValue > dbMaxValue)
                    {
                        dbMaxValue = dbFieldValue;
                    }
                }
                pRowBuffer = pCursor.NextRow();
            }
            #endregion

            //创建堆叠柱状符号
            IStackedChartSymbol pStackedChartSymbol = new StackedChartSymbolClass();
            pStackedChartSymbol.Width = 10;//柱子宽度
            IMarkerSymbol pMarkerSymbol = pStackedChartSymbol as IMarkerSymbol;
            pMarkerSymbol.Size = dbMarkerSize;//下面的大小
            IChartSymbol pChartSymbol = pStackedChartSymbol as IChartSymbol;
            pChartSymbol.MaxValue = dbMaxValue;//设置最大值

            #region 添加渲染符号
            ISymbolArray pSymbolArray = pStackedChartSymbol as ISymbolArray;
            IFillSymbol[] pFillSymbols = new IFillSymbol[szRenderField.Length];
            for (int i = 0; i < szRenderField.Length; i++)
            {
                //设置不同颜色的柱子
                pFillSymbols[i] = new SimpleFillSymbolClass();
                pFillSymbols[i].Color = pFillsymbolColor[i];
                pSymbolArray.AddSymbol(pFillSymbols[i] as ISymbol);
            }
            #endregion

            //设置柱状图符号
            pChartRenderer.ChartSymbol = pStackedChartSymbol as IChartSymbol;
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pBgColor;
            pChartRenderer.BaseSymbol = pFillSymbol as ISymbol;
            pChartRenderer.UseOverposter = false;
            //创建图例
            pChartRenderer.CreateLegend();
            pGeoFeatureLayer.Renderer = pChartRenderer as IFeatureRenderer;
        }
        

        //饼图渲染
        public void CreatePieChartSymbol(
            IGeoFeatureLayer pGeoFeatureLayer,//待渲染图层 
            string[] szRenderField, 
            IColor[] pFillsymbolColor, 
            Color pOutlineColor,
            Color pBgColor,
            double dbMarkerSize
            )
        {
            if (pGeoFeatureLayer == null)
                return;
            pGeoFeatureLayer.ScaleSymbols = true;

            //创建统计图渲染对象，即ChartRendererClass对象
            IChartRenderer pChartRenderer = new ChartRendererClass();

            #region 循环增加饼柱状图表示的字段
            //接口转换至IRendererFields
            IRendererFields pRendererFields = pChartRenderer as IRendererFields;
            for (int i = 0; i < szRenderField.Length; i++)
            {
                //增加饼状图表示的字段
                pRendererFields.AddField(szRenderField[i], szRenderField[i]);
            }
            #endregion

            IFeatureLayer pFeatureLayer = pGeoFeatureLayer as IFeatureLayer;
            ITable pTable = pFeatureLayer as ITable;//增加柱状图表示的字段
            ICursor pCursor = pTable.Search(null, true);//获取属性表游标

            #region 获取要素最大值，确定饼状图的最大高度
            double dbMaxValue = 0.0;
            IRowBuffer pRowBuffer = pCursor.NextRow();//获取第一行
            while (pRowBuffer != null)
            {
                for (int i = 0; i < szRenderField.Length; i++)
                {
                    int nIdx = pTable.FindField(szRenderField[i]);
                    double dbFieldValue = double.Parse(pRowBuffer.get_Value(nIdx).ToString());
                    if (dbFieldValue > dbMaxValue)
                    {
                        dbMaxValue = dbFieldValue;
                    }
                }
                pRowBuffer = pCursor.NextRow();
            }
            #endregion

            //创建饼状图符号
            IPieChartSymbol pPieChartSymbol = new PieChartSymbolClass();
            pPieChartSymbol.Clockwise = true;//饼图的方向：顺时针
            pPieChartSymbol.UseOutline = true;//是否显示外边框线

            #region 设置饼图的外边框线样式
            ILineSymbol pLineSymbol = new SimpleLineSymbolClass();
            pLineSymbol.Color = ColorHelper.GetRGBColor(
                pOutlineColor.R,
                pOutlineColor.G,
                pOutlineColor.B);
            pLineSymbol.Width = 2;
            pPieChartSymbol.Outline = pLineSymbol;
            #endregion

            //接口转换至IChartSymbol
            IChartSymbol pChartSymbol = pPieChartSymbol as IChartSymbol;
            pChartSymbol.MaxValue = dbMaxValue;//设置饼图符号的最大值
            //接口转换至IMarkerSymbol
            IMarkerSymbol pMarkerSymbol = pPieChartSymbol as IMarkerSymbol;
            pMarkerSymbol.Size = dbMarkerSize;//设置饼图符号大小

            #region 利用ISymbolArray接口添加符号
            //接口转换至ISymbolArray
            ISymbolArray pSymbolArrays = pPieChartSymbol as ISymbolArray;
            //创建填充符号数组，对其循环赋值
            IFillSymbol[] pFillSymbols = new IFillSymbol[szRenderField.Length];
            for (int i = 0; i < szRenderField.Length; i++)
            {
                //增加填充符号
                pFillSymbols[i] = new SimpleFillSymbolClass();
                pFillSymbols[i].Color = pFillsymbolColor[i];
                pSymbolArrays.AddSymbol(pFillSymbols[i] as ISymbol);
            }
            #endregion

            //设置背景
            pChartRenderer.ChartSymbol = pPieChartSymbol as IChartSymbol;
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = ColorHelper.GetRGBColor(
                pBgColor.R,
                pBgColor.G,
                pBgColor.B); 
            pChartRenderer.BaseSymbol = pFillSymbol as ISymbol;
            pChartRenderer.UseOverposter = false;
            //创建图例
            pChartRenderer.CreateLegend();
            pGeoFeatureLayer.Renderer = pChartRenderer as IFeatureRenderer;
        }

        //点密度专题图
        public void CreateDotDensityFillSymbol(
            IGeoFeatureLayer pGeoFeatureLayer, //要渲染的图层
            string szRenderField, //渲染字段
            double dbDotSize, //点大小
            Color pColor, //点颜色
            Color pColorBackground, //填充背景颜色
            ISimpleMarkerSymbol pSimpleMarkerSymbol, //点符号
            double dbRenderDensity//点密度
            )
        {
            if (pGeoFeatureLayer == null)
                return;
            
            //创建点密度渲染对象
            IDotDensityRenderer pDotDensityRenderer = new DotDensityRendererClass();

            //设置渲染字段
            IRendererFields pRendererFields = pDotDensityRenderer as IRendererFields;
            pRendererFields.AddField(szRenderField, szRenderField);

            //创建点密度符号对象
            IDotDensityFillSymbol pDotDensityFillSymbol = new DotDensityFillSymbolClass();
            pDotDensityFillSymbol = new DotDensityFillSymbolClass();
            pDotDensityFillSymbol.DotSize = dbDotSize;//点符号大小
            //点符号颜色
            pDotDensityFillSymbol.Color = ColorHelper.GetRGBColor(
                pColor.R,
                pColor.G,
                pColor.B);
            //点密度符号填充背景色
            pDotDensityFillSymbol.BackgroundColor = ColorHelper.GetRGBColor(
                pColorBackground.R, 
                pColorBackground.G,
                pColorBackground.B);

            //设置渲染符号
            ISymbolArray pSymbolArray = pDotDensityFillSymbol as ISymbolArray;
            pSymbolArray.AddSymbol(pSimpleMarkerSymbol as ISymbol);
            pDotDensityRenderer.DotDensitySymbol = pDotDensityFillSymbol;
            //设置渲染密度
            pDotDensityRenderer.DotValue = dbRenderDensity;
            //创建图例
            pDotDensityRenderer.CreateLegend();
            pGeoFeatureLayer.Renderer = pDotDensityRenderer as IFeatureRenderer;
        }
    }
}
