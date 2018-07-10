using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FromSalary
{
    public partial class FormPayNote : Form
    {
        public FormPayNote()
        {
            MethodListenerContainer.Register("FormPayNote", this);
            InitializeComponent();
        }

        private void FormPayNote_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "createTime",DateTime.Now},
                { "warehouseName",GlobalData.Warehouse["name"]}
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

        private void AaccountTitleExpenseNameEditEnded(int row, string accountTitleName)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == accountTitleName;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }

        private void AaccountTitleExpenseNoEditEnded(int row, string accountTitleNo)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == accountTitleNo;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }

        private void AaccountTitlePayableNameEditEnded(int row, string accountTitleName)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == accountTitleName;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }

        private void AaccountTitlePayableNoEditEnded(int row, string accountTitleNo)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == accountTitleNo;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }

        private void AaccountTitlePropertyNameEditEnded(int row, string accountTitleName)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == accountTitleName;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }

        private void AaccountTitlePropertyNoEditEnded(int row, string accountTitleNo)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["no"] == null) return false;
                    return s["no"].ToString() == accountTitleNo;
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }
    }
}
