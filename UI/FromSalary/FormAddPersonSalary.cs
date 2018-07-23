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
    public partial class FormAddPersonSalary : Form
    {
        public FormAddPersonSalary()
        {
            MethodListenerContainer.Register(this);
            this.CenterToScreen();
            InitializeComponent();          
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0,null);
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

                if (this.model1.GetRowSynchronizationState(i) == SynchronizationState.ADDED_UPDATED && (int)this.model1[i, "id"] != 0)
                {
                    typeId.Add((int)this.model1[i, "id"]);
                }
            }
            if (typeId.Count == 0) return;
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
