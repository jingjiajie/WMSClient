using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using WMS.DataAccess;
using unvell.ReoGrid;
using System.Data.SqlClient;

namespace WMS.UI
{
    public partial class FormJobTicketItem : Form
    {
        private int jobTicketID = -1;
        Action jobTicketStateChangedCallback = null;

        Func<int> JobPersonIDGetter = null;
        Func<int> ConfirmPersonIDGetter = null;

        private KeyName[] visibleColumns = (from kn in JobTicketItemViewMetaData.KeyNames
                                            where kn.Visible == true
                                            select kn).ToArray();

        public FormJobTicketItem(int jobTicketID)
        {
            InitializeComponent();
            this.jobTicketID = jobTicketID;
        }

        public void SetJobTicketStateChangedCallback(Action jobTicketStateChangedCallback)
        {
            this.jobTicketStateChangedCallback = jobTicketStateChangedCallback;
        }

        private void FormJobTicketItem_Load(object sender, EventArgs e)
        {
            InitComponents();
            WMSEntities wmsEntities = new WMSEntities();
            JobTicket jobTicket = null;
            try
            {
                jobTicket = (from j in wmsEntities.JobTicket where j.ID == jobTicketID select j).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("加载数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if (jobTicket == null)
            {
                MessageBox.Show("作业单信息不存在，请刷新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            this.Search();
        }

        private void InitComponents()
        {
            //初始化表格
            this.reoGridControlMain.SetSettings(WorkbookSettings.View_ShowSheetTabControl, false);
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            worksheet.FocusPosChanged += worksheet_FocusPosChanged;
        
            for (int i = 0; i < JobTicketItemViewMetaData.KeyNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = JobTicketItemViewMetaData.KeyNames[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = JobTicketItemViewMetaData.KeyNames[i].Visible;
            }
            worksheet.Columns = JobTicketItemViewMetaData.KeyNames.Length; //限制表的长度

            Utilities.CreateEditPanel(this.tableLayoutPanelProperties,JobTicketItemViewMetaData.KeyNames);
            this.JobPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxJobPersonName", "Name");
            this.ConfirmPersonIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxConfirmPersonName", "Name");
        }

        private void worksheet_FocusPosChanged(object sender, unvell.ReoGrid.Events.CellPosEventArgs e)
        {
            this.RefreshTextBoxes();
        }

        private JobTicketView GetJobTicketViewByNo(string jobTicketNo)
        {
            WMSEntities wmsEntities = new WMSEntities();
            return (from jt in wmsEntities.JobTicketView
                    where jt.JobTicketNo == jobTicketNo
                    select jt).FirstOrDefault();
        }

        private void Search(int selectID = -1)
        {
            var worksheet = this.reoGridControlMain.Worksheets[0];

            worksheet[0, 1] = "加载中...";
            JobTicketItemView[] jobTicketItemViews = null;
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    jobTicketItemViews = (from j in wmsEntities.JobTicketItemView
                                                              where j.JobTicketID == this.jobTicketID
                                                              orderby j.ID descending
                                                              select j).ToArray();
                }
                catch
                {
                    MessageBox.Show("加载数据失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.reoGridControlMain.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "加载完成";
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    worksheet.Rows = jobTicketItemViews.Length < 10 ? 10 : jobTicketItemViews.Length;
                    if (jobTicketItemViews.Length == 0)
                    {
                        worksheet[0, 1] = "没有符合条件的记录";
                    }
                    for (int i = 0; i < jobTicketItemViews.Length; i++)
                    {
                        var curJobTicketViews = jobTicketItemViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curJobTicketViews, (from kn in JobTicketItemViewMetaData.KeyNames select kn.Key).ToArray());
                        for (int j = 0; j < columns.Length; j++)
                        {
                            worksheet[i, j] = columns[j] == null ? "" : columns[j].ToString();
                        }
                    }
                    if (selectID != -1)
                    {
                        Utilities.SelectLineByID(this.reoGridControlMain, selectID);
                    }
                    this.RefreshTextBoxes();
                }));
            })).Start();
        }

