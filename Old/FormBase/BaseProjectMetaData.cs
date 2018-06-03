using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI.FormBase
{
    class BaseProjectMetaData
    {

        private static KeyName[] keyNames = {
            new KeyName(){Key="ID",Name="ID",Visible=false,Editable=false},
            new KeyName(){Key="Name",Name="ÏîÄ¿Ãû³Æ",Visible=true,Editable=true},
        };
        public static KeyName[] KeyNames { get => keyNames; set => keyNames = value; }
    }
}
