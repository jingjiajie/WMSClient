using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;

namespace WMS.UI
{
    class JobTicketUtilities
    {
        /// <summary>
        /// 无脑生成作业单，不选零件不填数量。
        /// </summary>
        /// <param name="shipmentTicketIDs"></param>
        /// <param name="wmsEntities"></param>
        public static bool GenerateJobTicketFullSync(int shipmentTicketID, WMSEntities wmsEntities,DateTime? createTime = null)
        {
            ShipmentTicket shipmentTicket = (from s in wmsEntities.ShipmentTicket
                                             where s.ID == shipmentTicketID
                                             select s).FirstOrDefault();
            if (shipmentTicket == null)
            {
                MessageBox.Show("发货单不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            ShipmentTicketItem[] shipmentTicketItems = (from s in wmsEntities.ShipmentTicketItem
                                                        where s.ShipmentTicketID == shipmentTicketID
                                                        select s).ToArray();
            List<JobTicketItem> newJobTicketItems = new List<JobTicketItem>();
            for (int i = 0; i < shipmentTicketItems.Length; i++)
            {
                ShipmentTicketItem shipmentTicketItem = shipmentTicketItems[i];
                if (shipmentTicketItem == null)
                {
                    MessageBox.Show("发货单条目不存在，可能已被删除，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                decimal restScheduableAmount = (shipmentTicketItem.ShipmentAmount - (shipmentTicketItem.ScheduledJobAmount ?? 0)) ?? 0;
                if (restScheduableAmount <= 0) continue;
                var jobTicketItem = new JobTicketItem();
                jobTicketItem.ScheduledAmount = restScheduableAmount;
                jobTicketItem.StockInfoID = shipmentTicketItem.StockInfoID;
                jobTicketItem.ShipmentTicketItemID = shipmentTicketItem.ID;
                jobTicketItem.State = JobTicketItemViewMetaData.STRING_STATE_UNFINISHED;
                jobTicketItem.Unit = shipmentTicketItem.Unit;
                jobTicketItem.UnitAmount = shipmentTicketItem.UnitAmount;
                jobTicketItem.RealAmount = 0;
                shipmentTicketItem.ScheduledJobAmount = (shipmentTicketItem.ScheduledJobAmount ?? 0) + jobTicketItem.ScheduledAmount;
                newJobTicketItems.Add(jobTicketItem);
            }
            if (newJobTicketItems.Count == 0)
            {
                MessageBox.Show("发货单 " + shipmentTicket.No + " 中无可分配翻包的零件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            JobTicket newJobTicket = new JobTicket();
            foreach(JobTicketItem item in newJobTicketItems)
            {
                newJobTicket.JobTicketItem.Add(item);
            }
            wmsEntities.JobTicket.Add(newJobTicket);

            newJobTicket.State = JobTicketViewMetaData.STRING_STATE_UNFINISHED;
            newJobTicket.ShipmentTicketID = shipmentTicket.ID;
            newJobTicket.ProjectID = GlobalData.ProjectID;
            newJobTicket.WarehouseID = GlobalData.WarehouseID;
            newJobTicket.CreateUserID = GlobalData.UserID;
            newJobTicket.CreateTime = createTime ?? DateTime.Now;

            if (string.IsNullOrWhiteSpace(newJobTicket.JobTicketNo))
            {
                DateTime createDay = new DateTime(shipmentTicket.CreateTime.Value.Year, shipmentTicket.CreateTime.Value.Month, shipmentTicket.CreateTime.Value.Day);
                DateTime nextDay = createDay.AddDays(1);
                int maxRankOfToday = Utilities.GetMaxTicketRankOfDay((from j in wmsEntities.JobTicket
                                                                      where j.CreateTime >= createDay && j.CreateTime < nextDay
                                                                      select j.JobTicketNo).ToArray());
                newJobTicket.JobTicketNo = Utilities.GenerateTicketNo("Z", newJobTicket.CreateTime.Value, maxRankOfToday + 1);
            }
            wmsEntities.SaveChanges();
            //更新发货单状态
            ShipmentTicketUtilities.UpdateShipmentTicketStateSync(shipmentTicketID, wmsEntities, true);
            return true;
        }
    }
}
