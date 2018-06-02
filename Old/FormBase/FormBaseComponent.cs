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
using System.Data.SqlClient;

namespace WMS.UI
{
    public partial class FormBaseComponent : Form
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private int authority;
        private int authority_self = (int)Authority.BASE_COMPONENT;
        int supplierID = -1;
        private int check_history = 0;
        int projectID = -1;
        int warehouseID = -1;
        int userID = -1;
        private int setitem = -1;
        private Supplier supplier = null;
        private int contractst;   //合同状态
        private int contract_change = 1;
        private PagerWidget<ComponentView> pagerWidget = null;
        private SearchWidget<ComponentView> searchWidget = null;

        public FormBaseComponent(int authority, int supplierID, int projectID, int warehouseID, int userID)
        {
            InitializeComponent();
            this.authority = authority;
            this.supplierID = supplierID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
            this.userID = userID;
        }
        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in ComponenViewMetaData.componenkeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化
            //this.toolStripComboBoxSelect.Items.Add("无");
            //this.toolStripComboBoxSelect.Items.AddRange(visibleColumnNames);
            //this.toolStripComboBoxSelect.SelectedIndex = 0;

            this.pagerWidget = new PagerWidget<ComponentView>(this.reoGridControlComponen, ComponenViewMetaData.componenkeyNames);
            this.panelPager.Controls.Add(pagerWidget);
            pagerWidget.Show();

