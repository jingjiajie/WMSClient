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
using System.Collections;

namespace WMS.UI.FromSalary
{
    public partial class FormAddPersonSalary : Form
    {
        private Action addFinishedCallback = null;
        public FormAddPersonSalary()
        {
            MethodListenerContainer.Register("formAddPersonSalary",this);
            this.CenterToScreen();
            InitializeComponent();          
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0,null);
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            List<int> typeId = new List<int>();
            for (int i = 0; i < this.model1.RowCount; i++)
            {             
                if (this.model1.GetRowSynchronizationState(i) == SynchronizationState.ADDED_UPDATED && this.model1[i, "id"] != null)
                {
                    typeId.Add((int)this.model1[i, "id"]);
                }
            }
            if (typeId.Count == 0) return;
            if (this.IsRepeat(typeId.ToArray())) { MessageBox.Show("添加的类型重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
            AddPersonSalary addPersonSalary = new AddPersonSalary();
            addPersonSalary.salaryTypeId = typeId;
            addPersonSalary.warehouseId =(int) GlobalData.Warehouse["id"];
            if (GlobalData.SalaryPeriod == null) { MessageBox.Show("当前仓库无任何区间，无法添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);return; }
            addPersonSalary.salaryPeriodId = (int)GlobalData.SalaryPeriod["id"];
            string json = (new JavaScriptSerializer()).Serialize(addPersonSalary);
            try
            {
                string body = json;
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/person_salary/add_person_salary_by_salary_type";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
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

        private void SalaryTypeNameEditEnded([Row]int row, [Data]string salaryTypeName)
        {
            IDictionary<string, object> foundSalaryTyped =
                GlobalData.AllSalaryType.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == salaryTypeName;
                });
            if (foundSalaryTyped == null)
            {
                MessageBox.Show($"薪金类型名称\"{salaryTypeName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "id"] = foundSalaryTyped["id"];               
            }
        }

        private void FormAddPersonSalary_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            { this.addFinishedCallback(); }
        }

        private  bool IsRepeat(int[] array)
        {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < array.Length; i++)
            {
                if (ht.Contains(array[i]))
                {
                    return true;
                }
                else
                {
                    ht.Add(array[i], array[i]);
                }
            }       
            return false;
        }

        private void buttonADD_Click(object sender, EventArgs e)
        {
            List<int> typeId = new List<int>();
            for (int i = 0; i < this.model1.RowCount; i++)
            {
                if (this.model1.GetRowSynchronizationState(i) == SynchronizationState.ADDED_UPDATED && this.model1[i, "id"] != null)
                {
                    typeId.Add((int)this.model1[i, "id"]);
                }
            }
            if (typeId.Count == 0) { MessageBox.Show("添加的类型不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }           
            if (this.IsRepeat(typeId.ToArray())) { MessageBox.Show("添加的类型重复！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
            AddPersonSalary addPersonSalary = new AddPersonSalary();
            addPersonSalary.salaryTypeId = typeId;
            addPersonSalary.warehouseId = (int)GlobalData.Warehouse["id"];
            if (GlobalData.SalaryPeriod == null) { MessageBox.Show("当前仓库无任何区间，无法添加！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            addPersonSalary.salaryPeriodId = (int)GlobalData.SalaryPeriod["id"];
            string json = (new JavaScriptSerializer()).Serialize(addPersonSalary);
            try
            {
                string body = json;
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/person_salary/add_person_salary_by_salary_type";
                RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
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

        private void FormAddPersonSalary_Load(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, null);
            Utilities.BindBlueButton(this.buttonADD);
        }
    }
}
