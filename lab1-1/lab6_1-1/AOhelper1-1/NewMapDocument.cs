using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4_1_1.AOhelper1_1
{
    /// <summary>
    /// 创建地图工具
    /// </summary>
    public class NewMapDocument : BaseCommand
    {
        private IHookHelper m_hookHelper = null;
        //constructor
        public NewMapDocument()
        {
            //update the base properties
            base.m_category = ".NET Application";
            base.m_caption = "NewDocument";
            base.m_message = "Create a new map";
            base.m_toolTip = "Create a new map";
            base.m_name = "DotNetTemplate_NewDocumentCommand";
        }
        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
        }
        // 点击事件
        public override void OnClick()
        {
            IMapControl3 mapControl = (IMapControl3)m_hookHelper.Hook;
            IEngineEditor engineEditor = new EngineEditorClass();
            if ((engineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
                  && (engineEditor.HasEdits() == true))
            {
                DialogResult result = MessageBox.Show("是否保存当前地图？", "提示",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Cancel) return;
                else if (result == DialogResult.No)
                    engineEditor.StopEditing(false);
                else if (result == DialogResult.Yes)
                {
                    engineEditor.StopEditing(true);
                    //launch the save command
                    ICommand command = new ControlsSaveAsDocCommandClass();
                    command.OnCreate(m_hookHelper.Hook);
                    command.OnClick();
                }
            }
            //create a new Map
            IMap map = new MapClass();
            map.Name = "Map";
            //assign the new map to the MapControl
            mapControl.DocumentFilename = string.Empty;
            mapControl.Map = map;
        }
    }
}
