using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI
{
    public partial class FormRemind : Form
    {
        string remindMessage = null;
        public FormRemind(string remindMessage)
        {
            this.remindMessage = remindMessage;
            InitializeComponent();
        }

        private void FormRemind_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = remindMessage;
        }
    }
}
