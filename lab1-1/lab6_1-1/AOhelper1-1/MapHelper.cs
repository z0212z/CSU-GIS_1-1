/*************************************************
 * 文件: MapHelper.cs
 * 说明：地图助手类类
 * 作者：中南大学李光强（QQ：41733233）
 * 时间：2022/12/1/
 * 声明：请尊重作者版权，使用此文件时，请保留此信息
 ***********************************************/

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_1_1.AOhelper1_1
{
    /// <summary>
    /// 地图助手类
    /// </summary>
    public class MapHelper
    {
        AxMapControl mapControl;
        IActiveView activeView;
        IScreenDisplay screenDisplay;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapControl"></param>
        public MapHelper(AxMapControl mapControl)
        {
            this.mapControl = mapControl;
            this.activeView = mapControl.ActiveView;
            screenDisplay = activeView.ScreenDisplay;
        }

        #region 画图形
        /// <summary>
        /// 绘制草图
        /// </summary>
        /// <param name="mapOp">地图操作类型</param>
        /// <param name="e">地图控件事件</param>
        /// <param name="drawElement">是否绘制图元</param>
        public IGeometry Draw(MapOperatorType mapOp = MapOperatorType.Default
            , IMapControlEvents2_OnMouseDownEvent e = null, bool drawElement = true)
        {
            //要根据用户当前选择的操作类型进行相应操作

            //图形
            IGeometry geom;

            // 要绘制的图元
            IElement element = null;
            //符号
            ISymbol symbol = null;
            IRgbColor color = new RgbColor();
            color.Red = 255;

            //创建点
            if (mapOp == MapOperatorType.DrawPoint)
            {
                IPoint point = new PointClass();
                //point.PutCoords(e.mapX, e.mapY);
                point.X = e.mapX;
                point.Y = e.mapY;
                symbol = Symbolizer.CreatePointSymbol(esriSimpleMarkerStyle.esriSMSCircle, color, 4);

                IMarkerElement el = new MarkerElementClass();
                el.Symbol = (ISimpleMarkerSymbol)symbol;
                element = el as IElement;
                geom = point;
            }
            else if (mapOp == MapOperatorType.DrawPolyline)
            {
                IRubberBand rubber = new RubberLine();
                IPolyline line = rubber.TrackNew(screenDisplay, null) as IPolyline;

                symbol = Symbolizer.CreateLineSymbol(esriSimpleLineStyle.esriSLSSolid, color, 2);

                ILineElement el = new LineElementClass();
                el.Symbol = (ISimpleLineSymbol)symbol;
                element = el as IElement;
                geom = line;
            }
            else if (mapOp == MapOperatorType.DrawPolygon)
            {
                IRubberBand rubber = new RubberPolygon();
                IPolygon polygon = rubber.TrackNew(screenDisplay, null) as IPolygon;

                symbol = Symbolizer.CreatePolygonSymbol(esriSimpleFillStyle.esriSFSHollow, color);

                IFillShapeElement el = new PolygonElementClass();
                el.Symbol = (ISimpleFillSymbol)symbol;
                element = el as IElement;
                geom = polygon;
            }
            else if (mapOp == MapOperatorType.DrawRectangle)
            {
                IRubberBand rubber = new RubberEnvelope();
                IEnvelope rect = rubber.TrackNew(screenDisplay, null) as IEnvelope;
                symbol = Symbolizer.CreatePolygonSymbol(esriSimpleFillStyle.esriSFSHollow, color);
                IFillShapeElement el = new PolygonElementClass();
                el.Symbol = (ISimpleFillSymbol)symbol;
                element = el as IElement;

                geom = Util.PolygonFromEnvlope(rect);
            }
            else
                return null;

            //绘制图元
            if (drawElement)
            {
                element.Geometry = geom;
                this.DrawElement(element);
            }

            return geom;
        }

        /// <summary>
        /// 绘制草图
        /// </summary>
        /// <param name="mapOp">地图操作类型</param>
        /// <param name="e">地图控件事件</param>
        public IGeometry Sktech(MapOperatorType mapOp = MapOperatorType.Default
            , IMapControlEvents2_OnMouseDownEvent e = null)
        {
            MapOperatorType mo;
            if (mapOp == MapOperatorType.CreatePoint)
                mo = MapOperatorType.DrawPoint;
            else if (mapOp == MapOperatorType.CreatePolyline)
                mo = MapOperatorType.DrawPolyline;
            else if (mapOp == MapOperatorType.CreatePolygon)
                mo = MapOperatorType.DrawPolygon;
            else
                return null;
            return this.Draw(mo, e);
        }
        #endregion

        #region 选择要素

        /// <summary>
        /// 选择要素
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="mapOp"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public IFeature SelectFeature(IFeatureLayer layer, MapOperatorType mapOp = MapOperatorType.Default
            , IMapControlEvents2_OnMouseDownEvent e = null)
        {
            if (mapOp == MapOperatorType.EditFeatureByLocation
                || mapOp == MapOperatorType.DeleteFeatureByLocation
                || mapOp == MapOperatorType.IdentifyFeature)
                return this.SelectFeatureByClick(layer, e);

            else if (mapOp == MapOperatorType.EditFeatureByRectangle
                  || mapOp == MapOperatorType.DeleteFeatureByRectangle)
                return this.SelectFeatureByRectangle(layer);

            else if (mapOp == MapOperatorType.DeleteFeatureByPolygon)
                return this.SelectFeatureByPolygon(layer);
            else
                return null;
        }

        /// <summary>
        /// 点选
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public IFeature SelectFeatureByClick(IFeatureLayer layer, IMapControlEvents2_OnMouseDownEvent e = null)
        {
            IPoint point = new PointClass();
            point.PutCoords(e.mapX, e.mapY);
            IIdentify identifyLayer = (IIdentify)layer;
            IArray array = identifyLayer.Identify(point);

            if (array != null)
            {
                object obj = array.get_Element(0);
                IFeatureIdentifyObj fobj = obj as IFeatureIdentifyObj;
                IRowIdentifyObject irow = fobj as IRowIdentifyObject;
                IFeature feature = irow.Row as IFeature;
                this.mapControl.Map.SelectFeature(layer, feature);
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                return feature;
            }
            else
                return null;
        }

        /// <summary>
        /// 框选
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public IFeature SelectFeatureByRectangle(IFeatureLayer layer)
        {
            IGeometry geom = this.Draw(MapOperatorType.DrawRectangle, null, false);
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = geom;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor cursor = layer.FeatureClass.Search(filter, true);

            IFeature feat = cursor.NextFeature();
            return feat;
        }

        /// <summary>
        /// 多边形选择
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public IFeature SelectFeatureByPolygon(IFeatureLayer layer)
        {
            IGeometry geom = this.Draw(MapOperatorType.DrawPolygon, null, false);
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = geom;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor cursor = layer.FeatureClass.Search(filter, true);

            IFeature feat = cursor.NextFeature();
            return feat;
        }

        #endregion

        #region 删除选择的要素

        /// <summary>
        /// 选择要素
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="mapOp"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public void DeleteFeature(IFeatureLayer layer, MapOperatorType mapOp = MapOperatorType.Default
            , IMapControlEvents2_OnMouseDownEvent e = null)
        {
            if (mapOp == MapOperatorType.DeleteFeatureByLocation)
                this.DeleteFeatureByClick(layer, e);
            else if (mapOp == MapOperatorType.DeleteFeatureByRectangle)
                this.DeleteFeatureByRectangle(layer);
            else if (mapOp == MapOperatorType.DeleteFeatureByPolygon)
                this.DeleteFeatureByPolygon(layer);
        }

        /// <summary>
        /// 点选删除
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public void DeleteFeatureByClick(IFeatureLayer layer, IMapControlEvents2_OnMouseDownEvent e = null)
        {
            IPoint point = new PointClass();
            point.PutCoords(e.mapX, e.mapY);
            IIdentify identifyLayer = (IIdentify)layer;
            IArray array = identifyLayer.Identify(point);

            if (array != null)
            {
                object obj = array.get_Element(0);
                IFeatureIdentifyObj fobj = obj as IFeatureIdentifyObj;
                IRowIdentifyObject irow = fobj as IRowIdentifyObject;
                IFeature feature = irow.Row as IFeature;
                this.mapControl.Map.SelectFeature(layer, feature);
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                if (MessageBox.Show("确认要删除选中的要素吗？"
                    , "提示"
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                FeatureClass.DeleteFeature(layer.FeatureClass, "FID=" + feature.OID.ToString());
                MessageBox.Show("删除成功!", "提示");
            }
        }

        /// <summary>
        /// 框选删除
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public void DeleteFeatureByRectangle(IFeatureLayer layer)
        {
            IGeometry geom = this.Draw(MapOperatorType.DrawRectangle, null, false);
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = geom;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor cursor = layer.FeatureClass.Update(filter, true);
            IFeature feat = cursor.NextFeature();
            if (feat != null)
            {
                if (MessageBox.Show("确认要删除选中的要素吗？"
                    , "提示"
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                while (feat != null)
                {
                    cursor.DeleteFeature();
                    feat = cursor.NextFeature();
                }
                MessageBox.Show("删除成功!", "提示");
            }
        }

        /// <summary>
        /// 多边形选择删除
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public void DeleteFeatureByPolygon(IFeatureLayer layer)
        {
            IGeometry geom = this.Draw(MapOperatorType.DrawPolygon, null, false);
            ISpatialFilter filter = new SpatialFilterClass();
            filter.Geometry = geom;
            filter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
            IFeatureCursor cursor = layer.FeatureClass.Update(filter, true);

            IFeature feat = cursor.NextFeature();
            if (feat != null)
            {
                if (MessageBox.Show("确认要删除选中的要素吗？"
                    , "提示"
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                while (feat != null)
                {
                    cursor.DeleteFeature();
                    feat = cursor.NextFeature();
                }
                MessageBox.Show("删除成功!", "提示");
            }
        }
        #endregion


        #region 画图元

        /// <summary>
        /// 绘制图元
        /// </summary>
        /// <param name="el">图元</param>
        /// <param name="clearAll">是否清除已有图元</param>
        public void DrawElement(IElement el, bool clearAll = true)
        {
            if (clearAll)
                activeView.GraphicsContainer.DeleteAllElements();
            activeView.GraphicsContainer.AddElement(el, 0);
            activeView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        #endregion
    }
}
