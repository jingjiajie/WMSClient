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
    public partial class FormPersonSalary : Form
    {
        public FormPersonSalary()
        {
            MethodListenerContainer.Register("FormPersonSalary", this);
            InitializeComponent();
        }

        private void FormPersonSalary_Load(object sender, EventArgs e)
        {
            //刷新期间
            this.comboBoxSalaryPeriod.Items.AddRange((from item in GlobalData.AllSalaryPeriod
                                                   select new ComboBoxItem(item["name"]?.ToString(), item)).ToArray());
            if (GlobalData.AllSalaryPeriod == null)
            {
                GlobalData.SalaryPeriod = GlobalData.AllSalaryPeriod[0];
                for (int i = 0; i < this.comboBoxSalaryPeriod.Items.Count; i++)
                {
                    if (GlobalData.AllSalaryPeriod[i] == GlobalData.SalaryPeriod)
                    {
                        this.comboBoxSalaryPeriod.SelectedIndexChanged -= this.comboBoxSalaryPeriod_SelectedIndexChanged;
                        this.comboBoxSalaryPeriod.SelectedIndex = i;
                        this.comboBoxSalaryPeriod.SelectedIndexChanged += this.comboBoxSalaryPeriod_SelectedIndexChanged;
                    }
                }
                this.searchView1.AddStaticCondition("salaryPeriodId", GlobalData.SalaryPeriod["id"]);
            }
                this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private int WarehouseIdDefaultValue()
        {
            return (int)GlobalData.Warehouse["id"];
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "salaryPeriodId",GlobalData.SalaryPeriod["id"]},
                { "salaryPeriodName",GlobalData.SalaryPeriod["name"]}
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

        private void SalaryItemNameEditEnded(int row, string salaryItemName)
        {
            IDictionary<string, object> foundSalaryIteme =
                GlobalData.AllSalaryItem.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == salaryItemName;
                });
            if (foundSalaryIteme == null)
            {
                MessageBox.Show($"薪金项目名称\"{salaryItemName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "salaryItemId"] = foundSalaryIteme["id"];
                this.model1[row, "amount"] = foundSalaryIteme["defaultAmount"];
            }
        }

        private void SalaryPeriodNameEditEnded(int row, string salaryPeriodName)
        {
            IDictionary<string, object> foundSalaryPeriod =
                GlobalData.AllSalaryPeriod.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == salaryPeriodName;
                });
            if (foundSalaryPeriod == null)
            {
                MessageBox.Show($"薪金期间名称\"{salaryPeriodName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "salaryPeriodId"] = foundSalaryPeriod["id"];
            }
        }

        private void PersonNameEditEnded(int row, string personName)
        {
            IDictionary<string, object> foundPerson =
                GlobalData.AllPersons.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == personName;
                });
            if (foundPerson == null)
            {
                MessageBox.Show($"人员名称\"{personName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "personId"] = foundPerson["id"];
            }
        }

        private void comboBoxSalaryPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalData.SalaryPeriod = ((ComboBoxItem)this.comboBoxSalaryPeriod.SelectedItem).Value as IDictionary<string, object>;
            this.searchView1.ClearStaticCondition("salaryPeriodId");
            this.searchView1.AddStaticCondition("salaryPeriodId", GlobalData.SalaryPeriod["id"]);
            this.searchView1.Search();
        }
    }
}
