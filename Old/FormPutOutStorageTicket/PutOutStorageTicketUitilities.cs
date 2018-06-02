using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMS.DataAccess;

namespace WMS.UI
{
    class PutOutStorageTicketUitilities
    {
        public static bool GeneratePutOutStorageTicketFullSync(int[] jobTicketIDs, WMSEntities wmsEntities)
        {
            int succeedCount = 0;
            foreach (int jobTicketID in jobTicketIDs)
            {
                var newPutOutStorageTicket = new PutOutStorageTicket();

                JobTicket jobTicket = (from s in wmsEntities.JobTicket
                                       where s.ID == jobTicketID
                                       select s).FirstOrDefault();

                if (jobTicket == null)
                {
                    MessageBox.Show("作业单不存在，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                newPutOutStorageTicket.State = PutOutStorageTicketViewMetaData.STRING_STATE_NOT_LOADED;
                newPutOutStorageTicket.JobTicketID = jobTicket.ID;
                newPutOutStorageTicket.ProjectID = GlobalData.ProjectID;
                newPutOutStorageTicket.WarehouseID = GlobalData.WarehouseID;
                newPutOutStorageTicket.CreateUserID = GlobalData.UserID;
                newPutOutStorageTicket.CreateTime = DateTime.Now;

                JobTicketItem[] jobTicketItems = jobTicket.JobTicketItem.ToArray();

                //把所有条目加进出库单
                foreach (var jobTicketItem in jobTicketItems)
                {
                    if (jobTicketItem == null)
                    {
                        MessageBox.Show("无法找到作业单条目，请重新查询", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    PutOutStorageTicketItem newPutOutStorageTicketItem = new PutOutStorageTicketItem();

                    newPutOutStorageTicketItem.State = PutOutStorageTicketItemViewMetaData.STRING_STATE_WAIT_FOR_LOAD;
                    newPutOutStorageTicketItem.StockInfoID = jobTicketItem.StockInfoID;
                    newPutOutStorageTicketItem.JobTicketItemID = jobTicketItem.ID;
                    newPutOutStorageTicketItem.RealAmount = 0;
                    newPutOutStorageTicketItem.Unit = jobTicketItem.Unit;
                    newPutOutStorageTicketItem.UnitAmount = jobTicketItem.UnitAmount;
                    newPutOutStorageTicketItem.ReturnQualityUnit = "个";
                    newPutOutStorageTicketItem.ReturnQualityUnitAmount = 1;
                    newPutOutStorageTicketItem.ReturnRejectUnit = "个";
                    newPutOutStorageTicketItem.ReturnRejectUnitAmount = 1;

                    decimal restSchedulableAmountNoUnit = ((jobTicketItem.RealAmount ?? 0) - (jobTicketItem.ScheduledPutOutAmount ?? 0)) * (jobTicketItem.UnitAmount ?? 1);
                    if (restSchedulableAmountNoUnit <= 0)
                    {
                        continue;
                    }
                    jobTicketItem.ScheduledPutOutAmount = jobTicketItem.RealAmount;
                    newPutOutStorageTicketItem.ScheduledAmount = restSchedulableAmountNoUnit / newPutOutStorageTicketItem.UnitAmount;
                    newPutOutStorageTicket.PutOutStorageTicketItem.Add(newPutOutStorageTicketItem);
                }
                if(newPutOutStorageTicket.PutOutStorageTicketItem.Count == 0)
                {
                    continue;
                }
                //生成出库单号
                if (string.IsNullOrWhiteSpace(newPutOutStorageTicket.No))
                {
                    DateTime createDay = new DateTime(newPutOutStorageTicket.CreateTime.Value.Year, newPutOutStorageTicket.CreateTime.Value.Month, newPutOutStorageTicket.CreateTime.Value.Day);
                    DateTime nextDay = createDay.AddDays(1);
                    int maxRankOfToday = Utilities.GetMaxTicketRankOfDay((from p in wmsEntities.PutOutStorageTicket
                                                                          where p.CreateTime >= createDay && p.CreateTime < nextDay
                                                                          select p.No).ToArray());
                    newPutOutStorageTicket.No = Utilities.GenerateTicketNo("C", newPutOutStorageTicket.CreateTime.Value, maxRankOfToday + 1);
                }
                wmsEntities.PutOutStorageTicket.Add(newPutOutStorageTicket);
                wmsEntities.SaveChanges();
                succeedCount++;
            }
            if(succeedCount == 0)
            {
                MessageBox.Show("没有可用于分配出库的零件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
