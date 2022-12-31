using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using stdole;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_1_1.MyForms
{
    public partial class FormLayerLabel : Form
    {
        ILayer pLayer;
        AxMapControl axMap;
        public FormLayerLabel(ILayer layer,AxMapControl axMap)
        {
            this.pLayer = layer;
            this.axMap = axMap;
            InitializeComponent();
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntOK_Click(object sender, EventArgs e)
        {
                // 创建颜色
                IRgbColor pRgbColor = new RgbColor();
                pRgbColor.Red = this.btnColor.BackColor.R;
                pRgbColor.Green = this.btnColor.BackColor.G;
                pRgbColor.Blue = this.btnColor.BackColor.B;

                // 创建字体
                IFontDisp pFontDisp = new StdFont() as IFontDisp;
                pFontDisp.Bold = this.ckbBold.Checked;
                pFontDisp.Italic = this.ckbItalic.Checked;
                pFontDisp.Name = this.cmbFont.Text;
                pFontDisp.Size = this.nudFontSize.Value;

                // 创建符号
                ITextSymbol pTextSymbol = new TextSymbol();
                pTextSymbol.Angle = 0;
                pTextSymbol.Color = pRgbColor;
                pTextSymbol.Font = pFontDisp;

                IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
                pGeoFeatureLayer.AnnotationProperties.Clear();//必须执行，因为里面有一个默认的
                IBasicOverposterLayerProperties pBasic = new BasicOverposterLayerProperties();
                ILabelEngineLayerProperties pLableEngine =
                new LabelEngineLayerProperties() as ILabelEngineLayerProperties;

                string pLable = "[" + (string)cmbField.SelectedItem + "]";
                pLableEngine.Expression = pLable;
                pLableEngine.IsExpressionSimple = true;
                pBasic.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerShape;
                pLableEngine.BasicOverposterLayerProperties = pBasic;
                pLableEngine.Symbol = pTextSymbol;
                pGeoFeatureLayer.AnnotationProperties.Add(pLableEngine as IAnnotateLayerProperties);
                pGeoFeatureLayer.DisplayAnnotation = true;

                this.axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewAll, null, null);
        }

        private void FormLayerLabel_Load(object sender, EventArgs e)
        {
            ITable pTable = pLayer as ITable;
            IField pField = null;
            for(int i=0;i<pTable.Fields.FieldCount;i++)
            {
                pField = pTable.Fields.get_Field(i);
                cmbField.Items.Add(pField.AliasName);
            }
            cmbField.SelectedIndex = 0;
            cmbFont.SelectedIndex = 0;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if(dlg.ShowDialog()==DialogResult.OK)
            {
                this.btnColor.BackColor = dlg.Color;
            }
        }

        private void btnConcel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
