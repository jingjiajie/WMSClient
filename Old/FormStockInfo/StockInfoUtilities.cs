using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.DataAccess;

namespace WMS.UI
{
    class StockInfoUtilities
    {
        public static void AddStockInfo(StockInfo stockInfo, WMSEntities wmsEntities, bool saveChanges = true)
        {
            wmsEntities.StockInfo.Add(stockInfo);
            if (saveChanges) wmsEntities.SaveChanges();
        }
    }
}
