using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class PutOutStorageTicketViewMetaData
    {
        public const string STRING_STATE_NOT_LOADED = "待装车";
        public const string STRING_STATE_PART_LOADED = "部分装车";
        public const string STRING_STATE_ALL_LOADED = "全部装车";
        public const string STRING_STATE_DELIVERED = "已发运";

        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false,Save=false},
            new KeyName(){Key="No",Name="出库单号",EditPlaceHolder="留空自动生成"},
            new KeyName(){Key="JobTicketJobTicketNo",Name="关联作业单号",Editable=false,Save=false},
            //new KeyName(){Key="TruckLoadingTicketNo",Name="装车单号",Editable=true},
            new KeyName(){Key="State",Name="状态",Editable=false,ComboBoxItems=new ComboBoxItem[]{
                new ComboBoxItem(STRING_STATE_NOT_LOADED),
                new ComboBoxItem(STRING_STATE_PART_LOADED),
                new ComboBoxItem(STRING_STATE_ALL_LOADED),
                new ComboBoxItem(STRING_STATE_DELIVERED),
            } },
            new KeyName(){Key="CarNum",Name="车牌号",Editable=true},
            new KeyName(){Key="Driver",Name="司机",Editable=true},
            //new KeyName(){Key="OriginalTicketType",Name="原始单据类型",Editable=true},
            new KeyName(){Key="ReceiverNo",Name="收货方编码",Editable=true},
            new KeyName(){Key="Factory",Name="出货单位",Editable=true,GetAllValueToComboBox="PutOutStorageTicket.Gate"},
            new KeyName(){Key="Destination",Name="使用单位",Editable=true,GetAllValueToComboBox="PutOutStorageTicket.Destination"},
            new KeyName(){Key="Gate",Name="卸货门"},
            //new KeyName(){Key="SortTypeNo",Name="排序类型编码",Editable=true},
            new KeyName(){Key="DeliverTime",Name="发运时间",DefaultValueFunc=(()=>DateTime.Now.ToString())},
            new KeyName(){Key="ExpectedArriveTime",Name="预计到达时间"},
            new KeyName(){Key="ReturnTicketNo",Name="回单号",EditPlaceHolder="回单时填写"},
            new KeyName(){Key="ReturnTicketTime",Name="回单时间",EditPlaceHolder="回单时生成"},
            new KeyName(){Key="PersonName",Name="责任人",Save=false,EditPlaceHolder="点击选择人员"},
            new KeyName(){Key="CreateUserUsername",Name="创建用户",Editable=false,Save=false},
            new KeyName(){Key="CreateTime",Name="创建时间",Editable=false,Save=false},
            new KeyName(){Key="LastUpdateUserUsername",Name="最后修改用户",Editable=false,Save=false},
            new KeyName(){Key="LastUpdateTime",Name="最后修改时间",Editable=false,Save=false}
        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
