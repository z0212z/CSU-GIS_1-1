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
        #region ˽�б���
        /// <summary>
        /// ��ǰѡ�е�ͼ��
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

        private void �ļ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ����sToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// �˳�
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
        /// ������ʾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMap_OnMouseMove(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            this.tslCoor.Text = string.Format("x={0},y={1}", e.mapX.ToString("0.00"), e.mapY.ToString("0.00"));
        }

        #region ���˵�ͼ��
        /// <summary>
        /// ���ȫ��shp����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerAllShp_Click(object sender, EventArgs e)
        {
            this.LayerAllShp();
        }

        /// <summary>
        /// ��ӵ���shp����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayerAddShp();
        }

        /// <summary>
        /// �Ƴ�ͼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerRemove_Click(object sender, EventArgs e)
        {
            this.removeLayer();
        }

        /// <summary>
        /// ���ÿ�ѡ״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerSelectable_Click(object sender, EventArgs e)
        {
            this.LayerSelectable();
        }

        /// <summary>
        /// ���ÿ��ӻ�״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerVisible_Click(object sender, EventArgs e)
        {
            this.LayerVisable();
        }

        /// <summary>
        /// ��ӵ�ӥ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLayerThum_Click(object sender, EventArgs e)
        {
            this.LayertoThum();
        }
        #endregion


        #region ��ͼ�ĵ�����
        /// <summary>
        /// ��ͼ�ĵ��Ĵ���
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
        /// ���ĵ�
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
        /// �����ĵ�
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


        #region ����ͼ������
        /// <summary>
        /// ����ָ��Ŀ¼������shp�ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerAllShp_Click(object sender, EventArgs e)
        {
            this.LayerAllShp();
        }

        /// <summary>
        /// ��ӵ���shp�ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerAddShp_Click(object sender, EventArgs e)
        {
            this.LayerAddShp();
        }

        /// <summary>
        /// TOC��굯���¼�����ȡ��ǰѡ�е�ͼ��
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
        /// �Ƴ�ͼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerRemove_Click(object sender, EventArgs e)
        {
            this.removeLayer();
        }

        /// <summary>
        /// ���ÿ�ѡ״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerSelectable_Click(object sender, EventArgs e)
        {
            this.LayerSelectable();
        }

        /// <summary>
        /// ���ÿ��ӻ�״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerVisible_Click(object sender, EventArgs e)
        {
            this.LayerVisable();
        }

        /// <summary>
        /// ��ӵ�ӥ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbLayerThum_Click(object sender, EventArgs e)
        {
            this.LayertoThum();
        }
        #endregion


        #region ˽�к���

        /// <summary>
        /// ����ָ��Ŀ¼������shp�ļ�
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
            this.tslMain.Text = " ��ʾ����ǰ���ˡ�" + folder + "��Ŀ¼�µ�����SHP.";
        }

        /// <summary>
        /// ��ӵ���shp�ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerAddShp()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "shp�ļ�|*.shp|ȫ���ļ�|*.*";
            ofd.Title = "��SHP�ļ�";
            ofd.InitialDirectory = @"D:\temp\csu";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            //��ȡ��ǰѡ�е�SHP�ļ�
            string shpfile = ofd.FileName;
            //����System.IO�����ռ��е�FileInfo���󣬻�ȡ�ļ�ȫ���е�Ŀ¼·�����ļ�����
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
        /// ��ǰ�Ƿ�ѡ����ͼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private bool isSelectedLayer()
        {
            if (this.selectedLayer == null)
            {
                MessageBox.Show("��ǰû��ѡ��ͼ�㣡", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }


        /// <summary>
        /// �Ƴ�ͼ��
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
        /// ͼ���ѡ״̬
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
        /// ���ÿ��ӻ�״̬
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
        /// ��ӵ�ӥ��
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


        #region TOC��ݲ˵�

        /// <summary>
        /// ����ͼ��
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
        /// ����ͼ��
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
        /// �Ƴ�ͼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmRemove_Click(object sender, EventArgs e)
        {
            this.removeLayer();
        }

        /// <summary>
        /// ���ÿ�ѡ״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmSelectable_Click(object sender, EventArgs e)
        {
            this.LayerSelectable();
        }

        /// <summary>
        /// ���ÿ��ӻ�״̬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmVisible_Click(object sender, EventArgs e)
        {
            this.LayerVisable();
        }

        /// <summary>
        /// ��ӵ�ӥ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmThum_Click(object sender, EventArgs e)
        {
            this.LayertoThum();
        }


        #endregion

        /// <summary>
        /// ӥ�۵�ͼ�л��ƺ�ɫͼ��
        /// </summary>
        IEnvelope ext;
        private void axMap_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            //��ȡ��ǰ����ͼ����ʾ��Χ
            this.ext = this.axMap.Extent;
            //����������
            IFillSymbol fillSymbol = new SimpleFillSymbol();
            //���������ɫΪ͸��
            IRgbColor color = new RgbColor();
            color.NullColor = true;
            color.Transparency = 0;
            fillSymbol.Color = color;

            //���������ŵı߽��߷���
            ILineSymbol lineSymbol = new SimpleLineSymbol();
            IRgbColor lineColor = new RgbColor();
            lineColor.Red = 255;
            lineSymbol.Color = lineColor;
            fillSymbol.Outline = lineSymbol;

            //����ͼԪ
            IElement element = new RectangleElementClass();
            element.Geometry = ext;
            IFillShapeElement fillElement = (IFillShapeElement)element;
            //����ͼԪ�ķ���
            fillElement.Symbol = fillSymbol;

            this.axThum.ActiveView.GraphicsContainer.DeleteAllElements();
            this.axThum.ActiveView.GraphicsContainer.AddElement(element, 0);

            this.axThum.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }


        private void contextToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ����ʹ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuUseHelp_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ����ϵͳ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuAboutsystem_Click(object sender, EventArgs e)
        {

        }

        private void Ҫ�������ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ���Ҫ��
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
        /// ����Ҫ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureClassNew_Click(object sender, EventArgs e)
        {
            MyForms.FormNewFeatureClass frm = new MyForms.FormNewFeatureClass();
            //��������ɹ������´�����Ҫ������ص���ͼ��
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show(string.Format("[{0}\\{1}] �����ɹ�.\n �Ƿ�������ݣ�"
                , frm.textFolder, frm.textShp)
                , "��ʾ"
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
        /// ��ѡ�༭
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
        /// ��ѡ�༭
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
        /// ��ѡ�༭
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
        /// ��ѡɾ��
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
        /// ��ѡɾ��
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
        /// �����ѡ��ɾ��
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
        /// ���
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

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tlbFeatureBrowse_Click(sender, e);
        }

        /// <summary>
        /// ��Ϣ
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
        /// �༭Ҫ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbFeatureClassEdit_Click(object sender, EventArgs e)
        {
            if (this.selectedLayer == null)
            {
                MessageBox.Show("��ǰû��ѡ��ͼ�㣡", "��ʾ");
                return;
            }

            if (!(this.selectedLayer is IFeatureLayer))
            {
                MessageBox.Show("��ǰѡ�е�ͼ�㲻��ʸ������ͼ�㣡", "��ʾ");
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
        /// ɾ��Ҫ����
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
                MessageBox.Show("��ǰû��ѡ��ͼ�㣡", "��ʾ");
                return;
            }

            if (!(this.selectedLayer is IFeatureLayer))
            {
                MessageBox.Show("��ǰѡ�е�ͼ�㲻��ʸ������ͼ�㣡", "��ʾ");
                return;
            }

            if (MessageBox.Show("ȷ��Ҫɾ��ѡ��ͼ���Ҫ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            lab4_1_1.AOhelper1_1.FeatureClass.DeleteFeatureClass(((IFeatureLayer)this.selectedLayer).FeatureClass);
            MessageBox.Show("Ҫ����ɾ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                IGeometry pGeometry = axMap.TrackRectangle();   //��ȡ����ͼ��Χ
                ISelectionEnvironment pSelectionEnv = new SelectionEnvironment(); //�½�ѡ�񻷾�
                IRgbColor pColor = new RgbColor();
                pColor.Red = 232;                       //����������ʾ����ɫ
                pSelectionEnv.DefaultColor = pColor;     //���ø�����ʾ����ɫ
                pMap.SelectByShape(pGeometry, pSelectionEnv, false);  //ѡ��ͼ��SelectByShape����
                axMap.Refresh(esriViewDrawPhase.esriViewGeoSelection, null, null); //ˢ��ͼ��
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


        #region ��·���ݴ���

        /// <summary>
        /// ȥ�����ߵ�·
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
        /// ��·��
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
                IQueryFilter qf = new QueryFilterClass { WhereClause = "Name='�������'" };
                IFeatureCursor cursor = fc.Search(qf, true);
                IFeature feat = cursor.NextFeature();
                if (feat == null)
                {
                    Debug.WriteLine("û����Ϊ[�������]�ĵ�·", "��ʾ");
                    return;
                }

                IPolyline line = (IPolyline)feat.Shape;
                IPointCollection pc = (IPointCollection)line;

                //����
                Debug.WriteLine(">>����ǰ��������:" + pc.PointCount.ToString());
                line.Generalize(5);//�򻯳̶�
                pc = (IPointCollection)line;
                //����
                Debug.WriteLine(">>�򻯺󶥵�����:" + pc.PointCount.ToString());
                feat.Store();//���

                #region ���Ƽ򻯺����
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
        /// �ӳ����
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
                IQueryFilter qf = new QueryFilterClass { WhereClause = "Name='�������'" };
                IFeatureCursor cursor = fc.Search(qf, true);
                IFeature feat_a = cursor.NextFeature();
                if (feat_a == null)
                {
                    Debug.WriteLine("û����Ϊ[�������]�ĵ�·", "��ʾ");
                    return;

                }

                this.tslMain.Text = "�����ӳ���������ܱߵĵ�·...";
                this.tslProgress.Value = 0;
                IPolyline line_a = (IPolyline)feat_a.Shape;
                IPolygon buffer = ((ITopologicalOperator)line_a).Buffer(5.0) as IPolygon;


                #region ����·�Ļ�����
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


                #region ��ѯ�뻺�����ཻ����ҲĿ�����ཻ�ĵ�·
                ISpatialFilter sf = new SpatialFilterClass { WhereClause = "FID<>" + feat_a.OID.ToString() };
                sf.Geometry = buffer;
                sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                this.tslProgress.Maximum = fc.FeatureCount(sf);
                this.statusBar.Refresh();
                IRelationalOperator ro = line_a as IRelationalOperator;
                IConstructCurve curve = new PolylineClass();
                bool isPerformed = false;
                cursor = fc.Search(sf, true);//������ѯ���
                IFeature feat_b;
                while ((feat_b = cursor.NextFeature()) != null)
                {
                    if (ro.Disjoint(feat_b.Shape) == true)
                    {
                        //��ʾ�ӳ�ǰ�ĳ���
                        IPolyline line_b = feat_b.Shape as IPolyline;
                        Debug.WriteLine(string.Format(" >>[FID={0}] �ӳ�ǰ����:{1}", feat_b.OID, line_b.Length));

                        curve.ConstructExtended(
                          line_b as ICurve
                        , line_a as ICurve
                        , (int)esriCurveExtension.esriDefaultCurveExtension, ref isPerformed);
                        if (isPerformed)
                        {
                            //�����ӳ�����ߣ����ڲ���
                            ILineElement le = new LineElementClass();
                            linesym.Style = esriSimpleLineStyle.esriSLSDash;
                            color = new RgbColorClass { Red = 255, Blue = 0, Green = 0 };
                            linesym.Color = color;
                            le.Symbol = linesym;
                            ((IElement)le).Geometry = curve as IPolyline;
                            this.axMap.ActiveView.GraphicsContainer.AddElement((IElement)le, 0);
                            this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                            //��ʾ�ӳ���ĳ��ȡ���............... ..... ...
                            Debug.WriteLine(string.Format(" ## [FID={0}] �ӳ��󳤶�:{1}", feat_a.OID, ((IPolyline)curve).Length));
                            //-----�����Խ�����-------------------------------
                            //����Ҫ����ӳ���ĵ�·,������
                        }
                        line_b = curve as IPolyline;
                        feat_b.Shape = line_b;
                        feat_b.Store();
                    }
                    this.tslProgress.Value++;
                    this.statusBar.Refresh();
                }
                #endregion
                this.tslMain.Text = " ����.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>Exception:" + ex.Message);
                this.tslMain.Text = " �ӳ���·ʱ����.";
            }
        }

        /// <summary>
        /// ·�ڴ��
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
                IQueryFilter qf = new QueryFilterClass { WhereClause = "Name='��ˮ·'" };
                IFeatureCursor cursor = fc.Search(qf, true);
                IFeature feat_a = cursor.NextFeature();
                if (feat_a == null)
                {
                    Debug.WriteLine("û����Ϊ[��ˮ·]�ĵ�·", "��ʾ");
                    return;

                }

                this.tslMain.Text = "���ڴ����·ë��...";
                this.tslProgress.Value = 0;

                #region ��ѯ����ˮ·�ཻ�ĵ�·
                ISpatialFilter sf = new SpatialFilterClass { WhereClause = "FID<>" + feat_a.OID.ToString() }; sf.Geometry = feat_a.Shape;
                sf.SpatialRel = esriSpatialRelEnum.esriSpatialRelCrosses;
                ITopologicalOperator topo = feat_a.Shape as ITopologicalOperator;
                cursor = fc.Search(sf, true);
                IFeature feat_b;

                //������ѯ���
                while ((feat_b = cursor.NextFeature()) != null)
                {
                    //����������·�Ľ���
                    IGeometry resultGeom = (IGeometry)topo.Intersect(feat_b.Shape, esriGeometryDimension.esriGeometry0Dimension);
                    IGeometryCollection pc = (IGeometryCollection)resultGeom;
                    if (pc.GeometryCount == 0)
                    {
                        Debug.WriteLine(" >>>�����·[FID=" + feat_b.OID.ToString() + "]û�н���!");
                        continue;
                    }
                    else
                        Debug.WriteLine(" >>>�����·[FID=" + feat_b.OID.ToString() + "]�ཻ!");
                    //ȡ��һ�����㣬��ʹ�ж�㣬Ҳ���������
                    IPoint pt = pc.Geometry[0] as IPoint;
                    #region����ʾ����,���ڲ���
                    ISimpleMarkerSymbol sym = new SimpleMarkerSymbolClass();
                    sym.Style = esriSimpleMarkerStyle.esriSMSSquare;
                    sym.Size = 5;
                    IMarkerElement me = new MarkerElementClass { Geometry = pt };
                    me.Symbol = sym;
                    this.axMap.ActiveView.GraphicsContainer.AddElement((IElement)me, 0);
                    this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    #endregion

                    //ʹ��IFeatureEdit�ӿڷָ�Ҫ��b���ָ��Ժ��ֱ�Ӹ���Ҫ�����У���Ҫע��!!!!
                    IFeatureEdit featureEdit = feat_b as IFeatureEdit2;
                    //�ָ�Ҫ��b�����ָ���Ҫ�ؼ�����set
                    ISet set = featureEdit.Split(pt);
                    //���ʷָ��Ľ�����������п��Լ���ĸ�Ҫ��С�ڸ�����ֵ��Ȼ����ɾ��
                    if (set != null)
                    {
                        set.Reset();
                        if (set.Count > 1)
                        {
                            //--���ָ��������ڵ�ͼ���Ա����--------------------------
                            IFeature newFeat = set.Next() as IFeature;
                            IPolyline pl_b = newFeat.Shape as IPolyline;
                            ILineElement le = new LineElementClass { Geometry = pl_b };
                            this.axMap.ActiveView.GraphicsContainer.AddElement((IElement)le, 0);
                            this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                            //--���Դ������--------------------------
                            newFeat.Delete();
                        }

                    }
                    this.tslProgress.Value++;
                    this.statusBar.Refresh();
                }
                #endregion
                this.tslMain.Text = " ����.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>Exception:" + ex.Message);
                this.tslMain.Text = " �����·ë��ʱ����.";
            }
        }
        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ����EDMToolStripMenuItem_Click(object sender, EventArgs e)
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
                MessageBox.Show("��Ϊ��ͼ��ӵ�״ʸ��ͼ��.", "��ʾ");
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
            ofd.Filter = "tif�ļ�|*.tif|ȫ���ļ�|*.*";
            ofd.Title = "��tif�ļ�";
            ofd.InitialDirectory = @"D:\temp\csu";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            //��ȡ��ǰѡ�е�SHP�ļ�
            string tiffile = ofd.FileName;
            //����System.IO�����ռ��е�FileInfo���󣬻�ȡ�ļ�ȫ���е�Ŀ¼·�����ļ�����
            FileInfo fi = new FileInfo(tiffile);
            string folder = fi.Directory.FullName;
            string fileName = fi.Name;

            IWorkspaceFactory wsf = new RasterWorkspaceFactoryClass();
            IRasterWorkspace rws = wsf.OpenFromFile(folder, 0) as IRasterWorkspace;
            IRasterDataset rd = (IRasterDataset)rws.OpenRasterDataset(fileName);
            IRasterLayer rl = new RasterLayerClass();//����һ��ʸ��ͼ�����
            rl.CreateFromDataset(rd);//����ʸ��ͼ�����ȥ������Ӧ��raster�ļ�
            this.axMap.AddLayer(rl);
        }

        private void ��ѡToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.SelectFeatureByLocation;
        }

        private void ��ѡToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.SelectFeatureByPolyline;
        }

        private void �����ѡToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.isSelectedLayer())
                return;
            this.mapOp = MapOperatorType.SelectFeatureByPolygon;
        }

        private void ��ѡToolStripMenuItem1_Click(object sender, EventArgs e)
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
                    //ȡ��ͼ��
                    ILegendClass pLegendClass = ((ILegendGroup)oLegendGroup).get_Class((int)oIndex);
                    //��������ѡ����SymbolSelectorʵ��
                    FormSymbolSelection SymbolSelectorFrm = new FormSymbolSelection(pLegendClass, pLayer);
                    if (SymbolSelectorFrm.ShowDialog() == DialogResult.OK)
                    {
                        //�ֲ�������Map�ؼ�
                        this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                        //�����µķ���
                        pLegendClass.Symbol = SymbolSelectorFrm.pSymbol;
                        //������Map�ؼ���ͼ��ؼ�
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
                render.CreateUniqueValueRenderer(((IFeatureLayer)pLayer).FeatureClass, "FID");//�Ѿ�д��FID��ΪΨһֵ�����Ż�

            IGeoFeatureLayer geoLayer = pLayer as IGeoFeatureLayer;
            geoLayer.Renderer = ur as IFeatureRenderer;

            //createUniqueValueRander(geoLayer, "FID", esriSimpleFillStyle.esriSFSNull, 34);
            this.axMap.Refresh();

        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmCalculateArea_Click(object sender, EventArgs e)
        {
            try
            {
                IFeatureClass fc = ((IFeatureLayer)this.axMap.get_Layer(0)).FeatureClass;
                int n = fc.FeatureCount(null);
                //Ѱ�ҽ�������Area�ֶΣ����û�о����
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

                this.tslMain.Text = " ��ʼ���ý��������...";
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
                MessageBox.Show("������ɣ�ɾ����Ҫ��" + Area0Num + "��", "��ʾ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message
                        , "�쳣����"
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Error);
            }
            finally
            {
                this.tslMain.Text = " ����.";
            }
        }

        /// <summary>
        /// ��ʶ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmIdentifyArea_Click(object sender, EventArgs e)
        {
            // ������ɫ
            IRgbColor pRgbColor = new RgbColor();
            pRgbColor.Red = 0;
            pRgbColor.Green = 0;
            pRgbColor.Blue = 255;

            // ��������
            IFontDisp pFontDisp = new StdFont() as IFontDisp;
            pFontDisp.Bold = true;
            pFontDisp.Size = 8;

            // ��������
            ITextSymbol pTextSymbol = new TextSymbol();
            pTextSymbol.Angle = 0;
            pTextSymbol.Color = pRgbColor;
            pTextSymbol.Font = pFontDisp;

            // ɾ�������ı�Ԫ��
            IActiveView pActiveView = axMap.ActiveView;
            IGraphicsContainer pGraphicsContainer = pActiveView.GraphicsContainer;
            pGraphicsContainer.DeleteAllElements();

            // ��ȡҪ���α�
            IFeatureLayer pFeatureLayer = axMap.get_Layer(0) as IFeatureLayer;
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, true);
            IFeature pFeature = pFeatureCursor.NextFeature();

            // ����Ҫ���α�
            int fieldIndex = pFeatureClass.Fields.FindField("FID");//��Name�ֶο����滻FID
            while (pFeature != null)
            {
                string label;

                IPointCollection pc = (IPointCollection)pFeature.Shape;
                // ��ȡ����
                IArea pArea = (IArea)pFeature.Shape;
                IPoint pPoint = pc.Point[0];//pArea.Centroid;��ֵΪ������ݻᱨ��

                label = pFeature.get_Value(fieldIndex).ToString() + "\n" + pArea.Area.ToString("0.00");
                // �����ı�Ԫ��
                ITextElement pTextElement = new TextElement() as ITextElement;
                pTextElement.Symbol = pTextSymbol;
                pTextElement.Text = label;

                // ����ı�Ԫ��
                IElement pElement = pTextElement as IElement;
                pElement.Geometry = pPoint;
                pGraphicsContainer.AddElement(pElement, 0);
                pFeature = pFeatureCursor.NextFeature();
            }

            // ˢ�µ�ͼ
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
                    tslMain.Text = "��ǰ�������ǵ�ͼ��" + pBasicMap.Name + "  ��ͼ��ͼ����Ϊ��" + pBasicMap.LayerCount.ToString();
                }
                else if (pTocItem == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    tslMain.Text = "��ǰ������ͼ�㣺" + pLayer.Name;
                }
                else if (pTocItem == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    tslMain.Text = "��ǰ������ͼ�����,ͼ�����ƣ�" + pLayer.Name;
                }
                else if (pTocItem == esriTOCControlItem.esriTOCControlItemNone)
                {
                    tslMain.Text = "��ǰ����Ϊ�հ�����";
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
            ITableHistogram tableHistogram;//���ֱ��ͼ
            IBasicHistogram basicHistogram;//Provides access to members that control histogram objects created from different data sources. 

            ITable table;

            ILayer layer = geoFeatureLayer as ILayer;
            table = layer as ITable;
            tableHistogram = (ITableHistogram)new BasicTableHistogram();
            //���� ��ֵ�ֶηּ�
            tableHistogram.Table = table;
            tableHistogram.Field = ClassField;
            basicHistogram = tableHistogram as IBasicHistogram;
            object values;
            object frequencys;
            //��ͳ��ÿ��ֵ�͸���ֵ���ֵĴ���
            basicHistogram.GetHistogram(out values, out frequencys);
            //����ƽ���ּ�����
            IClassifyGEN classifyGEN = new Quantile();
            //��ͳ�ƽ�����зּ� ��������ĿΪclassCount
            classifyGEN.Classify(values, frequencys, ref classCount);
            //��÷ּ����,�Ǹ� ˫������������ 
            double[] classes;
            classes = classifyGEN.ClassBreaks as double[];
            //���岻ͬ�ȼ���Ⱦ��ɫ����ɫ
            IEnumColors enumColors = CreateRandomColorRamp(classes.Length).Colors;
            IColor color;
            IClassBreaksRenderer classBreaksRenderer = new ClassBreaksRenderer();
            classBreaksRenderer.Field = ClassField;
            classBreaksRenderer.BreakCount = classCount;//�ּ���Ŀ
            classBreaksRenderer.SortClassesAscending = true;//��������Ƿ���TOC����ʾLegend

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