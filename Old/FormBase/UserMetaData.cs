using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class UserMetaData
    {
        public const int AUTHORITY_MANAGER = int.MaxValue; //管理员权限
        public const int AUTHORITY_SHIPMENT_MANAGER = //发货员权限
                  (int)Authority.DELIVERY_SEND
                | (int)Authority.DELIVERY_OUTPUT
                | (int)Authority.DELIVERY_JOB;
        public const int AUTHORITY_RECEIPT_MANAGER = //收货员权限
                  (int)Authority.RECEIPT_ARRIVAL
                | (int)Authority.RECEIPT_SHELVES
                | (int)Authority.SUBMISSION_TICKET;
        public const int AUTHORITY_STOCK_MANAGER = //库存管理员权限
                  (int)Authority.STOCKINFO
                | (int)Authority.STOCK_CHECK;
        public const int AUTHORITY_SUPPLIER = //供应商权限
                  (int)Authority.BASE_SUPPLY_SUPPLIER_SELFONLY
                | (int)Authority.BASE_SUPPLIER_SUPPLIER_SELFONLY
                | (int)Authority.RECEIPT_ARRIVAL_SUPPLIER_SELFONLY
                | (int)Authority.SETTLEMENT_SUPPLIER_SELFONLY
                | (int)Authority.STOCKINFO_SUPPLIER_SELFONLY
            ;

        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false,Save=false,ImportVisible=false},
            new KeyName(){Key="Username",Name="用户名",NotNull=true},
            new KeyName(){Key="Password",Name="密码",Visible=false,NotNull=true},
            //new KeyName(){Key="Authority",Name="权限",Editable=true,NotNull=true,ImportVisible=false,Import=false},
            new KeyName(){Key="AuthorityName",Name="角色"},
            new KeyName(){Key="SupplierName",Name="供应商名",Editable=false, Save=false,ImportVisible=true,Import=false},
        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
