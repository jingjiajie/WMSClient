using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class StockInfoCheckTickettModifyViewMetaData
    {
        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",              Visible=false,Editable=false,Save=false},
            new KeyName(){Key="ComponentName",Name="零件名称",Visible=true ,Editable=false,Save=false},
            new KeyName(){Key="ComponentNo",Name="零件代号",Visible=true ,Editable=false,Save=false},
            new KeyName(){Key="SupplierName",Name="供货商名称",Visible=true ,Editable=false,Save=false},
            // new KeyName(){Key="StockInfoCheckTicketID",Name="盘点单ID",Visible=false,Editable=false,Save=false},
            // new KeyName(){Key="StockInfoID",Name="库存信息ID",Visible=false,Editable=false,Save=false},


           // new KeyName(){Key="ExcpetedOverflowAreaAmount",Name="溢库区数量",Visible=true ,Editable=false,Save=true },
           // new KeyName(){Key="ExpectedShipmentAreaAmount",Name="发货区数量",Visible=true ,Editable=false,Save=true },
            //new KeyName(){Key="RealOverflowAreaAmount",Name="实际溢库区数量",Visible=true ,Editable=false,Save=true },
           // new KeyName(){Key="RealShipmentAreaAmount",Name="实际发货区数量",Visible=true ,Editable=false,Save=true },


            new KeyName(){Key="ProjectName",Name="项目名称",Visible=true ,Editable=false,Save=false},
            new KeyName(){Key="WarehouseName",Name="仓库名",Visible=true ,Editable=false,Save=false},
            
      


        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
