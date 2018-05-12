using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using System.Threading;
using unvell.ReoGrid.CellTypes;
using unvell.ReoGrid;

namespace WMS.UI
{
    public partial class FormPutOutStorageTicketNew : Form
    {
        private PagerWidget<JobTicketItemView> pagerWidget = null;
        private int jobTicketID = -1;
        private int userID = -1;
        private int projectID = -1;
        private int warehouseID = -1;

        private Func<int> personIDGetter = null;
        private int validRows { get => jobTicketItemViews == null ? 0 : jobTicketItemViews.Length; }
        private JobTicketItemView[] jobTicketItemViews = null;

        private int[] editableColumns = new int[] { 1, 2 };

        private Action<string,string> toPutOutStorageTicketCallback = null;

        private static KeyName[] newPutOutStorageTicketKeyNames =
        {
            new KeyName(){Key="SupplyNoOrComponentName",Name="零件代号/名称",NotNull=true},
            new KeyName(){Key="SchedulePutOutAmount",Name="计划装车数量",NotNull=true,Positive=true},
            new KeyName(){Key="Unit",Name="单位",NotNull=true},
            new KeyName(){Key="UnitAmount",Name="单位数量",NotNull=true,Positive=true}
        };

        class NewPutOutStorageTicketItemData
        {
            private string supplyNoOrComponentName;
            private decimal schedulePutOutAmount;
            private decimal unitAmount;
            private string unit;

            public string SupplyNoOrComponentName { get => supplyNoOrComponentName; set => supplyNoOrComponentName = value; }
            public decimal UnitAmount { get => unitAmount; set => unitAmount = value; }
            public decimal SchedulePutOutAmount { get => schedulePutOutAmount; set => schedulePutOutAmount = value; }
            public string Unit { get => unit; set => unit = value; }
        }

        private StandardImportForm<NewPutOutStorageTicketItemData> standardImportForm = null;


        public void SetToPutOutStorageTicketCallback(Action<string,string> callback)
        {
            this.toPutOutStorageTicketCallback = callback;
        }

