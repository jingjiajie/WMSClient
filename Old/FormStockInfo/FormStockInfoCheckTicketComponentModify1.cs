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
    public partial class FormStockInfoCheckTicketComponentModify1 : Form
    {
        private Action<int> selectFinishCallback = null;
        private WMSEntities wmsEntities = new WMSEntities();
        int stockinfocheckid = -1;
        int stockinfoid = -1;
        private Action addFinishedCallback = null;
        public FormStockInfoCheckTicketComponentModify1(int stockinfocheckid)
            
        {
            
            this.stockinfocheckid = stockinfocheckid;
            InitializeComponent();

        }
        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in StockInfoViewMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化
            this.toolStripComboBoxSelect1.Items.Add("无");
            this.toolStripComboBoxSelect1.Items.AddRange(visibleColumnNames);
            this.toolStripComboBoxSelect1.SelectedIndex = 0;


            //初始化表格
            var worksheet = this.reoGridControlComponen.Worksheets[0];
            worksheet.SelectionMode = unvell.ReoGrid.WorksheetSelectionMode.Row;
            for (int i = 0; i < StockInfoViewMetaData.KeyNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = StockInfoViewMetaData.KeyNames[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = StockInfoViewMetaData.KeyNames[i].Visible;
            }
            worksheet.Columns = StockInfoViewMetaData.KeyNames.Length;//限制表的长度
            Console.WriteLine("表格行数：" + StockInfoViewMetaData.KeyNames.Length);

            this.tableLayoutPanel2.Controls.Clear();
            for (int i = 0; i < StockInfoCheckTicksModifyMetaDate.KeyNames.Length; i++)
            {
                KeyName curKeyName = StockInfoCheckTicksModifyMetaDate.KeyNames[i];
                if (curKeyName.Visible == false && curKeyName.Editable == false)
                {
                    continue;
                }
                Label label = new Label();
                label.Text = curKeyName.Name;
                this.tableLayoutPanel2.Controls.Add(label);

                TextBox textBox = new TextBox();
                textBox.Name = "textBox" + curKeyName.Key;
                if (curKeyName.Editable == false)
                {
                    textBox.Enabled = false;
                }
                this.tableLayoutPanel2.Controls.Add(textBox);
            }
            


        }

        private void FormStockInfoCheckTicketComponentModify_Load(object sender, EventArgs e)
        {
            this.InitComponents();
            this.Search();
           
            
           


        }

        private void labelStatus_Click(object sender, EventArgs e)
        {

        }
        private void Search()
        {
           
            string key = null;
            string value = null;

            if (this.toolStripComboBoxSelect1.SelectedIndex != 0)
            {
                key = (from kn in StockInfoViewMetaData.KeyNames
                       where kn.Name == this.toolStripComboBoxSelect1 .SelectedItem.ToString()
                       select kn.Key).First();
                value = this.textBoxSearchValue.Text;
            }

            this.labelStatus.Text = "正在搜索中...";
            var worksheet = this.reoGridControlComponen .Worksheets[0];
            worksheet[0, 0] = "加载中...";
            new Thread(new ThreadStart(() =>
            {
                StockInfoView[] stockInfoViews = null;
                string sql = "SELECT * FROM StockInfoView WHERE 1=1";
                List<SqlParameter> parameters = new List<SqlParameter>();

             
                if (key != null && value != null) //查询条件不为null则增加查询条件
                {
                    sql += "AND " + key + " = @value ";
                    parameters.Add(new SqlParameter("value", value));
                }
                sql += " ORDER BY ID DESC"; //倒序排序
                stockInfoViews = wmsEntities.Database.SqlQuery<StockInfoView>(sql, parameters.ToArray()).ToArray();
                this.reoGridControlComponen.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "搜索完成";
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    if (stockInfoViews.Length == 0)
                    {
                        worksheet[0, 1] = "没有查询到符合条件的记录";
                    }
                    for (int i = 0; i < stockInfoViews.Length; i++)
                    {
                        StockInfoView curStockInfoView = stockInfoViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curStockInfoView, (from kn in StockInfoViewMetaData.KeyNames select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            worksheet[i, j] = columns[j] == null ? "" : columns[j].ToString();
                        }
                    }
                }));

            })).Start();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            DataAccess.StockInfoCheckTicketItem StockInfoCheckTicketItem = null;



            StockInfoCheckTicketItem = new DataAccess.StockInfoCheckTicketItem();
            this.wmsEntities.StockInfoCheckTicketItem.Add(StockInfoCheckTicketItem);


            StockInfoCheckTicketItem.StockInfoCheckTicketID = this.stockinfocheckid;
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlComponen );

            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项");
                return;
            }
            else if ((ids.Length == 1))
            {
                int stockiofocheckid = ids[0];
                
                        }
            this.stockinfoid = ids[0];
            //TODO StockInfoCheckTicketItem.StockInfoID = this.stockinfoid;
            
            TextBox textBoxOverflowAreaAmount = (TextBox)this.Controls.Find("textBoxOverflowAreaAmount",true)[0];
            TextBox textBoxShipmentAreaAmount = (TextBox)this.Controls.Find("textBoxShipmentAreaAmount", true)[0];

            if (textBoxOverflowAreaAmount.Text != string.Empty)
            {
                StockInfoCheckTicketItem.ExcpetedOverflowAreaAmount = Convert.ToDecimal(textBoxOverflowAreaAmount.Text);
            }
            if (textBoxShipmentAreaAmount.Text != string.Empty)
            {
                StockInfoCheckTicketItem.ExpectedShipmentAreaAmount = Convert.ToDecimal(textBoxShipmentAreaAmount.Text);
            }

            //开始数据库操作
            if (Utilities.CopyTextBoxTextsToProperties(this, StockInfoCheckTicketItem, StockInfoCheckTicksModifyMetaDate.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                Utilities.CopyComboBoxsToProperties(this, StockInfoCheckTicketItem, StockInfoCheckTicksModifyMetaDate.KeyNames);
            }
            wmsEntities.SaveChanges();
            MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if ( this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }

        }




     

        private void reoGridControlComponen_Click(object sender, EventArgs e)
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlComponen);
            
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项");
                return;
            }
            int b = ids[0];
            WMS.DataAccess.StockInfoView a = (from s in this.wmsEntities.StockInfoView 
                                             where s.ID == (b)
                                             select s).Single();
            Utilities.CopyPropertiesToTextBoxes(a, this);

        }

        private void toolStripComboBoxSelect1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxSelect1.SelectedIndex == 0)
            {
                this.textBoxSearchValue.Text = "";
                this.textBoxSearchValue.Enabled = false;
                this.Search();
            }
            else
            {
                this.textBoxSearchValue.Enabled = true;
            }
        }


        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        
    }
}
