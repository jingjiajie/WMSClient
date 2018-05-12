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
    public partial class FormStockInfoCheckTicketComponenModify : Form
    {
        public FormStockInfoCheckTicketComponenModify()
        {
            InitializeComponent();

        }
        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in ComponenMetaData.componenkeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化
            this.toolStripComboBoxSelect1.Items.Add("无");
            this.toolStripComboBoxSelect1.Items.AddRange(visibleColumnNames);
            this.toolStripComboBoxSelect1.SelectedIndex = 0;


            //初始化表格
            var worksheet = this.reoGridControlComponen.Worksheets[0];
            worksheet.SelectionMode = unvell.ReoGrid.WorksheetSelectionMode.Row;
            for (int i = 0; i < ComponenMetaData.componenkeyNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = ComponenMetaData.componenkeyNames[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = ComponenMetaData.componenkeyNames[i].Visible;
            }
            worksheet.Columns = ComponenMetaData.componenkeyNames.Length;//限制表的长度
            Console.WriteLine("表格行数：" + ComponenMetaData.componenkeyNames.Length);
        }

        private void FormStockInfoCheckTicketComponenModify_Load(object sender, EventArgs e)
        {
            this.InitComponents();
            this.Search();


        } 

        private void labelStatus_Click(object sender, EventArgs e)
        {

        }
        private void Search()
        {
            string key = null;
            string value = null;

            if (this.toolStripComboBoxSelect1.SelectedIndex != 0)
            {
                key = (from kn in ComponenMetaData.componenkeyNames
                       where kn.Name == this.toolStripComboBoxSelect1.SelectedItem.ToString()
                       select kn.Key).First();
                value = this.textBoxSearchValue.Text;
            }

            this.labelStatus.Text = "正在搜索中...";


            new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                var wmsEntities = new WMS.DataAccess.WMSEntities();

                WMS.DataAccess.ComponentView[] componentViews = null;


               
                    if (key == null || value == null)        //搜索所有
                    {
                        componentViews = wmsEntities.Database.SqlQuery<WMS.DataAccess.ComponentView>("SELECT * FROM ComponentView").ToArray();
                    }
                    else
                    {
                        //double tmp;
                        //if (Double.TryParse(value, out tmp) == false) //不是数字则加上单引号
                        //{
                        value = "'" + value + "'";
                        //}
                        try
                        {
                            componentViews = wmsEntities.Database.SqlQuery<WMS.DataAccess.ComponentView>(String.Format("SELECT * FROM ComponentView WHERE {0} = {1}", key, value)).ToArray();
                        }
                        catch
                        {
                            MessageBox.Show("查询的值不合法，请输入正确的值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
               
              

                this.reoGridControlComponen.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "搜索完成";
                    var worksheet = this.reoGridControlComponen.Worksheets[0];
                    worksheet.DeleteRangeData(unvell.ReoGrid.RangePosition.EntireRange);
                    for (int i = 0; i < componentViews.Length; i++)
                    {

                        WMS.DataAccess.ComponentView curComponentView = componentViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curComponentView, (from kn in ComponenMetaData.componenkeyNames select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            worksheet[i, j] = columns[j];
                        }
                    }
                }));

            })).Start();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.SelectItem();
        }

        private void SelectItem()
        {
            int[] ids = Utilities.GetSelectedIDs(this.reoGridControlComponen);
            if (ids.Length != 1)
            {
                MessageBox.Show("请选择一项");
                return;
            }
            //this.selectFinishCallback?.Invoke(ids[0]);
            this.Close();
        }
    }
}
