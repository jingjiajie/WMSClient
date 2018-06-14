using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormSupplierHistory : Form
    {
        int supplierId;
        public FormSupplierHistory(int supplierID)
        {
            //this.configuration1 = configuration;
            this.supplierId = supplierID;
            InitializeComponent();
        }

        private void FormSupplierHistory_Load(object sender, EventArgs e)
        {
            this.synchronizer.FindAPI.SetRequestParameter("$history", "history");
            this.synchronizer.GetCountAPI.SetRequestParameter("$history", "history");
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);
            this.searchView1.AddStaticCondition("newestSupplierId", this.supplierId);
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void configuration1_Load(object sender, EventArgs e)
        {

        }
    }
}
