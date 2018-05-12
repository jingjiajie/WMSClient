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

namespace WMS.UI.FormBase
{
    public partial class FormUser : Form
    {
        private WMSEntities wmsEntities = new WMSEntities();
        private int userID = -1;
        private int projectID = -1;
        private int warehouseID = -1;
        public FormUser(int userID,int projectID,int warehouseID)
        {
            InitializeComponent();
            this.userID = userID;
            this.projectID = projectID;
            this.warehouseID = warehouseID;
        }

        private void FormUser_Load(object sender, EventArgs e)
        {
            InitComponents();
            this.Search();
        }

        private void InitComponents()
        {
            this.wmsEntities.Database.Connection.Open();

            string[] visibleColumnNames = (from kn in UserMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化
            this.comboBoxSearchCondition.Items.Add("无");
            this.comboBoxSearchCondition.Items.AddRange(visibleColumnNames);
            this.comboBoxSearchCondition.Items.Remove("密码"); //不能按密码搜索
            this.comboBoxSearchCondition.SelectedIndex = 0;


            //初始化表格
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            for (int i = 0; i < UserMetaData.KeyNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = UserMetaData.KeyNames[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = UserMetaData.KeyNames[i].Visible;
            }
            worksheet.Columns = UserMetaData.KeyNames.Length; //限制表的长度
        }

        private void Search(int selectedID = -1)
        {
            string key = null;
            string value = null;

            if (this.comboBoxSearchCondition.SelectedIndex != 0)
            {
                key = (from kn in UserMetaData.KeyNames
                       where kn.Name == this.comboBoxSearchCondition.SelectedItem.ToString()
                       select kn.Key).First();
                value = this.textBoxSearchValue.Text;
            }

            this.labelStatus.Text = "正在搜索中...";
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet[0, 0] = "加载中...";
            new Thread(new ThreadStart(() =>
            {

                UserView[] userViews = null;
                string sql = "SELECT * FROM UserView WHERE 1=1 ";
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (key != null && value != null) //查询条件不为null则增加查询条件
                {
                    sql += "AND " + key + " = @value ";
                    parameters.Add(new SqlParameter("value", value));
                }
                sql += " ORDER BY ID DESC"; //倒序排序

                try
                {
                    userViews = wmsEntities.Database.SqlQuery<UserView>(sql, parameters.ToArray()).ToArray();
                }
                catch
                {
                    MessageBox.Show("查询失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.reoGridControlMain.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "搜索完成";
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    worksheet.Rows = userViews.Length < 10 ? 10 : userViews.Length;
                    if (userViews.Length == 0)
                    {
                        worksheet[0, 1] = "没有查询到符合条件的记录";
                    }
                    for (int i = 0; i < userViews.Length; i++)
                    {
                        UserView curUserView = userViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curUserView, (from kn in UserMetaData.KeyNames select kn.Key).ToArray());
                        for (int j = 0; j < worksheet.Columns; j++)
                        {
                            worksheet[i, j] = columns[j] == null ? "" : columns[j].ToString();
                        }
                    }
                    if(selectedID != -1)
                    {
                        Utilities.SelectLineByID(this.reoGridControlMain, selectedID);
                    }
                }));
            })).Start();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var formUserModify = new FormUserModify();
            formUserModify.SetMode(FormMode.ADD);
            formUserModify.SetAddFinishedCallback((addedID) =>
            {
                this.Search(addedID);
            });
            formUserModify.Show();
        }

        private void buttonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlMain.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int userID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var formUserModify = new FormUserModify(userID);
                formUserModify.SetModifyFinishedCallback((id) =>
                {
                    this.Search(id);
                });
                formUserModify.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlMain.Worksheets[0];
            int[] deleteIDs = Utilities.GetSelectedIDs(this.reoGridControlMain);

