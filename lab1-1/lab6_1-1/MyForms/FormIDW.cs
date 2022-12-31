using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_1_1.MyForms
{
    public partial class FormIDW : Form
    {
        IFeatureLayer layer;
        AxMapControl axMap;
        public FormIDW(AxMapControl axMap)
        {
            this.axMap = axMap;
            this.layer = axMap.get_Layer(0) as IFeatureLayer;
            InitializeComponent();
        }
        public FormIDW(AxMapControl axMap,ILayer layer)
        {
            this.axMap = axMap;
            this.layer = layer as IFeatureLayer;
            InitializeComponent();
        }
        private void GP_ToolExecuted(object sender,ToolExecutedEventArgs e)
        {
            IGeoProcessorResult2 gpResult = (IGeoProcessorResult2)e.GPResult;
            Debug.WriteLine(gpResult.ReturnValue);

            this.timer1.Enabled = false;
            this.tslprogress.Value = this.tslprogress.Maximum;

            if(gpResult.MessageCount>0)
            {
                for(int i=0;i<gpResult.MessageCount;i++)
                {
                    ListView_Result.Items.Add(new ListViewItem(
                        new string[2] { "消息" + (i + 1).ToString(), gpResult.GetMessage(i) }
                        , "information"));
                    Debug.WriteLine(string.Format("@@消息{0}:{1}", i + 1, gpResult.GetMessage(i)));
                }
                ListView_Result.Refresh();
            }
            if(gpResult.Status==ESRI.ArcGIS.esriSystem.esriJobStatus.esriJobSucceeded)
            {
                ListView_Result.Items.Add(new ListViewItem(
                        new string[2] { "[消息]","GP执行成功." }
                        , "success"));
                ListView_Result.Refresh();

                string file = gpResult.ReturnValue.ToString();
                IRasterLayer r1 = new RasterLayerClass();
                r1.CreateFromFilePath(file);
                r1.Name = file.Substring(file.LastIndexOf("\\") + 1);
                this.axMap.AddLayer((ILayer)r1, 1);
                this.axMap.ActiveView.Refresh();
            }
            else
            {
                ListView_Result.Items.Add(new ListViewItem(
                        new string[2] { "[消息]", "GP执行失败." }
                        , "error"));
                ListView_Result.Refresh();
                MessageBox.Show("执行失败！", "错误");
            }

            this.tslTip.Text = "就绪";
        }
        private void GP_MessageCreated(object sender, MessagesCreatedEventArgs e)
        {
            Debug.WriteLine(">>>GP产生了消息.");
            IGPMessages gpMsgs = (IGPMessages)e.GPMessages;

            if (gpMsgs.Count > 0)
            {
                for(int count=0;count<gpMsgs.Count;count++)
                {
                    IGPMessage msg = gpMsgs.GetMessage(count);
                    string imageToShow = "information";

                    //switch (msg.Type) { };
                    ListView_Result.Items.Add(new ListViewItem(
                        new string[2] { "MessageCreated", msg.Description }
                        , imageToShow));
                }
            }

        }
        private void GP_ProgressChanged(object sender, ESRI.ArcGIS.Geoprocessor.ProgressChangedEventArgs e)
        {
            Debug.WriteLine(">>>GP进程改变.");
            IGeoProcessorResult2 gpResult = (IGeoProcessorResult2)e.GPResult;

            if(e.ProgressChangedType==ProgressChangedType.Message)
            {
                ListView_Result.Items.Add(new ListViewItem(
                        new string[2] { "progressChanged", e.Message }
                        , "information"));
                ListView_Result.Refresh();

                Debug.WriteLine(string.Format("##消息：{1}", gpResult.Process.Tool.Name, gpResult.Status.ToString()));
            }
        }
        private void GP_ToolExecuting(object sender, ToolExecutingEventArgs e)
        {
            Debug.WriteLine(">>>GP执行中...");
            IGeoProcessorResult2 gpResult = (IGeoProcessorResult2)e.GPResult;
            ListView_Result.Items.Add(new ListViewItem(
                        new string[2] { "ToolExecuting", gpResult.Process.Tool.Name + "." + gpResult.Status.ToString() }
                        , "information" )) ;
            ListView_Result.Refresh();
            Debug.WriteLine(string.Format(">>消息：{1}", gpResult.Process.Tool.Name, gpResult.Status.ToString()));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cmbLayer.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择要搜索的点图层!”，”提示");
                    return;
                }
                if (this.cmbField.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择要分析的要素类字段! ", "提示");
                    return;
                }
                double size = 0;
                if (double.TryParse(this.txtcellsize.Text, out size) == false)
                {
                    MessageBox.Show("单元格大小尺寸值不是合法的数字!", "提示");
                    return;
                }
                int power = 0;
                if (int.TryParse(this.txtpower.Text, out power) == false)
                {
                    MessageBox.Show("幂次数不是合法的数字!", "提示");
                    return;
                }
                if (string.IsNullOrEmpty(this.txtsaveto.Text))
                {
                    MessageBox.Show("请输入要保存的DEM文件!", "提示");
                    return;
                }
                if(File.Exists(this.txtsaveto.Text))
                {
                    if (MessageBox.Show(string.Format("[{0}]文件已存在，是否覆盖？", this.txtsaveto.Text)
                        , "提示"
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                    else
                        File.Delete(this.txtsaveto.Text);
                }
                //string shpPath;
                this.tslTip.Text = "准备调用GP工具...";
                this.statusStrip1.Refresh();
                this.timer1.Enabled = true;
                //this.isGPRunning = true;
                Geoprocessor GP = new Geoprocessor();
                GP.OverwriteOutput = true; //此句无效，则且在文件已存在时会异常
                                           //需引用SpatialAnalystTools扩展，初始化IDW
                ESRI.ArcGIS.SpatialAnalystTools.Idw idw =
                new ESRI.ArcGIS.SpatialAnalystTools.Idw();
                ////输入要素
                idw.in_point_features = this.layer; //选中的图层，或者使用shp文件路径;
                idw.z_field = this.cmbField.Text; //选中的Z值字段名称
                idw.cell_size = size;
                idw.power = power;
                idw.out_raster = this.txtsaveto.Text; //输入的输入路径，如d:\temp\dem.tif
                                                      //同步执行
                GP.ToolExecuted += new EventHandler<ToolExecutedEventArgs>(GP_ToolExecuted);
                GP.MessagesCreated += new EventHandler<MessagesCreatedEventArgs>(GP_MessageCreated);
                GP.ProgressChanged += new EventHandler<ESRI.ArcGIS.Geoprocessor.ProgressChangedEventArgs>(GP_ProgressChanged);
                GP.ToolExecuting += new EventHandler<ToolExecutingEventArgs>(GP_ToolExecuting);
                IGeoProcessorResult result = GP.Execute(idw, null) as IGeoProcessorResult;
                //或者使用异步执行
                //IGeoProcessorResult result = GP.ExecuteAsync(idw) as IGeoProcessorResult;
                ListView_Result.Items.Add(new ListViewItem(
                    new string[2] { "[]消息", "GP已经启动..." }
                    , "information"));
                this.tslTip.Text = "开始生成DEM...";
                this.statusStrip1.Refresh();

            }
            catch(Exception ex)
            {
                MessageBox.Show("[异常]" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "保存DEM文件";
            sfd.DefaultExt = "TIF";
            sfd.Filter = "TIF文件|*.tif";
            sfd.InitialDirectory = @"c:\temp";
            sfd.OverwritePrompt = false;
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            this.txtsaveto.Text = sfd.FileName;
        }

        List<IFeatureLayer> layers;
        List<IFeatureLayer> polygonLayers;
        /// <summary>
        /// 设置图层
        /// </summary>
        private void SetLayers()
        {
            //选择地图中已加载的点状矢量图层加入到列表中
            this.layers = new List<IFeatureLayer>();
            this.polygonLayers = new List<IFeatureLayer>();
            for (int i = 0; i < this.axMap.LayerCount; i++)
            {
                if (this.axMap.get_Layer(i) is IFeatureLayer)
                {
                    IFeatureLayer fl = this.axMap.get_Layer(i) as IFeatureLayer;
                    if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        this.layers.Add(fl);
                        this.cmbLayer.Items.Add(fl.Name);
                    }
                }
            }
            if (layers.Count == 0)
            {
                MessageBox.Show("请为地图添加点状矢量图层.", "提示");
                this.Close();
            }
        }
        /// <summary>
        /// 目标图层更改时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param 
        private void cmbLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmbField.Items.Clear();
                //记录选中的图层
                this.layer = this.layers[this.cmbLayer.SelectedIndex];
                IFeatureClass featClass = layer.FeatureClass;
                for (int i = 0; i < featClass.Fields.FieldCount; i++)
                {
                    //忽略非数值型字段
                    if (featClass.Fields.Field[i].Type != esriFieldType.esriFieldTypeSingle
                    && featClass.Fields.Field[i].Type != esriFieldType.esriFieldTypeDouble
                    && featClass.Fields.Field[i].Type != esriFieldType.esriFieldTypeInteger
                    && featClass.Fields.Field[i].Type != esriFieldType.esriFieldTypeSmallInteger)
                        continue;
                    cmbField.Items.Add(featClass.Fields.Field[i].Name);
                }
                this.tslTip.Text = " 点要素数量：" + featClass.FeatureCount(null).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("搜索要素类字段时出错 : " + ex.Message, "错误");
            }
        }

        private void FormIDW_Load(object sender, EventArgs e)
        {
            SetLayers();
        }
    }
}
