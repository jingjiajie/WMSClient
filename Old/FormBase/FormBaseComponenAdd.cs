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
    public partial class FormBaseComponenAdd : Form
    {
        public FormBaseComponenAdd()
        {
            InitializeComponent();
        }

        private void FormBaseComponenAdd_Load(object sender, EventArgs e)
        {

        }
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (WarehouseID.Text == string.Empty)
            {
                MessageBox.Show("仓库ID不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (SupplierID.Text == string.Empty)
            {
                MessageBox.Show("供应商ID不能为空！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //添加
            DataAccess.Component objcomponen = new DataAccess.Component();
            {
                objcomponen.WarehouseID = Convert.ToInt32(WarehouseID.Text);
                objcomponen.SupplierID = Convert.ToInt32(SupplierID.Text);
                objcomponen.ContainerNo = ContainerNo.Text;
                objcomponen.Factroy = Factroy.Text;
                objcomponen.WorkPosition = WorkPosition.Text;
                objcomponen.No = No.Text;
                objcomponen.Name = Name1.Text;
                objcomponen.SupplierType = SupplierType.Text;
                objcomponen.Type = Type.Text;
                objcomponen.Size = Size.Text;
                objcomponen.Category = Category.Text;
                objcomponen.GroupPrincipal = GroupPrincipal.Text;
                objcomponen.SingleCarUsageAmount = Convert.ToDecimal(SingleCarUsageAmount.Text);
                objcomponen.Charge1 = Convert.ToDecimal(ChargeBelow50000.Text);
                objcomponen.Charge1 = Convert.ToDecimal(ChargeAbove50000.Text);
                objcomponen.InventoryRequirement1Day = Convert.ToDecimal(InventoryRequirement1Day.Text);
                objcomponen.InventoryRequirement3Day = Convert.ToDecimal(InventoryRequirement3Day.Text);
                objcomponen.InventoryRequirement5Day = Convert.ToDecimal(InventoryRequirement5Day.Text);
                objcomponen.InventoryRequirement10Day = Convert.ToDecimal(InventoryRequirement10Day.Text);

            }
            WMSEntities wms = new WMSEntities();
            wms.Component.Add(objcomponen);
            wms.SaveChanges();
            MessageBox.Show("添加零件成功");
            this.Close();
        }


        private void buttonClosing_Click(object sender, EventArgs e)
        {
            this.Close();
        }//关闭窗口

        private void WarehouseID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
