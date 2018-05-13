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
        public static List<IDictionary<string, object>> WarehouseList;

        private static int projectID = -1;
        private static int warehouseID = -1;
        private static int userID = -1;

        public static int ProjectID { get => projectID; set => projectID = value; }
        public static int WarehouseID { get => warehouseID; set => warehouseID = value; }
        public static int UserID { get => userID; set => userID = value; }
    }
}