            if (deleteIDs.Length == 0)
            {
                MessageBox.Show("请选择您要删除的记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("您真的要删除这些记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
            if (deleteIDs.Contains(this.userID))
            {
                MessageBox.Show("登录用户不可删除自己！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.labelStatus.Text = "正在删除...";


            new Thread(new ThreadStart(() =>
            {
                try
                {
                    int managerUserCount = (from u in wmsEntities.User
                                            where u.Authority == UserMetaData.AUTHORITY_MANAGER
                                            select u).Count();
                    int deleteManagerUserCount = (from u in wmsEntities.User
                                                  where u.Authority == UserMetaData.AUTHORITY_MANAGER
                                                  && deleteIDs.Contains(u.ID)
                                                  select u).Count();
                    if(deleteManagerUserCount == managerUserCount)
                    {
                        MessageBox.Show("删除失败，系统中至少保留一个管理员用户！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    foreach (int id in deleteIDs)
                    {
                        this.wmsEntities.Database.ExecuteSqlCommand("DELETE FROM [User] WHERE ID = @userID", new SqlParameter("@userID", id));
                    }
                    this.wmsEntities.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("删除失败，请检查网络连接","提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.Invoke(new Action(() =>
                {
                    this.Search();
                }));
                MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            })).Start();
        }

        private void comboBoxSearchCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.comboBoxSearchCondition.SelectedIndex == 0)
            {
                this.textBoxSearchValue.Text = "";
                this.textBoxSearchValue.Enabled = false;
            }
            else
            {
                this.textBoxSearchValue.Enabled = true;
            }
        }

        private void textBoxSearchValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                this.Search();
            }
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            //创建导入窗口
            StandardImportForm<User> formUserImport =
                new StandardImportForm<User>
                (
                    UserMetaData.KeyNames, //参数1：KeyName
                    (results, unimportedColumns) => //参数2：导入数据二次处理回调函数
                    {
                        WMSEntities wmsEntities = new WMSEntities();
                        for (int i = 0; i < results.Count; i++)
                        {
                            User result = results[i];
                            if (string.IsNullOrWhiteSpace(result.AuthorityName))
                            {
                                MessageBox.Show(string.Format("行{0}：角色不可以为空，必须为管理员，收货员，发货员，库存管理员，供应商中的一项或多项", i + 1), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            int sameNameUserCount = (from u in wmsEntities.User where u.Username == result.Username select u).Count();
                            if(sameNameUserCount > 0)
                            {
                                MessageBox.Show(string.Format("行{0}：已存在同名用户：{1}", i + 1, results[i].Username), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            string authorityName = results[i].AuthorityName;
                            string supplierName = unimportedColumns["SupplierName"][i];
                            if(string.IsNullOrWhiteSpace(supplierName) == false && authorityName != "供应商")
                            {
                                MessageBox.Show(string.Format("行{0}：填写了供应商名称的用户，角色必须为“供应商”！",i+1), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }

                            if (authorityName == "供应商")
                            {
                                if (string.IsNullOrWhiteSpace(supplierName))
                                {
                                    MessageBox.Show(string.Format("行{0}：角色为供应商的用户，必须填写供应商名称！", i + 1), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                                Supplier supplier = (from s in wmsEntities.Supplier where s.Name == supplierName && s.IsHistory==0 select s).FirstOrDefault();
                                if (supplier == null)
                                {
                                    MessageBox.Show(string.Format("行{0}：找不到名称为\"{1}\"的供应商！", i + 1, supplierName), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return false;
                                }
                                results[i].SupplierID = supplier.ID;
                                results[i].Authority = UserMetaData.AUTHORITY_SUPPLIER;
                            }
                            else if (authorityName.Contains("管理员"))
                            {
                                results[i].Authority |= UserMetaData.AUTHORITY_MANAGER;
                            }
                            else if (authorityName.Contains("收货员"))
                            {
                                results[i].Authority |= UserMetaData.AUTHORITY_RECEIPT_MANAGER;
                            }
                            else if (authorityName.Contains("发货员"))
                            {
                                results[i].Authority |= UserMetaData.AUTHORITY_SHIPMENT_MANAGER;
                            }
                            else if (authorityName.Contains("库存管理员"))
                            {
                                results[i].Authority |= UserMetaData.AUTHORITY_STOCK_MANAGER;
                            }
                        }
                        return true;
                    },
                    () => //参数3：导入完成回调函数
                    {
                        this.Search();
                    }
                );

            //显示导入窗口
            formUserImport.Show();
        }
    }
}
