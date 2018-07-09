using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI.FromSalary
{
    public partial class FormSalaryItem : Form
    {
        public FormSalaryItem()
        {
            InitializeComponent();
        }

        private string TypeForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "固定工资";
                case 1: return "计件工资";
                default: return "未知状态";
            }
        }

        private int TypeBackwardMapper(string enable)
        {
            switch (enable)
            {
                case "固定工资": return 0;
                case "计件工资": return 1;
                default: return -1;
            }
        }

        private void FormSalaryItem_Load(object sender, EventArgs e)
        {
            this.searchView1.AddStaticCondition("warehouseId", GlobalData.Warehouse["id"]);         
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();         
        }
    }
}
