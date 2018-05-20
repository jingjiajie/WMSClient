using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontWork;

namespace WMS.UI
{
    public partial class FormInspectionNote : Form
    {
        public FormInspectionNote()
        {
            MethodListenerContainer.Register(this);
            InitializeComponent();
        }

        private void FormInspectionNote_Load(object sender, EventArgs e)
        {
            this.synchronizer.SetRequestParameter("$url", Defines.ServerURL);
            this.synchronizer.SetRequestParameter("$accountBook", GlobalData.AccountBook);
            this.searchView1.Search();
        }

        private string StateForwardMapper(int state)
        {
            switch (state)
            {
                case 0: return "送检中";
                case 1: return "部分送检完成";
                case 2: return "全部送检完成";
                default: throw new Exception("状态错误:" + state);
            }
        }
    }
}
