﻿using FrontWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.Script.Serialization;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormSupplier : Form
    {
        public FormSupplier()
        {            
            InitializeComponent();
           this.model1.CellUpdated+= this.model_CellUpdated;
        }

        private List<int> rowChange = new List<int>();

        private void model_CellUpdated(object sender, ModelCellUpdatedEventArgs e)
        {            
            foreach (var cell in e.UpdatedCells)
            {          
                if (cell.ColumnName.StartsWith("lastUpdate")) return;
                this.model1[cell.Row, "lastUpdatePersonId"] = GlobalData.Person["id"];
                this.model1[cell.Row, "lastUpdatePersonName"] = GlobalData.Person["name"];
                this.model1[cell.Row, "lastUpdateTime"] = DateTime.Now;                
                if (!rowChange.Contains(cell.Row))
                {
                    rowChange.Add(cell.Row);
                }                
            }
        }

        
        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "warehouseId",GlobalData.Warehouse["id"]},
                { "createPersonId",GlobalData.Person["id"]},
                { "createPersonName",GlobalData.Person["name"]},
                { "warehouseName",GlobalData.Warehouse["name"]},
                { "createTime",DateTime.Now}
            });        
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否保留历史信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                if (this.synchronizer.Save())
                {
                    this.searchView1.Search();
                    Condition condWarehouse = new Condition().AddCondition("warehouseId", GlobalData.Warehouse["id"]);
                    GlobalData.AllSuppliers = RestClient.Get<List<IDictionary<string, object>>>(
                       $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/supplier/{condWarehouse.ToString()}");
                }
            }
            else
            {
                int[] row = rowChange.ToArray();
                var rowData = this.model1.GetRows(row);
                string json=(new JavaScriptSerializer()).Serialize(rowData);

            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void FormSupplier_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }
    }
}
