﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FromDeliverOrder
{
    public partial class FormDeliveryOrderChooseExcelType : Form
    {
        List<IDictionary<string, object>> previewData;
        public FormDeliveryOrderChooseExcelType(List<IDictionary<string, object>> previewData)
        {
            this.previewData = previewData;
            InitializeComponent();
        }

        private void buttonNormal_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("出库单预览");
            foreach (IDictionary<string, object> orderAndItem in previewData)
            {
                IDictionary<string, object> deliveryOrder = (IDictionary<string, object>)orderAndItem["deliveryOrder"];
                object[] deliveryOrderItems = (object[])orderAndItem["deliveryOrderItems"];
                string no = (string)deliveryOrder["no"];
                if (!formPreviewExcel.AddPatternTable("Excel/patternPutOutStorageTicketNormal.xlsx", no)) return;
                formPreviewExcel.AddData("deliveryOrder", deliveryOrder, no);
                formPreviewExcel.AddData("deliveryOrderItems", deliveryOrderItems, no);
            }
            formPreviewExcel.Show();
            this.Close();
        }

        private void buttonCover_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("出库单预览");
            foreach (IDictionary<string, object> orderAndItem in previewData)
            {
                IDictionary<string, object> deliveryOrder = (IDictionary<string, object>)orderAndItem["deliveryOrder"];
                object[] deliveryOrderItems = (object[])orderAndItem["deliveryOrderItems"];
                string no = (string)deliveryOrder["no"];
                if (!formPreviewExcel.AddPatternTable("Excel/patternPutOutStorageTicketCover.xlsx", no)) return;
                formPreviewExcel.AddData("deliveryOrder", deliveryOrder, no);
                formPreviewExcel.AddData("deliveryOrderItems", deliveryOrderItems, no);
            }
            formPreviewExcel.Show();
            this.Close();
        }

        private void buttonMoBiSi_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("出库单预览");
            foreach (IDictionary<string, object> orderAndItem in previewData)
            {
                IDictionary<string, object> deliveryOrder = (IDictionary<string, object>)orderAndItem["deliveryOrder"];
                object[] deliveryOrderItems = (object[])orderAndItem["deliveryOrderItems"];
                string no = (string)deliveryOrder["no"];
                if (!formPreviewExcel.AddPatternTable("Excel/patternPutOutStorageTicketMoBiSi.xlsx", no)) return;
                formPreviewExcel.AddData("deliveryOrder", deliveryOrder, no);
                formPreviewExcel.AddData("deliveryOrderItems", deliveryOrderItems, no);
            }
            formPreviewExcel.Show();
            this.Close();
        }

        private void buttonZhongDu_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("出库单预览");
            foreach (IDictionary<string, object> orderAndItem in previewData)
            {
                IDictionary<string, object> deliveryOrder = (IDictionary<string, object>)orderAndItem["deliveryOrder"];
                object[] deliveryOrderItems = (object[])orderAndItem["deliveryOrderItems"];
                string no = (string)deliveryOrder["no"];
                if (!formPreviewExcel.AddPatternTable("Excel/patternPutOutStorageTicketZhongDu.xlsx", no)) return;
                formPreviewExcel.AddData("deliveryOrder", deliveryOrder, no);
                formPreviewExcel.AddData("deliveryOrderItems", deliveryOrderItems, no);
            }
            formPreviewExcel.Show();
            this.Close();
        }

        private void buttonZhongDuFlow_Click(object sender, EventArgs e)
        {
            StandardFormPreviewExcel formPreviewExcel = new StandardFormPreviewExcel("出库单预览");
            foreach (IDictionary<string, object> orderAndItem in previewData)
            {
                IDictionary<string, object> deliveryOrder = (IDictionary<string, object>)orderAndItem["deliveryOrder"];
                object[] deliveryOrderItems = (object[])orderAndItem["deliveryOrderItems"];
                string no = (string)deliveryOrder["no"];
                if (!formPreviewExcel.AddPatternTable("Excel/patternPutOutStorageTicketZhongDuFlow.xlsx", no)) return;
                formPreviewExcel.AddData("deliveryOrder", deliveryOrder, no);
                formPreviewExcel.AddData("deliveryOrderItems", deliveryOrderItems, no);
            }
            formPreviewExcel.Show();
            this.Close();
        }
    }
}
