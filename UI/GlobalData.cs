using System;
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
        public static List<IDictionary<string, object>> AllWarehouses;
        public static List<IDictionary<string, object>> AllSuppliers;
        public static List<IDictionary<string, object>> AllMaterials;
        public static List<IDictionary<string, object>> AllSupplies;
        public static List<IDictionary<string, object>> AllStorageLocations;
        public static List<IDictionary<string, object>> AllStorageAreas;

        private static int projectID = -1;
        private static int warehouseID = -1;
        private static int userID = -1;

        [Obsolete("二期已弃用")]
        public static int ProjectID { get => projectID; set => projectID = value; }
        [Obsolete("二期已弃用")]
        public static int WarehouseID { get => warehouseID; set => warehouseID = value; }
        [Obsolete("二期已弃用")]
        public static int UserID { get => userID; set => userID = value; }
    }
}
