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
    public partial class FormPayNoteTax : Form
    {
        private int payNoteId;
        public FormPayNoteTax(int payNoteId)
        {
            InitializeComponent();
            this.payNoteId = payNoteId;
        }

        private void FormPayNoteTax_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.searchView1.AddStaticCondition("payNoteId",payNoteId);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "payNoteId",payNoteId},                           
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }


        private void TaxNameEditEnded(int row, string taxName)
        {
            IDictionary<string, object> foundTax =
                GlobalData.AllTax.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == taxName;
                });
            if (foundTax == null)
            {
                MessageBox.Show($"税务\"{taxName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "taxNo"] = foundTax["no"];
                this.model1[row, "taxId"] = foundTax["id"];
            }
        }

        private void TaxNoEditEnded(int row, string taxNo)
        {
            IDictionary<string, object> foundTax =
                GlobalData.AllTax.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == taxNo;
                });
            if (foundTax == null)
            {
                MessageBox.Show($"税务\"{taxNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "taxName"] = foundTax["name"];
                this.model1[row, "taxId"] = foundTax["id"];
            }
        }
    }
}
