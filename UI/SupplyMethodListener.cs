using FrontWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS.UI
{
    [MethodListener]
    class SupplyMethodListener
    {
        public void SupplySerialNoEditEnded([Model] IModel model, [Row] int row)
        {
            string supplySerialNo = model[row, "supplySerialNo"]?.ToString() ?? "";
            if (string.IsNullOrWhiteSpace(supplySerialNo)) return;
            var foundSupplies = (from m in GlobalData.AllSupplies
                                 where supplySerialNo == (string)m["serialNo"]
                                 select m).ToArray();
            if (foundSupplies.Length != 1)
            {
                goto FAILED;
            }
            model[row, "materialId"] = foundSupplies[0]["materialId"];
            model[row, "materialNo"] = foundSupplies[0]["materialNo"];
            model[row, "materialName"] = foundSupplies[0]["materialName"];
            model[row, "materialProductLine"] = foundSupplies[0]["materialProductLine"];
            model[row, "supplierId"] = foundSupplies[0]["supplierId"];
            model[row, "supplierNo"] = foundSupplies[0]["supplierNo"];
            model[row, "supplierName"] = foundSupplies[0]["supplierName"];
            return;

            FAILED:
            MessageBox.Show("供货不存在，请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
    }
}
