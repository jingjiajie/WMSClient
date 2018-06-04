using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class StockInfoCheckTicketViewMetaData
    {
        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false ,Editable=false },
            new KeyName(){Key="CheckDate",Name="盘点日期",Visible=true    ,Editable=false  ,EditPlaceHolder ="自动填写" },
            new KeyName(){Key="PersonName",Name="责任人",Visible=true    ,Editable=false  , Save=false , EditPlaceHolder ="点击选择人员" },
            new KeyName(){Key="CreateUserID",Name="创建用户ID",Visible=false ,Editable=false },
            new KeyName(){Key="CreateTime",Name="创建时间",Visible=true,Editable=true,DefaultValueFunc=(()=>DateTime.Now.ToString())},
            new KeyName(){Key="LastUpdateUserID",Name="最后更新用户ID",Visible=false ,Editable=false },
            new KeyName(){Key="LastUpdateTime",Name="最后更新时间",Visible=true,Editable=false ,EditPlaceHolder ="自动填写"},
            new KeyName(){Key="ProjectID",Name="项目ID",Visible=false ,Editable=false },
            new KeyName(){Key="WarehouseID",Name="仓库ID",Visible=false ,Editable=false },
            new KeyName(){Key="CreateUserUsername",Name="创建用户名称",Visible=true   ,Editable=false ,Save =false ,EditPlaceHolder ="自动填写"},
            new KeyName(){Key="LastUpdateUserUsername",Name="最后更新用户名称",Visible=true  ,Editable=false,Save =false ,EditPlaceHolder ="自动填写" },

        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
