using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class StockInfoViewMetaData
    {
        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",ImportVisible=false,Visible=false,Editable=false,Save=false},
            new KeyName(){Key="SupplyNo",Name="零件代号",ImportVisible=true,Import=false,Editable=false,Save=false},
            new KeyName(){Key="ComponentName",Name="零件名称",ImportVisible=false,Editable=false,Save=false},
            new KeyName(){Key="SupplierName",Name="供应商",ImportVisible=false,Editable=false,Save=false},
            new KeyName(){Key="ReceiptAreaAmount",Name="收货区数量"},
            new KeyName(){Key="SubmissionAmount",Name="送检数量"},
            new KeyName(){Key="OverflowAreaAmount",Name="溢库区数量"},
            new KeyName(){Key="ShipmentAreaAmount",Name="发货区数量"},
            new KeyName(){Key="RejectAreaAmount",Name="不良品区数量"},
            new KeyName(){Key="ScheduledShipmentAmount",Name="已分配发货数量"},
            new KeyName(){Key="AvailableAmount",Name="可用数量",Editable=false,Save=false,ImportVisible=false,Import=false},
            new KeyName(){Key="ReceiptTicketNo",Name="收货单号",Editable=false,Save=false},
            new KeyName(){Key="InventoryDate",Name="存货日期",Editable=false,Save=false},
            new KeyName(){Key="ManufactureDate",Name="生产日期",Editable=false,Save=false},
            new KeyName(){Key="ExpiryDate",Name="失效日期",Editable=false,Save=false},
        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
