using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI.FormBase
{
    public enum Position
    {
        POSITION_NULL,
        RECEIPT ,
        SHIPMENT ,
        STOCKINFO ,
        SETTLEMENT ,

    };
    class BasePersonMetaData
    {

        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false},
            new KeyName(){Key="Name",Name="人员姓名",Visible=true,Editable=true},
            new KeyName(){Key="Position",Name="岗位",Editable=false,ComboBoxItems = new ComboBoxItem[]{
                new ComboBoxItem("收货"),
                new ComboBoxItem("发货"),
                new ComboBoxItem("库存管理"),
                new ComboBoxItem("结算"),
            } },

            new KeyName(){Key="ProjectName",Name="所属项目名称",Visible=true,Editable=true,Save=false},
            new KeyName(){Key="WarehouseName",Name="所在仓库名称",Visible=true,Editable=true,Save=false},
        };
        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }


        private static KeyName[] positionKeyNames = {
            new KeyName(){Key="Receipt",Name="收货"},
            new KeyName(){Key="Shipment",Name="发货"},
            new KeyName(){Key="StockInfo",Name="库存管理"},
            new KeyName(){Key="Settlement",Name= "结算"},
        };
        public static KeyName[] PositionKeyNames { get => positionKeyNames; set => positionKeyNames = value; }
    }
}
