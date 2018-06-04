using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace WMS.UI
{
    public partial class FormInspectionNoteChoosePreviewType : Form
    {
        private int[] inspectionNoteIDs = null;
        public FormInspectionNoteChoosePreviewType(int[] inspectionNoteIDs)
        {
            InitializeComponent();
            this.inspectionNoteIDs = inspectionNoteIDs;
        }

        private void FormInspectionNoteChoosePreviewType_Load(object sender, EventArgs e)
        {
            Utilities.BindBlueButton(this.buttonAll);
            Utilities.BindBlueButton(this.buttonQualified);
        }

        private void buttonQualified_Click(object sender, EventArgs e)
        {
            this.ShowPreview(true);
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            this.ShowPreview(false);
        }

        private void ShowPreview(bool qualifiedOnly)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(inspectionNoteIDs);
            var previewData = RestClient.Get<List<IDictionary<string, object>>>(Defines.ServerURL + "/warehouse/WMS_Template/inspection_note/preview/" + (qualifiedOnly ? "qualified/" : "") + strIDs);            if (previewData == null) return;
            StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("送检单预览" + (qualifiedOnly ? " - 仅合格条目" : ""));
            foreach (IDictionary<string, object> noteAndItem in previewData)
            {
                IDictionary<string, object> inspectionNote = (IDictionary<string, object>)noteAndItem["inspectionNote"];
                object[] inspectionNoteItems = (object[])noteAndItem["inspectionNoteItems"];
                string no = (string)inspectionNote["no"];
                if (!formPreviewExcel.AddPatternTable("Excel/InspectionNote.xlsx", no)) return;
                formPreviewExcel.AddData("inspectionNote", inspectionNote, no);
                formPreviewExcel.AddData("inspectionNoteItems", inspectionNoteItems, no);
            }
            formPreviewExcel.Show();
            this.Close();
        }
    }
}
