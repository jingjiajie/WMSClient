using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class JobTicketItemViewMetaData
    {
        public const string STRING_STATE_UNFINISHED = "未完成";
        public const string STRING_STATE_PART_FINISHED = "部分完成";
        public const string STRING_STATE_ALL_FINISHED = "全部完成";

        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false,Save=false},
            //new KeyName(){Key="JobTicketJobTicketNo",Name="作业单号",Editable=false,Save=false},
            //new KeyName(){Key="StockInfoID",Name="库存信息ID",Editable=true,Save=true},
            new KeyName(){Key="SupplyNo",Name="零件代号",Editable=false,Save=false},
            new KeyName(){Key="ComponentName",Name="零件名",Editable=false,Save=false},
            new KeyName(){Key="SupplierName",Name="供应商",Editable=false,Save=false},
            new KeyName(){Key="State",Name="状态",Editable=false,
            ComboBoxItems = new ComboBoxItem[]{
                             new ComboBoxItem(STRING_STATE_UNFINISHED),
                             new ComboBoxItem(STRING_STATE_PART_FINISHED),
                             new ComboBoxItem(STRING_STATE_ALL_FINISHED)
            } },
            new KeyName(){Key="ScheduledAmount",Name="计划翻包数量",NotNull=true,Positive=true},
            new KeyName(){Key="RealAmount",Name="实际翻包数量",NotNegative=true,DefaultValueFunc=(()=>"0")},
            new KeyName(){Key="ScheduledPutOutAmount",Name="已分配出库数量",DefaultValueFunc=(()=>"0"),Editable=false},
            new KeyName(){Key="Unit",Name="单位",Editable=false,Save=false },
            new KeyName(){Key="UnitAmount",Name="单位数量",Editable=false,Save=false,Positive=true },
            new KeyName(){Key="HappenTime",Name="完成时间",EditPlaceHolder="点击完成自动生成"},
            new KeyName(){Key="JobPersonName",Name="实际作业人员",Save=false,EditPlaceHolder="点击选择人员"},
            new KeyName(){Key="ConfirmPersonName",Name="确认人",Save=false,EditPlaceHolder="点击选择人员"},
        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