        private void labelStatus_Click(object sender, EventArgs e)
        {

        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            TextBox textBoxRealAmount = (TextBox)this.Controls.Find("textBoxRealAmount", true)[0];
            TextBox textBoxHappenTime = (TextBox)this.Controls.Find("textBoxHappenTime", true)[0];
            textBoxHappenTime.Text = DateTime.Now.ToString();

            if (string.IsNullOrWhiteSpace(textBoxRealAmount.Text))
            {
                MessageBox.Show("请填写实际完成数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.buttonModify.PerformClick();
        }

        private void ClearTextBoxes()
        {
            foreach (Control control in this.tableLayoutPanelProperties.Controls)
            {
                if(control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    textBox.Text = "";
                }
            }
        }

        private void RefreshTextBoxes()
        {
            this.ClearTextBoxes();
            var worksheet = this.reoGridControlMain.Worksheets[0];
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length == 0)
            {
                Utilities.FillTextBoxDefaultValues(this.tableLayoutPanelProperties, JobTicketItemViewMetaData.KeyNames);
                return;
            }
            int id = ids[0];
            JobTicketItemView jobTicketItemView = null;
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                jobTicketItemView = (from jti in wmsEntities.JobTicketItemView
                                                       where jti.ID == id
                                                       select jti).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("加载数据失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (jobTicketItemView == null)
            {
                MessageBox.Show("作业单项目不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Utilities.CopyPropertiesToTextBoxes(jobTicketItemView, this);
            Utilities.CopyPropertiesToComboBoxes(jobTicketItemView, this);
        }

        private void worksheet_SelectionRangeChanged(object sender, EventArgs e)
        {
            RefreshTextBoxes();
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = ids[0];

            WMSEntities wmsEntities1 = new WMSEntities();
            JobTicketItem jobTicketItem = null;
            try
            {
                jobTicketItem = (from j in wmsEntities1.JobTicketItem where j.ID == id select j).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (jobTicketItem == null)
            {
                MessageBox.Show("作业任务不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Invoke(new Action(() =>
            {
                decimal oriScheduledAmountNoUnit = (jobTicketItem.ScheduledAmount ?? 0) * (jobTicketItem.UnitAmount ?? 1); //原翻包数量*单位
                    if (Utilities.CopyTextBoxTextsToProperties(this, jobTicketItem, JobTicketItemViewMetaData.KeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (Utilities.CopyComboBoxsToProperties(this, jobTicketItem, JobTicketItemViewMetaData.KeyNames) == false)
                {
                    MessageBox.Show("内部错误：拷贝单选框数据失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (jobTicketItem.RealAmount > jobTicketItem.ScheduledAmount)
                {
                    MessageBox.Show("实际翻包数量不能超过计划翻包数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (jobTicketItem.RealAmount < jobTicketItem.ScheduledPutOutAmount)
                {
                    MessageBox.Show("实际翻包数量不能小于已分配出库数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                    //如果修改了计划翻包数量，这个数量不可以大于发货单剩余可分配翻包数量
                    //新的计划翻包数量
                    decimal newScheduledAmountNoUnit = (jobTicketItem.ScheduledAmount ?? 0) * (jobTicketItem.UnitAmount ?? 1);
                    //变化的计划翻包数量
                    decimal deltaScheduledAmountNoUnit = newScheduledAmountNoUnit - oriScheduledAmountNoUnit;
                if (deltaScheduledAmountNoUnit != 0)
                {
                    if (jobTicketItem.ScheduledAmount < jobTicketItem.RealAmount)
                    {
                        MessageBox.Show("计划翻包数量不可以小于实际翻包数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ShipmentTicketItem shipmentTicketItem = (from s in wmsEntities1.ShipmentTicketItem
                                                             where s.ID == jobTicketItem.ShipmentTicketItemID
                                                             select s).FirstOrDefault();
                    if (shipmentTicketItem != null)
                    {
                            //发货总数量
                            decimal shipmentAmountNoUnit = (shipmentTicketItem.ShipmentAmount ?? 0) * (shipmentTicketItem.UnitAmount ?? 1);
                            //剩余可分配翻包数量
                            decimal restScheduableAmountNoUnit = shipmentAmountNoUnit - (shipmentTicketItem.ScheduledJobAmount ?? 0) * (shipmentTicketItem.UnitAmount ?? 1);
                        if (restScheduableAmountNoUnit < deltaScheduledAmountNoUnit)
                        {
                            MessageBox.Show(string.Format("发货单剩余可分配翻包数量不足！\n剩余可分配翻包数：{0}（{1}）", Utilities.DecimalToString(restScheduableAmountNoUnit / (jobTicketItem.UnitAmount ?? 1)), jobTicketItem.Unit), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                            //把多的计划翻包数量加进发货单的已分配发货数里
                            shipmentTicketItem.ScheduledJobAmount += deltaScheduledAmountNoUnit / shipmentTicketItem.UnitAmount;
                    }
                }

                int jobPersonID = this.JobPersonIDGetter();
                int confirmPersonID = this.ConfirmPersonIDGetter();
                jobTicketItem.JobPersonID = jobPersonID == -1 ? null : (int?)jobPersonID;
                jobTicketItem.ConfirmPersonID = confirmPersonID == -1 ? null : (int?)confirmPersonID;
                if (jobTicketItem.RealAmount == jobTicketItem.ScheduledAmount)
                {
                    jobTicketItem.State = JobTicketItemViewMetaData.STRING_STATE_ALL_FINISHED;
                }
                else if (jobTicketItem.RealAmount == 0)
                {
                    jobTicketItem.State = JobTicketItemViewMetaData.STRING_STATE_UNFINISHED;
                }
                else
                {
                    jobTicketItem.State = JobTicketItemViewMetaData.STRING_STATE_PART_FINISHED;
                }

                try
                {
                    wmsEntities1.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("修改失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.UpdateJobTicketStateSync();
                this.Invoke(new Action(() => this.Search()));
                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }));

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length == 0)
            {
                MessageBox.Show("请选择要删除的项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("确定要删除选中项吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            new Thread(() =>
            {
                WMSEntities wmsEntities = new WMSEntities();
                try
                {
                    foreach (int id in ids)
                    {
                        wmsEntities.Database.ExecuteSqlCommand("DELETE FROM JobTicketItem WHERE ID = @id", new SqlParameter("id", id));
                    }
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(() => this.Search()));
                MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }).Start();
        }

        private void buttonModify_MouseEnter(object sender, EventArgs e)
        {
            buttonModify.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonModify_MouseLeave(object sender, EventArgs e)
        {
            buttonModify.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonModify_MouseDown(object sender, MouseEventArgs e)
        {
            buttonModify.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }


        private void buttonFinish_MouseEnter(object sender, EventArgs e)
        {
            buttonFinish.BackgroundImage = WMS.UI.Properties.Resources.bottonB1_s;
        }

        private void buttonFinish_MouseLeave(object sender, EventArgs e)
        {
            buttonFinish.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonFinish_MouseDown(object sender, MouseEventArgs e)
        {
            buttonFinish.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonFinishAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要全额完成所有条目吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

                WMSEntities wmsEntities = new WMSEntities();
                try
                {
                    wmsEntities.Database.ExecuteSqlCommand(
                        String.Format(@"UPDATE JobTicketItem
                                        SET State = '{0}',
                                        RealAmount = ScheduledAmount,
                                        HappenTime='{1}' 
                                        WHERE JobTicketID = {2} 
                                        AND (RealAmount <> ScheduledAmount OR State<>'{3}');",
                                        JobTicketItemViewMetaData.STRING_STATE_ALL_FINISHED,
                                        DateTime.Now.ToString(), 
                                        this.jobTicketID,
                                        JobTicketItemViewMetaData.STRING_STATE_ALL_FINISHED));
                    wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE JobTicket SET State = '{0}' WHERE ID = {1}", JobTicketViewMetaData.STRING_STATE_ALL_FINISHED, this.jobTicketID));
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.jobTicketStateChangedCallback?.Invoke();
                this.Invoke(new Action(() => this.Search()));
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateJobTicketStateSync()
        {
            WMSEntities wmsEntities = new WMSEntities();
            int totalJobTicketItemCount = wmsEntities.Database.SqlQuery<int>(string.Format("SELECT COUNT(*) FROM JobTicketItem WHERE JobTicketID = {0}", this.jobTicketID)).Single();
            int allfinishedJobTicketItemCount = wmsEntities.Database.SqlQuery<int>(String.Format("SELECT COUNT(*) FROM JobTicketItem WHERE JobTicketID = {0} AND State = '{1}'", this.jobTicketID, JobTicketItemViewMetaData.STRING_STATE_ALL_FINISHED)).Single();
            int unfinishedJobTicketItemCount = wmsEntities.Database.SqlQuery<int>(String.Format("SELECT COUNT(*) FROM JobTicketItem WHERE JobTicketID = {0} AND State = '{1}'", this.jobTicketID, JobTicketItemViewMetaData.STRING_STATE_UNFINISHED)).Single();
            string jobTicketState = null;
            if (unfinishedJobTicketItemCount == totalJobTicketItemCount)
            {
                jobTicketState = JobTicketViewMetaData.STRING_STATE_UNFINISHED;
            }
            else if (allfinishedJobTicketItemCount == totalJobTicketItemCount)
            {
                jobTicketState = JobTicketViewMetaData.STRING_STATE_ALL_FINISHED;
            }
            else
            {
                jobTicketState = JobTicketViewMetaData.STRING_STATE_PART_FINISHED;
            }

            try
            {
                wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE JobTicket SET State = '{0}' WHERE ID = {1}", jobTicketState, this.jobTicketID));
                wmsEntities.SaveChanges();
            }
            catch
            {
                MessageBox.Show("更新作业单状态失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.IsDisposed) return;
            this.Invoke(new Action(() =>
            {
                this.jobTicketStateChangedCallback?.Invoke();
            }));
        }

        private void buttonFinishAll_MouseDown(object sender, MouseEventArgs e)
        {
            buttonFinishAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonFinishAll_MouseEnter(object sender, EventArgs e)
        {
            buttonFinishAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB1_s;
        }

        private void buttonFinishAll_MouseLeave(object sender, EventArgs e)
        {
            buttonFinishAll.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }
    }
}
