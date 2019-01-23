using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMacro;
using System.IO;

using unvell.ReoGrid;
using unvell.ReoGrid.Print;
using System.Drawing.Printing;

namespace WMS.UI
{
    public partial class StandardFormPreviewExcel : Form
    {
        ExcelGenerator excelGenerator = new ExcelGenerator();
        volatile bool loadFinished = false;

        Dictionary<string, float> defaultPrintScales = new Dictionary<string, float>();
        Dictionary<string, Worksheet> patternTables = new Dictionary<string, Worksheet>();
        Dictionary<string, Dictionary<string, object>> data = new Dictionary<string, Dictionary<string, object>>();

        private Action printedCallback = null;
        private Action exportedExcelCallback = null;

        public StandardFormPreviewExcel(string formTitle, float printScale = 1.0F)
        {
            InitializeComponent();
            this.comboBoxPrintSettingsRange.SelectedIndex = 0;
            this.Text = formTitle;
            this.SetPrintScale(printScale);
        }

        public void SetPrintedCallback(Action callback)
        {
            this.printedCallback = callback;
        }

        public void SetExportedExcelCallback(Action callback)
        {
            this.exportedExcelCallback = callback;
        }

        private void SetPrintScaleAll(float scale)
        {
            if (this.loadFinished == false)
            {
                throw new Exception("SetPrintScaleAll只实现了在窗口显示之后设置的功能。");
            }
            else
            {
                foreach (var worksheet in this.reoGridControlMain.Worksheets)
                {
                    worksheet.PrintSettings.PageScaling = scale;
                    worksheet.AutoSplitPage();
                }
                this.textBoxScale.TextChanged -= this.textBoxScale_TextChanged;
                this.textBoxScale.Text = scale.ToString();
                this.textBoxScale.TextChanged += this.textBoxScale_TextChanged;
            }
        }

        private void SetPaperSizeAll(string paperSizeStr)
        {
            if (this.loadFinished == false)
            {
                throw new Exception("SetPaperSizeAll只实现了在窗口显示之后设置的功能。");
            }
            else
            {
                string[] segments = paperSizeStr.Split('x');
                if (segments.Length != 2) return;
                if (float.TryParse(segments[0], out float paperWidth) == false)
                {
                    return;
                }
                if (float.TryParse(segments[1], out float paperHeight) == false)
                {
                    return;
                }
                foreach (var worksheet in this.reoGridControlMain.Worksheets)
                {
                    worksheet.PrintSettings.PaperWidth = paperWidth;
                    worksheet.PrintSettings.PaperHeight = paperHeight;
                    worksheet.AutoSplitPage();
                }
                this.textBoxPaperSize.TextChanged -= this.textBoxPaperSize_TextChanged;
                this.textBoxPaperSize.Text = paperSizeStr.ToString();
                this.textBoxPaperSize.TextChanged += this.textBoxPaperSize_TextChanged;
            }
        }

        public void SetPrintScale(float scale, string worksheetName = "sheet1")
        {
            if (this.loadFinished == false)
            {
                if (this.defaultPrintScales.ContainsKey(worksheetName))
                {
                    this.defaultPrintScales[worksheetName] = scale;
                }
                else
                {
                    this.defaultPrintScales.Add(worksheetName, scale);
                }
            }
            else
            {
                foreach (var worksheet in this.reoGridControlMain.Worksheets)
                {
                    if (worksheet.Name == worksheetName)
                    {
                        if (worksheet.PrintSettings.PageScaling == scale) break; //如果放大倍数没变，就不用再调整一次了
                        worksheet.PrintSettings.PageScaling = scale;
                        worksheet.AutoSplitPage();
                        break;
                    }
                }
                this.textBoxScale.TextChanged -= this.textBoxScale_TextChanged;
                this.textBoxScale.Text = this.reoGridControlMain.CurrentWorksheet.PrintSettings.PageScaling.ToString();
                this.textBoxScale.TextChanged += this.textBoxScale_TextChanged;
            }
        }

