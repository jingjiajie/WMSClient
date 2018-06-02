using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;

namespace WMS.UI.FormBase
{
    public partial class FormBaseComponenAlter : Form
    {
        public FormBaseComponenAlter()
        {
            InitializeComponent();
        }
        public FormBaseComponenAlter(int a, int b, int c, string d, string e1, string f, string g, string h, string i1, string j, string k, string l, string m, string n, string o, string p, string q, string r, string s1, string t)//定义重载 传参
        {
            InitializeComponent();
            ID.Text =Convert.ToString(a);
            WarehouseID.Text = Convert.ToString(b);
            SupplierID.Text= Convert.ToString(c);
            ContainerNo.Text =d;
            Factroy.Text =e1;
            WorkPosition.Text = f;
            No.Text =g;
            Name1.Text =h;
            SupplierType.Text =i1;
            Type.Text =j;
            Size.Text =k;
            Category.Text =l;
            GroupPrincipal.Text =m;
            SingleCarUsageAmount.Text =n;
            ChargeBelow50000.Text =o;
            ChargeAbove50000.Text =p;
            InventoryRequirement1Day.Text =q;
            InventoryRequirement3Day.Text =r;
            InventoryRequirement5Day.Text =s1;
            InventoryRequirement10Day.Text = t;
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            
            if (SupplierID.Text == string.Empty)
            {
                MessageBox.Show("供应商ID不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //修改
            WMSEntities wms = new WMSEntities();
            DataAccess.Component objuser = (from s in wms.Component
                            where s.Name == Name1.Text
                            select s).First();
            
            objuser.SupplierID = Convert.ToInt32(SupplierID.Text);
            objuser.ContainerNo = ContainerNo.Text;
            objuser.Factroy = Factroy.Text;
            objuser.WorkPosition = WorkPosition.Text;
            objuser.No = No.Text;
            objuser.SupplierType = SupplierType.Text;
            objuser.Type = Type.Text;
            objuser.Size = Size.Text;
            objuser.Category = Category.Text;
            objuser.GroupPrincipal = GroupPrincipal.Text;
            objuser.SingleCarUsageAmount = Convert.ToDecimal(SingleCarUsageAmount.Text);
            objuser.Charge1 = Convert.ToDecimal(ChargeBelow50000.Text);
            objuser.Charge2 = Convert.ToDecimal(ChargeAbove50000.Text);
            objuser.InventoryRequirement1Day = Convert.ToDecimal(InventoryRequirement1Day.Text);
            objuser.InventoryRequirement3Day = Convert.ToDecimal(InventoryRequirement3Day.Text);
            objuser.InventoryRequirement5Day = Convert.ToDecimal(InventoryRequirement5Day.Text);
            objuser.InventoryRequirement10Day = Convert.ToDecimal(InventoryRequirement10Day.Text);
           
            wms.SaveChanges();
            MessageBox.Show("修改零件成功");
            this.Close();
        }


        private void buttonClosing_Click(object sender, EventArgs e)
        {
            this.Close();
        }//关闭窗口
        private void FormBaseComponenAlter_Load(object sender, EventArgs e)
        {
            ID.ReadOnly = true;//定义 只读
            WarehouseID.ReadOnly = true;//定义 只读
            Name1.ReadOnly = true;//定义 只读
        }
    }
}
