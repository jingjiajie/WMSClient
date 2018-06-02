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
using unvell.ReoGrid;
using unvell.ReoGrid.CellTypes;

namespace WMS.UI
{
    public partial class FormJobTicketNew : Form
    {
        private int shipmentTicketID = -1;
        private int userID = -1;
        private int projectID = -1;
        private int warehouseID = -1;

        private int curPersonID = -1;

        private Action<string,string> toJobTicketCallback = null;
        private int[] editableColumns = new int[] { 1, 2 };
        private int validRows { get => shipmentTicketItems == null ? 0 : shipmentTicketItems.Length; }
        private ShipmentTicketItemView[] shipmentTicketItems = null;

        private static KeyName[] newJobTicketKeyNames =
        {
            new KeyName(){Key="SupplyNoOrComponentName",Name="零件代号/名称",NotNull=true},
            new KeyName(){Key="ScheduleJobAmount",Name="计划翻包数量",NotNull=true,Positive=true},
            new KeyName(){Key="Unit",Name="单位",NotNull=true},
            new KeyName(){Key="UnitAmount",Name="单位数量",NotNull=true,Positive=true}
        };

        class NewJobTicketItemData
        {
            private string supplyNoOrComponentName;
            private decimal scheduleJobAmount;
            private decimal unitAmount;
            private string unit;

            public string SupplyNoOrComponentName { get => supplyNoOrComponentName; set => supplyNoOrComponentName = value; }
            public decimal ScheduleJobAmount { get => scheduleJobAmount; set => scheduleJobAmount = value; }
            public decimal UnitAmount { get => unitAmount; set => unitAmount = value; }
            public string Unit { get => unit; set => unit = value; }
        }

        private StandardImportForm<NewJobTicketItemData> standardImportForm = null;

        public void SetToJobTicketCallback(Action<string,string> callback)
        {
            this.toJobTicketCallback = callback;
        }

