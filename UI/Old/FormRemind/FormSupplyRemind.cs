using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using WMS.UI.FormReceipt;
using WMS.UI.FormBase;
using WMS.DataAccess;
using System.Diagnostics;
using System.Threading;
using System.Data.SqlClient;

namespace WMS.UI
{
    public partial class FormSupplyRemind : Form
    {
        private int projectID = GlobalData.ProjectID;
        private int warehouseID = GlobalData.WarehouseID;
        private Action HidedCallback = null;
        private Action ShowCallback = null;
        private static FormSupplyRemind instance = null;
        System.Timers.Timer timer = new System.Timers.Timer();
        StringBuilder stringBuilder = new StringBuilder();
        private bool CheckFinished = false;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private extern static IntPtr SetActiveWindow(IntPtr handle);
        private const int WM_ACTIVATE = 0x006;
        private const int WM_ACTIVATEAPP = 0x01C;
        private const int WM_NCACTIVATE = 0x086;
        private const int WA_INACTIVE = 0;
        private const int WM_MOUSEACTIVATE = 0x21;
        private const int MA_NOACTIVATE = 3;
        public FormSupplyRemind()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            
            //计时            
            timer.Enabled = true;
            timer.Interval = 30000;//执行间隔时间,单位为毫秒  一千分之一
            timer.Start();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer1_Elapsed);
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = new IntPtr(MA_NOACTIVATE);
                return;
            }
            else if (m.Msg == WM_NCACTIVATE)
            {
                if (((int)m.WParam & 0xFFFF) != WA_INACTIVE)
                {
                    if (m.LParam != IntPtr.Zero)
                    {
                        SetActiveWindow(m.LParam);
                    }
                    else
                    {
                        SetActiveWindow(IntPtr.Zero);
                    }
                }
            }
            base.WndProc(ref m);
        }

        private void Timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            RemindStockinfo();
            //MessageBox.Show("执行了一次程序");
        }

        public static void HideForm()
        {
            
            if (instance == null)
            {
                instance = new FormSupplyRemind();

            }
            if (instance.IsDisposed) return;
            instance.timer.Stop();
            instance.Hide();
            instance.Opacity = 0;
            
            //if (instance.HidedCallback!= null)
            //{
            //    instance.HidedCallback();
            //}

        }
        public static void ShowForm()
        {
            if (instance == null)
            {
                instance = new FormSupplyRemind();

            }
            if (instance.IsDisposed) return;
            instance.timer.Start();
            instance.Show();
            instance.Opacity = 100;
        }
        public static void RemindStockinfoClick()
        {
            //while (true)
            //{
            //    if (instance.CheckFinished ==true )
            //    {
            //        break;
            //    }
            //    Thread.Sleep(10);
            //}
            //instance.CheckFinished = false;
            if (instance == null)
            {
                instance = new FormSupplyRemind();

            }
            if (instance.IsDisposed) return;
            instance.Show();
            //instance.Hide();
            instance.timer.Start();
            if (instance.HidedCallback != null)
            {
                instance.HidedCallback();
            }
            instance.stringBuilder = new StringBuilder();
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    WMSEntities wmsEntities = new WMSEntities();
                    string sql = "";
                    sql = "select TOP 50 SupplyView.SupplierName,SupplyView.ComponentName,SupplyView.No,(select (sum(StockInfoView.ShipmentAreaAmount)+sum(StockInfoView.OverflowAreaAmount)) from StockInfoView where SupplyView.No =StockInfoView.SupplyNo) stock_Sum,SupplyView.SafetyStock from SupplyView where (select (sum(StockInfoView.ShipmentAreaAmount)+sum(StockInfoView.OverflowAreaAmount)) from StockInfoView where SupplyView.No =StockInfoView.SupplyNo)<SupplyView.SafetyStock and IsHistory =0";
                    sql = sql + "and ProjectID=" + GlobalData.ProjectID;
                    sql = sql + "and WarehouseID=" + GlobalData.WarehouseID;
                    wmsEntities.Database.Connection.Open();
                    DataTable DataTabledt1 = new DataTable();// 实例化数据表
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, (SqlConnection)wmsEntities.Database.Connection);
                    sqlDataAdapter.Fill(DataTabledt1);
                    wmsEntities.Database.Connection.Close();
                    int count = DataTabledt1.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        string SupplierName = DataTabledt1.Rows[i][0].ToString();
                        string ComponentName = DataTabledt1.Rows[i][1].ToString();
                        string No = DataTabledt1.Rows[i][2].ToString();
                        string stock_Sum = DataTabledt1.Rows[i][3].ToString();
                        string SaftyStock = DataTabledt1.Rows[i][4].ToString();
                        instance.stringBuilder.Append(SupplierName + " " + ComponentName + " " + No + " " + "库存量" + " " + stock_Sum + " " + "已小于安全库存" + " " + SaftyStock + "\r\n" + "\r\n");
                    }
                    //日期查询
                    string sql1 = "";
                    sql1 = @"select TOP 50 StockInfoView.SupplierName,StockInfoView.ComponentName,StockInfoView.SupplyNo,StockInfoView.InventoryDate ,
                       (select SupplyView.ValidPeriod  from SupplyView where StockInfoView.SupplyID = SupplyView.ID  )ValidPeriod,(select SupplyView.IsHistory from SupplyView where StockInfoView.SupplyID = SupplyView.ID )IsHIstory,
                       GETDATE() Date_Now,(select dateadd(day, (select SupplyView.ValidPeriod from SupplyView where StockInfoView.SupplyID = SupplyView.ID), StockInfoView.InventoryDate))EndDate ,datediff(day, GETDATE(), (select dateadd(day, (select SupplyView.ValidPeriod from SupplyView where StockInfoView.SupplyID = SupplyView.ID), StockInfoView.InventoryDate))) dayss
                       from StockInfoView where(select SupplyView.IsHistory from SupplyView where StockInfoView.SupplyID= SupplyView.ID)= 0
                        and datediff(day, GETDATE(), (select dateadd(day, (select SupplyView.ValidPeriod from SupplyView where StockInfoView.SupplyID= SupplyView.ID), StockInfoView.InventoryDate)))<= 30";

                    sql1 = sql1 + "and ProjectID=" + GlobalData.ProjectID;
                    sql1 = sql1 + "and WarehouseID=" + GlobalData.WarehouseID;

                    DataTable DataTabledt2 = new DataTable();// 实例化数据表
                    SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sql1, (SqlConnection)wmsEntities.Database.Connection);
                    sqlDataAdapter2.Fill(DataTabledt2);
                    int count2 = DataTabledt2.Rows.Count;
                    wmsEntities.Database.Connection.Close();

                    for (int j = 0; j < count2; j++)
                    {
                        string SupplierName = DataTabledt2.Rows[j][0].ToString();
                        string ComponentName = DataTabledt2.Rows[j][1].ToString();
                        string SupplyNo = DataTabledt2.Rows[j][2].ToString();
                        string InventoryDate = DataTabledt2.Rows[j][3].ToString();
                        string days = DataTabledt2.Rows[j][8].ToString();
                        if (Convert.ToInt32(days) <= 0)
                        {
                            instance.stringBuilder.Append(SupplierName + "  " + ComponentName + "  " + SupplyNo + "  " + "存货日期" + " " + InventoryDate + "  " + "已过期" + "\r\n" + "\r\n");
                        }
                        else if (Convert.ToInt32(days) > 0)

                        {
                            instance.stringBuilder.Append(SupplierName + "  " + ComponentName + "  " + SupplyNo + "  " + "存货日期" + " " + InventoryDate + "  " + "有效期还剩" + days + "天" + "\r\n" + "\r\n");
                        }

                    }
                }
                catch
                {
                    instance.stringBuilder = new StringBuilder();
                    instance.stringBuilder.Append("刷新失败，请检查网络连接");
                    if (instance.IsDisposed) return;
                    while (true)
                    {
                        if (instance.IsHandleCreated)
                        {
                            break;
                        }
                        Thread.Sleep(10);
                    }
                    if (instance.IsHandleCreated)
                    {
                        try
                        {
                            instance.Invoke(new Action(() =>
                        {
                            instance.Show();
                            instance.textBox1.Text = "刷新失败，请检查网络连接";
                            instance.textBox1.ForeColor = Color.Red;
                            instance.Opacity = 100;
                        }));
                        }
                        catch (ThreadAbortException ex)
                        {

                        }
                    }
                
                    if (instance.ShowCallback != null)
                    {
                        instance.ShowCallback();
                    }
                    return;
                }
                while (true)
                {
                    if (instance.IsHandleCreated)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }
                
                if (instance.stringBuilder.ToString() != "")
                {
                    if (instance.IsDisposed) return;

                    if (instance.IsHandleCreated)
                    {
                        try
                        {
                            instance.Invoke(new Action(() =>
                        {

                            instance.Show();
                            instance.Opacity = 100;
                        }));
                        }
                        catch (ThreadAbortException ex)
                        {

                        }
                    }

                    if (instance.ShowCallback != null)
                    {
                        instance.ShowCallback();
                    }
                }
                else
                {
                    instance.Opacity = 0;
                    instance.Hide();
                    

                    if (instance.HidedCallback != null)
                    {
                        instance.HidedCallback();
                    }
                    MessageBox.Show("当前项目、仓库中无预警信息" ,"提示",MessageBoxButtons.OK ,MessageBoxIcon.Information  );
                    return;
                }

                if (instance.IsDisposed) return;
                instance.Invoke(new Action(() =>
                {
                    if (instance.IsDisposed) return;
                    else
                    {
                        instance.textBox1.Text = instance.stringBuilder.ToString();

                        //if (instance.textBox1.Text == "刷新失败，请检查网络连接")
                        //{
                        //    instance.textBox1.ForeColor = Color.Red;
                        //}
                        //else if (instance.textBox1.Text == "")
                        //{
                        //    instance.Visible = false;
                        //    if (instance.HidedCallback != null)
                        //    {
                        //        instance.HidedCallback();
                        //    }
                        //}
                        //else
                        //{
                        instance.textBox1.ForeColor = Color.Black;

                        //}
                    }
                }));
            })).Start();
            
        }




            public static void RemindStockinfo()
        {
            
            if(instance ==null)
            {
                instance = new FormSupplyRemind();
                
            }
            if (instance.IsDisposed) return;
            instance.Show();
            //instance.Hide();
            instance.timer.Start();          
            if (instance.HidedCallback != null)
            {
                instance.HidedCallback();
            }
            instance.stringBuilder = new StringBuilder();
            new Thread(new ThreadStart(() =>
            {              
            try
            {
                WMSEntities  wmsEntities = new WMSEntities();
                string sql = "";
                sql = "select TOP 50 SupplyView.SupplierName,SupplyView.ComponentName,SupplyView.No,(select (sum(StockInfoView.ShipmentAreaAmount)+sum(StockInfoView.OverflowAreaAmount)) from StockInfoView where SupplyView.No =StockInfoView.SupplyNo) stock_Sum,SupplyView.SafetyStock from SupplyView where (select (sum(StockInfoView.ShipmentAreaAmount)+sum(StockInfoView.OverflowAreaAmount)) from StockInfoView where SupplyView.No =StockInfoView.SupplyNo)<SupplyView.SafetyStock and IsHistory =0";
                sql = sql + "and ProjectID=" + GlobalData.ProjectID;
                sql = sql + "and WarehouseID=" + GlobalData.WarehouseID;
                wmsEntities.Database.Connection.Open();
                DataTable DataTabledt1 = new DataTable();// 实例化数据表
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, (SqlConnection)wmsEntities.Database.Connection);
                sqlDataAdapter.Fill(DataTabledt1);
                wmsEntities.Database.Connection.Close();
                int count = DataTabledt1.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    string SupplierName = DataTabledt1.Rows[i][0].ToString();
                    string ComponentName = DataTabledt1.Rows[i][1].ToString();
                    string No = DataTabledt1.Rows[i][2].ToString();
                    string stock_Sum = DataTabledt1.Rows[i][3].ToString();
                    string SaftyStock = DataTabledt1.Rows[i][4].ToString();
                    instance.stringBuilder.Append(SupplierName + " " + ComponentName + " " + No + " " + "库存量" + " " + stock_Sum + " " + "已小于安全库存" + " " + SaftyStock + "\r\n" + "\r\n");
                }
                //日期查询
                string sql1 = "";
                sql1 = @"select TOP 50 StockInfoView.SupplierName,StockInfoView.ComponentName,StockInfoView.SupplyNo,StockInfoView.InventoryDate ,
                       (select SupplyView.ValidPeriod  from SupplyView where StockInfoView.SupplyID = SupplyView.ID  )ValidPeriod,(select SupplyView.IsHistory from SupplyView where StockInfoView.SupplyID = SupplyView.ID )IsHIstory,
                       GETDATE() Date_Now,(select dateadd(day, (select SupplyView.ValidPeriod from SupplyView where StockInfoView.SupplyID = SupplyView.ID), StockInfoView.InventoryDate))EndDate ,datediff(day, GETDATE(), (select dateadd(day, (select SupplyView.ValidPeriod from SupplyView where StockInfoView.SupplyID = SupplyView.ID), StockInfoView.InventoryDate))) dayss
                       from StockInfoView where(select SupplyView.IsHistory from SupplyView where StockInfoView.SupplyID= SupplyView.ID)= 0
                        and datediff(day, GETDATE(), (select dateadd(day, (select SupplyView.ValidPeriod from SupplyView where StockInfoView.SupplyID= SupplyView.ID), StockInfoView.InventoryDate)))<= 30";

                sql1 = sql1 + "and ProjectID=" + GlobalData.ProjectID;
                sql1 = sql1 + "and WarehouseID=" + GlobalData.WarehouseID;

                DataTable DataTabledt2 = new DataTable();// 实例化数据表
                SqlDataAdapter sqlDataAdapter2 = new SqlDataAdapter(sql1, (SqlConnection)wmsEntities.Database.Connection);
                sqlDataAdapter2.Fill(DataTabledt2);
                int count2 = DataTabledt2.Rows.Count;
                wmsEntities.Database.Connection.Close();

                for (int j = 0; j < count2; j++)
                {
                    string SupplierName = DataTabledt2.Rows[j][0].ToString();
                    string ComponentName = DataTabledt2.Rows[j][1].ToString();
                    string SupplyNo = DataTabledt2.Rows[j][2].ToString();
                    string InventoryDate = DataTabledt2.Rows[j][3].ToString();
                    string days = DataTabledt2.Rows[j][8].ToString();
                    if (Convert.ToInt32(days) <= 0)
                    {
                        instance.stringBuilder.Append(SupplierName + "  " + ComponentName + "  " + SupplyNo + "  " + "存货日期" + " " + InventoryDate + "  " + "已过期" + "\r\n" + "\r\n");
                    }
                    else if (Convert.ToInt32(days) > 0)

                    {
                        instance.stringBuilder.Append(SupplierName + "  " + ComponentName + "  " + SupplyNo + "  " + "存货日期" + " " + InventoryDate + "  " + "有效期还剩" + days + "天" + "\r\n" + "\r\n");
                    }

                }
            }
            catch
            {
                instance.stringBuilder = new StringBuilder();
                instance.stringBuilder.Append("刷新失败，请检查网络连接");
                if (instance.IsDisposed) return;
                instance.Invoke(new Action(() =>
                {
                    while (true)
                    {
                        if (instance.IsHandleCreated)
                        {
                            break;
                        }
                        Thread.Sleep(10);
                    }
                 instance.Show();
                 instance.textBox1.Text = "刷新失败，请检查网络连接";
                 instance.textBox1.ForeColor = Color.Red;
                 instance.Opacity = 100;
                }));
                if (instance.ShowCallback != null)
                {
                instance.ShowCallback();
                }
                return;
            }
                while (true)
                {
                    if (instance.IsHandleCreated)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }
                instance.CheckFinished = true;
                if (instance.stringBuilder.ToString() != "")
                {
                    if (instance.IsDisposed) return;
                    
                    instance.Invoke(new Action(() =>
                    {
                        
                        instance.Show();
                        instance.Opacity = 100;
                    }));

                    if (instance.ShowCallback != null)
                    {
                        instance.ShowCallback();
                    }
                }
                else
                {
                    instance.Opacity = 0;
                    instance.Hide();
                    
                    
                    if (instance.HidedCallback != null)
                    {
                        instance.HidedCallback();
                    }
                    return ;
                }




                if (instance.IsDisposed) return;
                instance.Invoke(new Action(() =>
                    {
                        if (instance.IsDisposed) return;
                        else
                        {
                            instance.textBox1.Text = instance.stringBuilder.ToString();
                            
                            //if (instance.textBox1.Text == "刷新失败，请检查网络连接")
                            //{
                            //    instance.textBox1.ForeColor = Color.Red;
                            //}
                            //else if (instance.textBox1.Text == "")
                            //{
                            //    instance.Visible = false;
                            //    if (instance.HidedCallback != null)
                            //    {
                            //        instance.HidedCallback();
                            //    }
                            //}
                            //else
                            //{
                            instance.textBox1.ForeColor = Color.Black;

                            //}
                        }
                    }));
            })).Start();

        }

        private void FormSupplyRemind_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            this.Left = 3;
            this.Top = (int)(0.7 * Screen.PrimaryScreen.Bounds.Height);
            this.Width = (int)(0.35 * Screen.PrimaryScreen.Bounds.Width);
            this.Height = (int)(0.25 * Screen.PrimaryScreen.Bounds.Height);//75
            //this.textBox1.Text = "数据加载中...";  
            
            if (instance.HidedCallback != null)
            {
                instance.HidedCallback();
            }
            this.ShowInTaskbar = false;///使窗体不显示在任务栏                            
        }

        private void FormSupplyRemind_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            this.Opacity = 0;
            this.Hide();
            e.Cancel = true;           
            if (instance.HidedCallback != null)
            {
                instance.HidedCallback();              
            }
        }
        public static void SetFormHidedCallback(Action callback)
        {
            instance.HidedCallback = callback;
        }


        public static void SetFormShowCallback(Action callback)
        {
            instance.ShowCallback = callback;
        }

    }
     
     
}
 