using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using NPOI;
using NPOI.OpenXml4Net;
using NPOI.OpenXmlFormats;
using NPOI.HSSF;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

using NPOI.HPSF;

using NPOI.POIFS.FileSystem;
using NPOI.SS.Util;


namespace WMS.UI.FormAcccount
{
    public partial class FormSelectSummaryTime : Form
    {
        private Action addFinishedCallback = null;
        public FormSelectSummaryTime()
        {
            InitializeComponent();
        }

        private void FormSelectSummaryTime_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            Utilities.BindBlueButton(this.buttonADD);
            this.model1.InsertRow(0, null);
            this.model1[this.model1.SelectionRange.Row, "endTime"] = DateTime.Now;
        }

        public void SetAddFinishedCallback(Action callback)
        {
            this.addFinishedCallback = callback;
        }

        private void buttonADD_Click(object sender, EventArgs e)
        {
            this.buttonADD.Focus();
            var startTime = this.model1[this.model1.SelectionRange.Row, "startTime"];
            var endTime = this.model1[this.model1.SelectionRange.Row, "endTime"];
            try
            {
                string body = "{\"warehouseId\":\"" + GlobalData.Warehouse["id"] + "\",\"endTime\":\"" + endTime + "\",\"startTime\":\"" + startTime + "\"}";
                string url = Defines.ServerURL + "/warehouse/" + GlobalData.AccountBook + "/account_record/summary_all_title";
                var returnSummaryList=RestClient.RequestPost<List<IDictionary<string, object>>>(url, body);

                    StringBuilder remindBody = new StringBuilder();
                    foreach (IDictionary<string, object> accountTitleSummary in returnSummaryList)
                    {
                        //remindBody = remindBody
                        //        .Append("科目名称：“").Append(accountTitleSummary["curAccountTitleName"])
                        //        .Append("”，余额：“").Append(accountTitleSummary["balance"])
                        //        .Append("”存在赤字！请核准账目记录！\r\n");

                    }

                string localFilePath = ShowSaveFileDialog();

                if (string.IsNullOrEmpty(localFilePath)) return;

                DataTable dt = new DataTable();

                //创建一个工作簿
                IWorkbook workbook = new HSSFWorkbook();

                //创建一个 sheet 表
                ISheet sheet = workbook.CreateSheet(dt.TableName);

                //创建一行
                IRow rowH = sheet.CreateRow(0);

                //创建一个单元格
                ICell cell = null;

                //创建单元格样式
                ICellStyle cellStyle = workbook.CreateCellStyle();

                //创建格式
                IDataFormat dataFormat = workbook.CreateDataFormat();

                //设置为文本格式，也可以为 text，即 dataFormat.GetFormat("text");
                cellStyle.DataFormat = dataFormat.GetFormat("@");

                //设置列名
                foreach (DataColumn col in dt.Columns)
                {
                    //创建单元格并设置单元格内容
                    rowH.CreateCell(col.Ordinal).SetCellValue(col.Caption);

                    //设置单元格格式
                    rowH.Cells[col.Ordinal].CellStyle = cellStyle;
                }

                //写入数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //跳过第一行，第一行为列名
                    IRow row = sheet.CreateRow(i + 1);

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                        cell.CellStyle = cellStyle;
                    }
                }

              
                //设置新建文件路径及名称
                string savePath = localFilePath + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xls";

                //创建文件
                FileStream file = new FileStream(savePath, FileMode.CreateNew, FileAccess.Write);

                //创建一个 IO 流
                MemoryStream ms = new MemoryStream();

                //写入到流
                workbook.Write(ms);

                //转换为字节数组
                byte[] bytes = ms.ToArray();

                file.Write(bytes, 0, bytes.Length);
                file.Flush();

           

                //释放资源
                bytes = null;

                ms.Close();
                ms.Dispose();

                file.Close();
                file.Dispose();

                workbook.Close();
                sheet = null;
                workbook = null;


            }
            catch (WebException ex)
            {
                string message = ex.Message;
                if (ex.Response != null)
                {
                    message = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                }
                MessageBox.Show("汇总失败：" + message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Close();
        }


        //选择保存路径
        private string ShowSaveFileDialog()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择条形码导出文件夹";
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }
            return dialog.SelectedPath;
        }

        private void FormSelectSummaryTime_Closed(object sender, FormClosedEventArgs e)
        {
            if (this.addFinishedCallback != null)
            {
                this.addFinishedCallback();
            }
        }
    }
}
