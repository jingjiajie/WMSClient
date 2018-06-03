using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class StockInfoCheckTicksModifyMetaDate
    {
        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false,Save=false},
            
            new KeyName(){Key="SupplyNo",Name="供货零件代号",Editable=false,Save=false,EditPlaceHolder ="点击选择供货信息"},
            //new KeyName(){Key="SupplyNumber",Name="供货零件代号",Editable=false,Save=false},
            new KeyName(){Key="ComponentName",Name="零件名称",Editable=false,Save=false},
            new KeyName(){Key="SupplierName",Name="供货商名称",Editable=false,Save=false},
            new KeyName(){Key="ExpectedRejectAreaAmount",Name="账面不良品区数量",Editable=false ,Save=true  },
            new KeyName(){Key="RealRejectAreaAmount",Name="实际不良品区数量",Editable=true  ,Save=true  },
            new KeyName(){Key="ExpectedReceiptAreaAmount",Name="账面收货区数量",Editable=false  ,Save=true  },
            new KeyName(){Key="RealReceiptAreaAmount",Name="实际收货区数量",Editable=true  ,Save=true  },
            new KeyName(){Key="ExpectedSubmissionAmount",Name="账面送检数量",Editable=false ,Save=true  },
            new KeyName(){Key="RealSubmissionAmount",Name="实际送检数量",Editable=true  ,Save=true  },
            new KeyName(){Key="ExcpetedOverflowAreaAmount",Name="账面溢库区数量",Editable=false ,Save=true  },
            new KeyName(){Key="RealOverflowAreaAmount",Name="实际溢库区数量",Editable=true ,Save=true }
            ,
             new KeyName(){Key="ExpectedShipmentAreaAmount",Name="账面发货区数量",Editable=false ,Save=true  },
            new KeyName(){Key="RealShipmentAreaAmount",Name="实际发货区数量",Editable=true ,Save=true }
            ,            
            new KeyName(){Key="PersonName",Name="实际盘点人",Editable=false  ,Save=false ,EditPlaceHolder ="点击选择盘点人"  }
            ,            
            new KeyName(){Key="QualifiedQuantityVariance",Name="合格品差异数",Editable =false ,EditPlaceHolder ="自动计算" , Save=false    }
            ,
            new KeyName(){Key="Comment",Name="备注",Editable=true,Save=true ,EditPlaceHolder ="填写备注"   }
            ,
        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
