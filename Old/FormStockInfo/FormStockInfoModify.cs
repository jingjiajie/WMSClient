using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;
using System.Reflection;

namespace WMS.UI
{
    public partial class FormStockInfoModify : Form
    {
        private int stockInfoID = -1;
        private WMSEntities wmsEntities = new WMSEntities();
        private Action modifyFinishedCallback = null;
        private Action addFinishedCallback = null;
        private FormMode mode = FormMode.ALTER;

        public FormStockInfoModify(int stockInfoID = -1)
        {
            InitializeComponent();
            this.stockInfoID = stockInfoID;
        }
        

        private void FormStockInfoModify_Load(object sender, EventArgs e)
        {
            if (this.mode == FormMode.ALTER && this.stockInfoID == -1)
            {
                throw new Exception("未设置源库存信息");
            }

            Utilities.CreateEditPanel(this.tableLayoutPanelTextBoxes, StockInfoViewMetaData.KeyNames);

            if(this.mode == FormMode.ALTER)
            {
                StockInfoView stockInfoView = null;
                try
                {
                    stockInfoView = (from s in this.wmsEntities.StockInfoView
                                                   where s.ID == this.stockInfoID
                                                   select s).Single();
                }
                catch
                {
                    MessageBox.Show("加载数据失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                Utilities.CopyPropertiesToTextBoxes(stockInfoView, this);
            }

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            StockInfo stockInfo = null;
            
            //若修改，则查询原StockInfo对象。若添加，则新建一个StockInfo对象。
            if (this.mode == FormMode.ALTER)
            {
                try
                {
                    stockInfo = (from s in this.wmsEntities.StockInfo
                                 where s.ID == this.stockInfoID
                                 select s).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("修改失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(stockInfo == null)
                {
                    MessageBox.Show("库存信息不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (mode == FormMode.ADD)
            {
                stockInfo = new StockInfo();
                this.wmsEntities.StockInfo.Add(stockInfo);
            }

            //开始数据库操作
            if (Utilities.CopyTextBoxTextsToProperties(this, stockInfo, StockInfoViewMetaData.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                wmsEntities.SaveChanges();
            }
            catch
            {
                MessageBox.Show("修改失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //调用回调函数
            if (this.mode == FormMode.ALTER && this.modifyFinishedCallback != null)
            {
                this.modifyFinishedCallback();
                MessageBox.Show("修改成功！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }else if(this.mode == FormMode.ADD && this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
            this.Close();
        }

        public void SetModifyFinishedCallback(Action callback)
        {
            this.modifyFinishedCallback = callback;
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        public void SetMode(FormMode mode)
        {
            this.mode = mode;
            if(mode == FormMode.ALTER)
            {
                this.Text = "修改库存信息";
                this.buttonOK.Text = "修改库存信息";
            }else if (mode == FormMode.ADD)
            {
                this.Text = "添加库存信息";
                this.buttonOK.Text = "添加库存信息";
            }
        }

        private void FormStockInfoModify_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
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
    }
}
