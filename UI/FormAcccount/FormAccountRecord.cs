using System;
using FrontWork;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace WMS.UI.FormAcccount
{
    public partial class FormAccountRecord : Form
    {
        private Boolean DoneDeficitCheck = false;
        public static FormAccountRecord formAccountRecord = null;
        System.Timers.Timer timer = new System.Timers.Timer();
        Timer T = new Timer();
        public FormAccountRecord()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
            InitTree();
            formAccountRecord = this;
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
            GlobalData.AccountTitle = null;
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



            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();

            this.showAccrual();
            
            timer.Enabled = true;
            timer.AutoReset = false;

            T.Interval=100;
            T.Tick += new EventHandler(t_tick);
            T.Start();
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(Timerup);
            //timer.Start();

        }

        private void t_tick(object sender, EventArgs e)
        {
            if (GlobalData.REMAINDENABLE)
            {
                this.DeficitCheck();
            }
            T.Stop();
        }

        //private void Timerup(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    if (GlobalData.REMAINDENABLE)
        //    {
        //        this.DeficitCheck();
        //    }
        //}

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (GlobalData.AccountTitle != null)
            {
                this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "personId",GlobalData.Person["id"]},
                { "personName",GlobalData.Person["name"]},
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "accountPeriodName",GlobalData.AccountPeriod["name"]},
                { "accountPeriodId",GlobalData.AccountPeriod["id"]},
                { "accountTitleName",GlobalData.AccountTitle["name"]},
                { "accountTitleNo",GlobalData.AccountTitle["no"]},
                { "accountTitleId",GlobalData.AccountTitle["id"]},
            });
            }
            else
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
                this.showAccrual();
            }
        }

        private void comboBoxAccountPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            GlobalData.AccountPeriod = ((ComboBoxItem)this.comboBoxAccountPeriod.SelectedItem).Value as IDictionary<string, object>;
            this.searchView1.ClearStaticCondition("accountPeriodId");
            this.searchView1.AddStaticCondition("accountPeriodId", GlobalData.AccountPeriod["id"]);

            //如果该期间已经截止
            if ((int)GlobalData.AccountPeriod["ended"] == 1)
            {
                this.toolStripButtonAdd.Visible = false;
                this.toolStripButtonAlter.Visible = false;
                this.toolStripButtonDelete.Visible = false;
                this.ButtonTransfer.Visible = false;
                this.ButtonWriteOff.Visible = false;
            }
            else {
                this.toolStripButtonAdd.Visible = true;
                this.toolStripButtonAlter.Visible = true;
                this.toolStripButtonDelete.Visible = true;
                this.ButtonTransfer.Visible = true;
                this.ButtonWriteOff.Visible = true;
            }

            this.searchView1.Search();
            this.showAccrual();
        }

        private void ButtonWriteOff_Click(object sender, EventArgs e)
        {
            int[] selectedIDs = this.model1.GetSelectedRows<int>("id").Except(new int[] { 0 }).ToArray();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择一项进行冲销操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string strIDs = serializer.Serialize(selectedIDs);
            try
            {
                string operatioName = "write_off";
                RestClient.RequestPost<string>(Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_record/" + operatioName, strIDs, "POST");
                this.searchView1.Search();
                this.showAccrual();
                MessageBox.Show("冲销操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show(("选中条目冲销") + "失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void tableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void showAccrual()
        {
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"curAccountPeriodId\":\"" + GlobalData.AccountPeriod["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_record/accrual_check";
                var returnAccrualCheck = RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                foreach (IDictionary<string, object> theReturnAccrualCheck in returnAccrualCheck)
                {
                    this.textBoxCreditAmount.Text = theReturnAccrualCheck["creditAmount"].ToString();
                    this.textBoxDebitAmount.Text = theReturnAccrualCheck["debitAmount"].ToString();
                }

            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("发生额显示失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void showBalance()
        {
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"curAccountPeriodId\":\"" + GlobalData.AccountPeriod["id"] + "\",\"curAccountTitleId\":\"" + GlobalData.AccountTitle["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_record/show_balance";
                var returnAccrualCheck = RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                foreach (IDictionary<string, object> theReturnAccrualCheck in returnAccrualCheck)
                {
                    this.textBoxBalance.Text = theReturnAccrualCheck["balance"].ToString();
                }

            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("发生额显示失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void toolStripButtonDeficit_Click(object sender, EventArgs e)
        {
            this.DeficitCheck();
        }

        private void DeficitCheck()
        {
            try
            {
                this.DoneDeficitCheck = true;
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"curAccountPeriodId\":\"" + GlobalData.AccountPeriod["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_record/deficit_check";
                var returnDeficitCheck = RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                if (returnDeficitCheck.Count == 0)
                {
                    //MessageBox.Show("当前仓库无赤字记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    StringBuilder remindBody = new StringBuilder();
                    foreach (IDictionary<string, object> AccountRecordView in returnDeficitCheck)
                    {

                        remindBody = remindBody
                                .Append("科目名称：“").Append(AccountRecordView["accountTitleName"])
                                .Append("”，余额：“").Append(AccountRecordView["balance"])
                                .Append("”存在赤字！请核准账目记录！\r\n");

                    }
                    new FormRemind(remindBody.ToString()).Show();

                }
                GlobalData.REMAINDENABLE = false;

            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                GlobalData.REMAINDENABLE = false;
                MessageBox.Show("赤字提醒失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
            }
        }

        private void ButtonAccrualCheck_Click(object sender, EventArgs e)
        {
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"curAccountPeriodId\":\"" + GlobalData.AccountPeriod["id"] + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_record/accrual_check";
                var returnAccrualCheck = RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);
                foreach (IDictionary<string, object> theReturnAccrualCheck in returnAccrualCheck)
                {
                    if (theReturnAccrualCheck["debitAmount"].ToString() == theReturnAccrualCheck["creditAmount"].ToString())
                    {
                        MessageBox.Show("自动对账正确！\n\r借方发生额："+ theReturnAccrualCheck["debitAmount"].ToString()+ "，\n\r贷方发生额：" + theReturnAccrualCheck["creditAmount"].ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else {
                        MessageBox.Show("自动对账错误！\n\r借方发生额：" + theReturnAccrualCheck["debitAmount"].ToString() + "，\n\r贷方发生额：" + theReturnAccrualCheck["creditAmount"].ToString()+"，不相等！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }

            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("发生额显示失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonTransfer_Click(object sender, EventArgs e)
        {
            var a1 = new FormTransferAccount();
            a1.SetAddFinishedCallback(() =>
            {
                this.searchView1.Search();
            });
            a1.Show();
        }

        //初始化树
        public void InitTree()
        {
            string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_record/build_tree_view";
            string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"personId\":\"" + GlobalData.Person["id"] + "\",\"curAccountPeriodId\":\"" + GlobalData.AccountPeriod["id"] + "\"}";
            var buildAccountTitleTreeView = RestClient.RequestPost<List<IDictionary<string, object>>>(url);

            foreach (IDictionary<string, object> accountTitleNode in buildAccountTitleTreeView)
            {
                if (accountTitleNode["accountTitleId"].ToString() == "0")
                {
                    TreeNode tchild = new TreeNode();
                    tchild.Name = accountTitleNode["accountTitleNo"].ToString();
                    tchild.Text = accountTitleNode["accountTitleName"].ToString();
                    LoadAll(accountTitleNode["accountTitleId"].ToString(), tchild, buildAccountTitleTreeView);
                    this.treeViewAccountTitle.Nodes.Add(tchild);//把根节点加入到treeview的根节点
                }
            }

            
        }
        //加载所属节点
        public void LoadAll(string parentAccountTitleId, TreeNode tn, List<IDictionary<string, object>> buildAccountTitleTreeView)
        {

            foreach (IDictionary<string, object> accountTitleNode in buildAccountTitleTreeView)
            {
                if (accountTitleNode["parentAccountTitleId"].ToString() == parentAccountTitleId
                    &&accountTitleNode["accountTitleId"].ToString()!= "0")
                {
                    TreeNode tchild = new TreeNode();
                    tchild.Name = accountTitleNode["accountTitleNo"].ToString();
                    tchild.Text = accountTitleNode["accountTitleName"].ToString();
                    LoadAll(accountTitleNode["accountTitleId"].ToString(), tchild, buildAccountTitleTreeView);
                    tn.Nodes.Add(tchild);//把当前节点加入到tn数的节点中
                }
            }

        }

        private void treeViewAccountTitle_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string accountTitleNo = treeViewAccountTitle.SelectedNode.Name;
            string accountTitleName = treeViewAccountTitle.SelectedNode.Text;

            this.searchView1.ClearStaticCondition("accountTitleNo");
            if (accountTitleNo == "全部科目")
            {
                GlobalData.AccountTitle = null;
                this.searchView1.Search();
                this.textBoxBalance.Text = null;
            }
            else {
                GlobalData.AccountTitle = (from item in GlobalData.AllAccountTitleTure
                                          where item["name"].ToString() == accountTitleName
                                           select item).ToList().First();

                this.searchView1.AddStaticCondition("accountTitleNo", accountTitleNo, Relation.STARTS_WITH);
                this.searchView1.Search();

                this.showBalance();
            }
        }
    }
}
