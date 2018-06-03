using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI.FormBase
{
    class BaseWarehouseMetaData
    {
        public static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="仓库ID",Visible = false, Editable = false,Save=false},
            new KeyName(){Key="Name",Name="仓库名"},
        };
        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
