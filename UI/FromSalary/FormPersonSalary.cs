using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;

namespace WMS.UI.FromSalary
{
    public partial class FormPersonSalary : Form
    {
        public static FormPersonSalary formPersonSalary=null;

        public FormPersonSalary()
        {
            MethodListenerContainer.Register("FormPersonSalary", this);
            InitializeComponent();
            formPersonSalary = this;
        }

        public void Search()
        {        
            this.searchView1.Search();
        }

        public void RefreshSalaryPeriod() {
            //刷新期间
            this.comboBoxSalaryPeriod.Items.Clear();
            this.comboBoxSalaryPeriod.Items.AddRange((from item in GlobalData.AllSalaryPeriod
                                                      select new ComboBoxItem(item["name"]?.ToString(), item)).ToArray());
            if (GlobalData.AllSalaryPeriod.Count != 0)
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
            else
            {
                this.comboBoxSalaryPeriod.SelectedIndexChanged -= this.comboBoxSalaryPeriod_SelectedIndexChanged;
                this.comboBoxSalaryPeriod.Items.Add("无");
                this.comboBoxSalaryPeriod.SelectedIndex = 0;
            }
        }

        public void RefreshSalaryType()
        {
            //刷新类别
            this.comboBoxSalaryType.Items.Clear();
            this.comboBoxSalaryType.Items.AddRange((from item in GlobalData.AllSalaryType
                                                    select new ComboBoxItem(item["name"]?.ToString(), item)).ToArray());
            if (GlobalData.AllSalaryType.Count != 0)
            {
                this.comboBoxSalaryType.Items.Add("全部类型");
                GlobalData.SalaryType = GlobalData.AllSalaryType[0];
                for (int i = 0; i < this.comboBoxSalaryType.Items.Count-1; i++)
                {
                    if (GlobalData.AllSalaryType[i] == GlobalData.SalaryType)
                    {
                        this.comboBoxSalaryType.SelectedIndexChanged -= this.comboBoxSalaryType_SelectedIndexChanged;
                        this.comboBoxSalaryType.SelectedIndex = i;
                        this.comboBoxSalaryType.SelectedIndexChanged += this.comboBoxSalaryType_SelectedIndexChanged;
                    }
                }
                this.searchView1.AddStaticCondition("salaryTypeId", GlobalData.SalaryType["id"]);
            }
            else
            {
                this.comboBoxSalaryType.SelectedIndexChanged -= this.comboBoxSalaryType_SelectedIndexChanged;
                this.comboBoxSalaryType.Items.Add("无");
                this.comboBoxSalaryType.SelectedIndex = 0;
            }
        }

        private void FormPersonSalary_Load(object sender, EventArgs e)
        {
            //刷新期间
            this.RefreshSalaryPeriod();
            //刷新类别
            this.RefreshSalaryType();
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
            FormAddPersonSalary form = new FormAddPersonSalary();
            form.SetAddFinishedCallback(() =>
            {
                this.searchView1.Search();               
            });
            form.Show();
           
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

        private void comboBoxSalaryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str="";
            try { str = (string)this.comboBoxSalaryType.SelectedItem; }
            catch {  }
            if (str == "全部类型")
            {
                this.searchView1.ClearStaticCondition("salaryTypeId");
                this.searchView1.Search();
            }
            else
            {
                GlobalData.SalaryType = ((ComboBoxItem)this.comboBoxSalaryType.SelectedItem).Value as IDictionary<string, object>;
                this.searchView1.ClearStaticCondition("salaryTypeId");
                this.searchView1.AddStaticCondition("salaryTypeId", GlobalData.SalaryType["id"]);
                this.searchView1.Search();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            IDictionary<string, object> rowData;
            for (int i = 0; i < this.model1.RowCount; i++)
            {
                rowData = this.model1.GetRows(new int[] { i })[0];
                ids.Add((int)rowData["id"]);
            }
            AddPersonSalary addPersonSalary = new AddPersonSalary();
            addPersonSalary.personSalaryIds = ids;
            if (GlobalData.SalaryPeriod == null) {
                MessageBox.Show($"无薪资期间无法进行刷新！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (GlobalData.SalaryType == null)
            {
                MessageBox.Show($"无薪资类型无法进行刷新！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            addPersonSalary.salaryPeriodId =(int) GlobalData.SalaryPeriod["id"];
            addPersonSalary.salaryTypeId = (int)GlobalData.SalaryType["id"];
            addPersonSalary.warehouseId = (int)GlobalData.Warehouse["id"];
            string json = (new JavaScriptSerializer()).Serialize(addPersonSalary);
            try
            {
                string body = json;
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/person_salary/refresh_formula_and_valuation";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("刷新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("刷新公式、计件条目") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            IDictionary<string, object> rowData;
            for (int i = 0; i < this.model1.RowCount; i++)
            {
                rowData = this.model1.GetRows(new int[] { i })[0];
                ids.Add((int)rowData["id"]);
            }
            AddPersonSalary addPersonSalary = new AddPersonSalary();
            addPersonSalary.personSalaryIds = ids;
            if (GlobalData.SalaryPeriod == null)
            {
                MessageBox.Show($"无薪资期间无法进行刷新！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (GlobalData.SalaryType == null)
            {
                MessageBox.Show($"无薪资类型无法进行刷新！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            addPersonSalary.salaryPeriodId = (int)GlobalData.SalaryPeriod["id"];
            addPersonSalary.salaryTypeId = (int)GlobalData.SalaryType["id"];
            addPersonSalary.warehouseId = (int)GlobalData.Warehouse["id"];
            string json = (new JavaScriptSerializer()).Serialize(addPersonSalary);
            try
            {
                string body = json;
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/person_salary/refresh_person_salary";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("刷新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("刷新") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            IDictionary<string, object> rowData;
            for (int i = 0; i < this.model1.RowCount; i++)
            {
                rowData = this.model1.GetRows(new int[] { i })[0];
                ids.Add((int)rowData["id"]);
            }
            AddPersonSalary addPersonSalary = new AddPersonSalary();
            addPersonSalary.personSalaryIds = ids;
            if (GlobalData.SalaryPeriod == null)
            {
                MessageBox.Show($"无薪资期间无法进行添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (GlobalData.SalaryType == null)
            {
                MessageBox.Show($"无薪资类型无法进行添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            addPersonSalary.salaryPeriodId = (int)GlobalData.SalaryPeriod["id"];
            addPersonSalary.salaryTypeId = (int)GlobalData.SalaryType["id"];
            addPersonSalary.warehouseId = (int)GlobalData.Warehouse["id"];
            string json = (new JavaScriptSerializer()).Serialize(addPersonSalary);
            try
            {
                string body = json;
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/person_salary/add_last_period";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.searchView1.Search();
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("添加") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
    }
}
