using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;

namespace WMS.UI
{
    class ShipmentTicketUtilities
    {
        public static void UpdateShipmentTicketStateSync(int shipmentTicketID,WMSEntities wmsEntities,bool saveChanges = true)
        {
            int total = (from s in wmsEntities.ShipmentTicketItem where s.ShipmentTicketID == shipmentTicketID select s).Count();
            int fullAssignedCount = (from s in wmsEntities.ShipmentTicketItem where s.ShipmentTicketID == shipmentTicketID && s.ScheduledJobAmount==s.ShipmentAmount select s).Count();
            int notAssignedCount = (from s in wmsEntities.ShipmentTicketItem where s.ShipmentTicketID == shipmentTicketID && s.ScheduledJobAmount==0 select s).Count();
            Console.WriteLine("未分配："+notAssignedCount+"  总数："+total);
            if (notAssignedCount == total)
            {
                wmsEntities.Database.ExecuteSqlCommand(string.Format(
                    @"UPDATE ShipmentTicket SET State = '{0}' WHERE ID = {1}", ShipmentTicketViewMetaData.STRING_STATE_NOT_ASSIGNED_JOB, shipmentTicketID));
            }
            else if (fullAssignedCount == total)
            {
                wmsEntities.Database.ExecuteSqlCommand(string.Format(
                    @"UPDATE ShipmentTicket SET State = '{0}' WHERE ID = {1}", ShipmentTicketViewMetaData.STRING_STATE_ALL_ASSIGNED_JOB, shipmentTicketID));
            }
            else
            {
                wmsEntities.Database.ExecuteSqlCommand(string.Format(
                    @"UPDATE ShipmentTicket SET State = '{0}' WHERE ID = {1}", ShipmentTicketViewMetaData.STRING_STATE_PART_ASSIGNED_JOB, shipmentTicketID));
            }
            if (saveChanges) wmsEntities.SaveChanges();
        }

        public static bool DeleteItemsSync(int[] itemIDs,out string errorMessage)
        {
            try
            {
                using (WMSEntities wmsEntities = new WMSEntities())
                {
                    foreach (int id in itemIDs)
                    {
                        ShipmentTicketItem item = (from s in wmsEntities.ShipmentTicketItem where s.ID == id select s).FirstOrDefault();
                        if (item == null) continue;
                        if (item.ScheduledJobAmount > 0)
                        {
                            errorMessage = "不能删除已分配翻包的零件！";
                            return false;
                        }

                        //把库存已分配发货数减回去
                        decimal? amount = item.ShipmentAmount * item.UnitAmount;
                        StockInfo stockInfo = (from s in wmsEntities.StockInfo where s.ID == item.StockInfoID select s).FirstOrDefault();
                        if (stockInfo == null) continue;
                        stockInfo.ScheduledShipmentAmount -= amount ?? 0;
                        wmsEntities.ShipmentTicketItem.Remove(item);
                    }
                    wmsEntities.SaveChanges();
                    errorMessage = null;
                    return true;
                }
            }
            catch
            {
                errorMessage = "操作失败，请检查网络连接";
                return false;
            }
        }
    }
}
