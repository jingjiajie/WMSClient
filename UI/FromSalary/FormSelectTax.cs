using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FromSalary
{
    public partial class FormSelectTax : Form
    {
        public FormSelectTax(int payNoteId,string payNoteNo="adadadadada")
        {
            InitializeComponent();
            this.CenterToScreen();
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.AddStaticCondition("payNoteId", payNoteId);
            this.searchView1.Search();
            int a = this.model1.RowCount;
            if (this.model1.RowCount == 0)
            {
                this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "payNoteId",payNoteId},
                { "no",payNoteNo}
            });
            }
            FrontWork.Range[] range = new FrontWork.Range[] { new FrontWork.Range(0,0,1,1)};
            this.model1.SetSelectionRanges(range);        
        }

        private void buttonADD_Click(object sender, EventArgs e)
        {
            int a = this.model1.RowCount;
            if (this.synchronizer.Save())
            {
                MessageBox.Show("选择税务成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else { MessageBox.Show("选择税务失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }
    }
}
