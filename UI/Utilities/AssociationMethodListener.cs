using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontWork;

namespace WMS.UI
{
    class AssociationMethodListener : MethodListenerBase
    {
        //供应商名称输入联想
        private object[] SupplierNameAssociation(string str)
        {
            return (from s in GlobalData.AllSuppliers
                    where s["name"] != null && s["name"].ToString().StartsWith(str)
                    select s["name"]).ToArray();
        }

        //供应商代号输入联想
        private object[] SupplierNoAssociation(string str)
        {
            return (from s in GlobalData.AllSuppliers
                    where s["no"] != null && s["no"].ToString().StartsWith(str)
                    select s["no"]).ToArray();
        }

        //库位名称输入联想
        private object[] StorageLocationNameAssociation(string str)
        {
            return (from s in GlobalData.AllStorageLocations
                    where s["name"] != null && s["name"].ToString().StartsWith(str)
                    select s["name"]).ToArray();
        }

        //库位编码输入联想
        private object[] StorageLocationNoAssociation(string str)
        {
            return (from s in GlobalData.AllStorageLocations
                    where s["no"] != null && s["no"].ToString().StartsWith(str)
                    select s["no"]).ToArray();
        }
    }
}
