using System;
using FrontWork;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormAcccount
{
    public partial class FormAccountRecord : Form
    {
        public FormAccountRecord()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }


        private void AccountTitleNameEditEnded(int row, string accountTitleName)
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
                this.model1[row, "accountTitleId"] = foundAccountTitle["id"];
                this.model1[row, "accountTitleNo"] = foundAccountTitle["no"];
            }

        }
            private void AccountTitleNoEditEnded(int row, string accountTitleNo)
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
                    this.model1[row, "accountTitleId"] = foundAccountTitle["id"];
                this.model1[row, "accountTitleName"] = foundAccountTitle["name"];
            }
            }

        private void FormAccountRecord_Load(object sender, EventArgs e)
        {
            //刷新期间
            this.comboBoxAccountPeriod.Items.AddRange((from item in GlobalData.AllAccountPeriod
                                                      select new ComboBoxItem(item["name"]?.ToString(), item)).ToArray());
            if (GlobalData.AllAccountPeriod.Count != 0)
            {
                GlobalData.AccountPeriod = GlobalData.AllAccountPeriod[0];
                for (int i = 0; i < this.comboBoxAccountPeriod.Items.Count; i++)
                {
                    if (GlobalData.AllAccountPeriod[i] == GlobalData.AccountPeriod)
                    {
                        this.comboBoxAccountPeriod.SelectedIndexChanged -= this.comboBoxAccountPeriod_SelectedIndexChanged;
                        this.comboBoxAccountPeriod.SelectedIndex = i;
                        this.comboBoxAccountPeriod.SelectedIndexChanged += this.comboBoxAccountPeriod_SelectedIndexChanged;
                    }
                }
                this.searchView1.AddStaticCondition("accountPeriodId", GlobalData.AccountPeriod["id"]);
            }

            //刷新科目
            this.comboBoxAccountTitle.Items.Add("无");
            this.comboBoxAccountTitle.Items.AddRange((from item in GlobalData.AllAccountTitle
                                                       select new ComboBoxItem(item["name"]?.ToString(), item)).ToArray());

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
                { "personId",GlobalData.Person["id"]},
                { "personName",GlobalData.Person["name"]},
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "accountPeriodName",GlobalData.AccountPeriod["name"]},
                { "accountPeriodId",GlobalData.AccountPeriod["id"]},
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

        private void comboBoxAccountPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalData.AccountPeriod = ((ComboBoxItem)this.comboBoxAccountPeriod.SelectedItem).Value as IDictionary<string, object>;
            this.searchView1.ClearStaticCondition("accountPeriodId");
            this.searchView1.AddStaticCondition("accountPeriodId", GlobalData.AccountPeriod["id"]);
            this.searchView1.Search();
        }

        private void comboBoxAccountTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.searchView1.ClearStaticCondition("accountTitleId");
            if (this.comboBoxAccountTitle.SelectedIndex==0) {
                this.searchView1.Search();
            }
            else
            {
                GlobalData.AccountTitle = ((ComboBoxItem)this.comboBoxAccountTitle.SelectedItem).Value as IDictionary<string, object>;
                this.searchView1.AddStaticCondition("accountTitleId", GlobalData.AccountTitle["id"]);
                this.searchView1.Search();
            }        
        }
    }
}
