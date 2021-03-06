﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI
{
    class GlobalData
    {
        public static string AccountBook;
        public static IDictionary<string, object> Person;
        public static IDictionary<string, object> Warehouse;
        public static IDictionary<string, object> SalaryPeriod;
        public static IDictionary<string, object> SalaryType;
        public static IDictionary<string, object> AccountTitle;
        //public static IDictionary<string, IDictionary<string, object[]>> AllDate;
        public static  IDictionary<string, object[]> AllDate;
        public static List<IDictionary<string, object>> AllWarehouses;
        public static List<IDictionary<string, object>> AllSuppliers;
        public static List<IDictionary<string, object>> AllMaterials;
        public static List<IDictionary<string, object>> AllSupplies;
        public static List<IDictionary<string, object>> AllStorageLocations;
        public static List<IDictionary<string, object>> AllStorageAreas;
        public static List<IDictionary<string, object>> AllPersons;
        public static List<IDictionary<string, object>> AllDestinations;
        public static List<IDictionary<string, object>> AllPackage;
        public static List<IDictionary<string, object>> AllSalaryType;
        public static List<IDictionary<string, object>> AllSalaryPeriod;
        public static List<IDictionary<string, object>> AllSalaryItem;        
        public static List<IDictionary<string, object>> AllAccountTitle;
        public static List<IDictionary<string, object>> AllAccountTitleTure;
        public static List<IDictionary<string, object>> AllTax;
        public static List<IDictionary<string, object>> AllAccountPeriod;
        public static List<IDictionary<string, object>> AllSummaryNote;
        public static IDictionary<string, object> AccountPeriod;
        private static int projectID = -1;
        private static int warehouseID = -1;
        private static int userID = -1;
        public static Boolean REMAINDENABLE=true;

        [Obsolete("二期已弃用")]
        public static int ProjectID { get => projectID; set => projectID = value; }
        [Obsolete("二期已弃用")]
        public static int WarehouseID { get => warehouseID; set => warehouseID = value; }
        [Obsolete("二期已弃用")]
        public static int UserID { get => userID; set => userID = value; }
    }
}
