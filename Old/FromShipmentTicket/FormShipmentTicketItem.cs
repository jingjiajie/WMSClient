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
    public partial class FormShipmentTicketItem : Form
    {
        private int shipmentTicketID = -1;
        Action shipmentTicketStateChangedCallback = null;
        private int projectID = -1;
        private int warehouseID = -1;

        private int curStockInfoID = -1;
        private int curConfirmPersonID = -1;
        private int curJobPersonID = -1;

        private TextBox textBoxConfirmPersonName = null;
        private TextBox textBoxJobPersonName = null;
        private TextBox textBoxComponentName = null;
        private FormSelectStockInfo formSelectStockInfo = null;

        private StandardImportForm<ShipmentTicketItem> standardImportForm = null;

        private KeyName[] usedKeyNames = (from kn in ShipmentTicketItemViewMetaData.KeyNames
                                            where kn.Visible == true || kn.Key == "ID"
                                            select kn).ToArray();

        public FormShipmentTicketItem(int shipmentTicketID,int projectID,int warehouseID)
        {
            InitializeComponent();
            this.shipmentTicketID = shipmentTicketID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        public void SetShipmentTicketStateChangedCallback(Action jobTicketStateChangedCallback)
        {
            this.shipmentTicketStateChangedCallback = jobTicketStateChangedCallback;
        }

        private void FormShipmentTicketItem_Load(object sender, EventArgs e)
        {
            ShipmentTicket shipmentTicket = null;
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                shipmentTicket = (from s in wmsEntities.ShipmentTicket where s.ID == shipmentTicketID select s).FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("加载数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if(shipmentTicket == null)
            {
                MessageBox.Show("发货单信息不存在，请刷新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            InitComponents();
            this.Search();
        }

        private void InitComponents()
        {
            //初始化表格
            this.reoGridControlMain.SetSettings(WorkbookSettings.View_ShowSheetTabControl, false);
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            worksheet.FocusPosChanged += this.worksheet_FocusPosChanged;

            for (int i = 0; i < usedKeyNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = usedKeyNames[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = usedKeyNames[i].Visible;
            }
            worksheet.Columns = usedKeyNames.Length; //限制表的长度

            Utilities.CreateEditPanel(this.tableLayoutPanelProperties, usedKeyNames);

            this.textBoxComponentName = (TextBox)this.Controls.Find("textBoxComponentName", true)[0];
            textBoxComponentName.Click += textBoxComponentName_Click;
            textBoxComponentName.ReadOnly = true;
            textBoxComponentName.BackColor = Color.White;

            FormSelectPerson.DefaultPosition = FormBase.Position.SHIPMENT;

            this.textBoxJobPersonName = (TextBox)this.Controls.Find("textBoxJobPersonName", true)[0];
            textBoxJobPersonName.ReadOnly = true;
            textBoxJobPersonName.BackColor = Color.White;
            textBoxJobPersonName.Click += textBoxJobPersonName_Click;

            this.textBoxConfirmPersonName = (TextBox)this.Controls.Find("textBoxConfirmPersonName", true)[0];
            textBoxConfirmPersonName.ReadOnly = true;
            textBoxConfirmPersonName.BackColor = Color.White;
            textBoxConfirmPersonName.Click += textBoxConfirmPersonName_Click;
        }

        private void textBoxConfirmPersonName_Click(object sender, EventArgs e)
        {
            FormSelectPerson form = new FormSelectPerson();
            form.SetSelectFinishedCallback((id)=>
            {
                this.curConfirmPersonID = id;
                if (!this.IsDisposed)
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    Person person = (from p in wmsEntities.Person
                                     where p.ID == id
                                     select p).FirstOrDefault();
                    if(person == null)
                    {
                        MessageBox.Show("选中人员不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.curConfirmPersonID = id;
                    this.textBoxConfirmPersonName.Text = person.Name;
                }
            });
            form.Show();
        }

        private void textBoxJobPersonName_Click(object sender, EventArgs e)
        {
            FormSelectPerson form = new FormSelectPerson();
            
            form.SetSelectFinishedCallback((id) =>
            {
                this.curJobPersonID = id;
                if (!this.IsDisposed)
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    Person person = (from p in wmsEntities.Person
                                     where p.ID == id
                                     select p).FirstOrDefault();
                    if (person == null)
                    {
                        MessageBox.Show("选中人员不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    this.curJobPersonID = id;
                    this.textBoxJobPersonName.Text = person.Name;
                }
            });
            form.Show();
        }

        private void textBoxComponentName_Click(object sender, EventArgs e)
        {
            if (this.formSelectStockInfo == null)
            {
                this.formSelectStockInfo = new FormSelectStockInfo(this.projectID, this.warehouseID);

                formSelectStockInfo.SetSelectFinishedCallback((selectedStockInfoID) =>
                {
                    this.curStockInfoID = selectedStockInfoID;
                    if (!this.IsDisposed)
                    {
                        this.Invoke(new Action(() =>
                        {
                            textBoxComponentName.Text = "加载中...";
                        }));
                    }
                    new Thread(new ThreadStart(() =>
                    {
                        WMSEntities wmsEntities = new WMSEntities();
                        StockInfoView stockInfoView = null;
                        Supply supply = null;
                        try
                        {
                            stockInfoView = (from s in wmsEntities.StockInfoView
                                             where s.ID == selectedStockInfoID
                                             select s).FirstOrDefault();
                            supply = (from s in wmsEntities.Supply where s.ID == stockInfoView.SupplyID select s).FirstOrDefault();
                        }
                        catch
                        {
                            MessageBox.Show("刷新数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        this.Invoke(new Action(() =>
                        {
                            Utilities.CopyPropertiesToTextBoxes(stockInfoView, this);
                            TextBox textBoxUnit = (TextBox)this.Controls.Find("textBoxUnit",true)[0];
                            TextBox textBoxUnitAmount = (TextBox)this.Controls.Find("textBoxUnitAmount", true)[0];
                            if (supply != null && string.IsNullOrWhiteSpace(textBoxUnit.Text) && string.IsNullOrWhiteSpace(textBoxUnitAmount.Text))
                            {
                                textBoxUnit.Text = supply.DefaultShipmentUnit;
                                if (supply.DefaultShipmentUnitAmount != null)
                                {
                                    textBoxUnitAmount.Text = Utilities.DecimalToString(supply.DefaultShipmentUnitAmount.Value);
                                }
                            }

                        }));
                    })).Start();
                });
            }

            formSelectStockInfo.ShowDialog();
        }

        private void worksheet_FocusPosChanged(object sender, unvell.ReoGrid.Events.CellPosEventArgs e)
        {
            this.RefreshTextBoxes();
        }

        private void ClearTextBoxes()
        {
            foreach (Control control in this.tableLayoutPanelProperties.Controls)
            {
                if (control is TextBox)
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
                this.buttonAdd.Text = "添加条目";
                this.curStockInfoID = -1;
                this.curConfirmPersonID = -1;
                this.curJobPersonID = -1;
                //为编辑框填写默认值
                Utilities.FillTextBoxDefaultValues(this.tableLayoutPanelProperties, usedKeyNames);
                return;
            }
            this.buttonAdd.Text = "复制条目";
            int id = ids[0];
            ShipmentTicketItemView shipmentTicketItemView = null;
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                var tmp = (from s in wmsEntities.ShipmentTicketItemView
                           where s.ID == id
                           select s);
                shipmentTicketItemView = tmp.FirstOrDefault();
            }
            catch
            {
                MessageBox.Show("刷新数据失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (shipmentTicketItemView == null)
            {
                MessageBox.Show("系统错误，未找到相应发货单项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (shipmentTicketItemView.StockInfoID != null)
            {
                this.curStockInfoID = shipmentTicketItemView.StockInfoID.Value;
            }
            else
            {
                this.curStockInfoID = -1;
            }
            Utilities.CopyPropertiesToTextBoxes(shipmentTicketItemView, this);
            Utilities.CopyPropertiesToComboBoxes(shipmentTicketItemView, this);
        }


        private void Search(int selectID = -1)
        {
            WMSEntities wmsEntities = new WMSEntities();
            var worksheet = this.reoGridControlMain.Worksheets[0];

            worksheet[0, 1] = "加载中...";
            new Thread(new ThreadStart(() =>
            {
                ShipmentTicketItemView[] shipmentTicketItemViews = null;
                try
                {
                    shipmentTicketItemViews = (from s in wmsEntities.ShipmentTicketItemView
                                                                        where s.ShipmentTicketID == this.shipmentTicketID
                                                                        orderby s.ID descending
                                                                        select s).ToArray();
                }
                catch
                {
                    MessageBox.Show("查询数据失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (this.IsDisposed == false)
                    {
                        this.Invoke(new Action(this.Close));
                    }
                    return;
                }

                this.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "加载完成";
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    worksheet.Rows = shipmentTicketItemViews.Length < 10 ? 10 : shipmentTicketItemViews.Length;
                    if (shipmentTicketItemViews.Length == 0)
                    {
                        worksheet[0, 1] = "没有符合条件的记录";
                    }
                    for (int i = 0; i < shipmentTicketItemViews.Length; i++)
                    {
                        var curShipmentTicketViews = shipmentTicketItemViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(
                            curShipmentTicketViews,
                            (from kn in usedKeyNames where(kn.Visible == true || kn.Key=="ID") select kn.Key).ToArray()
                            );
                        for (int j = 0; j < columns.Length; j++)
                        {
                            worksheet[i, j] = columns[j] == null ? "" : columns[j].ToString();
                        }
                    }
                    if(selectID != -1)
                    {
                        Utilities.SelectLineByID(this.reoGridControlMain, selectID);
                    }
                    this.Invoke(new Action(this.RefreshTextBoxes));
                }));
            })).Start();
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            const string STRING_FINISHED = "已完成";
            int[] selectedIDs = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择您要操作的条目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            new Thread(new ThreadStart(() =>
            {
                WMSEntities wmsEntities = new WMSEntities();
                try
                {
                    //将状态置为已完成
                    foreach (int id in selectedIDs)
                    {
                        wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE ShipmentTicketItem SET State = '{0}' WHERE ID = {1};", STRING_FINISHED, id));
                    }
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //如果作业单中所有条目都完成，询问是否将作业单标记为完成
                int unfinishedShipmentTicketItemCount = wmsEntities.Database.SqlQuery<int>(String.Format("SELECT COUNT(*) FROM ShipmentTicketItem WHERE ShipmentTicketID = {0} AND State <> '{1}'", this.shipmentTicketID, STRING_FINISHED)).Single();
                if (unfinishedShipmentTicketItemCount == 0)
                {
                    if (MessageBox.Show("检测到所有的零件都已经收货完成，是否将出库单状态更新为完成？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE ShipmentTicket SET State = '{0}' WHERE ID = {1}", STRING_FINISHED, this.shipmentTicketID));
                            wmsEntities.SaveChanges();
                        }
                        catch
                        {
                            MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    this.shipmentTicketStateChangedCallback?.Invoke();
                }

                this.Invoke(new Action(() =>
                {
                    this.Search();
                }));
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            })).Start();

        }


        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (this.curStockInfoID == -1)
            {
                MessageBox.Show("请选择零件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ShipmentTicketItem shipmentTicketItem = new ShipmentTicketItem();
            shipmentTicketItem.StockInfoID = this.curStockInfoID;
            shipmentTicketItem.ShipmentTicketID = this.shipmentTicketID;
            shipmentTicketItem.ConfirmPersonID = this.curConfirmPersonID == -1 ? null : (int?)this.curConfirmPersonID;
            shipmentTicketItem.JobPersonID = this.curJobPersonID == -1 ? null : (int?)this.curJobPersonID;


            if (Utilities.CopyTextBoxTextsToProperties(this, shipmentTicketItem, ShipmentTicketItemViewMetaData.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (shipmentTicketItem.ShipmentAmount < shipmentTicketItem.ScheduledJobAmount)
            {
                MessageBox.Show("发货数量不能小于已分配翻包作业数量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            WMSEntities wmsEntities = new WMSEntities();
            try
            {
                //不扣除库存发货区数量，但记录库存已分配发货数量
                StockInfo stockInfo = (from s in wmsEntities.StockInfo
                                       where s.ID == shipmentTicketItem.StockInfoID
                                       select s).FirstOrDefault();
                if (stockInfo == null)
                {
                    MessageBox.Show("零件不存在，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                decimal shipmentableAmount = (stockInfo.ShipmentAreaAmount ?? 0) - stockInfo.ScheduledShipmentAmount;
                shipmentableAmount = shipmentableAmount < 0 ? 0 : shipmentableAmount;
                if (shipmentTicketItem.ShipmentAmount * shipmentTicketItem.UnitAmount > shipmentableAmount)
                {
                    MessageBox.Show("添加失败，库存可发货数量不足！可发货数：" + Utilities.DecimalToString(shipmentableAmount), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                stockInfo.ScheduledShipmentAmount += shipmentTicketItem.ShipmentAmount * shipmentTicketItem.UnitAmount ?? 0;
                wmsEntities.ShipmentTicketItem.Add(shipmentTicketItem);
                wmsEntities.SaveChanges();
            }
            catch
            {
                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Invoke(new Action(() =>
            {
                this.Search(shipmentTicketItem.ID);
            }));
            MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if(ids.Length != 1)
            {
                MessageBox.Show("请选择一项进行修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(this.curStockInfoID == -1)
            {
                MessageBox.Show("请选择零件！","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
                WMSEntities wmsEntities = new WMSEntities();
                int id = ids[0];
                ShipmentTicketItem shipmentTicketItem = null;
                try
                {
                    shipmentTicketItem = (from s in wmsEntities.ShipmentTicketItem where s.ID == id select s).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (shipmentTicketItem == null)
                {
                    MessageBox.Show("发货单条目不存在，请重新查询","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                StockInfo oriStockInfo = (from s in wmsEntities.StockInfo where s.ID == shipmentTicketItem.StockInfoID select s).FirstOrDefault();
                shipmentTicketItem.StockInfoID = this.curStockInfoID;
                StockInfo newStockInfo = (from s in wmsEntities.StockInfo where s.ID == this.curStockInfoID select s).FirstOrDefault();
                if(newStockInfo == null)
                {
                    MessageBox.Show("零件不存在，请重新选择！","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                shipmentTicketItem.ConfirmPersonID = this.curConfirmPersonID == -1 ? null : (int?)this.curConfirmPersonID;
                shipmentTicketItem.JobPersonID = this.curJobPersonID == -1 ? null : (int?)this.curJobPersonID;
                decimal oriShipmentAmount = shipmentTicketItem.ShipmentAmount * (shipmentTicketItem.UnitAmount ?? 1) ?? 0;
                if (Utilities.CopyTextBoxTextsToProperties(this, shipmentTicketItem, ShipmentTicketItemViewMetaData.KeyNames, out string errorMessage) == false)
                {
                    MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(shipmentTicketItem.ShipmentAmount <= 0)
                {
                    MessageBox.Show("发货数量必须大于0！","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                decimal? deltaStockAmount = (shipmentTicketItem.ShipmentAmount * shipmentTicketItem.UnitAmount) - oriShipmentAmount;
                decimal shipmentableAmount = (newStockInfo.ShipmentAreaAmount ?? 0) - newStockInfo.ScheduledShipmentAmount;
                shipmentableAmount = shipmentableAmount < 0 ? 0 : shipmentableAmount;
                if (deltaStockAmount > shipmentableAmount)
                {
                    MessageBox.Show("库存不足！当前可发货库存数量：" + Utilities.DecimalToString(shipmentableAmount) + "个", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //把库存已分配发货数量同步！
                if (oriStockInfo != newStockInfo)
                {
                    if(oriStockInfo != null)
                    {
                        oriStockInfo.ScheduledShipmentAmount -= oriShipmentAmount;
                    }
                    if(newStockInfo != null)
                    {
                        newStockInfo.ScheduledShipmentAmount += (shipmentTicketItem.ShipmentAmount * shipmentTicketItem.UnitAmount) ?? 0;
                    }
                }
                else
                {
                    newStockInfo.ScheduledShipmentAmount += deltaStockAmount ?? 0;
                }

                try
                {
                    wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(() =>
                {
                    this.Search();
                    Utilities.SelectLineByID(this.reoGridControlMain, shipmentTicketItem.ID);
                }));
                MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
         
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if(ids.Length == 0)
            {
                MessageBox.Show("请选择要删除的项目","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(ShipmentTicketUtilities.DeleteItemsSync(ids,out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Search();
            MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void buttonAdd_MouseEnter(object sender, EventArgs e)
        {
            buttonAdd.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonAdd_MouseLeave(object sender, EventArgs e)
        {
            buttonAdd.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonAdd_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAdd.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }



        private void buttonDelete_MouseEnter(object sender, EventArgs e)
        {
            buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonDelete_MouseLeave(object sender, EventArgs e)
        {
            buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonDelete_MouseDown(object sender, MouseEventArgs e)
        {
            buttonDelete.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonAlter_MouseEnter(object sender, EventArgs e)
        {
            buttonAlter.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonAlter_MouseLeave(object sender, EventArgs e)
        {
            buttonAlter.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonAlter_MouseDown(object sender, MouseEventArgs e)
        {
            buttonAlter.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            this.standardImportForm = new StandardImportForm<ShipmentTicketItem>(
                ShipmentTicketItemViewMetaData.KeyNames,
                importItemHandler,
                null,
                "导入发货单条目"
                );

            this.standardImportForm.AddButton("置入套餐", Properties.Resources.check, this.buttonAddCategoryClickedCallback);
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
            standardImportForm.AddAssociation("JobPersonName", string.Format("SELECT Name FROM Person WHERE Name LIKE '%'+@value+'%'"));
            standardImportForm.AddAssociation("ConfirmPersonName", string.Format("SELECT Name FROM Person WHERE Name LIKE '%'+@value+'%'"));

            //standardImportForm.AddDefaultValue("ShipmentAmount", "SELECT CASE WHEN @focus = 'ShipmentAmount' THEN NULL WHEN LEN(@UnitAmount)>0 AND LEN(@Unit)>0 THEN CAST(@UnitAmount AS DECIMAL(18,3))*CAST(@Unit AS DECIMAL(18,3)) ELSE NULL END", true, true);
            //standardImportForm.AddDefaultValue("Unit", "SELECT CASE WHEN @focus = 'Unit' THEN NULL WHEN LEN(@ShipmentAmount)>0 AND LEN(@UnitAmount)>0 THEN CAST(@ShipmentAmount AS DECIMAL(18,3))/CAST(@UnitAmount AS DECIMAL(18,3)) ELSE NULL END", true, true);

            standardImportForm.Show();
        }

        private void buttonAddCategoryClickedCallback()
        {
            string packageName = FormSelectPackage.SelectCategory();
            if(packageName == null)
            {
                return;
            }
            using (WMSEntities wmsEntities = new WMSEntities())
            {
                Supply[] supplies = (from s in wmsEntities.Supply
                                     where s.ProjectID == this.projectID
                                     && s.WarehouseID == this.warehouseID 
                                     && s.IsHistory != 1 
                                     && s.Package == packageName
                                     select s).ToArray();
                Dictionary<string, string> keyConvert = new Dictionary<string, string>();
                keyConvert.Add("SupplyNoOrComponentName", "No");
                keyConvert.Add("ShipmentAmount", "PackageDefaultShipmentAmount");
                keyConvert.Add("Unit", "PackageDefaultShipmentUnit");
                keyConvert.Add("UnitAmount", "PackageDefaultShipmentUnitAmount");
                this.standardImportForm.PushData(supplies, keyConvert, true);
            }
        }

        private bool importItemHandler(List<ShipmentTicketItem> results,Dictionary<string,string[]> unimportedColumns)
        {
            List<ShipmentTicketItem> realImportList = new List<ShipmentTicketItem>(); //真正要导入的ShipmentTicketItem（有的一个result项可能对应多个导入项）
            Dictionary<int, decimal> dicImportedSupplyIDAmountNoUnit = new Dictionary<int, decimal>();
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                for (int i = 0; i < results.Count; i++)
                {
                    string supplyNoOrComponentName = unimportedColumns["SupplyNoOrComponentName"][i];
                    string realName = null;
                    string jobPersonName = unimportedColumns["JobPersonName"][i];
                    string confirmPersonName = unimportedColumns["ConfirmPersonName"][i];
                    //封装的根据 零件名/供货代号 获取 零件/供货的函数
                    if(Utilities.GetSupplyOrComponentAmbiguous(supplyNoOrComponentName,out DataAccess.Component component,out Supply supply,out string errorMessage,-1,wmsEntities)==false)
                    {
                        MessageBox.Show(string.Format("行{0}：{1}", i + 1, errorMessage), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    StockInfoView[] stockInfoViews = null;
                    //如果填写的是供货
                    if (supply != null)
                    {
                        realName = supply.No;
                        stockInfoViews = (from s in wmsEntities.StockInfoView
                                          where s.SupplyID == supply.ID
                                                && s.ProjectID == this.projectID
                                                && s.WarehouseID == this.warehouseID
                                                && s.ShipmentAreaAmount - s.ScheduledShipmentAmount > 0
                                          orderby s.InventoryDate ascending
                                          select s).ToArray();
                    } //否则填写的是零件
                    else if (component != null)
                    {
                        realName = component.Name;
                        stockInfoViews = (from s in wmsEntities.StockInfoView
                                          where s.ComponentID == component.ID
                                                && s.ProjectID == this.projectID
                                                && s.WarehouseID == this.warehouseID
                                                && s.ShipmentAreaAmount - s.ScheduledShipmentAmount > 0
                                          orderby s.InventoryDate ascending
                                          select s).ToArray();
                    }
                    int jobPersonID = -1;
                    int confirmPersonID = -1;
                    //搜索作业人名
                    if (string.IsNullOrWhiteSpace(jobPersonName) == false)
                    {
                        if (Utilities.GetPersonByNameAmbiguous(jobPersonName, out Person jobPerson, out errorMessage, wmsEntities) == false) 
                        {
                            MessageBox.Show(string.Format("行{0}：{1}", i + 1, errorMessage), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        jobPersonID = jobPerson.ID;
                    }
                    //搜索确认人名
                    if (string.IsNullOrWhiteSpace(confirmPersonName) == false)
                    {
                        if (Utilities.GetPersonByNameAmbiguous(confirmPersonName, out Person confirmPerson, out errorMessage, wmsEntities) == false)
                        {
                            MessageBox.Show(string.Format("行{0}：{1}", i + 1, errorMessage), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        confirmPersonID = confirmPerson.ID;
                    }
                    //计算总共还有多少可以发货的零件，如果不够提示库存不足
                    decimal totalShipmentableAmountNoUnit = stockInfoViews.Sum((stockInfoView) => {
                        decimal shipmentableAmountNoUnit = (stockInfoView.ShipmentAreaAmount ?? 0) - stockInfoView.ScheduledShipmentAmount;
                        if (dicImportedSupplyIDAmountNoUnit.ContainsKey(stockInfoView.ID))
                        {
                            shipmentableAmountNoUnit -= dicImportedSupplyIDAmountNoUnit[stockInfoView.ID];
                        }
                        shipmentableAmountNoUnit = shipmentableAmountNoUnit < 0 ? 0 : shipmentableAmountNoUnit;
                        return shipmentableAmountNoUnit;
                    });
                    if (totalShipmentableAmountNoUnit < results[i].ShipmentAmount * results[i].UnitAmount)
                    {
                        MessageBox.Show(string.Format("行{0}：零件\"{1}\"库存不足，库存可发货数：{2} \n（如果之前行有重复此零件，则此数量为库存数减去之前行分配的数量）", i + 1, realName, Utilities.DecimalToString(totalShipmentableAmountNoUnit)), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    results[i].ShipmentTicketID = this.shipmentTicketID;
                    results[i].JobPersonID = jobPersonID == -1 ? null : (int?)jobPersonID;
                    results[i].ConfirmPersonID = confirmPersonID == -1 ? null : (int?)confirmPersonID;
                    decimal curAmountNoUnit = 0;
                    for (int j = 0; j < stockInfoViews.Length; j++)
                    {
                        ShipmentTicketItem newItem = new ShipmentTicketItem();
                        Utilities.CopyProperties(results[i], newItem);
                        newItem.StockInfoID = stockInfoViews[j].ID;
                        //计算当前StockInfo里还有多少可以发的数量
                        decimal curShipmentableAmountNoUnit = (stockInfoViews[j].ShipmentAreaAmount ?? 0) - stockInfoViews[j].ScheduledShipmentAmount;
                        if (dicImportedSupplyIDAmountNoUnit.ContainsKey(stockInfoViews[j].ID))
                        {
                            curShipmentableAmountNoUnit -= dicImportedSupplyIDAmountNoUnit[stockInfoViews[j].ID];
                        }
                        if (curShipmentableAmountNoUnit <= 0) continue; //当前如果没有可发货的数量，就跳过
                        //当前StockInfo的数量小于要发货的数量
                        if (curAmountNoUnit + curShipmentableAmountNoUnit < results[i].ShipmentAmount * results[i].UnitAmount)
                        {
                            newItem.ShipmentAmount = curShipmentableAmountNoUnit/newItem.UnitAmount;
                            realImportList.Add(newItem);
                            //把已经发货的数量加到dicImportedSupplyIDAmountNoUnit里，在COMMIT之前，下轮循环用
                            if (dicImportedSupplyIDAmountNoUnit.ContainsKey(newItem.StockInfoID.Value)==false)
                            {
                                dicImportedSupplyIDAmountNoUnit.Add(newItem.StockInfoID.Value, curShipmentableAmountNoUnit);
                            }
                            else
                            {
                                dicImportedSupplyIDAmountNoUnit[newItem.StockInfoID.Value] += curShipmentableAmountNoUnit;
                            }
                            curAmountNoUnit += newItem.ShipmentAmount.Value * newItem.UnitAmount.Value;
                        }
                        else //当前StockInfo数量大于等于需要发货的数量
                        {
                            newItem.ShipmentAmount = (results[i].ShipmentAmount*results[i].UnitAmount - curAmountNoUnit)/results[i].UnitAmount;
                            realImportList.Add(newItem);
                            //把已经发货的数量加到dicImportedSupplyIDAmountNoUnit里，在COMMIT之前，下轮循环用
                            if (dicImportedSupplyIDAmountNoUnit.ContainsKey(newItem.StockInfoID.Value)==false)
                            {
                                dicImportedSupplyIDAmountNoUnit.Add(newItem.StockInfoID.Value, ((results[i].ShipmentAmount * results[i].UnitAmount - curAmountNoUnit)??0));
                            }
                            else
                            {
                                dicImportedSupplyIDAmountNoUnit[newItem.StockInfoID.Value] += ((results[i].ShipmentAmount * results[i].UnitAmount - curAmountNoUnit) ?? 0);
                            }
                            //更新curAmountNoUnit
                            curAmountNoUnit += newItem.ShipmentAmount.Value * (newItem.UnitAmount ?? 1);
                            break;
                        }
                    }
                }

                foreach (ShipmentTicketItem item in realImportList)
                {
                    //增加条目，记录库存分配发货数量
                    StockInfo stockInfo = (from s in wmsEntities.StockInfo where s.ID == item.StockInfoID select s).FirstOrDefault();
                    stockInfo.ScheduledShipmentAmount += (item.ShipmentAmount ?? 0) * (item.UnitAmount ?? 1);
                    //设置发货单条目初始信息
                    item.ScheduledJobAmount = 0;
                    wmsEntities.ShipmentTicketItem.Add(item);
                }
                wmsEntities.SaveChanges();
                this.Search();
                MessageBox.Show("导入成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.standardImportForm.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("操作失败，请检查网络连接！\n请把如下错误信息反馈给我们！\n"+ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return false;
        }

        private void buttonImport_MouseEnter(object sender, EventArgs e)
        {
            buttonImport.BackgroundImage = WMS.UI.Properties.Resources.bottonW_s;
        }

        private void buttonImport_MouseLeave(object sender, EventArgs e)
        {
            buttonImport.BackgroundImage = WMS.UI.Properties.Resources.bottonW_q;
        }

        private void buttonImport_MouseDown(object sender, MouseEventArgs e)
        {
            buttonImport.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }


    }
}
