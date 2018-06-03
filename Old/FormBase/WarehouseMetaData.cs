using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI.FormBase
{
    class WarehouseMetaData
    {
        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID"},
            new KeyName(){Key="Name",Name="仓库名称"},
        };

        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