        public FormPutOutStorageTicketNew(int jobTicketID, int userID, int projectID, int warehouseID)
        {
            InitializeComponent();
            this.jobTicketID = jobTicketID;
            this.userID = userID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        private void BindButtonStyle(Button button)
        {
            button.MouseEnter += (obj, e) =>
            {
                button.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
            };
            button.MouseLeave += (obj, e) =>
            {
                button.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
            };
            button.MouseDown += (obj, e) =>
            {
                button.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
            };
        }

        private void FormPutOutStorageTicketNew_Load(object sender, EventArgs e)
        {
            this.InitComponents();
            Utilities.CreateEditPanel(this.tableLayoutEditPanel, PutOutStorageTicketViewMetaData.KeyNames);
            this.personIDGetter = Utilities.BindTextBoxSelect<FormSelectPerson, Person>(this, "textBoxPersonName", "Name");
            JobTicket jobTicket = null;
            User user = null;
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                jobTicket = (from s in wmsEntities.JobTicket
                                  where s.ID == jobTicketID
                                  select s).FirstOrDefault();
                user = (from u in wmsEntities.User
                        where u.ID == this.userID
                        select u).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("无法连接到服务器，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if (jobTicket == null)
            {
                MessageBox.Show("作业单信息不存在，请刷新显示", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if (user == null)
            {
                MessageBox.Show("登录失效，操作失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Text = "生成出库单（作业单号：" + jobTicket.JobTicketNo+"）";
            this.Controls.Find("textBoxCreateUserUsername", true)[0].Text = user.Username;
            this.Controls.Find("textBoxCreateTime", true)[0].Text = DateTime.Now.ToString();
            this.Controls.Find("textBoxJobTicketJobTicketNo", true)[0].Text = jobTicket.JobTicketNo;
            this.Search();
        }

        private void InitComponents()
        {
            BindButtonStyle(this.buttonImport);
            BindButtonStyle(this.buttonSelectAll);
            this.reoGridControlMain.SetSettings(unvell.ReoGrid.WorkbookSettings.View_ShowSheetTabControl, false);
            this.pagerWidget = new PagerWidget<JobTicketItemView>(this.reoGridControlMain, JobTicketItemViewMetaData.KeyNames);
            this.pagerWidget.SetPageSize(-1);
        }

        private void Search()
        {
            this.pagerWidget.AddStaticCondition("JobTicketID", this.jobTicketID.ToString());
            this.pagerWidget.Search(false,-1,(results)=>
            {
                this.jobTicketItemViews = results;

                if (this.IsDisposed) return;
                this.Invoke(new Action(() =>
                {
                    var worksheet = this.reoGridControlMain.Worksheets[0];
                    worksheet.SelectionMode = unvell.ReoGrid.WorksheetSelectionMode.Cell;
                    worksheet.InsertColumns(1, 2);
                    worksheet.ColumnHeaders[1].Text = "选择";
                    worksheet.ColumnHeaders[2].Text = "计划出库数量";
                    worksheet.BeforeCellEdit += (s, e) =>
                    {
                        e.IsCancelled = !(editableColumns.Contains(e.Cell.Column) && e.Cell.Row < validRows);
                    };
                    for (int i = 0; i < results.Length; i++)
                    {
                        JobTicketItemView curJobTicketItemView = results[i];
                        if (curJobTicketItemView.RealAmount == curJobTicketItemView.ScheduledPutOutAmount)
                        {
                            worksheet.Cells[i, 1].Style.BackColor = Color.LightGray;
                            worksheet.Cells[i, 1].IsReadOnly = true;
                        }
                        else
                        {
                            worksheet[i, 1] = new CheckBoxCell(false); //显示复选框
                        }
                        //上颜色，边框
                        worksheet.Cells[i, 2].Style.BackColor = Color.AliceBlue;
                        worksheet.SetRangeBorders(i, 2, 1, 1, unvell.ReoGrid.BorderPositions.All, unvell.ReoGrid.RangeBorderStyle.SilverSolid);
                        //计划翻包数量
                        worksheet.Cells[i, 2].DataFormat = unvell.ReoGrid.DataFormat.CellDataFormatFlag.Text;
                        worksheet[i, 2] = Utilities.DecimalToString((curJobTicketItemView.RealAmount - (curJobTicketItemView.ScheduledPutOutAmount ?? 0)) ?? 0); //计划出库数量默认等于实际作业数量-已经计划出库过的数量
                    }
                }));
            });
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.reoGridControlMain.CurrentWorksheet.EndEdit(new EndEditReason());
            PutOutStorageTicket newPutOutStorageTicket = new PutOutStorageTicket();
            if (Utilities.CopyTextBoxTextsToProperties(this, newPutOutStorageTicket, PutOutStorageTicketViewMetaData.KeyNames, out string errorMesage) == false)
            {
                MessageBox.Show(errorMesage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Utilities.CopyComboBoxsToProperties(this, newPutOutStorageTicket, PutOutStorageTicketViewMetaData.KeyNames) == false)
            {
                MessageBox.Show("内部错误：读取复选框数据失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<int> checkedLines = new List<int>();
            var worksheet = this.reoGridControlMain.Worksheets[0];
            for (int i = 0; i < this.reoGridControlMain.Worksheets[0].Rows; i++)
            {
                if ((worksheet[i, 1] as bool? ?? false) == false)
                {
                    continue;
                }
                checkedLines.Add(i);
            }
            if (checkedLines.Count == 0)
            {
                MessageBox.Show("至少选择一项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            WMSEntities wmsEntities = new WMSEntities();
            wmsEntities.PutOutStorageTicket.Add(newPutOutStorageTicket);

            JobTicket jobTicket = (from s in wmsEntities.JobTicket
                                   where s.ID == this.jobTicketID
                                   select s).FirstOrDefault();

            if (jobTicket == null)
            {
                MessageBox.Show("作业单不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            newPutOutStorageTicket.JobTicketID = jobTicket.ID;
            newPutOutStorageTicket.ProjectID = this.projectID;
            newPutOutStorageTicket.WarehouseID = this.warehouseID;
            newPutOutStorageTicket.CreateUserID = this.userID;
            newPutOutStorageTicket.CreateTime = DateTime.Now;
            int personID = this.personIDGetter();
            if (personID != -1)
            {
                newPutOutStorageTicket.PersonID = personID;
            }

            //把选中的条目加进出库单
            foreach (int line in checkedLines)
            {
                int id = int.Parse(worksheet[line, 0].ToString());
                JobTicketItem jobTicketItem = (from j in wmsEntities.JobTicketItem
                                               where j.ID == id
                                               select j).FirstOrDefault();
                if (jobTicketItem == null)
                {
                    MessageBox.Show("无法找到作业单条目，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                PutOutStorageTicketItem newPutOutStorageTicketItem = new PutOutStorageTicketItem();

                if (Utilities.CopyTextToProperty(worksheet[line, 2].ToString(), "ScheduledAmount", newPutOutStorageTicketItem, PutOutStorageTicketItemViewMetaData.KeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (newPutOutStorageTicketItem.ScheduledAmount <= 0)
                {
                    MessageBox.Show("行" + (line + 1) + "：计划出库数量必须大于0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (newPutOutStorageTicketItem.ScheduledAmount + (jobTicketItem.ScheduledPutOutAmount ?? 0) > jobTicketItem.RealAmount)
                {
                    MessageBox.Show("行" + (line + 1) + "：计划出库数量不能大于实际翻包完成数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                jobTicketItem.ScheduledPutOutAmount = (jobTicketItem.ScheduledPutOutAmount ?? 0) + newPutOutStorageTicketItem.ScheduledAmount;
                newPutOutStorageTicketItem.State = PutOutStorageTicketItemViewMetaData.STRING_STATE_WAIT_FOR_LOAD;
                newPutOutStorageTicketItem.StockInfoID = jobTicketItem.StockInfoID;
                newPutOutStorageTicketItem.JobTicketItemID = jobTicketItem.ID;
                newPutOutStorageTicketItem.RealAmount = 0;
                newPutOutStorageTicketItem.Unit = jobTicketItem.Unit;
                newPutOutStorageTicketItem.UnitAmount = jobTicketItem.UnitAmount;
                newPutOutStorageTicketItem.ReturnQualityUnit = "个";
                newPutOutStorageTicketItem.ReturnQualityUnitAmount = 1;
                newPutOutStorageTicketItem.ReturnRejectUnit = "个";
                newPutOutStorageTicketItem.ReturnRejectUnitAmount = 1;
                newPutOutStorageTicket.PutOutStorageTicketItem.Add(newPutOutStorageTicketItem);
            }
            //生成出库单号
            if (string.IsNullOrWhiteSpace(newPutOutStorageTicket.No))
            {
                DateTime createDay = new DateTime(newPutOutStorageTicket.CreateTime.Value.Year, newPutOutStorageTicket.CreateTime.Value.Month, newPutOutStorageTicket.CreateTime.Value.Day);
                DateTime nextDay = createDay.AddDays(1);
                int maxRankOfToday = Utilities.GetMaxTicketRankOfDay((from p in wmsEntities.PutOutStorageTicket
                                                                      where p.CreateTime >= createDay && p.CreateTime < nextDay
                                                                      select p.No).ToArray());
                newPutOutStorageTicket.No = Utilities.GenerateTicketNo("C", newPutOutStorageTicket.CreateTime.Value, maxRankOfToday + 1);
            }
            wmsEntities.SaveChanges();
            if (MessageBox.Show("生成出库单成功，是否查看出库单？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (this.toPutOutStorageTicketCallback == null)
                {
                    throw new Exception("toPutOutStorageTicketCallback不可以为空！");
                }
                this.toPutOutStorageTicketCallback("No", newPutOutStorageTicket.No);
            }
            if (!this.IsDisposed)
            {
                this.Invoke(new Action(() =>
                {
                    this.Close();
                }));
            }
        }

        private void buttonOK_MouseEnter(object sender, EventArgs e)
        {
            buttonOK.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonOK_MouseLeave(object sender, EventArgs e)
        {
            buttonOK.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonOK_MouseDown(object sender, MouseEventArgs e)
        {
            buttonOK.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private enum SelectButtonMode { SELECT_ALL, SELECT_NONE };
        SelectButtonMode selectButtonMode = SelectButtonMode.SELECT_ALL;
        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlMain.CurrentWorksheet;
            int[] unselectableIDs = (from j in this.jobTicketItemViews
                                     where j.ScheduledPutOutAmount >= j.RealAmount
                                     select j.ID).ToArray();
            if (selectButtonMode == SelectButtonMode.SELECT_ALL)
            {
                this.buttonSelectAll.Text = "全不选";
                selectButtonMode = SelectButtonMode.SELECT_NONE;
                for (int i = 0; i < this.validRows; i++)
                {
                    if(int.TryParse(worksheet[i,0].ToString(),out int id) == false)
                    {
                        continue;
                    }
                    if (unselectableIDs.Contains(id))
                    {
                        continue;
                    }
                    worksheet[i, 1] = true;
                }
            }
            else
            {
                this.buttonSelectAll.Text = "全选";
                selectButtonMode = SelectButtonMode.SELECT_ALL;
                for (int i = 0; i < this.validRows; i++)
                {
                    if(worksheet[i,1] as bool? != true)
                    {
                        continue;
                    }
                    worksheet[i, 1] = false;
                }
            }

        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            this.standardImportForm = new StandardImportForm<NewPutOutStorageTicketItemData>(
                newPutOutStorageTicketKeyNames,
                importHandler,
                null,
                "导入出库单条目"
                );
            //搜索默认值，首先精确匹配，没有再模糊匹配
            standardImportForm.AddDefaultValue("Unit", string.Format("SELECT DefaultShipmentUnit FROM Supply WHERE [No] = @SupplyNoOrComponentName AND ProjectID = {0} AND WarehouseID = {1} AND IsHistory=0;", this.projectID, this.warehouseID));
            standardImportForm.AddDefaultValue("UnitAmount", string.Format("SELECT DefaultShipmentUnitAmount FROM Supply WHERE [No] = @SupplyNoOrComponentName AND ProjectID = {0} AND WarehouseID = {1} AND IsHistory=0;", this.projectID, this.warehouseID));
            standardImportForm.AddDefaultValue("Unit", string.Format("SELECT DefaultShipmentUnit FROM SupplyView WHERE ComponentName = @SupplyNoOrComponentName AND ProjectID = {0} AND WarehouseID = {1} AND IsHistory=0;", this.projectID, this.warehouseID));
            standardImportForm.AddDefaultValue("UnitAmount", string.Format("SELECT DefaultShipmentUnitAmount FROM SupplyView WHERE ComponentName = @SupplyNoOrComponentName AND ProjectID = {0} AND WarehouseID = {1} AND IsHistory=0;", this.projectID, this.warehouseID));

            standardImportForm.AddDefaultValue("Unit", string.Format("SELECT TOP 2 DefaultShipmentUnit FROM Supply WHERE [No] LIKE '%'+@SupplyNoOrComponentName+'%' AND ProjectID = {0} AND WarehouseID = {1} AND IsHistory=0;", this.projectID, this.warehouseID));
            standardImportForm.AddDefaultValue("UnitAmount", string.Format("SELECT TOP 2 DefaultShipmentUnitAmount FROM Supply WHERE [No] LIKE '%'+@SupplyNoOrComponentName+'%' AND ProjectID = {0} AND WarehouseID = {1} AND IsHistory=0;", this.projectID, this.warehouseID));
            standardImportForm.AddDefaultValue("Unit", string.Format("SELECT TOP 2 DefaultShipmentUnit FROM SupplyView WHERE ComponentName LIKE '%'+@SupplyNoOrComponentName+'%' AND ProjectID = {0} AND WarehouseID = {1} AND IsHistory=0;", this.projectID, this.warehouseID));
            standardImportForm.AddDefaultValue("UnitAmount", string.Format("SELECT TOP 2 DefaultShipmentUnitAmount FROM SupplyView WHERE ComponentName LIKE '%'+@SupplyNoOrComponentName+'%' AND ProjectID = {0} AND WarehouseID = {1} AND IsHistory=0;", this.projectID, this.warehouseID));

            standardImportForm.AddAssociation("SupplyNoOrComponentName", string.Format("SELECT No,SupplierName FROM SupplyView WHERE ProjectID={0} AND WarehouseID = {1} AND IsHistory=0 AND No LIKE '%'+@value+'%'; ", this.projectID, this.warehouseID));
            standardImportForm.AddAssociation("SupplyNoOrComponentName", string.Format("SELECT DISTINCT ComponentName FROM SupplyView WHERE ProjectID={0} AND WarehouseID = {1} AND IsHistory=0 AND ComponentName LIKE '%'+@value+'%'; ", this.projectID, this.warehouseID));
            this.standardImportForm.Show();
        }

        private bool importHandler(List<NewPutOutStorageTicketItemData> results, Dictionary<string, string[]> unimportedColumns)
        {
            var worksheet = this.reoGridControlMain.CurrentWorksheet;
            //已经勾选的项
            Dictionary<int, decimal> checkedIDAndAmount = new Dictionary<int, decimal>();
            //本次导入的项
            Dictionary<int, decimal> idAndAmount = new Dictionary<int, decimal>();
            //项和单位的对应关系
            Dictionary<int, decimal> idAndUnitAmount = new Dictionary<int, decimal>();
            //统计已经选中的项
            for (int i = 0; i < this.validRows; i++)
            {
                if ((worksheet[i, 1] as bool? ?? false) == false)
                {
                    continue;
                }
                int id = int.Parse(worksheet[i, 0].ToString());
                if (decimal.TryParse(worksheet[i, 2] == null ? "" : worksheet[i, 2].ToString(), out decimal amount) == false)
                {
                    amount = 0;
                }
                if (checkedIDAndAmount.ContainsKey(id))
                {
                    checkedIDAndAmount[id] += amount;
                }
                else
                {
                    checkedIDAndAmount.Add(id, amount);
                }
            }
            try
            {
                Dictionary<string, bool> dicImportedItems = new Dictionary<string, bool>(); //再特么加需求我特么打死你
                WMSEntities wmsEntities = new WMSEntities();
                JobTicketItemView[] jobTicketItemViews = (from j in wmsEntities.JobTicketItemView where j.JobTicketID == this.jobTicketID select j).ToArray();
                for (int i = 0; i < results.Count; i++)
                {
                    if (dicImportedItems.ContainsKey(results[i].SupplyNoOrComponentName))
                    {
                        MessageBox.Show("行" + (i + 1) + "：请不要录入重复的零件\"" + results[i].SupplyNoOrComponentName + "\"", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else
                    {
                        dicImportedItems.Add(results[i].SupplyNoOrComponentName, true);
                    }
                    string supplyNoOrComponentName = results[i].SupplyNoOrComponentName;
                    decimal scheduleAmountNoUnit = results[i].SchedulePutOutAmount * results[i].UnitAmount;
                    //封装的根据 零件名/供货代号 获取 零件/供货的函数
                    if (Utilities.GetSupplyOrComponentAmbiguous(supplyNoOrComponentName, out DataAccess.Component component, out Supply supply, out string errorMessage, -1,wmsEntities) == false)
                    {
                        MessageBox.Show(string.Format("行{0}：{1}", i + 1, errorMessage), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    string realName = null;
                    List<JobTicketItemView> selectedItems = null;
                    if (supply != null)
                    {
                        realName = supply.No;
                        selectedItems = (from s in wmsEntities.JobTicketItemView
                                         where s.JobTicketID == this.jobTicketID
                                         && s.SupplyID == supply.ID
                                         orderby s.StockInfoInventoryDate ascending
                                         select s).ToList();
                    }
                    else if (component != null)
                    {
                        realName = component.Name;
                        selectedItems = (from s in wmsEntities.JobTicketItemView
                                         where s.JobTicketID == this.jobTicketID
                                         && s.ComponentID == component.ID
                                         orderby s.StockInfoInventoryDate ascending
                                         select s).ToList();
                    }
                    if(selectedItems.Count == 0)
                    {
                        MessageBox.Show(string.Format("此作业单中不包含{0}！请检查输入", realName), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    //记录每一项的剩余可分配数量
                    Dictionary<int, decimal> restAmounts = new Dictionary<int, decimal>();
                    //记录每一项的剩余数量，并把单位记录到单位对应关系里
                    selectedItems.ForEach((item) =>
                    {
                        if (idAndUnitAmount.ContainsKey(item.ID) == false)
                        {
                            idAndUnitAmount.Add(item.ID, item.UnitAmount ?? 1);
                        }

                        decimal restAmount = (item.RealAmount - (item.ScheduledPutOutAmount ?? 0)) ?? 0;
                        if (idAndAmount.ContainsKey(item.ID))
                        {
                            restAmount -= idAndAmount[item.ID];
                        }
                        if (checkedIDAndAmount.ContainsKey(item.ID))
                        {
                            restAmount -= checkedIDAndAmount[item.ID];
                        }
                        restAmounts.Add(item.ID, restAmount);
                    });
                    //删除所有可分配数量为0的项
                    selectedItems.RemoveAll((item) => restAmounts[item.ID] <= 0);
                    decimal totalStockAmountNoUnit = restAmounts.Sum((item) =>
                    {
                        if (item.Value > 0) return item.Value * idAndUnitAmount[item.Key];
                        else return 0;
                    });

                    if (scheduleAmountNoUnit > totalStockAmountNoUnit)
                    {
                        MessageBox.Show(string.Format("行{0}：零件{1} 在此翻包作业单剩余待分配翻包数量不足，剩余量：{2}", i + 1, realName, Utilities.DecimalToString(totalStockAmountNoUnit)), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    decimal curAmountNoUnit = 0;
                    for (int j = 0; j < selectedItems.Count; j++)
                    {
                        decimal curItemRestAmount = restAmounts[selectedItems[j].ID];
                        //当前项的剩余数量（不带单位）
                        decimal curItemRestAmountNoUnit = curItemRestAmount * idAndUnitAmount[selectedItems[j].ID];
                        //当前项的剩余数量小于要分配的数量
                        if (curAmountNoUnit + curItemRestAmountNoUnit < scheduleAmountNoUnit)
                        {
                            if (idAndAmount.ContainsKey(selectedItems[j].ID))
                            {
                                idAndAmount[selectedItems[j].ID] += curItemRestAmount;
                            }
                            else
                            {
                                idAndAmount.Add(selectedItems[j].ID, curItemRestAmount);
                            }
                            curAmountNoUnit += curItemRestAmountNoUnit;
                        }
                        else //当前项的剩余数量大于等于要分配的数量
                        {
                            if (idAndAmount.ContainsKey(selectedItems[j].ID))
                            {
                                idAndAmount[selectedItems[j].ID] += ((scheduleAmountNoUnit - curAmountNoUnit) / selectedItems[j].UnitAmount ?? 1);
                            }
                            else
                            {
                                idAndAmount.Add(selectedItems[j].ID, ((scheduleAmountNoUnit - curAmountNoUnit) / selectedItems[j].UnitAmount ?? 1));
                            }
                            curAmountNoUnit = scheduleAmountNoUnit;
                            break;
                        }
                    }
                }
                this.CheckItemsAndFillScheduleAmountByIDs(idAndAmount);
                this.standardImportForm.Close();
                MessageBox.Show("导入成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            catch
            {
                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void CheckItemsAndFillScheduleAmountByIDs(Dictionary<int, decimal> idAndAmount)
        {
            var worksheet = this.reoGridControlMain.CurrentWorksheet;
            for (int i = 0; i < this.validRows; i++)
            {
                if (int.TryParse(worksheet[i, 0].ToString(), out int id) == false)
                {
                    continue;
                }
                if (idAndAmount.ContainsKey(id) == false) continue;
                decimal amount = idAndAmount[id];
                bool oriChecked = worksheet[i, 1] as bool? ?? false;
                worksheet[i, 1] = true;
                //如果之前没点击，则覆盖数量。否则累加数量
                if (oriChecked && decimal.TryParse(worksheet[i, 2].ToString(), out decimal oriAmount))
                {
                    worksheet[i, 2] = Utilities.DecimalToString(amount + oriAmount);
                }
                else
                {
                    worksheet[i, 2] = Utilities.DecimalToString(amount);
                }
            }
        }
    }
}