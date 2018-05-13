using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI.FormBasicInfos
{
    public partial class FormPerson : Form
    {
        public FormPerson()
        {
            MethodListenerContainer.Register("FormPerson",this);
            InitializeComponent();
        }

        private void FormPerson_Load(object sender, EventArgs e)
        {
            this.jsonRESTSynchronizer1.SetRequestParameter("$url", Defines.ServerURL);
            this.searchView1.Search();
        }

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            this.jsonRESTSynchronizer1.PushToServer();
        }

        private void pagerSearchJsonRESTAdapter1_Load(object sender, EventArgs e)
        {

        }

        public void PullCallback(WebException ex)
        {
            if(ex != null)
            {
                MessageBox.Show("查询失败！\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void PushCallback(WebException ex,HttpWebResponse res)
        {
            if (ex != null)
            {
                String message = ex.Message;
                if(ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("保存失败！\n" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void PushFinishedCallback()
        {
            MessageBox.Show("保存成功！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RoleChanged(int row,string role)
        {
            if(role == "管理员")
            {
                this.model1[row, "authorityString"] = "hehe";
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            this.model1.RemoveSelectedRows();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            this.model1.InsertRow(0, null);
        }
    }
}
