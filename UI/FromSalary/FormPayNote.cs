﻿using System;
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

        private void AccountTitleExpenseNameEditEnded([Row]int row, [Data]string accountTitleName)
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
                this.model1[row, "accountTitleExpenseNo"] = foundAccountTitle["no"];
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitleExpenseNoEditEnded([Row]int row, [Data]string accountTitleNo)
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
                this.model1[row, "accountTitleExpenseName"] = foundAccountTitle["name"];
                this.model1[row, "accountTitleExpenseId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitlePayableNameEditEnded([Row]int row, [Data]string accountTitleName)
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
                this.model1[row, "accountTitlePayableNo"] = foundAccountTitle["no"];
                this.model1[row, "accountTitlePayableId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitlePayableNoEditEnded([Row]int row,[Data] string accountTitleNo)
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
                this.model1[row, "accountTitlePayableName"] = foundAccountTitle["name"];
                this.model1[row, "accountTitlePayableId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitlePropertyNameEditEnded([Row]int row,[Data] string accountTitleName)            
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
                this.model1[row, "accountTitlePropertyNo"] = foundAccountTitle["no"];
                this.model1[row, "accountTitlePropertyId"] = foundAccountTitle["id"];
            }
        }

        private void AccountTitlePropertyNoEditEnded([Row]int row,[Data] string accountTitleNo)
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
                this.model1[row, "accountTitlePropertyName"] = foundAccountTitle["name"];
                this.model1[row, "accountTitlePropertyId"] = foundAccountTitle["id"];
            }
        }

        private void SalaryPeriodNameEditEnded([Row]int row,[Data] string periodName)
        {
            IDictionary<string, object> foundSalaryPeriod =
                GlobalData.AllSalaryPeriod.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == periodName;
                });
            if (foundSalaryPeriod == null)
            {
                MessageBox.Show($"期间\"{periodName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "salaryPeriodName"] = foundSalaryPeriod["name"];
                this.model1[row, "salaryPeriodId"] = foundSalaryPeriod["id"];
            }
        }

        private void buttonItems_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项税务条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];       
            FormPayNoteTax form = new FormPayNoteTax((int)rowData["id"],(string)rowData["no"]);
            form.Show();
        }

        private string StateForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "待确认";
                case 1: return "已确认应付";
                case 2: return "已付款实付";
                default: return "未知状态";
            }
        }

        private int StateBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "待确认": return 0;
                case "已确认应付": return 1;
                case "已确认实付": return 2;
                default: return -1;
            }
        }

        private void TaxNameEditEnded([Row]int row, [Data] string taxName)
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

        private void TaxNoEditEnded([Row]int row, [Data] string taxNo)
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.model1.SelectionRange.Rows != 1)
            {
                MessageBox.Show("请选择一项税务条目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var rowData = this.model1.GetRows(new int[] { this.model1.SelectionRange.Row })[0];
            FormPayNoteItem form = new FormPayNoteItem((int)rowData["id"]);
            form.Show();
        }
    }
}
