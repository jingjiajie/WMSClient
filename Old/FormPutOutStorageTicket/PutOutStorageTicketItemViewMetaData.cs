using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class PutOutStorageTicketItemViewMetaData
    {
        public const string STRING_STATE_WAIT_FOR_LOAD = "待装车";
        public const string STRING_STATE_PART_LOADED = "部分装车";
        public const string STRING_STATE_ALL_LOADED = "全部装车";

        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false,Save=false},
            //new KeyName(){Key="StockInfoID",Name="库存信息ID",Editable=true},
            //new KeyName(){Key="PutOutStorageTicketNo",Name="出库单号",Editable=false,Save=false},
            new KeyName(){Key="SupplyNo",Name="零件代号",Editable=false,Save=false},
            new KeyName(){Key="ComponentName",Name="零件",Editable=false,Save=false},
            new KeyName(){Key="SupplierName",Name="供应商",Editable=false,Save=false},
            new KeyName(){Key="ScheduledAmount",Name="计划装车数量",Positive=true},
            new KeyName(){Key="State",Name="状态",Editable=false,ComboBoxItems=new ComboBoxItem[]{
                new ComboBoxItem(STRING_STATE_WAIT_FOR_LOAD),
                new ComboBoxItem(STRING_STATE_PART_LOADED),
                new ComboBoxItem(STRING_STATE_ALL_LOADED)
            } },
            new KeyName(){Key="RealAmount",Name="实际装车数量",NotNegative=true},
            //new KeyName(){Key="ExceedStockAmount",Name="超库存数量",Editable=true},
            new KeyName(){Key="Unit",Name="单位",Editable=false,Save=false },
            new KeyName(){Key="UnitAmount",Name="单位数量",Editable=false,Save=false,Positive=true},
            new KeyName(){Key="LoadingTime",Name="装车时间",Editable=true,EditPlaceHolder="装车时生成"},
            new KeyName(){Key="JobPersonName",Name="实际作业人员",Save=false,EditPlaceHolder="点击选择人员"},
            new KeyName(){Key="ReturnQualityAmount",Name="正品退回数量",EditPlaceHolder="退回时填写",NotNull=true,NotNegative=true},
            new KeyName(){Key="ReturnQualityUnit",Name="正品退回单位",EditPlaceHolder="退回时填写",NotNull=true},
            new KeyName(){Key="ReturnQualityUnitAmount",Name="正品退回单位数量",EditPlaceHolder="退回时填写",NotNull=true,NotNegative=true},
            new KeyName(){Key="ReturnRejectAmount",Name="不良品退回数量",EditPlaceHolder="退回时填写",NotNull=true,NotNegative=true},
            new KeyName(){Key="ReturnRejectUnit",Name="不良品退回单位",EditPlaceHolder="退回时填写",NotNull=true},
            new KeyName(){Key="ReturnRejectUnitAmount",Name="不良品退回单位数量",EditPlaceHolder="退回时填写",NotNull=true,NotNegative=true},
            new KeyName(){Key="ReturnTime",Name="退回时间",EditPlaceHolder="退回时生成"},
            new KeyName(){Key="ConfirmPersonName",Name="确认人",Save=false,EditPlaceHolder="点击选择人员"},
            new KeyName(){Key="Comment",Name="备注",EditPlaceHolder="填写备注"}
        };


        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
