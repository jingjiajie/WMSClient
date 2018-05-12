using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using WMS.DataAccess;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI
{
    public partial class FormStockInfo : Form
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private int userID = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        private PagerWidget<StockInfoView> pagerWidget = null;
        private SearchWidget<StockInfoView> searchWidget = null;

        public FormStockInfo(int userID,int projectID,int warehouseID)
        {
            InitializeComponent();
            this.userID = userID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void FormStockInfo_Load(object sender, EventArgs e)
        {
            InitComponents();
            try
            {
                WMSEntities wmsEntities = new WMSEntities();
                User user = (from u in wmsEntities.User where u.ID == this.userID select u).FirstOrDefault();
                if (user == null)
                {
                    MessageBox.Show("登录失效，请重新登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //如果登录失效，不能查出任何数据。
                    this.pagerWidget.AddStaticCondition("SupplierID", "-1");
                    return;
                }
                if(user.Supplier != null)
                {
                    this.pagerWidget.AddStaticCondition("SupplierID", user.SupplierID.ToString());
                    this.buttonAlter.Enabled = false;
                    this.buttonImport.Enabled = false;
                }
            }
            catch
            {
                MessageBox.Show("加载失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //如果加载失败，不能查出任何数据。
                this.pagerWidget.AddStaticCondition("SupplierID", "-1");
                return;
            }

            this.pagerWidget.Search();
        }


        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in StockInfoViewMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化分页控件
            this.pagerWidget = new PagerWidget<StockInfoView>(this.reoGridControlMain, StockInfoViewMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPager.Controls.Add(pagerWidget);
            pagerWidget.Show();

            this.searchWidget = new SearchWidget<StockInfoView>(StockInfoViewMetaData.KeyNames, this.pagerWidget);
            this.panelSearchWidget.Controls.Add(searchWidget);
        }

        private void reoGridControlMain_Click(object sender, EventArgs e)
        {

        }
        

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlMain.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int stockInfoID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var formStockInfoModify = new FormStockInfoModify(stockInfoID);
                formStockInfoModify.SetModifyFinishedCallback(()=>
                {
                    this.searchWidget.Search(true);
                });
                formStockInfoModify.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormStockInfoModify();
            form.SetMode(FormMode.ADD);
            form.SetAddFinishedCallback(() =>
            {
                this.pagerWidget.Search();
            });
            form.Show();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int[] deleteIDs = Utilities.GetSelectedIDs(this.reoGridControlMain);

            if(deleteIDs.Length == 0)
            {
                MessageBox.Show("请选择您要删除的记录","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            new Thread(new ThreadStart(()=>
            {
                foreach(int id in deleteIDs)
                {
                    StockInfo stockInfo = (from s in wmsEntities.StockInfo
                                           where s.ID == id
                                           select s).FirstOrDefault();
                    if (stockInfo == null) continue;
                    if(stockInfo.ReceiptTicketItemID != null)
                    {
                        MessageBox.Show("被收货单引用的库存批次不能删除！","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int shipmentTicketItemCount = (from s in wmsEntities.ShipmentTicketItem
                                                   where s.StockInfoID == stockInfo.ID
                                                   select s).Count();
                    if(shipmentTicketItemCount > 0)
                    {
                        MessageBox.Show("被发货单引用的库存批次不可以删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    wmsEntities.StockInfo.Remove(stockInfo);
                }
                if (MessageBox.Show("您真的要删除这些记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
                this.wmsEntities.SaveChanges();
                this.Invoke(new Action(()=>
                {
                    this.pagerWidget.Search();
                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            })).Start();
        } 

        private void buttonImport_Click(object sender, EventArgs e)
        {
            StandardImportForm<StockInfo> formImport = new StandardImportForm<StockInfo>(
                StockInfoViewMetaData.KeyNames,
                importHandler,
                () =>
                {
                    this.searchWidget.Search();
                },
                "导入库存信息");
            formImport.Show();
        }

        private bool importHandler(List<StockInfo> results,Dictionary<string,string[]> unimportedColumns)
        {
            WMSEntities wmsEntities = new WMSEntities();
            for(int i = 0; i < results.Count; i++)
            {
                results[i].ProjectID = this.projectID;
                results[i].WarehouseID = this.warehouseID;

                string supplyNo = unimportedColumns["SupplyNo"][i];

                //先精确搜索，没有再模糊搜索
                Supply supply = (from s in wmsEntities.Supply
                                 where s.ProjectID == this.projectID
                                     && s.WarehouseID == this.warehouseID
                                     && s.No == supplyNo
                                 select s).FirstOrDefault();
                if (supply == null)
                {
                    Supply[] supplies = (from s in wmsEntities.Supply
                                         where s.ProjectID == this.projectID
                                         && s.WarehouseID == this.warehouseID
                                         && s.No.Contains(supplyNo)
                                         select s).ToArray();
                    if (supplies.Length == 0)
                    {
                        MessageBox.Show(string.Format("行{0}：未找到代号为 {1} 的零件！", i + 1, supplyNo), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    if (supplies.Length > 1)
                    {
                        StringBuilder sbHint = new StringBuilder();
                        sbHint.AppendFormat("行{0}：零件不明确，您是否要搜索：\n", i + 1);
                        foreach (var s in supplies)
                        {
                            sbHint.AppendFormat("{0}\n", s.No);
                        }
                        MessageBox.Show(sbHint.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    supply = supplies[0];
                }

                results[i].SupplyID = supply.ID;
            }
            return true;
        }
    }
}