            this.searchWidget = new SearchWidget<ComponentView>(ComponenViewMetaData.KeyNames, this.pagerWidget);
            this.panelSearchWidget.Controls.Add(searchWidget);
        }

        private void FormBaseComponent_Load(object sender, EventArgs e)
        {
                InitComponents();
                this.pagerWidget.Search();

        }

        private void reoGridControlUser_Click(object sender, EventArgs e)
        {

        }

        //private void buttonSearch_Click(object sender, EventArgs e)
        //{

        //    if (this.buttonSearch.Text == "全部信息")
        //    {
        //        this.buttonSearch.Text = "查询";
        //        this.buttonSearch.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
        //        this.toolStripButtonAdd.Enabled = true;
        //        this.toolStripButtonAlter.Enabled = true;
        //        this.toolStripComboBoxSelect.Enabled = true;
        //        this.buttonImport.Enabled = true;
        //    }

        //    this.pagerWidget.ClearCondition();

        //    if (this.toolStripComboBoxSelect.SelectedIndex != 0)
        //    {
        //        this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.textBoxSearchValue.Text);
        //    }
        //    if ((this.authority & authority_self) != authority_self)
        //    {
        //        this.check_history = 0;
        //        this.pagerWidget.Search();
        //    }
        //    if ((this.authority & authority_self) == authority_self)
        //    {
        //        this.check_history = 0;
        //        this.pagerWidget.Search();
        //    }
        //}


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormComponenModify(this.projectID, this.warehouseID, this.supplierID, this.userID);
            form.SetMode(FormMode.ADD);
            form.SetAddFinishedCallback((addedID) =>
            {
                this.pagerWidget.Search(false,addedID);
            });
            form.Show();

        }//添加
  

        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlComponen.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var formComponenModify = new FormComponenModify(this.projectID, this.warehouseID, this.supplierID, this.userID, componenID);
                formComponenModify.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false,addedID);
                });
                formComponenModify.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }//修改

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlComponen.Worksheets[0];
            List<int> deleteIDs = new List<int>();
            for (int i = 0; i < worksheet.SelectionRange.Rows; i++)
            {
                try
                {
                    int curID = int.Parse(worksheet[i + worksheet.SelectionRange.Row, 0].ToString());
                    deleteIDs.Add(curID);
                }
                catch
                {
                    continue;
                }
            }
            if (deleteIDs.Count == 0)
            {
                MessageBox.Show("请选择您要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("您真的要删除这些记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            this.labelStatus.Text = "正在删除...";




            try
            {
                foreach (int id in deleteIDs)
                {

                    var supplycall = (from kn in wmsEntities.Supply
                                            where kn.ComponentID == id
                                            select kn.ID).ToArray();
                    if (supplycall.Length > 0)
                    {
                        MessageBox.Show("删除失败，选择的零件被供货信息引用，需要删除响应供货信息才能删除此零件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }

            }
            catch
            {
                MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        foreach (int id in deleteIDs)
                        {
                            this.wmsEntities.Database.ExecuteSqlCommand("DELETE FROM Component WHERE ID = @componenID", new SqlParameter("componenID", id));
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("删除失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.wmsEntities.SaveChanges();
                    this.Invoke(new Action(() =>
                    {
                        this.pagerWidget.Search();
                        MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                })).Start();


        }//删除

        private void textBoxSearchValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.pagerWidget.Search();
            }
        }

        //private void toolStripComboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (this.toolStripComboBoxSelect.SelectedIndex == 0)
        //    {
        //        this.textBoxSearchValue.Text = "";
        //        this.textBoxSearchValue.Enabled = false;
        //        this.textBoxSearchValue.BackColor = Color.LightGray;
        //    }
        //    else
        //    {
        //        this.textBoxSearchValue.Enabled = true;
        //        this.textBoxSearchValue.BackColor = Color.White;
        //    }
        //}

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButtonComponentSingleBoxTranPackingInfo_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlComponen.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                this.setitem = 1;
                var form = new ComponentSingleBoxTranPackingInfoModify(this.userID, this.setitem, componenID);
                if (check_history == 1)
                {
                    form.SetMode(FormMode.CHECK);
                }
                else
                {
                    form.SetMode(FormMode.ALTER);
                }
                form.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                form.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButtonComponentOuterPackingSize_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlComponen.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                this.setitem = 1;
                var form = new ComponentOuterPackingSizeModify(this.userID,this.setitem,componenID);
                if (check_history == 1)
                {
                    form.SetMode(FormMode.CHECK);
                }
                else
                {
                    form.SetMode(FormMode.ALTER);
                }
                form.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                form.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButtonComponentShipmentInfo_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlComponen.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int componenID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                this.setitem = 1;
                var form = new ComponentShipmentInfoModify(this.userID, this.setitem,componenID);

                if (check_history == 1)
                {
                    form.SetMode(FormMode.CHECK);
                }
                else
                {
                    form.SetMode(FormMode.ALTER);
                }
                form.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                form.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            //创建导入窗口
            StandardImportForm<DataAccess.Component> formImport =
                new StandardImportForm<DataAccess.Component>
                (
                    //参数1：KeyName
                    ComponenViewMetaData.componenkeyNames,
                    (results, unimportedColumns) => //参数2：导入数据二次处理回调函数
                    {
                        for (int i = 0; i < results.Count; i++)


                        {
                            string componentnameimport;
                            componentnameimport = results[i].Name;
                            //检查导入列表中是否重名
                            for (int j = i + 1; j < results.Count; j++)

                            {
                                if (componentnameimport == results[j].Name)
                                {
                                    MessageBox.Show("您输入的零件名" + componentnameimport + "在导入列表中重复", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    return false;

                                }

                            }

                            //检查数据库中同名
                            try
                            {
                                var sameNameUsers = (from u in wmsEntities.Component
                                                     where u.Name == componentnameimport
                                                     select u).ToArray();
                                if (sameNameUsers.Length > 0)
                                {
                                    MessageBox.Show("导入零件失败，已存在同名零件：" + componentnameimport, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                            }
                            catch
                            {

                                MessageBox.Show("操作失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                        return true;
                    },
                    () => //参数3：导入完成回调函数
                    {
                        this.searchWidget.Search();
                    }
                );

            //显示导入窗口
            formImport.Text = "导入零件信息";
            formImport.Show();
        }
    }
    }
