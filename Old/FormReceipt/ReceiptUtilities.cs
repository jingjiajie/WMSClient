using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMS.DataAccess;

namespace WMS.UI
{
    public class ReceiptUtilities
    {
        public static SubmissionTicketItem ReceiptTicketItemToSubmissionTicketItem(ReceiptTicketItem receiptTicketItem, int submissionTicketID)
        {
            //WMSEntities wmsEntities = new WMSEntities();
            SubmissionTicketItem submissionTicketItem = new SubmissionTicketItem();
            //ReceiptTicketItemView receiptTicketItemView = (from rti in wmsEntities.ReceiptTicketItemView where rti.ID == receiptTicketItem.ID select rti).Single();
            //submissionTicketItem.ComponentID = receiptTicketItem.ComponentID;
            submissionTicketItem.SubmissionTicketID = submissionTicketID;
            submissionTicketItem.ID = 0;
            submissionTicketItem.State = "送检中";
            submissionTicketItem.SubmissionAmount = receiptTicketItem.ReceiviptAmount;
            submissionTicketItem.ReceiptTicketItemID = receiptTicketItem.ID;

            return submissionTicketItem;
        }

        public static PutawayTicketItem ReceiptTicketItemToPutawayTicketItem(ReceiptTicketItem receiptTicketItem, int putawayTicketID)
        {
            PutawayTicketItem putawayTicketItem = new PutawayTicketItem();

            putawayTicketItem.PutawayTicketID = putawayTicketID;
            putawayTicketItem.ReceiptTicketItemID = receiptTicketItem.ID;
            putawayTicketItem.State = "待上架";
            putawayTicketItem.ID = 0;

            return putawayTicketItem;
        }

        public static StockInfo PutawayTicketItemToStockInfo(PutawayTicketItem putawayTicketItem)
        {
            WMSEntities wmsEntities = new WMSEntities();
            StockInfo stockInfo = new StockInfo();
            stockInfo.ID = 0;
            stockInfo.ReceiptTicketItemID = putawayTicketItem.ReceiptTicketItemID;
            stockInfo.OverflowAreaAmount = 10000;
            ReceiptTicketItemView receiptTicketItemView = (from rti in wmsEntities.ReceiptTicketItemView where rti.ID == putawayTicketItem.ReceiptTicketItemID select rti).FirstOrDefault();
            //TODO stockInfo.ProjectID = receiptTicketItemView.ReceiptTicketProjectID;
            //TODO stockInfo.WarehouseID = receiptTicketItemView.ReceiptTicketWarehouse;

            return stockInfo;
        }

        public static int GetFirstColumnIndex(KeyName[] keyName)
        {
            int n = 0;
            foreach(KeyName kn in keyName)
            {
                if (kn.Visible == false)
                {
                    n++;
                }
                else
                {
                    return n;
                }
            }
            return n;
        }
    }
}
