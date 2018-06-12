using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontWork;

namespace WMS.UI
{
    class AssociationMethodListener : MethodListenerBase
    {
        //物料名称输入联想
        private object[] MaterialNameAssociation(string str)
        {
            return (from s in GlobalData.AllMaterials
                    where s["name"] != null && s["name"].ToString().StartsWith(str)
                    select s["name"]).ToArray();
        }

        //物料代号输入联想
        private object[] MaterialNoAssociation(string str)
        {
            return (from s in GlobalData.AllMaterials
                    where s["no"] != null && s["no"].ToString().StartsWith(str)
                    select s["no"]).ToArray();
        }

        //物料系列输入联想
        private object[] MaterialProductLineAssociation(string str)
        {
            return (from s in GlobalData.AllMaterials
                    where s["productLine"] != null && s["productLine"].ToString().StartsWith(str)
                    select s["productLine"]).ToArray();
        }

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

        //库区编码输入联想
        private object[] StorageAreaNoAssociation(string str)
        {
            return (from s in GlobalData.AllStorageAreas
                    where s["no"] != null && s["no"].ToString().StartsWith(str)
                    select s["no"]).ToArray();
        }

        //库区名称输入联想
        private object[] StorageAreaNameAssociation(string str)
        {
            return (from s in GlobalData.AllStorageAreas
                    where s["name"] != null && s["name"].ToString().StartsWith(str)
                    select s["name"]).ToArray();
        }

        //人员名称输入联想
        private object[] PersonAssociation(string str)
        {
            return (from s in GlobalData.AllPersons
                    where s["name"] != null && s["name"].ToString().StartsWith(str)
                    select s["name"]).ToArray();
        }

        //套餐名称输入联想
        private object[] PackageAssociation(string str)
        {
            return (from s in GlobalData.AllPackage
                    where s["name"] != null && s["name"].ToString().StartsWith(str)
                    select s["name"]).ToArray();
        }
    }
}