        public FormJobTicketNew(int shipmentTicketID,int userID,int projectID,int warehouseID)
        {
            InitializeComponent();
            this.shipmentTicketID = shipmentTicketID;
            this.userID = userID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        private void FormJobTicketNew_Load(object sender, EventArgs e)
        {
            this.InitComponents();
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

        private void InitComponents()
        {
            BindButtonStyle(this.buttonSelectAll);
            BindButtonStyle(this.buttonImport);
            Utilities.InitReoGrid(this.reoGridControlMain, ShipmentTicketItemViewMetaData.KeyNames,WorksheetSelectionMode.Cell);
            this.reoGridControlMain.SetSettings(WorkbookSettings.View_ShowSheetTabControl, false);
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet.InsertColumns(1, 2);
            worksheet.ColumnHeaders[1].Text = "选择";
            worksheet.ColumnHeaders[2].Text = "计划翻包数量";
            worksheet.BeforeCellEdit += (s, e) =>
            {
                e.IsCancelled = !(editableColumns.Contains(e.Cell.Column) && e.Cell.Row < validRows);
            };

            Utilities.CreateEditPanel(this.tableLayoutEditPanel, JobTicketViewMetaData.KeyNames);
            ShipmentTicket shipmentTicket = null;
            User user = null;
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                shipmentTicket = (from s in wmsEntities.ShipmentTicket
                                  where s.ID == shipmentTicketID
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
            if (shipmentTicket == null)
            {
                MessageBox.Show("发货单信息不存在，请刷新显示", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if (user == null)
            {
                MessageBox.Show("登录失效，操作失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Text = string.Format("生成翻包作业单（发货单号：{0}）", shipmentTicket.No);
            this.Controls.Find("textBoxCreateUserUsername", true)[0].Text = user.Username;
            this.Controls.Find("textBoxCreateTime", true)[0].Text = DateTime.Now.ToString();
            this.Controls.Find("textBoxShipmentTicketNo", true)[0].Text = shipmentTicket.No;
            this.Controls.Find("textBoxPersonName", true)[0].Click += textBoxPersonName_Click;
            this.Search();
        }

        private void textBoxPersonName_Click(object sender, EventArgs e)
        {
            TextBox textBoxPersonName = (TextBox)this.Controls.Find("textBoxPersonName", true)[0];
            FormSelectPerson formSelectPerson = new FormSelectPerson();
            formSelectPerson.SetSelectFinishedCallback((id)=>
            {
                WMSEntities wmsEntities = new WMSEntities();
                Person person = (from p in wmsEntities.Person
                                 where p.ID == id
                                 select p).FirstOrDefault();
                if(person == null)
                {
                    MessageBox.Show("人员不存在，请重新查询","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.curPersonID = id;
                if (this.IsDisposed) return;
                this.Invoke(new Action(() =>
                {
                    textBoxPersonName.Text = person.Name;
                }));
            });
            formSelectPerson.Show();
        }

        private void Search()
        {
            WMSEntities wmsEntities = new WMSEntities();
            ShipmentTicketItemView[] shipmentTicketItemViews = (from s in wmsEntities.ShipmentTicketItemView
                                                                where s.ShipmentTicketID == this.shipmentTicketID
                                                                select s).ToArray();
            this.shipmentTicketItems = shipmentTicketItemViews;
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet.DeleteRangeData(RangePosition.EntireRange);
            worksheet.Rows = (shipmentTicketItemViews.Length < 10 ? 10 : shipmentTicketItemViews.Length);
            //给第二列上颜色，加上边框
            worksheet.SetRangeBorders(0, 2, validRows, 1, BorderPositions.All, RangeBorderStyle.SilverSolid);

            if (shipmentTicketItemViews.Length == 0)
            {
                worksheet[0, 1] = "没有查询到符合条件的记录";
            }
            for (int i = 0; i < shipmentTicketItemViews.Length; i++)
            {
                ShipmentTicketItemView cur = shipmentTicketItemViews[i];
                if (cur.ShipmentAmount <= cur.ScheduledJobAmount)
                {
                    worksheet.Cells[i, 1].Style.BackColor = Color.LightGray;
                    worksheet.Cells[i, 1].IsReadOnly = true;
                }
                else
                {
                    worksheet[i, 1] = new CheckBoxCell(false); //显示复选框
                }
                //上颜色
                worksheet.Cells[i, 2].Style.BackColor = Color.AliceBlue;
                //计划翻包数量
                worksheet.Cells[i, 2].DataFormat = unvell.ReoGrid.DataFormat.CellDataFormatFlag.Text;
                decimal defaultScheduledJobAmount = (cur.ShipmentAmount - (cur.ScheduledJobAmount ?? 0)) ?? 0; //计划翻包数量默认等于发货数量-已经计划翻包过的数量
                worksheet[i, 2] = Utilities.DecimalToString(defaultScheduledJobAmount < 0 ? 0 : defaultScheduledJobAmount);
                object[] columns = Utilities.GetValuesByPropertieNames(cur, (from kn in ShipmentTicketItemViewMetaData.KeyNames where kn.Visible==true || kn.Key=="ID" select kn.Key).ToArray());
                int offsetColumn = 0;
                for (int j = 0; j < columns.Length; j++)
                {
                    while (this.editableColumns.Contains(j + offsetColumn)) offsetColumn++;
                    if (columns[j] == null) continue;
                    worksheet.Cells[i, offsetColumn + j].DataFormat = unvell.ReoGrid.DataFormat.CellDataFormatFlag.Text;
                    string text = null;
                    if(columns[j] is decimal)
                    {
                        text = Utilities.DecimalToString((decimal)columns[j]);
                    }
                    else
                    {
                        text = columns[j].ToString();
                    }
                    worksheet[i, offsetColumn + j] = text;
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.reoGridControlMain.CurrentWorksheet.EndEdit(new EndEditReason());
            JobTicket newJobTicket = new JobTicket();
            string errorMessage = null;
            if (Utilities.CopyTextBoxTextsToProperties(this, newJobTicket, JobTicketViewMetaData.KeyNames, out errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            WMSEntities wmsEntities = new WMSEntities();
            wmsEntities.JobTicket.Add(newJobTicket);

            ShipmentTicket shipmentTicket = (from s in wmsEntities.ShipmentTicket
                                             where s.ID == shipmentTicketID
                                             select s).FirstOrDefault();
            if (shipmentTicket == null)
            {
                MessageBox.Show("发货单不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            shipmentTicket.State = ShipmentTicketViewMetaData.STRING_STATE_PART_ASSIGNED_JOB;
            newJobTicket.State = JobTicketViewMetaData.STRING_STATE_UNFINISHED;
            newJobTicket.ShipmentTicketID = shipmentTicket.ID;
            newJobTicket.ProjectID = this.projectID;
            newJobTicket.WarehouseID = this.warehouseID;
            newJobTicket.CreateUserID = this.userID;
            newJobTicket.CreateTime = DateTime.Now;
            newJobTicket.PersonID = this.curPersonID == -1 ? null : (int?)this.curPersonID;

            var worksheet = this.reoGridControlMain.Worksheets[0];
            for (int i = 0; i < worksheet.Rows; i++)
            {
                if ((worksheet[i, 1] as bool? ?? false) == false) continue;
                int shipmentTicketItemID = int.Parse(worksheet[i, 0].ToString());
                ShipmentTicketItem shipmentTicketItem = (from s in wmsEntities.ShipmentTicketItem
                                                         where s.ID == shipmentTicketItemID
                                                         select s).FirstOrDefault();
                if (shipmentTicketItem == null)
                {
                    MessageBox.Show("发货单条目不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var jobTicketItem = new JobTicketItem();
                if (Utilities.CopyTextToProperty(worksheet[i, 2]?.ToString(), "ScheduledAmount", jobTicketItem, JobTicketItemViewMetaData.KeyNames, out errorMessage) == false)
                {
                    MessageBox.Show(string.Format("行{0}：{1}", i + 1, errorMessage), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (jobTicketItem.ScheduledAmount + (shipmentTicketItem.ScheduledJobAmount ?? 0) > shipmentTicketItem.ShipmentAmount)
                {
                    MessageBox.Show(string.Format("行{0}：{1}", i + 1, "计划翻包数量总和不可以超过发货数量！"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                jobTicketItem.StockInfoID = shipmentTicketItem.StockInfoID;
                jobTicketItem.ShipmentTicketItemID = shipmentTicketItem.ID;
                jobTicketItem.State = JobTicketItemViewMetaData.STRING_STATE_UNFINISHED;
                jobTicketItem.Unit = shipmentTicketItem.Unit;
                jobTicketItem.UnitAmount = shipmentTicketItem.UnitAmount;
                jobTicketItem.RealAmount = 0;
                shipmentTicketItem.ScheduledJobAmount = (shipmentTicketItem.ScheduledJobAmount ?? 0) + jobTicketItem.ScheduledAmount;
                newJobTicket.JobTicketItem.Add(jobTicketItem);
            }
            if (newJobTicket.JobTicketItem.Count == 0)
            {
                MessageBox.Show("至少选择一项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(newJobTicket.JobTicketNo))
            {
                DateTime createDay = new DateTime(newJobTicket.CreateTime.Value.Year, newJobTicket.CreateTime.Value.Month, newJobTicket.CreateTime.Value.Day);
                DateTime nextDay = createDay.AddDays(1);
                int maxRankOfToday = Utilities.GetMaxTicketRankOfDay((from j in wmsEntities.JobTicket
                                                                      where j.CreateTime >= createDay && j.CreateTime < nextDay
                                                                      select j.JobTicketNo).ToArray());
                newJobTicket.JobTicketNo = Utilities.GenerateTicketNo("Z", newJobTicket.CreateTime.Value, maxRankOfToday + 1);
            }
            wmsEntities.SaveChanges();
            ShipmentTicketUtilities.UpdateShipmentTicketStateSync(this.shipmentTicketID, wmsEntities);
            if (MessageBox.Show("生成作业单成功，是否查看作业单？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                if (this.toJobTicketCallback == null)
                {
                    throw new Exception("ToJobTicketCallback不允许为空！");
                }
                this.toJobTicketCallback("ShipmentTicketNo", shipmentTicket.No);
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

        private enum SelectButtonMode { SELECT_ALL,SELECT_NONE};
        SelectButtonMode selectButtonMode = SelectButtonMode.SELECT_ALL;
        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlMain.CurrentWorksheet;
            int[] unselectableIDs = (from s in this.shipmentTicketItems
                                     where s.ScheduledJobAmount >= s.ShipmentAmount
                                     select s.ID).ToArray();
            if (selectButtonMode == SelectButtonMode.SELECT_ALL)
            {
                this.buttonSelectAll.Text = "全不选";
                selectButtonMode = SelectButtonMode.SELECT_NONE;
                for (int i = 0; i < this.validRows; i++)
                {
                    if (int.TryParse(worksheet[i, 0].ToString(), out int id) == false)
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
                    if (worksheet[i, 1] as bool? == true)
                    {
                        worksheet[i, 1] = false;
                    }
                }
            }

        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            this.standardImportForm = new StandardImportForm<NewJobTicketItemData>(
                newJobTicketKeyNames,
                importHandler,
                null,
                "导入作业单条目"
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

        private bool importHandler(List<NewJobTicketItemData> results, Dictionary<string, string[]> unimportedColumns)
        {
            var worksheet = this.reoGridControlMain.CurrentWorksheet;
            int[] showedIDs = new int[this.validRows];
            //已经被选中的项和数量
            Dictionary<int, decimal> checkedIDAndAmount = new Dictionary<int, decimal>();
            //本次导入填写的项和数量
            Dictionary<int, decimal> idAndAmount = new Dictionary<int, decimal>();
            //项和单位数量的对应关系
            Dictionary<int, decimal> idAndUnitAmount = new Dictionary<int, decimal>();
            //统计已经显示的项和选中的项
            for (int i = 0; i < this.validRows; i++)
            {
                int id = int.Parse(worksheet[i, 0].ToString());
                showedIDs[i] = id;
                if ((worksheet[i,1] as bool? ?? false)==false)
                {
                    continue;
                }
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
                WMSEntities wmsEntities = new WMSEntities();
                Dictionary<String, bool> dicImportedItems = new Dictionary<string, bool>(); //记录一下已经导入的零件，不允许重复导入（管它零件号还是零件名，凑合着吧。反正二期要重写了）
                for (int i = 0; i < results.Count; i++)
                {
                    if (dicImportedItems.ContainsKey(results[i].SupplyNoOrComponentName))
                    {
                        MessageBox.Show("行" + (i + 1) + "：请不要录入重复的零件\"" + results[i].SupplyNoOrComponentName + "\"","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else
                    {
                        dicImportedItems.Add(results[i].SupplyNoOrComponentName, true);
                    }
                    string supplyNoOrComponentName = results[i].SupplyNoOrComponentName;
                    decimal scheduleAmountNoUnit = results[i].ScheduleJobAmount * results[i].UnitAmount;
                    //封装的根据 零件名/供货代号 获取 零件/供货的函数
                    if (Utilities.GetSupplyOrComponentAmbiguous(supplyNoOrComponentName, out DataAccess.Component component, out Supply supply, out string errorMessage, -1,wmsEntities) == false)
                    {
                        MessageBox.Show(string.Format("行{0}：{1}", i + 1, errorMessage), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    string realName = null;
                    List<ShipmentTicketItemView> selectedItems = null;
                    //根据零件，或者供货，来查询发货单条目
                    if (supply != null)
                    {
                        realName = supply.No;
                        selectedItems = (from s in wmsEntities.ShipmentTicketItemView
                                         where s.ShipmentTicketID == this.shipmentTicketID
                                         && s.SupplyID == supply.ID
                                         && showedIDs.Contains(s.ID)
                                         orderby s.StockInfoInventoryDate ascending
                                         select s).ToList();
                    }
                    else if (component != null)
                    {
                        realName = component.Name;
                        selectedItems = (from s in wmsEntities.ShipmentTicketItemView
                                         where s.ShipmentTicketID == this.shipmentTicketID
                                         && s.ComponentID == component.ID
                                         && showedIDs.Contains(s.ID)
                                         orderby s.StockInfoInventoryDate ascending
                                         select s).ToList();
                    }
                    if(selectedItems.Count == 0)
                    {
                        MessageBox.Show(string.Format("此发货单中不包含{0}！请检查输入", realName), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                        decimal restAmount = (item.ShipmentAmount - (item.ScheduledJobAmount ?? 0)) ?? 0;
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
                    if(scheduleAmountNoUnit > totalStockAmountNoUnit)
                    {
                        MessageBox.Show(string.Format("行{0}：零件{1} 在此发货单剩余待分配翻包数量不足，剩余量：{2}个", i + 1,realName, Utilities.DecimalToString(totalStockAmountNoUnit)), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    decimal curAmountNoUnit = 0; //累计分配了多少
                    for (int j = 0; j < selectedItems.Count; j++)
                    {
                        //当前项的剩余数量
                        decimal curItemRestAmount = restAmounts[selectedItems[j].ID];
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
                            decimal amount = ((scheduleAmountNoUnit - curAmountNoUnit) / selectedItems[j].UnitAmount ?? 1);
                            if (idAndAmount.ContainsKey(selectedItems[j].ID))
                            {
                                idAndAmount[selectedItems[j].ID] += amount;
                            }
                            else
                            {
                                idAndAmount.Add(selectedItems[j].ID,amount);
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

        private void CheckItemsAndFillScheduleAmountByIDs(Dictionary<int,decimal> idAndAmount)
        {
            var worksheet = this.reoGridControlMain.CurrentWorksheet;
            for(int i = 0; i < this.validRows; i++)
            {
                if(int.TryParse(worksheet[i, 0].ToString(), out int id) == false)
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
