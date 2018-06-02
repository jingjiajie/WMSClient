using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI.FormBase
{
    class BasePackageUnitMetaData
    {

        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false},
            new KeyName(){Key="Name",Name="单位名称",Visible=true,Editable=true},
            new KeyName(){Key="UnitAmount",Name="单位数量",Visible=true,Editable=true},
            
        };
        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
