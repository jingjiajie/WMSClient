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
    public partial class FormBaseProject : Form
    {
        private PagerWidget<ProjectView> pagerWidget = null;
        private WMSEntities wmsEntities = new WMSEntities();
        public FormBaseProject()
        {
            InitializeComponent();
        }

        private void InitComponents()
        {
            string[] visibleColumnNames = (from kn in BaseProjectMetaData.KeyNames
                                           where kn.Visible == true
                                           select kn.Name).ToArray();

            //初始化
            this.toolStripComboBoxSelect.Items.Add("无");
            this.toolStripComboBoxSelect.Items.AddRange(visibleColumnNames);
            this.toolStripComboBoxSelect.SelectedIndex = 0;

            this.pagerWidget = new PagerWidget<ProjectView>(this.reoGridControlProject, BaseProjectMetaData.KeyNames);
            this.panelPager.Controls.Add(pagerWidget);
            pagerWidget.Show();
        }

        private void FormBaseProject_Load(object sender, EventArgs e)
        {
            InitComponents();
            this.pagerWidget.Search();
        }

        //private void Search(int selectedID = -1)
        //{
        //    string key = null;
        //    string value = null;

        //    if (this.toolStripComboBoxSelect.SelectedIndex != 0)
        //    {
        //        key = (from kn in BaseProjectMetaData.KeyNames
        //               where kn.Name == this.toolStripComboBoxSelect.SelectedItem.ToString()
        //               select kn.Key).First();
        //        value = this.toolStripTextBoxSelect.Text;
        //    }

        //    this.labelStatus.Text = "正在搜索中...";
        //    var worksheet = this.reoGridControlProject.Worksheets[0];
        //    worksheet[0, 0] = "加载中...";
        //    new Thread(new ThreadStart(() =>
        //    {
        //        var wmsEntities = new WMSEntities();
        //        ProjectView[] projectViews = null;
        //        string sql = "SELECT * FROM ProjectView WHERE 1=1 ";
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        if (key != null && value != null) //查询条件不为null则增加查询条件
        //        {
        //            sql += "AND " + key + " = @value ";
        //            parameters.Add(new SqlParameter("value", value));
        //        }
        //        sql += " ORDER BY ID DESC"; //倒序排序
        //        try
        //        {
        //            projectViews = wmsEntities.Database.SqlQuery<ProjectView>(sql, parameters.ToArray()).ToArray();
        //        }
        //        catch (EntityCommandExecutionException)
        //        {
        //            MessageBox.Show("查询失败，请检查输入查询条件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("查询失败，请检查网络连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        //if (key == null || value == null) //查询条件为null则查询全部内容
        //        //{
        //        //    Projects = wmsEntities.Database.SqlQuery<DataAccess.Project>("SELECT * FROM Project").ToArray();
        //        //}
        //        //else
        //        //{
        //        //    if (Utilities.IsQuotateType(typeof(Project).GetProperty(key).PropertyType)) //不是数字则加上单引号
        //        //    {
        //        //        value = "'" + value + "'";
        //        //    }
        //        //    try
        //        //    {
        //        //        Projects = wmsEntities.Database.SqlQuery<DataAccess.Project>(String.Format("SELECT * FROM Project WHERE {0} = {1}", key, value)).ToArray();
        //        //    }
        //        //    catch
        //        //    {
        //        //        MessageBox.Show("查询的值不合法，请输入正确的值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        //        return;
        //        //    }
        //        //}
        //        this.reoGridControlProject.Invoke(new Action(() =>
        //        {
        //            this.labelStatus.Text = "搜索完成";
        //            worksheet.DeleteRangeData(RangePosition.EntireRange);
        //            if (projectViews.Length == 0)
        //            {
        //                worksheet[0, 1] = "没有查询到符合条件的记录";
        //            }
        //            for (int i = 0; i < projectViews.Length; i++)
        //            {
        //                ProjectView curprojectViews = projectViews[i];
        //                object[] columns = Utilities.GetValuesByPropertieNames(curprojectViews, (from kn in BaseProjectMetaData.KeyNames select kn.Key).ToArray());
        //                for (int j = 0; j < worksheet.Columns; j++)
        //                {
        //                    worksheet[i, j] = columns[j] == null ? "" : columns[j].ToString();
        //                }
        //            }
        //        }));
        //    })).Start();
        //    if (selectedID != -1)
        //    {
        //        Utilities.SelectLineByID(this.reoGridControlProject, selectedID);
        //    }
        //}

        private void toolStripButtonSelect_Click(object sender, EventArgs e)
        {
            this.pagerWidget.ClearCondition();
            if (this.toolStripComboBoxSelect.SelectedIndex != 0)
            {
                this.pagerWidget.AddCondition(this.toolStripComboBoxSelect.SelectedItem.ToString(), this.toolStripTextBoxSelect.Text);
            }
            this.pagerWidget.Search();
        }

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            var formBaseProjectModify = new FormBaseProjectModify();
            formBaseProjectModify.SetMode(FormMode.ADD);

            formBaseProjectModify.SetAddFinishedCallback((addedID) =>
            {
                this.pagerWidget.Search(false, addedID);
                var worksheet = this.reoGridControlProject.Worksheets[0];
            });
            //formBaseProjectModify.SetAddFinishedCallback(this.Search);
            formBaseProjectModify.Show();
        }


        private void toolStripButtonAlter_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlProject.Worksheets[0];
            try
            {
                if (worksheet.SelectionRange.Rows != 1)
                {
                    throw new Exception();
                }
                int projectID = int.Parse(worksheet[worksheet.SelectionRange.Row, 0].ToString());
                var formBaseProjectModify = new FormBaseProjectModify(projectID);
                formBaseProjectModify.SetModifyFinishedCallback((addedID) =>
                {
                    this.pagerWidget.Search(false, addedID);
                });
                //formBaseProjectModify.SetModifyFinishedCallback(this.Search);
                formBaseProjectModify.Show();
            }
            catch
            {
                MessageBox.Show("请选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var worksheet = this.reoGridControlProject.Worksheets[0];
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
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    foreach (int id in deleteIDs)
                    {
                        this.wmsEntities.Database.ExecuteSqlCommand("DELETE FROM Project WHERE ID = @projectID", new SqlParameter("projectID", id));
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
        }

        private void toolStripTextBoxSelect_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.toolStripButtonSelect.PerformClick();
            }
        }

        private void toolStripComboBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.toolStripComboBoxSelect.SelectedIndex == 0)
            {
                this.toolStripTextBoxSelect.Text = "";
                this.toolStripTextBoxSelect.Enabled = false;
                this.toolStripTextBoxSelect.BackColor = Color.LightGray;

            }
            else
            {
                this.toolStripTextBoxSelect.Enabled = true;
                this.toolStripTextBoxSelect.BackColor = Color.White;
            }
        }
    }
}
