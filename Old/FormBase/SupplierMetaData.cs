using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class SupplierMetaData
    {

       private static KeyName[]  keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible = false, Editable = false ,ImportVisible=false},
            new KeyName(){Key="Name",Name="供货商名称",Visible = true, Editable = true,NotNull =true  },
            new KeyName(){Key="No",Name="供货商代号",Visible = true, Editable = true,NotNull =true  },

            new KeyName(){Key="Code",Name="企业编码",Visible = true, Editable = true  },
            new KeyName(){Key="ContractNo",Name="合同编码",Visible = true, Editable = true},
            new KeyName(){Key="StartingTime",Name="合同生效时间",Visible = true, Editable = true},
            new KeyName(){Key="EndingTime",Name="合同截止时间",Visible = true, Editable = true},
            new KeyName(){Key="InvoiceDelayMonth",Name="开票延迟月",Visible = true, Editable = true},
            new KeyName(){Key="BalanceDelayMonth",Name="结算延迟月",Visible = true, Editable = true},
            new KeyName(){Key="FullName",Name="供货商全称",Visible = true, Editable = true},

            new KeyName(){Key="NetArea",Name="净面积",Visible = true, Editable = true},
            new KeyName(){Key="FixedStorageCost",Name="仓储固定费用",Visible = true, Editable = true},
            new KeyName(){Key="ContractStorageArea",Name="合同面积",Visible = true, Editable = true},
            

            new KeyName(){Key="TaxpayerNumber",Name="纳税人识别号",Visible = true, Editable = true},
            new KeyName(){Key="Address",Name="地址",Visible = true, Editable = true},
            new KeyName(){Key="Tel",Name="电话",Visible = true, Editable = true},
            new KeyName(){Key="BankName",Name="开户行",Visible = true, Editable = true},
            new KeyName(){Key="BankAccount",Name="帐号",Visible = true, Editable = true},
            new KeyName(){Key="BankNo",Name="开户行行号",Visible = true, Editable = true},
            new KeyName(){Key="ZipCode",Name="邮编",Visible = true, Editable = true},
            new KeyName(){Key="RecipientName",Name="收件人",Visible = true, Editable = true},
            new KeyName(){Key="Number",Name="编号",Visible = true, Editable = true},
            new KeyName(){Key="ContractState",Name="合同状态",Visible = true, Editable = false ,
            ComboBoxItems=new ComboBoxItem[]{
                new ComboBoxItem("待审核","待审核"),
                new ComboBoxItem("已过审","已过审"),                
            }},
            new KeyName(){Key="IsHistory",Name="是否历史信息",Visible = false , Editable = false ,ImportVisible=false  },
            new KeyName(){Key="NewestSupplierID",Name="最新供应商信息ID",Visible = false    , Editable = false ,ImportVisible=false  },
            new KeyName(){Key="CreateUserID",Name="创建用户ID",Visible = false , Editable = false,ImportVisible=false },
            new KeyName(){Key="CreateTime",Name="创建时间",Visible = true  , Editable = false ,EditPlaceHolder= "自动填写",ImportVisible =false },
            new KeyName(){Key="LastUpdateUserID",Name="最后更新用户ID",Visible = false , Editable = false ,ImportVisible=false },
            new KeyName(){Key="LastUpdateTime",Name="最后更新时间",Visible = true  , Editable = false,EditPlaceHolder ="自动填写" ,ImportVisible =false  },
            new KeyName(){Key="CreateUserUsername",Name="创建用户",Visible = true  , Editable = false,Save =false ,ImportVisible=false,EditPlaceHolder ="自动填写"},
            new KeyName(){Key="LastUpdateUserUsername",Name="最后更新用户",Visible = true  , Editable = false,Save =false ,ImportVisible=false,EditPlaceHolder ="自动填写"} ,
            
        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }



        public static Dictionary<string, string> keyConvert = new Dictionary<string, string>()
        {
            {"Name","Name"},
            {"No","No"},
            { "Code","Code"},
            { "ContractNo","ContractNo"},
            { "StartingTime","StartingTime"},

            { "EndingTime","EndingTime"},
            { "InvoiceDelayMonth","InvoiceDelayMonth"},
            { "BalanceDelayMonth","BalanceDelayMonth"},
            { "FullName","FullName"},
            { "NetArea","NetArea"},
            { "FixedStorageCost","FixedStorageCost"},
            { "ContractStorageArea","ContractStorageArea"},
            { "TaxpayerNumber","TaxpayerNumber"},
            { "Address","Address"},

            { "Tel","Tel"},
            { "BankName","BankName"},
            { "ZipCode","ZipCode"},
            { "RecipientName","RecipientName"},
            { "Number","Number"},
            { "ContractState","ContractState"},
           
        };



    }


}