        private void StandardFormPreviewExcel_Load(object sender, EventArgs e)
        {
            this.reoGridControlMain.SetSettings(WorkbookSettings.View_ShowHorScroll,false);
            try
            {
                this.reoGridControlMain.Worksheets.Clear();
                foreach (var item in this.patternTables)
                {
                    string sheetName = item.Key;
                    Worksheet patternTable = item.Value;
                    this.excelGenerator.SetPatternTable(patternTable);
                    if (this.data.ContainsKey(sheetName))
                    {
                        foreach (var datum in this.data[sheetName])
                        {
                            this.excelGenerator.AddData(datum.Key, datum.Value);
                        }
                    }
                    Worksheet newWorksheet = this.excelGenerator.Generate();
                    newWorksheet.Name = sheetName;
                    newWorksheet.EnableSettings(WorksheetSettings.View_ShowPageBreaks);
                    newWorksheet.SetSettings(WorksheetSettings.Behavior_AllowUserChangingPageBreaks, true);
                    newWorksheet.PrintSettings.Margins = new PageMargins(0,0.21f,0,0); //页边距设为0，打印用
                    this.reoGridControlMain.Worksheets.Add(newWorksheet);
                }

                loadFinished = true; //状态更新为加载完成，开始设置打印放大比例和纸张大小
                
                //默认打印纸尺寸
                float paperWidth = 9.6f; 
                float paperHeight = 5.51f;
                string paperSizeStr = paperWidth + " x " + paperHeight;

                foreach(Worksheet worksheet in this.reoGridControlMain.Worksheets)
                {
                    if (this.defaultPrintScales.ContainsKey(worksheet.Name))
                    {
                        this.SetPrintScale(this.defaultPrintScales[worksheet.Name], worksheet.Name);
                    }
                    worksheet.PrintSettings.PaperWidth = paperWidth;
                    worksheet.PrintSettings.PaperHeight = paperHeight;
                    worksheet.AutoSplitPage();
                }
                this.textBoxPaperSize.TextChanged -= this.textBoxPaperSize_TextChanged;
                this.textBoxPaperSize.Text = paperSizeStr;
                this.textBoxPaperSize.TextChanged += this.textBoxPaperSize_TextChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成报表错误：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public bool AddPatternTable(string filePath, string sheetName = "sheet1")
        {
            if (this.patternTables.ContainsKey(sheetName))
            {
                return false;
            }
            ReoGridControl tmpReoGrid = new ReoGridControl();
            try
            {
                tmpReoGrid.Load(filePath);
            }
            catch (Exception e)
            {
                MessageBox.Show("加载Excel文件失败！详细信息：\n" + e.Message);
                return false;
            }
            this.patternTables.Add(sheetName, tmpReoGrid.Worksheets[0]);
            return true;
        }

        public bool SetPatternTable(string filePath)
        {
            return this.AddPatternTable(filePath);
        }

        public void AddData<T>(string name, T data, string sheetName = "sheet1")
        {
            if (this.data.ContainsKey(sheetName) == false)
            {
                this.data.Add(sheetName, new Dictionary<string, object>());
            }
            if (this.data[sheetName].ContainsKey(name) == false)
            {
                this.data[sheetName].Add(name, data);
            }
            else
            {
                this.data[sheetName][name] = data;
            }
        }

        private void StandardFormPreviewExcel_Shown(object sender, EventArgs e)
        {

        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel文件|.xlsx";
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string filePath = dialog.FileName;
            this.reoGridControlMain.Save(filePath);
            this.exportedExcelCallback?.Invoke();
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            PrintDocument doc = null;
            try
            {
                doc = this.reoGridControlMain.CreatePrintSession().PrintDocument;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            //printPreviewDialog.Document = doc;
            //printPreviewDialog.ShowDialog();

            using (var pd = new System.Windows.Forms.PrintDialog())
            {
                pd.Document = doc;
                pd.UseEXDialog = true;  // in 64bit Windows

                if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    doc.PrinterSettings = pd.PrinterSettings;
                    try
                    {
                        doc.Print();
                        this.printedCallback?.Invoke();
                    }
                    catch
                    {
                        if (doc != null) doc.Dispose();
                        return;
                    }
                }
            }

            if (doc != null) doc.Dispose();
        }

        private void textBoxScale_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(this.textBoxScale.Text, out float printScale) == false)
            {
                return;
            }
            if(comboBoxPrintSettingsRange.SelectedIndex == 1)
            {
                this.SetPrintScaleAll(printScale);
                return;
            }
            this.SetPrintScale(printScale, this.reoGridControlMain.CurrentWorksheet.Name);
        }

        private void textBoxScale_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBoxScale_Click(object sender, EventArgs e)
        {
            this.textBoxScale.SelectAll();
        }

        private void textBoxScale_KeyDown(object sender, KeyEventArgs e)
        {
            if (float.TryParse(this.textBoxScale.Text, out float printScale) == false)
            {
                return;
            }
            if (printScale <= 0)
            {
                return;
            }
            if (e.KeyCode == Keys.Up)
            {
                printScale += 0.1F;
                this.textBoxScale.Text = string.Format("{0:0.##}", printScale);
            }
            else if (e.KeyCode == Keys.Down)
            {
                printScale -= 0.1F;
                this.textBoxScale.Text = string.Format("{0:0.##}", printScale);
            }
        }

        private void textBoxScale_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void reoGridControlMain_CurrentWorksheetChanged(object sender, EventArgs e)
        {
            if (loadFinished == false)
            {
                return;
            }
            try
            {
                var worksheet = this.reoGridControlMain.CurrentWorksheet;
                this.textBoxScale.Text = this.reoGridControlMain.CurrentWorksheet.PrintSettings.PageScaling.ToString();
                this.textBoxPaperSize.Text = worksheet.PrintSettings.PaperWidth + "x" + worksheet.PrintSettings.PaperHeight;
            }
            catch
            {
                //如果已经关闭窗口，这里会抛异常。直接返回即可
            }
        }

        private void textBoxPaperSize_TextChanged(object sender, EventArgs e)
        {
            if(this.comboBoxPrintSettingsRange.SelectedIndex == 1) //如果设置全部页，调用封装好的函数
            {
                this.SetPaperSizeAll(this.textBoxPaperSize.Text);
                return;
            }
            //否则自己处理
            string[] segments = textBoxPaperSize.Text.Split('x');
            if (segments.Length != 2) return;
            if (float.TryParse(segments[0], out float paperWidth) == false)
            {
                return;
            }
            if (float.TryParse(segments[1], out float paperHeight) == false)
            {
                return;
            }
            this.reoGridControlMain.CurrentWorksheet.PrintSettings.PaperWidth = paperWidth;
            this.reoGridControlMain.CurrentWorksheet.PrintSettings.PaperHeight = paperHeight;
            this.reoGridControlMain.CurrentWorksheet.AutoSplitPage();
        }

        private void comboBoxPrintSettingsRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBoxPrintSettingsRange.SelectedIndex == 1) //设置全部页
            {
                if (float.TryParse(this.textBoxScale.Text,out float printScale)==false)
                {
                    return;
                }
                this.SetPrintScaleAll(printScale);
                this.SetPaperSizeAll(this.textBoxPaperSize.Text);
            }
        }

        public void SetAllCowsHeigth(int row,int rowCounts,ushort height) {
            foreach (Worksheet worksheet in this.reoGridControlMain.Worksheets) { 
                worksheet.SetRowsHeight(row, rowCounts, height);
            }

        }
    }
}