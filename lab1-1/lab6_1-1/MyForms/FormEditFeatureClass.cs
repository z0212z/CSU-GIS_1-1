using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using lab4_1_1.AOhelper1_1;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace lab4_1_1.MyForms
{

    public partial class FormEditFeatureClass : Form
    {
        IFeatureClass featureClass;
        IFeatureLayer layer;
        public IFeatureClass FeatureClass { get => this.featureClass; }

        public string Folder { get => this.textFolder.Text; }
        public string ShpFileName { get => this.textShp.Text; }
        public FormEditFeatureClass(IFeatureLayer layer)
        {
            InitializeComponent();
            this.layer = layer;
            this.featureClass = layer.FeatureClass;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void FormEditFeatureClass_Load(object sender, EventArgs e)
        {
            this.setFeatureLayer();
        }

        private void setFeatureLayer()
        {
            try
            {
                this.textShp.Text = LayerHelper.GetDataSource(layer);

                ISpatialReference sr = LayerHelper.GetSpatialReference(layer);
                this.textSR.Text = sr.Name;
                this.textGeometryType.Text = layer.FeatureClass.ShapeType.ToString();
                this.DisplayFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("[异常]" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayFields()
        {
            IFields fields = featureClass.Fields;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                this.dgvFields.Rows.Add(
                    fields.Field[i].Name
                    , fields.Field[i].AliasName
                    , FieldHelper.TypeToHZ(fields.Field[i].Type)
                    , fields.Field[i].Length);

                this.dgvFields.Rows[i].ReadOnly = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            IFieldsEdit myfields = new Fields() as IFieldsEdit;
            foreach (DataGridViewRow row in this.dgvFields.Rows)
            {
                if (row.ReadOnly)
                    continue;
                if (row.Cells[0].Value == null
                    || string.IsNullOrEmpty(row.Cells[0].Value.ToString())
                    || row.Cells[2].Value == null
                    || string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                    continue;

                IFieldEdit field = new Field() as IFieldEdit;
                field.Name_2 = row.Cells[0].Value.ToString();

                if (row.Cells[1].Value == null || string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                    field.AliasName_2 = field.Name;
                else
                    field.AliasName_2 = row.Cells[1].Value.ToString().Trim();

                field.Type_2 = Util.ToFieldType(row.Cells[2].Value.ToString());

                if (field.Type == esriFieldType.esriFieldTypeString)
                {
                    if (row.Cells[3].Value == null)
                        field.Length_2 = 50;
                    else if (string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                        field.Length_2 = 50;
                    else
                    {
                        int len = 50;
                        if (!int.TryParse(row.Cells[3].Value.ToString(), out len))
                            len = 50;
                        field.Length_2 = len;
                    }
                }
                myfields.AddField(field);
            }
            try
            {
                for (int i = 0; i < myfields.FieldCount; i++)
                    this.featureClass.AddField(myfields.Field[i]);
                for (int i = 0; i < fields_del.FieldCount; i++)
                    this.featureClass.DeleteField(fields_del.Field[i]);

                MessageBox.Show("字段编辑完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("[异常]" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        IFieldsEdit fields_del = new Fields() as IFieldsEdit;

        private void btnDelFeild_Click(object sender, EventArgs e)
        {
            DataGridViewRow row;
            for (int i = 0; i < this.dgvFields.SelectedRows.Count; i++)
            {
                row = this.dgvFields.SelectedRows[i];
                if (row.ReadOnly)
                {
                    IField field = featureClass.Fields.Field[row.Index];
                    Debug.WriteLine(string.Format("Name:{0},Type:{1}", field.Name, field.Type.ToString()));

                    if (field.Type != esriFieldType.esriFieldTypeOID && field.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        fields_del.AddField(field);
                        this.dgvFields.Rows.Remove(row);
                    }
                }

                else
                    this.dgvFields.Rows.Remove(row);
            }
        }
    }
}
