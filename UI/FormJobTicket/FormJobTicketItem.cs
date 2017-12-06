﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using WMS.DataAccess;
using unvell.ReoGrid;

namespace WMS.UI
{
    public partial class FormJobTicketItem : Form
    {
        private int jobTicketID = -1;
        private WMSEntities wmsEntities = new WMSEntities();
        Action jobTicketStateChangedCallback = null;

        private KeyName[] visibleColumns = (from kn in JobTicketItemViewMetaData.KeyNames
                                            where kn.Visible == true
                                            select kn).ToArray();

        public FormJobTicketItem(int jobTicketID)
        {
            InitializeComponent();
            this.jobTicketID = jobTicketID;
        }

        public void SetJobTicketStateChangedCallback(Action jobTicketStateChangedCallback)
        {
            this.jobTicketStateChangedCallback = jobTicketStateChangedCallback;
        }

        private void FormJobTicketItem_Load(object sender, EventArgs e)
        {
            InitComponents();
            this.Search();
        }

        private void InitComponents()
        {
            this.wmsEntities.Database.Connection.Open();

            //初始化表格
            var worksheet = this.reoGridControlMain.Worksheets[0];
            worksheet.SelectionMode = WorksheetSelectionMode.Row;
            worksheet.SelectionRangeChanged += this.worksheet_SelectionRangeChanged;

            for (int i = 0; i < JobTicketItemViewMetaData.KeyNames.Length; i++)
            {
                worksheet.ColumnHeaders[i].Text = JobTicketItemViewMetaData.KeyNames[i].Name;
                worksheet.ColumnHeaders[i].IsVisible = JobTicketItemViewMetaData.KeyNames[i].Visible;
            }
            worksheet.Columns = JobTicketItemViewMetaData.KeyNames.Length; //限制表的长度

            //初始化属性编辑框
            this.tableLayoutPanelProperties.Controls.Clear();
            for (int i = 0; i < JobTicketItemViewMetaData.KeyNames.Length; i++)
            {
                KeyName curKeyName = JobTicketItemViewMetaData.KeyNames[i];
                if (curKeyName.Visible == false && curKeyName.Editable == false)
                {
                    continue;
                }
                Label label = new Label();
                label.Text = curKeyName.Name;
                label.Dock = DockStyle.Fill;
                this.tableLayoutPanelProperties.Controls.Add(label);

                //如果是编辑框形式
                if (curKeyName.ComboBoxItems == null)
                {
                    TextBox textBox = new TextBox();
                    textBox.Name = "textBox" + curKeyName.Key;
                    if (curKeyName.Editable == false)
                    {
                        textBox.Enabled = false;
                    }
                    textBox.Dock = DockStyle.Fill;
                    this.tableLayoutPanelProperties.Controls.Add(textBox);
                }
                else //否则是下拉列表形式
                {
                    ComboBox comboBox = new ComboBox();
                    comboBox.Name = "comboBox" + curKeyName.Key;
                    comboBox.Items.AddRange(curKeyName.ComboBoxItems);
                    comboBox.SelectedIndex = 0;
                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                    comboBox.Dock = DockStyle.Fill;
                    this.tableLayoutPanelProperties.Controls.Add(comboBox);
                }

            }

            this.Controls.Find("textBoxStockInfoID", true)[0].TextChanged += textBoxStockInfoID_TextChanged; ;
        }

        private void textBoxStockInfoID_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxStockInfoID = (TextBox)this.Controls.Find("textBoxStockInfoID", true)[0];
            if (int.TryParse(textBoxStockInfoID.Text,out int stockInfoID))
            {
                StockInfo stockInfo = (from s in this.wmsEntities.StockInfo
                                       where s.ID == stockInfoID
                                       select s).FirstOrDefault();
                if(stockInfo == null)
                {
                    return;
                }
                Utilities.CopyPropertiesToTextBoxes(stockInfo,this);
            }
        }

        private JobTicketView GetJobTicketViewByNo(string jobTicketNo)
        {
            return (from jt in this.wmsEntities.JobTicketView
                    where jt.JobTicketNo == jobTicketNo
                    select jt).FirstOrDefault();
        }

        private void Search()
        {
            var worksheet = this.reoGridControlMain.Worksheets[0];

            worksheet[0, 1] = "加载中...";
            new Thread(new ThreadStart(() =>
            {
                JobTicketItemView[] jobTicketItemViews = (from j in wmsEntities.JobTicketItemView
                                                          where j.JobTicketID == this.jobTicketID
                                                          select j).ToArray();

                this.reoGridControlMain.Invoke(new Action(() =>
                {
                    this.labelStatus.Text = "加载完成";
                    worksheet.DeleteRangeData(RangePosition.EntireRange);
                    if (jobTicketItemViews.Length == 0)
                    {
                        worksheet[0, 1] = "没有符合条件的记录";
                    }
                    for (int i = 0; i < jobTicketItemViews.Length; i++)
                    {
                        var curJobTicketViews = jobTicketItemViews[i];
                        object[] columns = Utilities.GetValuesByPropertieNames(curJobTicketViews, (from kn in JobTicketItemViewMetaData.KeyNames select kn.Key).ToArray());
                        for (int j = 0; j < columns.Length; j++)
                        {
                            worksheet[i, j] = columns[j] == null ? "" : columns[j].ToString();
                        }
                    }
                    this.RefreshTextBoxes();
                }));
            })).Start();
        }

