using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class ShipmentTicketViewMetaData
    {
        public const string STRING_STATE_NOT_ASSIGNED_JOB = "未分配翻包";
        public const string STRING_STATE_PART_ASSIGNED_JOB = "部分分配翻包";
        public const string STRING_STATE_ALL_ASSIGNED_JOB = "全部分配翻包";

        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false,Save=false},
            //new KeyName(){Key="ProjectName",Name="项目",Editable=false,Save=false},
            //new KeyName(){Key="WarehouseName",Name="仓库",Editable=false,Save=false},
            //new KeyName(){Key="Number",Name="编号",EditPlaceHolder="留空自动生成"},
            new KeyName(){Key="No",Name="发货单号",EditPlaceHolder="留空自动生成"},
            new KeyName(){Key="Type",Name="出库类型",Editable=false,ComboBoxItems = new ComboBoxItem[]{
                new ComboBoxItem("正常出库"),
                new ComboBoxItem("售后出库"),
                new ComboBoxItem("返厂出库"),
                new ComboBoxItem("其他"),
            } },
            new KeyName(){Key="Date",Name="订单日期",DefaultValueFunc=(()=>DateTime.Now.ToString())},
            new KeyName(){Key="RequireArriveDate",Name="要求到达日期"},
            new KeyName(){Key="State",Name="状态",Editable=false,ComboBoxItems=new ComboBoxItem[]{
                new ComboBoxItem(STRING_STATE_NOT_ASSIGNED_JOB),
                new ComboBoxItem(STRING_STATE_PART_ASSIGNED_JOB),
                new ComboBoxItem(STRING_STATE_ALL_ASSIGNED_JOB)
             } },
            new KeyName(){Key="Station",Name="工位"},
            new KeyName(){Key="ReceivingPersonName",Name="收货人姓名"},
            new KeyName(){Key="ContactAddress",Name="联系地址"},
            new KeyName(){Key="Source",Name="单据来源"},
            new KeyName(){Key="DeliveryPath",Name="配送线路"},
            new KeyName(){Key="Description",Name="描述"},
            new KeyName(){Key="DeliveryTicketNo",Name="配送单号"},
            new KeyName(){Key="OuterPhysicalDistributionPath",Name="外物流路线"},
            new KeyName(){Key="Emergency",Name="是否紧急",Editable=false,
                Translator =Translator.BoolTranslator,
                ComboBoxItems =new ComboBoxItem[]{
                new ComboBoxItem("否",0),
                new ComboBoxItem("是",1),
            } },
            new KeyName(){Key="ShipmentPlaceNo",Name="发货地编码"},
            new KeyName(){Key="PersonName",Name="责任人",Save=false,EditPlaceHolder="点击选择人员"},
            new KeyName(){Key="CreateUserUsername",Name="创建用户",Editable=false,Save=false},
            new KeyName(){Key="CreateTime",Name="创建时间",Editable=false,Save=false},
            new KeyName(){Key="LastUpdateUserUsername",Name="最后修改用户",Editable=false,Save=false},
            new KeyName(){Key="LastUpdateTime",Name="最后修改时间",Editable=false,Save=false},
        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
