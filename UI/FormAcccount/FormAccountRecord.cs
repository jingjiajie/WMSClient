﻿using System;
using FrontWork;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FormAcccount
{
    public partial class FormAccountRecord : Form
    {
        public FormAccountRecord()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }


        private void AccountTitleNameEditEnded(int row, string accountTitleName)
        {
            IDictionary<string, object> foundAccountTitle =
                GlobalData.AllAccountTitle.Find((s) =>
                {
                    if (s["name"] == null) return false;
                    return s["name"].ToString() == accountTitleName && s["warehouseId"] != GlobalData.Warehouse["id"];
                });
            if (foundAccountTitle == null)
            {
                MessageBox.Show($"科目\"{accountTitleName}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.model1[row, "accountTitleId"] = foundAccountTitle["id"];
            }

        }
            private void AccountTitleNoEditEnded(int row, string accountTitleNo)
            {
                IDictionary<string, object> foundAccountTitle =
                    GlobalData.AllAccountTitle.Find((s) =>
                    {
                        if (s["no"] == null) return false;
                        return s["no"].ToString() == accountTitleNo && s["warehouseId"] != GlobalData.Warehouse["id"];
                    });
                if (foundAccountTitle == null)
                {
                    MessageBox.Show($"科目\"{accountTitleNo}\"不存在，请重新填写", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    this.model1[row, "accountTitleId"] = foundAccountTitle["id"];
                }
            }

        private void FormAccountRecord_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {
                { "personId",GlobalData.Person["id"]},
                { "persomName",GlobalData.Person["name"]}
            });
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();
            }
        }

        private void pagerSearchJsonRESTAdapter1_Load(object sender, EventArgs e)
        {

        }
    }
}