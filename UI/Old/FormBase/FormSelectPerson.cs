using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using unvell.ReoGrid;
using WMS.DataAccess;
using System.Threading;
using WMS.UI.FormBase;

namespace WMS.UI
{
    public partial class FormSelectPerson : Form,IFormSelect
    {
        public static Position DefaultPosition = Position.SHIPMENT;
        private PagerWidget<PersonView> pagerWidget = null;
        private int selectPosition =0;

        private int defaultPersonID = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        static Point staticPos = new Point(-1, -1);
        private Action<int> selectFinishCallback = null;

        public FormSelectPerson()
        {
            selectPosition = Convert.ToInt32(FormSelectPerson.DefaultPosition);
            InitializeComponent();
            //this.defaultPersonID = personid;
        }

        public void SetSelectFinishedCallback(Action<int> selectFinishedCallback)
        {
            this.selectFinishCallback = selectFinishedCallback;
        }

        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in BasePersonMetaData.PositionKeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化
            this.toolStripComboBoxSelectPosition.Items.Add("无");
            this.toolStripComboBoxSelectPosition.Items.AddRange(visibleColumnNames);
            this.toolStripComboBoxSelectPosition.SelectedIndex = selectPosition;


            this.textBoxPersonName.Text = "";
            this.pagerWidget = new PagerWidget<PersonView>(this.reoGridControlMain, FormBase.BasePersonMetaData.KeyNames, this.projectID, this.warehouseID);
            this.panelPagerWidget.Controls.Add(this.pagerWidget);
            this.pagerWidget.Show();

        }

        private void FormSelectPerson_Load(object sender, EventArgs e)
        {

            InitComponents();
            if (this.defaultPersonID != -1)
            {
                try
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    this.textBoxPersonName.Text = (from s in wmsEntities.PersonView where s.ID == defaultPersonID select s.Name).FirstOrDefault();
                    this.Search(defaultPersonID);
                }
                catch
                {
                    MessageBox.Show("加载数据失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
            }
            else
            {
                this.Search();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void Search(int selectID = -1)
        {
            this.pagerWidget.ClearCondition();
            if (this.checkBoxOnlyThisProAndWare.Checked == true)
            {
                this.pagerWidget.AddCondition("ProjectID", Convert.ToString(GlobalData.ProjectID));
                this.pagerWidget.AddCondition("WarehouseID", Convert.ToString(GlobalData.WarehouseID));
            }
            if (this.toolStripComboBoxSelectPosition.SelectedIndex != 0)
            {
                this.pagerWidget.AddCondition("Position", this.toolStripComboBoxSelectPosition.SelectedItem.ToString());
            }


            if (this.textBoxPersonName.Text != "")
            {
                string value = this.textBoxPersonName.Text;

                this.pagerWidget.AddCondition("人员姓名", value);
                this.pagerWidget.Search(false, selectID);
            }
            else
            {
                this.pagerWidget.Search(false, selectID);
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            this.SelectItem();
        }

        private void SelectItem()
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项");
                return;
            }
            this.selectFinishCallback?.Invoke(ids[0]);
            this.Hide();
        }

        private void textBoxPersonName_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPersonName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                this.Search();
            }
        }

        private void FormSelectPerson_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
  


        private void textBoxPersonName_VisbleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                staticPos = this.Location;
            }
            else
            {
                if (staticPos != new Point(-1, -1))
                {
                    this.Location = staticPos;
                }
                int[] ids = Utilities.GetSelectedIDs(this.reoGridControlMain);
                int id = -1;
                if (ids.Length > 0)
                {
                    id = ids[0];
                }
                this.pagerWidget.Search(true, id);
            }
        }

        private void checkBoxOnlyThisProAndWare_CheckedChanged(object sender, EventArgs e)
        {
            this.Search();
        }
    }
}
