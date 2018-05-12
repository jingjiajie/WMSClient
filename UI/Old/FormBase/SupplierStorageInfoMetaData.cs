using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class SupplierStorageInfoMetaData
    {

       private static KeyName[]  keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible = false, Editable = false },
            new KeyName(){Key="SupplierID",Name="供应商ID",Visible = false, Editable = false },
            new KeyName(){Key="ExecuteSupplierID",Name="实际执行供应商合同ID",Visible = false  , Editable = false    },
            new KeyName(){Key="SupplierName",Name="供货商名称",Visible = true   , Editable = false ,Save =false   ,EditPlaceHolder ="自动填写" },
            new KeyName(){Key="SupplierNumber",Name="供应商编号",Visible = true   , Editable = false   ,Save =false ,EditPlaceHolder ="自动填写" },
            
            new KeyName(){Key="Year",Name="存货年份",Visible = true  , Editable = true  },
            new KeyName(){Key="Month",Name="存货月份",Visible = true , Editable = true  },
            new KeyName(){Key="AreaIncrement",Name="增面积",Visible = true , Editable = true   },
            new KeyName(){Key="StorageDays",Name="放置天数",Visible = true , Editable = true  },
            new KeyName(){Key="StorageFee",Name="仓储费",Visible = true , Editable = true  },
            //new KeyName(){Key="Year",Name="年份",Visible = true, Editable = true},
            //new KeyName(){Key="NetArea",Name="净面积",Visible = true, Editable = true},
            //new KeyName(){Key="FixedStorageCost",Name="仓储固定费用",Visible = true, Editable = true},
            //new KeyName(){Key="JanContractArea",Name="1月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="FebContractArea",Name="2月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="MarContractArea",Name="3月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="AprContractArea",Name="4月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="MayContractArea",Name="5月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="JunContractArea",Name="6月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="JulContractArea",Name="7月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="AugContractArea",Name="8月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="SeptContractArea",Name="9月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="OctContractArea",Name="10月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="NovContractArea",Name="11月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="DecContractArea",Name="12月合同面积",Visible = true, Editable = true},
            //new KeyName(){Key="JanAreaIncreasement",Name="1月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="FebAreaIncreasement",Name="2月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="MarAreaIncreasement",Name="3月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="AprAreaIncreasement",Name="4月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="MayAreaIncreasement",Name="5月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="JunAreaIncreasement",Name="6月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="JulAreaIncreasement",Name="7月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="AugAreaIncreasement",Name="8月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="SeptAreaIncreasement",Name="9月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="OctAreaIncreasement",Name="10月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="NovAreaIncreasementa",Name="11月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="DecAreaIncreasement",Name="12月增面积",Visible = true, Editable = true},
            //new KeyName(){Key="JanStorageDays",Name="1月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="FebStorageDays",Name="2月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="MarStorageDays",Name="3月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="AprStorageDays",Name="4月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="MayStorageDays",Name="5月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="JunStorageDays",Name="6月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="JulStorageDays",Name="7月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="AugStorageDays",Name="8月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="SeptStorageDays",Name="9月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="OctStorageDays",Name="10月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="NovStorageDays",Name="11月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="DecStorageDays",Name="12月放置天数",Visible = true, Editable = true},
            //new KeyName(){Key="JanStorageFee",Name="1月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="FebStorageFee",Name="2月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="MarStorageFee",Name="3月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="AprStorageFee",Name="4月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="MayStorageFee",Name="5月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="JunStorageFee",Name="6月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="JulStorageFee",Name="7月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="AugStorageFee",Name="8月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="SeptStorageFee",Name="9月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="OctStorageFee",Name="10月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="NovStorageFee",Name="11月仓储费",Visible = true, Editable = true},
            //new KeyName(){Key="DecStorageFee",Name="12月仓储费",Visible = true, Editable = true},


        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