        private void labelStatus_Click(object sender, EventArgs e)
        {

        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            const string STRING_FINISHED = "已完成";
            int[] selectedIDs = this.GetSelectedIDs();
            if(selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择您要操作的条目","提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            new Thread(new ThreadStart(()=>
            {
                //将状态置为已完成
                foreach (int id in selectedIDs)
                {
                    this.wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE JobTicketItem SET State = '{0}' WHERE ID = {1};", STRING_FINISHED, id));
                }
                this.wmsEntities.SaveChanges();
                
                //如果作业单中所有条目都完成，询问是否将作业单标记为完成
                int unfinishedJobTicketItemCount = wmsEntities.Database.SqlQuery<int>(String.Format("SELECT COUNT(*) FROM JobTicketItem WHERE JobTicketID = {0} AND State <> '{1}'", this.jobTicketID, STRING_FINISHED)).Single();
                if (unfinishedJobTicketItemCount == 0)
                {
                    if (MessageBox.Show("检测到所有的作业任务都已经完成，是否将作业单状态更新为完成？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE JobTicket SET State = '{0}' WHERE ID = {1}",STRING_FINISHED,this.jobTicketID));
                        this.wmsEntities.SaveChanges();
                    }
                    this.jobTicketStateChangedCallback?.Invoke();
                }
                this.Invoke(new Action(this.Search));
                MessageBox.Show("操作成功！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            })).Start();
        }



        private int[] GetSelectedIDs()
        {
            List<int> ids = new List<int>();
            var worksheet = this.reoGridControlMain.Worksheets[0];
            for(int row = worksheet.SelectionRange.Row; row <= worksheet.SelectionRange.EndRow; row++)
            {
                if (worksheet[row, 0] == null) continue;
                if(int.TryParse(worksheet[row, 0].ToString(),out int jobTicketID))
                {
                    ids.Add(jobTicketID);
                }
            }
            return ids.ToArray();
        }

        private void buttonUnfinish_Click(object sender, EventArgs e)
        {
            const string STRING_UNFINISHED = "未完成";
            int[] selectedIDs = this.GetSelectedIDs();
            if (selectedIDs.Length == 0)
            {
                MessageBox.Show("请选择您要操作的条目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            new Thread(new ThreadStart(() =>
            {
                foreach (int id in selectedIDs)
                {
                    this.wmsEntities.Database.ExecuteSqlCommand(String.Format("UPDATE JobTicketItem SET State = '{0}' WHERE ID = {1};", STRING_UNFINISHED, id));
                }
                this.wmsEntities.SaveChanges();
                this.Invoke(new Action(this.Search));
                MessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            })).Start();
        }

        private void ClearTextBoxes()
        {
            foreach (Control control in this.tableLayoutPanelProperties.Controls)
            {
                if(control is TextBox)
                {
                    TextBox textBox = control as TextBox;
                    textBox.Text = "";
                }
            }
        }

        private void RefreshTextBoxes()
        {
            this.ClearTextBoxes();
            var worksheet = this.reoGridControlMain.Worksheets[0];
            int[] ids = this.GetSelectedIDs();
            if (ids.Length == 0) return;
            int id = ids[0];
            JobTicketItemView jobTicketItemView = (from jti in this.wmsEntities.JobTicketItemView
                                           where jti.ID == id
                                           select jti).FirstOrDefault();
            if (jobTicketItemView == null)
            {
                MessageBox.Show("系统错误，未找到相应作业单项目", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Utilities.CopyPropertiesToTextBoxes(jobTicketItemView, this);
            Utilities.CopyPropertiesToComboBoxes(jobTicketItemView, this);
        }

        private void worksheet_SelectionRangeChanged(object sender, EventArgs e)
        {
            RefreshTextBoxes();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            JobTicketItem newItem = new JobTicketItem();
            if (Utilities.CopyTextBoxTextsToProperties(this, newItem, JobTicketItemViewMetaData.KeyNames, out string errorMessage) == false)
            {
                MessageBox.Show(errorMessage,"提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            newItem.JobTicketID = this.jobTicketID;
            this.wmsEntities.JobTicketItem.Add(newItem);
            this.wmsEntities.SaveChanges();
        }
    }
}