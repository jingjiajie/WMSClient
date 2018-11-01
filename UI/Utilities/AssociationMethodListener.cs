using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontWork;

namespace WMS.UI
{
    [MethodListener]
    class AssociationMethodListener
    {
        //物料名称输入联想
        private object[] MaterialNameAssociation([Data]string str)
        {
            return (from s in GlobalData.AllMaterials
                    where s["name"] != null
                    && s["name"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["name"]).Distinct().ToArray();
        }

        //物料代号输入联想
        private object[] MaterialNoAssociation([Data]string str)
        {
            return (from s in GlobalData.AllMaterials
                    where s["no"] != null
                    && s["no"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["no"]).Distinct().ToArray();
        }

        //物料系列输入联想
        private object[] MaterialProductLineAssociation([Data]string str)
        {
            return (from s in GlobalData.AllMaterials
                    where s["productLine"] != null
                    && s["productLine"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["productLine"]).Distinct().ToArray();
        }

        //供应商名称输入联想
        private object[] SupplierNameAssociation([Data]string str)
        {
            return (from s in GlobalData.AllSuppliers
                    where s["name"] != null 
                    && s["name"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["name"]).Distinct().ToArray();
        }

        //供应商代号输入联想
        private object[] SupplierNoAssociation([Data]string str)
        {
            return (from s in GlobalData.AllSuppliers
                    where s["no"] != null 
                    && s["no"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["no"]).Distinct().ToArray();
        }

        //库位名称输入联想
        private object[] StorageLocationNameAssociation([Data]string str)
        {
            return (from s in GlobalData.AllStorageLocations
                    where s["name"] != null
                    && s["name"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["name"]).Distinct().ToArray();
        }

        //库位编码输入联想
        private object[] StorageLocationNoAssociation([Data]string str)
        {
            return (from s in GlobalData.AllStorageLocations
                    where s["no"] != null 
                    && s["no"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["no"]).Distinct().ToArray();
        }

        //库区编码输入联想
        private object[] StorageAreaNoAssociation([Data]string str)
        {
            return (from s in GlobalData.AllStorageAreas
                    where s["no"] != null
                    && s["no"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["no"]).Distinct().ToArray();
        }

        //库区名称输入联想
        private object[] StorageAreaNameAssociation([Data]string str)
        {
            return (from s in GlobalData.AllStorageAreas
                    where s["name"] != null 
                    && s["name"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["name"]).Distinct().ToArray();
        }

        //人员名称输入联想
        private object[] PersonAssociation([Data]string str)
        {
            return (from s in GlobalData.AllPersons
                    where s["name"] != null 
                    && s["name"].ToString().StartsWith(str)                 
                    select s["name"]).Distinct().ToArray();
        }

        //套餐名称输入联想
        private object[] PackageAssociation([Data]string str)
        {
            return (from s in GlobalData.AllPackage
                    where s["name"] != null
                    && s["name"].ToString().StartsWith(str)
                    && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["name"]).Distinct().ToArray();
        }

        //薪资类型名称输入联想       
        private object[] SalaryTypeNameAssociation([Data]string str)
            {
              return (from s in GlobalData.AllSalaryType
                     where s["name"] != null
                        && s["name"].ToString().StartsWith(str)
                        && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                        select s["name"]).Distinct().ToArray();
            }

        //薪资项目名称输入联想
        private object[] SalaryItemNameAssociation([Data]string str)
        {
            return (from s in GlobalData.AllSalaryItem
                    where s["name"] != null
                       && s["name"].ToString().StartsWith(str)
                       && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["name"]).Distinct().ToArray();
        }

        //薪资期间名称输入联想
        private object[] SalaryPeriodNameAssociation([Data]string str)
        {
            return (from s in GlobalData.AllSalaryPeriod
                    where s["name"] != null
                       && s["name"].ToString().StartsWith(str)
                       && s["warehouseId"].Equals(GlobalData.Warehouse["id"])
                    select s["name"]).Distinct().ToArray();
        }

        //科目名称输入联想
        private object[] AccountTitleNameAssociation([Data]string str)
        {
            return (from s in GlobalData.AllAccountTitle
                    where s["name"] != null
                       && s["name"].ToString().StartsWith(str)
                      
                    select s["name"]).Distinct().ToArray();
        }

        //科目编码输入联想
        private object[] AccountTitleNoAssociation([Data]string str)
        {
            return (from s in GlobalData.AllAccountTitle
                    where s["no"] != null
                       && s["no"].ToString().StartsWith(str)
                    select s["no"]).Distinct().ToArray();
        }


        //税务编码输入联想
        private object[] TaxNoAssociation([Data]string str)
        {
            return (from s in GlobalData.AllTax
                    where s["no"] != null
                       && s["no"].ToString().StartsWith(str)
                    select s["no"]).Distinct().ToArray();
        }

        //税务编码输入联想
        private object[] TaxNameAssociation([Data]string str)
        {
            return (from s in GlobalData.AllTax
                    where s["name"] != null
                       && s["name"].ToString().StartsWith(str)
                    select s["name"]).Distinct().ToArray();
        }

        //税务编码输入联想
        private object[] SummaryNoteNoAssociation([Data]string str)
        {
            return (from s in GlobalData.AllSummaryNote
                    where s["no"] != null
                       && s["no"].ToString().StartsWith(str)
                    select s["no"]).Distinct().ToArray();
        }
    }
}
