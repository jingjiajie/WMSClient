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
                this.searchView1.Search();            
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

