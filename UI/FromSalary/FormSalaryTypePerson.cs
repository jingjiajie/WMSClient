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
using System.Net;
using System.IO;

namespace WMS.UI.FromSalary
{
    public partial class FormSalaryTypePerson : Form
    {
        private int salaryTypeId;
        public FormSalaryTypePerson(int salaryTypeId)
        {
            MethodListenerContainer.Register("FormSalaryTypePerson", this);
            InitializeComponent();
            this.salaryTypeId = salaryTypeId;
        }


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "salaryTypeId",salaryTypeId}
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
                this.judegeSalaryType();
                this.searchView1.Search();            
            }
        }

        private void judegeSalaryType()
        {
            try
            {
                AddPersonSalary addPersonSalary = new AddPersonSalary();
                addPersonSalary.warehouseId = (int)GlobalData.Warehouse["id"];
                string json = (new JavaScriptSerializer()).Serialize(addPersonSalary);
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/person_salary/judge_salary_type_person";
                IDictionary<string, object> salaryTypePerson = RestClient.RequestPost<IDictionary<string, object>>(url, json);

                if ((int)salaryTypePerson["personId"] != -1)
                {
                    MessageBox.Show($"人员\"{salaryTypePerson["personName"]}\"在多个类型中重复，如果其对应薪资项目名称完全相同，则人员薪资窗口显示“所有类型”工资时金额可能不准确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

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

        private void FormSalaryTypePerson_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            this.searchView1.AddStaticCondition("salaryTypeId", this.salaryTypeId);

            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void PersonNameEditEnded([Row]int row, [Data]string personName)
        {
            IDictionary<string, object> foundPerson =
                GlobalData.AllPersons.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == personName;
                });
            if (foundPerson == null)
            {
                MessageBox.Show($"人员\"{personName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "personId"] = foundPerson["id"];              
            }
        }
    }
}

