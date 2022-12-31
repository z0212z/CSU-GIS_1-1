using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.SystemUI;
using lab4_1_1.AOhelper1_1;
using lab4_1_1.MyForms;
using stdole;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using FeatureClass = lab4_1_1.AOhelper1_1.FeatureClass;

namespace lab4_1_1
{
    public partial class Form1 : Form
    {
        #region 私有变量
        /// <summary>
        /// 当前选中的图层
        /// </summary>
        ILayer selectedLayer = null;
        MapOperatorType mapOp;
        IEnvelope pEnvelope;
        object oLegendGroup = new object();
        object oIndex = new object();
        //IToolbarMenu m_menuLayer;
        esriTOCControlItem pTocItem = new esriTOCControlItem();
        IBasicMap pBasicMap = new Map() as IBasicMap;
        ILayer pLayer = new FeatureLayer();
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 保存sToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 隐藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 坐标显示栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            this.tslCoor.Text = string.Format("x={0},y={1}", e.mapX.ToString("0.00"), e.mapY.ToString("0.00"));
        }

        #region 主菜单图层
        /// <summary>
        /// 添加全部shp数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerAllShp_Click(object sender, EventArgs e)
        {
            this.LayerAllShp();
        }

        /// <summary>
        /// 添加单个shp数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 添加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayerAddShp();
        }

        /// <summary>
        /// 移除图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerRemove_Click(object sender, EventArgs e)
        {
            this.removeLayer();
        }

        /// <summary>
        /// 设置可选状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerSelectable_Click(object sender, EventArgs e)
        {
            this.LayerSelectable();
        }

        /// <summary>
        /// 设置可视化状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerVisible_Click(object sender, EventArgs e)
        {
            this.LayerVisable();
        }

        /// <summary>
        /// 添加到鹰眼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerThum_Click(object sender, EventArgs e)
        {
            this.LayertoThum();
        }
        #endregion


        #region 地图文档管理
        /// <summary>
        /// 地图文档的创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileNew_Click(object sender, EventArgs e)
        {
            ICommand command = new AOhelper1_1.NewMapDocument();
            command.OnCreate(this.axMap.Object);
            command.OnClick();
        }

        /// <summary>
        /// 打开文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            ICommand command = new ControlsOpenDocCommandClass();
            command.OnCreate(this.axMap.Object);
            command.OnClick();
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileSave_Click(object sender, EventArgs e)
        {
            ICommand command = new ControlsSaveAsDocCommandClass();
            command.OnCreate(this.axMap.Object);
            command.OnClick();
        }
        #endregion


        #region 加载图层数据
        /// <summary>
        /// 加载指定目录下所有shp文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerAllShp_Click(object sender, EventArgs e)
        {
            this.LayerAllShp();
        }

        /// <summary>
        /// 添加单个shp文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerAddShp_Click(object sender, EventArgs e)
        {
            this.LayerAddShp();
        }

        /// <summary>
        /// TOC鼠标弹起事件，获取当前选中的图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axTOC_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            esriTOCControlItem type = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap basicMap = null;
            object unk = null, data = null;
            axTOC.GetSelectedItem(ref type, ref basicMap, ref selectedLayer, ref unk, ref data);
            if (type == esriTOCControlItem.esriTOCControlItemLayer
                 && selectedLayer != null && e.button == 2)
                cmTOC.Show(axTOC, e.x, e.y);
        }

        /// <summary>
        /// 移除图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerRemove_Click(object sender, EventArgs e)
        {
            this.removeLayer();
        }

        /// <summary>
        /// 设置可选状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerSelectable_Click(object sender, EventArgs e)
        {
            this.LayerSelectable();
        }

        /// <summary>
        /// 设置可视化状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerVisible_Click(object sender, EventArgs e)
        {
            this.LayerVisable();
        }

        /// <summary>
        /// 添加到鹰眼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerThum_Click(object sender, EventArgs e)
        {
            this.LayertoThum();
        }
        #endregion


        #region 私有函数

        /// <summary>
        /// 加载指定目录下所有shp文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerAllShp()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() != DialogResult.OK)
                return;

            string folder = fbd.SelectedPath;

            IWorkspaceFactory wsf = new ShapefileWorkspaceFactory();
            IWorkspace ws = wsf.OpenFromFile(folder, 0);
            IEnumDatasetName edn = ws.DatasetNames[esriDatasetType.esriDTFeatureClass];
            IDatasetName dn;
            IFeatureClass fc;
            IFeatureLayer fl;
            while ((dn = edn.Next()) != null)
            {
                fc = ((IFeatureWorkspace)ws).OpenFeatureClass(dn.Name);
                fl = new FeatureLayerClass();
                fl.FeatureClass = fc;
                fl.Name = fc.AliasName;
                this.axMap.AddLayer(fl);
            }
            this.tslMain.Text = " 提示：当前打开了【" + folder + "】目录下的所有SHP.";
        }

        /// <summary>
        /// 添加单个shp文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerAddShp()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "shp文件|*.shp|全部文件|*.*";
            ofd.Title = "打开SHP文件";
            ofd.InitialDirectory = @"D:\temp\csu";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            //获取当前选中的SHP文件
            string shpfile = ofd.FileName;
            //利用System.IO命名空间中的FileInfo对象，获取文件全名中的目录路径及文件名称
            FileInfo fi = new FileInfo(shpfile);
            string folder = fi.Directory.FullName;
            string fileName = fi.Name;

            IWorkspaceFactory wsf = new ShapefileWorkspaceFactory();
            IFeatureWorkspace fws = wsf.OpenFromFile(folder, 0) as IFeatureWorkspace;
            IFeatureClass fc = fws.OpenFeatureClass(fileName);

            IFeatureLayer fl = new FeatureLayer();
            fl.FeatureClass = fc;
            fl.Name = fc.AliasName;

            this.axMap.AddLayer(fl);
        }
        /// <summary>
        /// 当前是否选中了图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private bool isSelectedLayer()
        {
            if (this.selectedLayer == null)
            {
                MessageBox.Show("当前没有选中图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 移除图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeLayer()
        {
            if (!isSelectedLayer())
                return;

            int index = -1;
            for (int i = 0; i < this.axMap.LayerCount; i++)
                if (axMap.get_Layer(i).Name == selectedLayer.Name)
                {
                    index = i;
                    break;
                }
            if (index >= 0)
                this.axMap.DeleteLayer(index);
        }

        /// <summary>
        /// 图层可选状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerSelectable()
        {
            if (!isSelectedLayer())
                return;

            for (int i = 0; i < this.axMap.LayerCount; i++)
                if (this.axMap.get_Layer(i) is IFeatureLayer)
                {

                    ((IFeatureLayer)this.axMap.get_Layer(i)).Selectable =
                        (this.axMap.get_Layer(i).Name == this.selectedLayer.Name);
                }
        }

        /// <summary>
        /// 设置可视化状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerVisable()
        {
            if (!isSelectedLayer())
                return;

            this.selectedLayer.Visible = !this.selectedLayer.Visible;

        }

        /// <summary>
        /// 添加到鹰眼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayertoThum()
        {
            if (!isSelectedLayer())
                return;
            this.axThum.AddLayer(this.selectedLayer);
        }
        #endregion


        #region TOC快捷菜单

        /// <summary>
        /// 上移图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmUp_Click(object sender, EventArgs e)
        {
            int layerCount = axMap.LayerCount;
            for (int i = 0; i < layerCount; i++)
            {
                if (axMap.get_Layer(i).Name == selectedLayer.Name
                     && i - 1 >= 0)
                {
                    axMap.MoveLayerTo(i, i - 1);
                    break;
                }
            }
        }

        /// <summary>
        /// 下移图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmDown_Click(object sender, EventArgs e)
        {
            int layerCount = axMap.LayerCount;
            for (int i = 0; i < layerCount; i++)
            {
                if (axMap.get_Layer(i).Name == selectedLayer.Name
                     && i + 1 < layerCount)
                {
                    axMap.MoveLayerTo(i, i + 1);
                    break;
                }
            }

        }

        /// <summary>
        /// 移除图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmRemove_Click(object sender, EventArgs e)
        {
            this.removeLayer();
        }

        /// <summary>
        /// 设置可选状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmSelectable_Click(object sender, EventArgs e)
        {
            this.LayerSelectable();
        }

        /// <summary>
        /// 设置可视化状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmVisible_Click(object sender, EventArgs e)
        {
            this.LayerVisable();
        }

        /// <summary>
        /// 添加到鹰眼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmThum_Click(object sender, EventArgs e)
        {
            this.LayertoThum();
        }


        #endregion

        /// <summary>
        /// 鹰眼地图中绘制红色图框
        /// </summary>
        IEnvelope ext;
        private void axMap_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            //获取当前主地图的显示范围
            this.ext = this.axMap.Extent;
            //创建填充符号
            IFillSymbol fillSymbol = new SimpleFillSymbol();
            //设置填充颜色为透明
            IRgbColor color = new RgbColor();
            color.NullColor = true;
            color.Transparency = 0;
            fillSymbol.Color = color;

            //设置填充符号的边界线符号
            ILineSymbol lineSymbol = new SimpleLineSymbol();
            IRgbColor lineColor = new RgbColor();
            lineColor.Red = 255;
            lineSymbol.Color = lineColor;
            fillSymbol.Outline = lineSymbol;

            //创建图元
            IElement element = new RectangleElementClass();
            element.Geometry = ext;
            IFillShapeElement fillElement = (IFillShapeElement)element;
            //设置图元的符号
            fillElement.Symbol = fillSymbol;

            this.axThum.ActiveView.GraphicsContainer.DeleteAllElements();
            this.axThum.ActiveView.GraphicsContainer.AddElement(element, 0);

            this.axThum.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }


        private void contextToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 帮助使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUseHelp_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 关于系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAboutsystem_Click(object sender, EventArgs e)
        {

        }

        private void 要素类管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 添加要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureNew_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;

            IFeatureLayer fl = this.selectedLayer as IFeatureLayer;
            if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                this.mapOp = MapOperatorType.CreatePoint;
            else if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                this.mapOp = MapOperatorType.CreatePolyline;
            else if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                this.mapOp = MapOperatorType.CreatePolygon;
            else
                this.mapOp = MapOperatorType.Default;
        }

        private void menuFeatureNew_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;

            IFeatureLayer fl = this.selectedLayer as IFeatureLayer;
            if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                this.mapOp = MapOperatorType.CreatePoint;
            else if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                this.mapOp = MapOperatorType.CreatePolyline;
            else if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                this.mapOp = MapOperatorType.CreatePolygon;
            else
                this.mapOp = MapOperatorType.Default;
        }



        /// <summary>
        /// 创建要素类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureClassNew_Click(object sender, EventArgs e)
        {
            MyForms.FormNewFeatureClass frm = new MyForms.FormNewFeatureClass();
            //如果创建成功，则将新创建的要素类加载到地图中
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show(string.Format("[{0}\\{1}] 创建成功.\n 是否加载数据？"
                , frm.textFolder, frm.textShp)
                , "提示"
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    IFeatureLayer layer = new FeatureLayer();
                    layer.FeatureClass = frm.featureClass;
                    layer.Name = frm.featureClass.AliasName;
                    this.axMap.AddLayer(layer);
                }
            }
        }

        private void menuFeatureClassNew_Click(object sender, EventArgs e)
        {
            tlbFeatureClassNew_Click(sender, e);
        }


        /// <summary>
        /// 点选编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureEditByLocation_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.EditFeatureByLocation;
        }

        private void menuFeatureEditByLocation_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.EditFeatureByLocation;
        }

        /// <summary>
        /// 框选编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFeatureEditByRectangle_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.EditFeatureByRectangle;
        }

        private void tlbFeatureEditByRectangle_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.EditFeatureByRectangle;
        }

        /// <summary>
        /// 框选编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureEditByPolygon_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.EditFeatureByRectangle;
        }

        private void menuFeatureEditByPolygon_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.EditFeatureByRectangle;
        }

        /// <summary>
        /// 点选删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureDeleteByLocation_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.DeleteFeatureByLocation;
        }

        private void menuFeatureDeleteByLocation_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.DeleteFeatureByLocation;
        }

        /// <summary>
        /// 框选删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFeatureDeleteByRectangle_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.DeleteFeatureByRectangle;
        }

        private void tlbFeatureDeleteByRectangle_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.DeleteFeatureByRectangle;
        }

        /// <summary>
        /// 多边形选择删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureDeleteByPolygon_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.DeleteFeatureByPolygon;
        }

        private void menuFeatureDeleteByPolygon_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.DeleteFeatureByPolygon;
        }

        /// <summary>
        /// 浏览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureBrowse_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            IFeatureLayer layer = this.selectedLayer as IFeatureLayer;
            MyForms.FormDisplayFeatures frm = new MyForms.FormDisplayFeatures(layer.FeatureClass);
            frm.ShowDialog();
        }

        private void 管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tlbFeatureBrowse_Click(sender, e);
        }

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureIdentify_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.EditFeatureByLocation;
        }

        private void menuFeatureIdentify_Click(object sender, EventArgs e)
        {
            tlbFeatureIdentify_Click(sender, e);
        }

        /// <summary>
        /// 编辑要素类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureClassEdit_Click(object sender, EventArgs e)
        {
            if (this.selectedLayer == null)
            {
                MessageBox.Show("当前没有选中图层！", "提示");
                return;
            }

            if (!(this.selectedLayer is IFeatureLayer))
            {
                MessageBox.Show("当前选中的图层不是矢量数据图层！", "提示");
                return;
            }

            MyForms.FormEditFeatureClass frm = new MyForms.FormEditFeatureClass((IFeatureLayer)this.selectedLayer);
            frm.ShowDialog();
        }
        private void menuFeatureClassEdit_Click(object sender, EventArgs e)
        {
            tlbFeatureClassEdit_Click(sender, e);
        }

        /// <summary>
        /// 删除要素类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFeatureClassDelete_Click(object sender, EventArgs e)
        {
            tlbFeatureClassDelete_Click(sender, e);
        }

        private void tlbFeatureClassDelete_Click(object sender, EventArgs e)
        {
            if (this.selectedLayer == null)
            {
                MessageBox.Show("当前没有选中图层！", "提示");
                return;
            }

            if (!(this.selectedLayer is IFeatureLayer))
            {
                MessageBox.Show("当前选中的图层不是矢量数据图层！", "提示");
                return;
            }

            if (MessageBox.Show("确认要删除选中图层的要素类吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            lab4_1_1.AOhelper1_1.FeatureClass.DeleteFeatureClass(((IFeatureLayer)this.selectedLayer).FeatureClass);
            MessageBox.Show("要素已删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void axMap_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            MapHelper mapHelper = new MapHelper(this.axMap);
            if (mapOp >= MapOperatorType.CreatePoint && mapOp <= MapOperatorType.CreatePolygon)
            {
                IGeometry geom = mapHelper.Sktech(this.mapOp, e);

                MyForms.FormNewFeature frm = new MyForms.FormNewFeature(this.selectedLayer as IFeatureLayer, geom);
                frm.ShowDialog();
            }
            else if (mapOp >= MapOperatorType.EditFeatureByLocation && mapOp <= MapOperatorType.EditFeatureByRectangle)
            {
                this.axMap.Map.ClearSelection();
                IFeature feature = mapHelper.SelectFeature(this.selectedLayer as IFeatureLayer, mapOp, e);
                if (feature == null)
                    return;
                else
                {
                    MyForms.FormLine3DInfo frm = new MyForms.FormLine3DInfo(this.axMap);
                    frm.Show();
                }
            }
            else if (mapOp >= MapOperatorType.DeleteFeatureByLocation && mapOp <= MapOperatorType.DeleteFeatureByPolygon)
            {
                this.axMap.Map.ClearSelection();
                mapHelper.DeleteFeature(this.selectedLayer as IFeatureLayer, mapOp, e);
            }
            else if (mapOp == MapOperatorType.IdentifyFeature)
            {
                this.axMap.Map.ClearSelection();
                IFeature feature = mapHelper.SelectFeature(this.selectedLayer as IFeatureLayer, mapOp, e);
                MyForms.FormIdentify frm = new MyForms.FormIdentify(feature);
                frm.ShowDialog();
            }
            else if (mapOp == MapOperatorType.AnalystPoint)
            {
                IPoint point = new PointClass();
                point.X = e.mapX;
                point.Y = e.mapY;
                SpatialAnalyst spatialAnalyst = new SpatialAnalyst(selectedLayer);
                spatialAnalyst.pointAnalyst(point);
            }
            else if (mapOp == MapOperatorType.SelectHighlight)
            {
                axMap.MousePointer = esriControlsMousePointer.esriPointerDefault;
                IMap pMap = axMap.Map;
                IGeometry pGeometry = axMap.TrackRectangle();   //获取几何图框范围
                ISelectionEnvironment pSelectionEnv = new SelectionEnvironment(); //新建选择环境
                IRgbColor pColor = new RgbColor();
                pColor.Red = 232;                       //调整高亮显示的颜色
                pSelectionEnv.DefaultColor = pColor;     //设置高亮显示的颜色
                pMap.SelectByShape(pGeometry, pSelectionEnv, false);  //选择图形SelectByShape方法
                axMap.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null); //刷新图层
            }
            else if(mapOp==MapOperatorType.ZoomRectangle)
            {
                pEnvelope = this.axMap.TrackRectangle();
                axMap.Extent = pEnvelope;
                axMap.Refresh();
            }
            else if(mapOp==MapOperatorType.Pan)
            {
                this.axMap.Pan();
            }
        }


        #region 道路数据处理

        /// <summary>
        /// 去除短线道路
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuRoadRemove_Click(object sender, EventArgs e)
        {
            if (this.isSelectedLayer() == false)
                return;
            IFeatureClass fc = ((IFeatureLayer)this.selectedLayer).FeatureClass;

            try
            {
                IFeatureCursor cursor = fc.Search(null, true);
                IFeature feat;
                while ((feat = cursor.NextFeature()) != null)
                {
                    if (((IPolyline)feat.Shape).Length < 20)
                    {
                        ISpatialFilter sf = new SpatialFilterClass();
                        sf.WhereClause = "FID<>" + feat.OID.ToString();
                        sf.Geometry = ((ITopologicalOperator)feat.Shape).Buffer(0.01);
                        sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        int n = fc.FeatureCount(sf);
                        if (n == 0)
                        {
                            Debug.WriteLine("small road:" + feat.OID.ToString());
                            MessageBox.Show("small road:" + feat.OID.ToString());
                            feat.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>Exception:" + ex.Message);
            }
        }

        /// <summary>
        /// 道路简化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuRoadGeneralize_Click(object sender, EventArgs e)
        {
            if (this.isSelectedLayer() == false)
                return;
            IFeatureClass fc = ((IFeatureLayer)this.selectedLayer).FeatureClass;

            try
            {
                IQueryFilter qf = new QueryFilterClass { WhereClause = "Name='升华大道'" };
                IFeatureCursor cursor = fc.Search(qf, true);
                IFeature feat = cursor.NextFeature();
                if (feat == null)
                {
                    Debug.WriteLine("没有名为[升华大道]的道路", "提示");
                    return;
                }

                IPolyline line = (IPolyline)feat.Shape;
                IPointCollection pc = (IPointCollection)line;

                //测试
                Debug.WriteLine(">>·简化前顶点数量:" + pc.PointCount.ToString());
                line.Generalize(5);//简化程度
                pc = (IPointCollection)line;
                //测试
                Debug.WriteLine(">>简化后顶点数量:" + pc.PointCount.ToString());
                feat.Store();//存回

                #region 绘制简化后的线
                IElement el = new LineElementClass();
                el.Geometry = line;
                this.axMap.ActiveView.GraphicsContainer.AddElement((IElement)el, 0);
                this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                #endregion

            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>Exception:" + ex.Message);
            }
        }

        /// <summary>
        /// 延长相接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuRoadExtent_Click(object sender, EventArgs e)
        {
            if (this.isSelectedLayer() == false)
                return;
            IFeatureClass fc = ((IFeatureLayer)this.selectedLayer).FeatureClass;

            try
            {
                IQueryFilter qf = new QueryFilterClass { WhereClause = "Name='升华大道'" };
                IFeatureCursor cursor = fc.Search(qf, true);
                IFeature feat_a = cursor.NextFeature();
                if (feat_a == null)
                {
                    Debug.WriteLine("没有名为[升华大道]的道路", "提示");
                    return;

                }

                this.tslMain.Text = "正在延长升华大道周边的道路...";
                this.tslProgress.Value = 0;
                IPolyline line_a = (IPolyline)feat_a.Shape;
                IPolygon buffer = ((ITopologicalOperator)line_a).Buffer(5.0) as IPolygon;


                #region 画道路的缓冲区
                ISimpleLineSymbol linesym = new SimpleLineSymbolClass(); IRgbColor color = new RgbColorClass();
                color.Blue = 255;
                linesym.Color = color;
                linesym.Style = esriSimpleLineStyle.esriSLSDash;
                ISimpleFillSymbol fillsym = new SimpleFillSymbolClass(); fillsym.Style = esriSimpleFillStyle.esriSFSHollow;
                fillsym.Outline = linesym;
                IFillShapeElement el = new PolygonElementClass();
                el.Symbol = fillsym;
                ((IElement)el).Geometry = buffer;

                this.axMap.ActiveView.GraphicsContainer.AddElement((IElement)el, 0);
                this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                #endregion


                #region 查询与缓冲区相交但不也目标线相交的道路
                ISpatialFilter sf = new SpatialFilterClass { WhereClause = "FID<>" + feat_a.OID.ToString() };
                sf.Geometry = buffer;
                sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                this.tslProgress.Maximum = fc.FeatureCount(sf);
                this.statusBar.Refresh();
                IRelationalOperator ro = line_a as IRelationalOperator;
                IConstructCurve curve = new PolylineClass();
                bool isPerformed = false;
                cursor = fc.Search(sf, true);//遍历查询结果
                IFeature feat_b;
                while ((feat_b = cursor.NextFeature()) != null)
                {
                    if (ro.Disjoint(feat_b.Shape) == true)
                    {
                        //显示延长前的长度
                        IPolyline line_b = feat_b.Shape as IPolyline;
                        Debug.WriteLine(string.Format(" >>[FID={0}] 延长前长度:{1}", feat_b.OID, line_b.Length));

                        curve.ConstructExtended(
                          line_b as ICurve
                        , line_a as ICurve
                        , (int)esriCurveExtension.esriDefaultCurveExtension, ref isPerformed);
                        if (isPerformed)
                        {
                            //绘制延长后的线，用于测试
                            ILineElement le = new LineElementClass();
                            linesym.Style = esriSimpleLineStyle.esriSLSDash;
                            color = new RgbColorClass { Red = 255, Blue = 0, Green = 0 };
                            linesym.Color = color;
                            le.Symbol = linesym;
                            ((IElement)le).Geometry = curve as IPolyline;
                            this.axMap.ActiveView.GraphicsContainer.AddElement((IElement)le, 0);
                            this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                            //显示延长后的长度··............... ..... ...
                            Debug.WriteLine(string.Format(" ## [FID={0}] 延长后长度:{1}", feat_a.OID, ((IPolyline)curve).Length));
                            //-----·测试结束·-------------------------------
                            //以下要存回延长后的道路,代码略
                        }
                        line_b = curve as IPolyline;
                        feat_b.Shape = line_b;
                        feat_b.Store();
                    }
                    this.tslProgress.Value++;
                    this.statusBar.Refresh();
                }
                #endregion
                this.tslMain.Text = " 就绪.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>Exception:" + ex.Message);
                this.tslMain.Text = " 延长道路时出错.";
            }
        }

        /// <summary>
        /// 路口打断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuRoadBreak_Click(object sender, EventArgs e)
        {
            if (this.isSelectedLayer() == false)
                return;
            IFeatureClass fc = ((IFeatureLayer)this.selectedLayer).FeatureClass;

            try
            {
                IQueryFilter qf = new QueryFilterClass { WhereClause = "Name='清水路'" };
                IFeatureCursor cursor = fc.Search(qf, true);
                IFeature feat_a = cursor.NextFeature();
                if (feat_a == null)
                {
                    Debug.WriteLine("没有名为[清水路]的道路", "提示");
                    return;

                }

                this.tslMain.Text = "正在处理道路毛刺...";
                this.tslProgress.Value = 0;

                #region 查询与清水路相交的道路
                ISpatialFilter sf = new SpatialFilterClass { WhereClause = "FID<>" + feat_a.OID.ToString() }; sf.Geometry = feat_a.Shape;
                sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                ITopologicalOperator topo = feat_a.Shape as ITopologicalOperator;
                cursor = fc.Search(sf, true);
                IFeature feat_b;

                //遍历查询结果
                while ((feat_b = cursor.NextFeature()) != null)
                {
                    //计算两条道路的交点
                    IGeometry resultGeom = (IGeometry)topo.Intersect(feat_b.Shape, esriGeometryDimension.esriGeometry0Dimension);
                    IGeometryCollection pc = (IGeometryCollection)resultGeom;
                    if (pc.GeometryCount == 0)
                    {
                        Debug.WriteLine(" >>>·与道路[FID=" + feat_b.OID.ToString() + "]没有交点!");
                        continue;
                    }
                    else
                        Debug.WriteLine(" >>>·与道路[FID=" + feat_b.OID.ToString() + "]相交!");
                    //取第一个交点，即使有多点，也忽略其余点
                    IPoint pt = pc.Geometry[0] as IPoint;
                    #region·显示交点,用于测试
                    ISimpleMarkerSymbol sym = new SimpleMarkerSymbolClass();
                    sym.Style = esriSimpleMarkerStyle.esriSMSSquare;
                    sym.Size = 5;
                    IMarkerElement me = new MarkerElementClass { Geometry = pt };
                    me.Symbol = sym;
                    this.axMap.ActiveView.GraphicsContainer.AddElement((IElement)me, 0);
                    this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    #endregion

                    //使用IFeatureEdit接口分割要素b，分割以后会直接更新要素类中，需要注意!!!!
                    IFeatureEdit featureEdit = feat_b as IFeatureEdit2;
                    //分割要素b，将分割后的要素集存入set
                    ISet set = featureEdit.Split(pt);
                    //访问分割后的结果集，在其中可以检查哪个要素小于给定阈值，然后将其删除
                    if (set != null)
                    {
                        set.Reset();
                        if (set.Count > 1)
                        {
                            //--将分割结果绘制在地图，以便测试--------------------------
                            IFeature newFeat = set.Next() as IFeature;
                            IPolyline pl_b = newFeat.Shape as IPolyline;
                            ILineElement le = new LineElementClass { Geometry = pl_b };
                            this.axMap.ActiveView.GraphicsContainer.AddElement((IElement)le, 0);
                            this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                            //--测试代码结束--------------------------
                            newFeat.Delete();
                        }

                    }
                    this.tslProgress.Value++;
                    this.statusBar.Refresh();
                }
                #endregion
                this.tslMain.Text = " 就绪.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>Exception:" + ex.Message);
                this.tslMain.Text = " 处理道路毛刺时出错.";
            }
        }
        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 生成EDMToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tsbAnalystDenoise_Click(object sender, EventArgs e)
        {
            this.menuAnalystDenoise_Click(null, null);
        }

        private void menuAnalystDenoise_Click(object sender, EventArgs e)
        {
            List<IFeatureLayer> layers = new List<IFeatureLayer>();
            for (int i = 0; i < this.axMap.LayerCount; i++)
            {
                if (this.axMap.get_Layer(i) is IFeatureLayer)
                {
                    IFeatureLayer f1 = this.axMap.get_Layer(i) as IFeatureLayer;
                    if (f1.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        layers.Add(this.axMap.get_Layer(i) as IFeatureLayer);
                }
            }
            if (layers.Count == 0)
            {
                MessageBox.Show("请为地图添加点状矢量图层.", "提示");
                return;
            }

            MyForms.FormOutlier frm = new MyForms.FormOutlier(this.axMap);
            frm.Show();
        }

        private void tsbAnalystDem_Click(object sender, EventArgs e)
        {
            this.maneAnalystDem_Click(null, null);
        }

        private void menuAnalystSlope_Click(object sender, EventArgs e)
        {
            MyForms.FormAnalysisSlope frm = new MyForms.FormAnalysisSlope(axMap);
            frm.Show();
        }

        private void tsbAnalystSlope_Click(object sender, EventArgs e)
        {
            this.menuAnalystSlope_Click(null, null);
        }

        private void maneAnalystDem_Click(object sender, EventArgs e)
        {
            MyForms.FormIDW frm = new MyForms.FormIDW(this.axMap);
            frm.ShowDialog();
        }

        private void menuAnalystPoint3DInfo_Click(object sender, EventArgs e)
        {
            this.tsbAnalystPoint3DInfo_Click(null, null);
        }

        private void tsbAnalystPoint3DInfo_Click(object sender, EventArgs e)
        {
            this.mapOp = MapOperatorType.AnalystPoint;
        }

        private void maneAnalystPolyline_Click(object sender, EventArgs e)
        {
            this.tsbAnalystPolyline_Click(null, null);
        }

        private void tsbAnalystPolyline_Click(object sender, EventArgs e)
        {
            //this.mapOp = MapOperatorType.Analystline;
            MyForms.FormLine3DInfo frm = new MyForms.FormLine3DInfo(this.axMap);
            frm.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "tif文件|*.tif|全部文件|*.*";
            ofd.Title = "打开tif文件";
            ofd.InitialDirectory = @"D:\temp\csu";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            //获取当前选中的SHP文件
            string tiffile = ofd.FileName;
            //利用System.IO命名空间中的FileInfo对象，获取文件全名中的目录路径及文件名称
            FileInfo fi = new FileInfo(tiffile);
            string folder = fi.Directory.FullName;
            string fileName = fi.Name;

            IWorkspaceFactory wsf = new RasterWorkspaceFactoryClass();
            IRasterWorkspace rws = wsf.OpenFromFile(folder, 0) as IRasterWorkspace;
            IRasterDataset rd = (IRasterDataset)rws.OpenRasterDataset(fileName);
            IRasterLayer rl = new RasterLayerClass();//生成一个矢量图层对象
            rl.CreateFromDataset(rd);//利用矢量图层对象去创建对应的raster文件
            this.axMap.AddLayer(rl);
        }

        private void 点选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.SelectFeatureByLocation;
        }

        private void 框选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.SelectFeatureByPolyline;
        }

        private void 多边形选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.SelectFeatureByPolygon;
        }

        private void 框选ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.SelectFeatureByRectangle;
        }

        private void tlbSelectHighlight_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.SelectHighlight;
        }

        private void menuSelectHighlight_Click(object sender, EventArgs e)
        {
            tlbSelectHighlight_Click(sender, e);
        }

        private void axTOC_OnDoubleClick(object sender, ITOCControlEvents_OnDoubleClickEvent e)
        {
            axTOC.HitTest(e.x, e.y, ref pTocItem, ref pBasicMap, ref pLayer, ref oLegendGroup, ref oIndex);

            if (e.button == 1)
            {
                if (pTocItem == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    //取得图例
                    ILegendClass pLegendClass = ((ILegendGroup)oLegendGroup).get_Class((int)oIndex);
                    //创建符号选择器SymbolSelector实例
                    FormSymbolSelection SymbolSelectorFrm = new FormSymbolSelection(pLegendClass, pLayer);
                    if (SymbolSelectorFrm.ShowDialog() == DialogResult.OK)
                    {
                        //局部更新主Map控件
                        this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        //设置新的符号
                        pLegendClass.Symbol = SymbolSelectorFrm.pSymbol;
                        //更新主Map控件和图层控件
                        this.axMap.ActiveView.Refresh();
                        this.axTOC.Refresh();
                        this.axMap.Refresh();
                    }
                }
            }
        }

        private void tsmLabel_Click(object sender, EventArgs e)
        {
            MyForms.FormLayerLabel frm = new MyForms.FormLayerLabel(this.selectedLayer,this.axMap);
            frm.ShowDialog();
        }

        private void tsmUniqueRender_Click(object sender, EventArgs e)
        {
            pLayer = (IFeatureLayer)this.axMap.get_Layer(0);

            FeatureLayerRenderHelper render = FeatureLayerRenderHelper.GetInstance();
            IUniqueValueRenderer ur = 
                render.CreateUniqueValueRenderer(((IFeatureLayer)pLayer).FeatureClass, "FID");//已经写死FID作为唯一值，可优化

            IGeoFeatureLayer geoLayer = pLayer as IGeoFeatureLayer;
            geoLayer.Renderer = ur as IFeatureRenderer;

            //createUniqueValueRander(geoLayer, "FID", esriSimpleFillStyle.esriSFSNull, 34);
            this.axMap.Refresh();

        }

        /// <summary>
        /// 计算面积
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmCalculateArea_Click(object sender, EventArgs e)
        {
            try
            {
                IFeatureClass fc = ((IFeatureLayer)this.axMap.get_Layer(0)).FeatureClass;
                int n = fc.FeatureCount(null);
                //寻找建筑物中Area字段，如果没有就添加
                IFields fields = fc.Fields;
                int indexArea = fields.FindField("Area");
                if (indexArea < 0)
                {
                    IFieldEdit fe = new FieldClass();
                    fe.Name_2 = "Area";
                    fe.Type_2 = esriFieldType.esriFieldTypeSingle;
                    fc.AddField(fe);
                    indexArea = fc.Fields.FieldCount - 1;
                }

                this.tslMain.Text = " 开始设置建筑物面积...";
                this.tslProgress.Value = 0;
                this.statusBar.Refresh();

                this.tslProgress.Maximum = n;

                //Building Cursor
                IFeatureCursor cur1 = fc.Search(null, true);
                IFeature feat1;
                int Area0Num = 0;
                while ((feat1 = cur1.NextFeature()) != null)
                {
                    IArea area = (IArea)feat1.Shape;
                    feat1.Value[indexArea] = area.Area;
                    if (area.Area != 0)
                        feat1.Store();
                    else
                    {
                        FeatureClass.DeleteFeature(((IFeatureLayer)this.axMap.get_Layer(0)).FeatureClass, "FID=" + feat1.OID.ToString());
                        Area0Num++;
                    }
                    this.tslProgress.Value++;
                    this.statusBar.Refresh();
                }
                MessageBox.Show("计算完成，删除空要素" + Area0Num + "个", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message
                        , "异常错误"
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Error);
            }
            finally
            {
                this.tslMain.Text = " 就绪.";
            }
        }

        /// <summary>
        /// 标识面积
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmIdentifyArea_Click(object sender, EventArgs e)
        {
            // 创建颜色
            IRgbColor pRgbColor = new RgbColor();
            pRgbColor.Red = 0;
            pRgbColor.Green = 0;
            pRgbColor.Blue = 255;

            // 创建字体
            IFontDisp pFontDisp = new StdFont() as IFontDisp;
            pFontDisp.Bold = true;
            pFontDisp.Size = 8;

            // 创建符号
            ITextSymbol pTextSymbol = new TextSymbol();
            pTextSymbol.Angle = 0;
            pTextSymbol.Color = pRgbColor;
            pTextSymbol.Font = pFontDisp;

            // 删除已有文本元素
            IActiveView pActiveView = axMap.ActiveView;
            IGraphicsContainer pGraphicsContainer = pActiveView.GraphicsContainer;
            pGraphicsContainer.DeleteAllElements();

            // 获取要素游标
            IFeatureLayer pFeatureLayer = axMap.get_Layer(0) as IFeatureLayer;
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, true);
            IFeature pFeature = pFeatureCursor.NextFeature();

            // 遍历要素游标
            int fieldIndex = pFeatureClass.Fields.FindField("FID");//有Name字段可以替换FID
            while (pFeature != null)
            {
                string label;

                IPointCollection pc = (IPointCollection)pFeature.Shape;
                // 获取重心
                IArea pArea = (IArea)pFeature.Shape;
                IPoint pPoint = pc.Point[0];//pArea.Centroid;有值为零的数据会报错

                label = pFeature.get_Value(fieldIndex).ToString() + "\n" + pArea.Area.ToString("0.00");
                // 创建文本元素
                ITextElement pTextElement = new TextElement() as ITextElement;
                pTextElement.Symbol = pTextSymbol;
                pTextElement.Text = label;

                // 添加文本元素
                IElement pElement = pTextElement as IElement;
                pElement.Geometry = pPoint;
                pGraphicsContainer.AddElement(pElement, 0);
                pFeature = pFeatureCursor.NextFeature();
            }

            // 刷新地图
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void tsbZoomRectangle_Click(object sender, EventArgs e)
        {
            this.mapOp = MapOperatorType.ZoomRectangle;
        }

        private void tsbPan_Click(object sender, EventArgs e)
        {
            this.mapOp = MapOperatorType.Pan;
        }

        private void axTOC_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            axTOC.HitTest(e.x, e.y, ref pTocItem, ref pBasicMap, ref pLayer, ref oLegendGroup, ref oIndex);
            if (e.button == 1)
            {
                if (pTocItem == esriTOCControlItem.esriTOCControlItemMap)
                {
                    tslMain.Text = "当前单击的是地图：" + pBasicMap.Name + "  地图中图层数为：" + pBasicMap.LayerCount.ToString();
                }
                else if (pTocItem == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    tslMain.Text = "当前单击的图层：" + pLayer.Name;
                }
                else if (pTocItem == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    tslMain.Text = "当前单击的图层符号,图层名称：" + pLayer.Name;
                }
                else if (pTocItem == esriTOCControlItem.esriTOCControlItemNone)
                {
                    tslMain.Text = "当前单击为空白区域";
                }
            }
        }

        private void tsmClassRender_Click(object sender, EventArgs e)
        {
            pLayer = (IFeatureLayer)this.axMap.get_Layer(0);
            IGeoFeatureLayer geoLayer = pLayer as IGeoFeatureLayer;
            createClassBreakRender(geoLayer, 15, "Area", esriSimpleFillStyle.esriSFSSolid);
            this.axMap.Refresh();
        }

        public void createClassBreakRender(IGeoFeatureLayer geoFeatureLayer,
            int classCount,
            string ClassField,
            esriSimpleFillStyle FillStyle)
        {
            //int classCount = 6;
            ITableHistogram tableHistogram;//表格直方图
            IBasicHistogram basicHistogram;//Provides access to members that control histogram objects created from different data sources. 

            ITable table;

            ILayer layer = geoFeatureLayer as ILayer;
            table = layer as ITable;
            tableHistogram = (ITableHistogram)new BasicTableHistogram();
            //按照 数值字段分级
            tableHistogram.Table = table;
            tableHistogram.Field = ClassField;
            basicHistogram = tableHistogram as IBasicHistogram;
            object values;
            object frequencys;
            //先统计每个值和各个值出现的次数
            basicHistogram.GetHistogram(out values, out frequencys);
            //创建平均分级对象
            IClassifyGEN classifyGEN = new Quantile();
            //用统计结果进行分级 ，级别数目为classCount
            classifyGEN.Classify(values, frequencys, ref classCount);
            //获得分级结果,是个 双精度类型数组 
            double[] classes;
            classes = classifyGEN.ClassBreaks as double[];
            //定义不同等级渲染的色带用色
            IEnumColors enumColors = CreateRandomColorRamp(classes.Length).Colors;
            IColor color;
            IClassBreaksRenderer classBreaksRenderer = new ClassBreaksRenderer();
            classBreaksRenderer.Field = ClassField;
            classBreaksRenderer.BreakCount = classCount;//分级数目
            classBreaksRenderer.SortClassesAscending = true;//定义分类是否在TOC中显示Legend

            ISimpleFillSymbol simpleFillSymbol;
            for (int i = 0; i < classes.Length - 1; i++)
            {
                color = enumColors.Next();
                simpleFillSymbol = new SimpleFillSymbol();
                simpleFillSymbol.Color = color;
                simpleFillSymbol.Style = FillStyle;

                classBreaksRenderer.set_Symbol(i, simpleFillSymbol as ISymbol);
                classBreaksRenderer.set_Break(i, classes[i]);
            }
            if (geoFeatureLayer != null)
            {
                geoFeatureLayer.Renderer = classBreaksRenderer as IFeatureRenderer;
            }
        }
        public IColorRamp CreateRandomColorRamp(int count)
        {
            //IUniqueValueRenderer pUniqueValueR;  
            IEnumColors pEnumRamp;
            IRandomColorRamp pColorRamp;
            //pUniqueValueR = new UniqueValueRendererClass();  
            //pUniqueValueR.FieldCount = 1;  
            //pUniqueValueR.set_Field(0, FielName);  

            pColorRamp = new RandomColorRamp();
            pColorRamp.StartHue = 0;
            pColorRamp.MinValue = 99;
            pColorRamp.MinSaturation = 15;
            pColorRamp.EndHue = 360;
            pColorRamp.MaxValue = 100;
            pColorRamp.MaxSaturation = 30;
            pColorRamp.Size = count * 2;

            bool ok = true;
            pColorRamp.CreateRamp(out ok);
            pEnumRamp = pColorRamp.Colors;
            return pColorRamp;
        }
    }
}