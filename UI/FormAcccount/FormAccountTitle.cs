using System;
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
    public partial class FormAccountTitle : Form
    {
        public FormAccountTitle()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private string EnableForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "禁用";
                case 1: return "启用";
                default: return "未知状态";
            }
        }

        private int EnableBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "禁用": return 0;
                case "启用": return 1;
                default: return -1;
            }
        }


        private string typeForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "资产类";
                case 1: return "负债类";
                case 2: return "共同类";
                case 3: return "权益类";
                case 4: return "成本类";
                case 5: return "损益类";
                default: return "未知类别";
            }
        }

        private int typeBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "资产类": return 0;
                case "负债类": return 1;
                case "共同类": return 2;
                case "权益类": return 3;
                case "成本类": return 4;
                case "损益类": return 5;
                default: return -1;
            }
        }

        private string DirectionForwardMapper([Data]int state)
        {
            switch (state)
            {
                case 0: return "借方";
                case 1: return "贷方";
                default: return "    0资产类 1负债类 2共同类 3.权益类 4.成本类 5.损益类";
            }
        }

        private int DirectionBackwardMapper([Data]string enable)
        {
            switch (enable)
            {
                case "借方": return 0;
                case "贷方": return 1;
                default: return -1;
            }
        }

        private void FormAccountTitle_Load(object sender, EventArgs e)
        {
            //设置两个请求参数
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            if (this.synchronizer.Save())
            {
                this.searchView1.Search();

                Condition condAccountTitle = new Condition();
                
                GlobalData.AllAccountTitle = RestClient.Get<List<IDictionary<string, object>>>(
            $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/pay_note/{condAccountTitle.AddCondition("enabled", 1)}/find_son");

                GlobalData.AllAccountTitleTure = RestClient.Get<List<IDictionary<string, object>>>(
            $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_title/{condAccountTitle.ToString()}");
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            this.model1.RemoveSelectedRows();

            Condition condAccountTitle = new Condition();

            GlobalData.AllAccountTitle = RestClient.Get<List<IDictionary<string, object>>>(
            $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_title/{condAccountTitle.AddCondition("enabled", 1)}");

            GlobalData.AllAccountTitleTure = RestClient.Get<List<IDictionary<string, object>>>(
            $"{Defines.ServerURL}/warehouse/{GlobalData.AccountBook}/account_title/{condAccountTitle.ToString()}");

        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, new Dictionary<string, object>()
            {          
            });
        }
    }
}
