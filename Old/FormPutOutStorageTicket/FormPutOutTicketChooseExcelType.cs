using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;

namespace WMS.UI
{
    public partial class FormPutOutTicketChooseExcelType : Form
    {
        int[] putOutStorageTicketIDs = null;
        public FormPutOutTicketChooseExcelType(int[] putOutStorageTicketIDs)
        {
            InitializeComponent();
            this.putOutStorageTicketIDs = putOutStorageTicketIDs;
        }

        private void FormPutOutTicketChooseExcelType_Load(object sender, EventArgs e)
        {

        }

        private void buttonCover_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreview = new StandardFormPreviewExcel("发货单预览");
            WMSEntities wmsEntities = new WMSEntities();
            foreach (int id in this.putOutStorageTicketIDs)
            {
                PutOutStorageTicketView putOutStorageTicketView = null;
                ShipmentTicketView shipmentTicketView = null;
                PutOutStorageTicketItemView[] putOutStorageTicketItemViews = null;
                try
                {
                    putOutStorageTicketView = (from p in wmsEntities.PutOutStorageTicketView
                                               where p.ID == id
                                               select p).FirstOrDefault();
                    putOutStorageTicketItemViews =
                        (from p in wmsEntities.PutOutStorageTicketItemView
                         where p.PutOutStorageTicketID == putOutStorageTicketView.ID
                         select p).ToArray();
                    if (putOutStorageTicketView == null)
                    {
                        MessageBox.Show("出库单"+ putOutStorageTicketView.No+ "不存在，可能已被删除，请重新查询！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    shipmentTicketView =
                        wmsEntities.Database.SqlQuery<ShipmentTicketView>(string.Format(@"SELECT * FROM ShipmentTicketView WHERE ID = 
                                                                    (SELECT ShipmentTicketID FROM JobTicket
                                                                    WHERE JobTicket.ID = {0})", putOutStorageTicketView.JobTicketID)).FirstOrDefault();
                }
                catch
                {
                    MessageBox.Show("加载失败，请检查网络连接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string worksheetName = id.ToString();
                if (formPreview.AddPatternTable(@"Excel\patternPutOutStorageTicketNormal.xlsx", worksheetName) == false)
                {
                    this.Close();
                    return;
                }
                formPreview.AddData("putOutStorageTicket", putOutStorageTicketView, worksheetName);
                formPreview.AddData("putOutStorageTicketItems", putOutStorageTicketItemViews,worksheetName);
                formPreview.AddData("shipmentTicket", shipmentTicketView, worksheetName);
                formPreview.SetPrintScale(0.9F,worksheetName);
            }
            formPreview.Show();
            this.Close();
        }

        private void buttonInner_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreview = new StandardFormPreviewExcel("出库单预览");
            WMSEntities wmsEntities = new WMSEntities();
            foreach(int id in putOutStorageTicketIDs)
            {
                PutOutStorageTicketView putOutStorageTicketView = (from p in wmsEntities.PutOutStorageTicketView
                                                                   where p.ID == id
                                                                   select p).FirstOrDefault();
                PutOutStorageTicketItemView[] putOutStorageTicketTiemViews =
                    (from p in wmsEntities.PutOutStorageTicketItemView
                     where p.PutOutStorageTicketID == putOutStorageTicketView.ID
                     select p).ToArray();
                if (putOutStorageTicketView == null)
                {
                    MessageBox.Show("出库单不存在，请重新查询！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ShipmentTicketView shipmentTicketView =
                    wmsEntities.Database.SqlQuery<ShipmentTicketView>(string.Format(@"SELECT * FROM ShipmentTicketView WHERE ID = 
                                                    (SELECT ShipmentTicketID FROM JobTicket
                                                        WHERE JobTicket.ID = {0})", putOutStorageTicketView.JobTicketID)).FirstOrDefault();
                string worksheetName = id.ToString();
                if (formPreview.AddPatternTable(@"Excel\patternPutOutStorageTicketCover.xlsx", worksheetName) == false)
                {
                    this.Close();
                    return;
                }
                formPreview.AddData("putOutStorageTicket", putOutStorageTicketView, worksheetName);
                formPreview.AddData("putOutStorageTicketItems", putOutStorageTicketTiemViews,worksheetName);
                formPreview.AddData("shipmentTicket", shipmentTicketView, worksheetName);
            }
            formPreview.Show();
            this.Close();
        }

        private void buttonZhongDu_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreview = new StandardFormPreviewExcel("出库单预览");

            WMSEntities wmsEntities = new WMSEntities();
            foreach (int id in putOutStorageTicketIDs)
            {
                PutOutStorageTicketView putOutStorageTicketView = (from p in wmsEntities.PutOutStorageTicketView
                                                                   where p.ID == id
                                                                   select p).FirstOrDefault();
                PutOutStorageTicketItemView[] putOutStorageTicketTiemViews =
                    (from p in wmsEntities.PutOutStorageTicketItemView
                     where p.PutOutStorageTicketID == putOutStorageTicketView.ID
                     select p).ToArray();
                if (putOutStorageTicketView == null)
                {
                    MessageBox.Show("出库单不存在，请重新查询！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ShipmentTicketView shipmentTicketView =
                    wmsEntities.Database.SqlQuery<ShipmentTicketView>(string.Format(@"SELECT * FROM ShipmentTicketView WHERE ID = 
                                                    (SELECT ShipmentTicketID FROM JobTicket
                                                        WHERE JobTicket.ID = {0})", putOutStorageTicketView.JobTicketID)).FirstOrDefault();

                string worksheetName = id.ToString();
                if (formPreview.AddPatternTable(@"Excel\patternPutOutStorageTicketZhongDu.xlsx",worksheetName) == false)
                {
                    this.Close();
                    return;
                }
                formPreview.AddData("putOutStorageTicket", putOutStorageTicketView, worksheetName);
                formPreview.AddData("putOutStorageTicketItems", putOutStorageTicketTiemViews,worksheetName);
                formPreview.AddData("shipmentTicket", shipmentTicketView,worksheetName);
                formPreview.SetPrintScale(1.2f, worksheetName);
            }
            formPreview.Show();
            this.Close();
        }

        private void buttonMoBiSi_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreview = new StandardFormPreviewExcel("出库单预览");
            WMSEntities wmsEntities = new WMSEntities();
            if(putOutStorageTicketIDs.Length > 1)
            {
                MessageBox.Show("摩比斯格式暂不支持批量打印", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }
            int id = putOutStorageTicketIDs[0];
            PutOutStorageTicketView putOutStorageTicketView = (from p in wmsEntities.PutOutStorageTicketView
                                                               where p.ID == id
                                                               select p).FirstOrDefault();
            PutOutStorageTicketItemView[] putOutStorageTicketItemViews =
                (from p in wmsEntities.PutOutStorageTicketItemView
                 where p.PutOutStorageTicketID == putOutStorageTicketView.ID
                 select p).ToArray();
            if (putOutStorageTicketView == null)
            {
                MessageBox.Show("出库单不存在，请重新查询！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ShipmentTicketView shipmentTicketView =
                wmsEntities.Database.SqlQuery<ShipmentTicketView>(string.Format(@"SELECT * FROM ShipmentTicketView WHERE ID = 
                                                    (SELECT ShipmentTicketID FROM JobTicket
                                                        WHERE JobTicket.ID = {0})", putOutStorageTicketView.JobTicketID)).FirstOrDefault();

            int[] supplierIDs = (from item in putOutStorageTicketItemViews
                                 where item.SupplierID != null
                                 select item.SupplierID.Value).ToArray();
            Supplier[] suppliers = (from s in wmsEntities.Supplier
                                    where supplierIDs.Contains(s.ID)
                                    select s).ToArray();

            foreach(Supplier supplier in suppliers)
            {
                formPreview.AddPatternTable(@"Excel\patternPutOutStorageTicketMoBiSi.xlsx",supplier.Name);
                formPreview.AddData("ticket", putOutStorageTicketView,supplier.Name);
                formPreview.AddData("items", (from item in putOutStorageTicketItemViews where item.SupplierID == supplier.ID select item).ToArray(), supplier.Name);
                formPreview.AddData("shipmentTicket", shipmentTicketView, supplier.Name);
                formPreview.AddData("supplier", supplier, supplier.Name);
            }

            //没有供应商信息的单独显示一个tab（非正常情况）
            PutOutStorageTicketItemView[] noSupplierItems = (from item in putOutStorageTicketItemViews
                                                           where item.SupplierID == null
                                                           select item).ToArray();
            if (noSupplierItems.Length > 0)
            {
                formPreview.AddPatternTable(@"Excel\patternPutOutStorageTicketMoBiSi.xlsx", "无供应商");
                formPreview.AddData("ticket", putOutStorageTicketView, "无供应商");
                formPreview.AddData("items", noSupplierItems, "无供应商");
                formPreview.AddData("shipmentTicket", shipmentTicketView, "无供应商");
                formPreview.AddData("supplier", default(Supplier), "无供应商");
            }

            formPreview.Show();
            this.Close();
        }

        private void buttonZhongDu_MouseEnter(object sender, EventArgs e)
        {
            buttonZhongDu.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonZhongDu_MouseLeave(object sender, EventArgs e)
        {
            buttonZhongDu.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonZhongDu_MouseDown(object sender, MouseEventArgs e)
        {
            buttonZhongDu.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonMoBiSi_MouseEnter(object sender, EventArgs e)
        {
            buttonMoBiSi.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonMoBiSi_MouseLeave(object sender, EventArgs e)
        {
            buttonMoBiSi.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonMoBiSi_MouseDown(object sender, MouseEventArgs e)
        {
            buttonMoBiSi.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonCover_MouseEnter(object sender, EventArgs e)
        {
            buttonCover.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonCover_MouseLeave(object sender, EventArgs e)
        {
            buttonCover.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonCover_MouseDown(object sender, MouseEventArgs e)
        {
            buttonCover.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }


        private void buttonNormal_MouseLeave(object sender, EventArgs e)
        {
            buttonNormal.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }

        private void buttonNormal_MouseDown(object sender, MouseEventArgs e)
        {
            buttonNormal.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonNormal_MouseEnter(object sender, EventArgs e)
        {
            buttonNormal.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonZhongDuFlow_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreview = new StandardFormPreviewExcel("出库流水预览");

            WMSEntities wmsEntities = new WMSEntities();
            List<PutOutStorageTicketItemView> items = new List<PutOutStorageTicketItemView>();
            foreach (int id in putOutStorageTicketIDs)
            {
                PutOutStorageTicketItemView[] putOutStorageTicketItemViews =
                    (from p in wmsEntities.PutOutStorageTicketItemView
                     where p.PutOutStorageTicketID == id
                     select p).ToArray();
                items.AddRange(putOutStorageTicketItemViews);
            }
            if (formPreview.AddPatternTable(@"Excel\patternPutOutStorageTicketZhongDuFlow.xlsx") == false)
            {
                this.Close();
                return;
            }
            formPreview.AddData("putOutStorageTicketItems", items.ToArray());
            formPreview.SetPrintScale(1.2f);
            formPreview.Show();
            this.Close();
        }

        private void buttonZhongDuFlow_MouseDown(object sender, MouseEventArgs e)
        {
            buttonZhongDuFlow.BackgroundImage = WMS.UI.Properties.Resources.bottonB3_q;
        }

        private void buttonZhongDuFlow_MouseEnter(object sender, EventArgs e)
        {
            buttonZhongDuFlow.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_s;
        }

        private void buttonZhongDuFlow_MouseLeave(object sender, EventArgs e)
        {
            buttonZhongDuFlow.BackgroundImage = WMS.UI.Properties.Resources.bottonB2_q;
        }
    }
}
