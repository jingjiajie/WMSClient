using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS.UI.FromSalary
{
    class AccountSynchronize
    {
        public int getPersonId()
        {
            return personId;
        }

        public int getWarehouseId()
        {
            return warehouseId;
        }

        public int getPayNoteId()
        {
            return payNoteId;
        }

        public String getComment()
        {
            return comment;
        }

        public String getVoucherInfo()
        {
            return voucherInfo;
        }

        public String getSummary()
        {
            return summary;
        }

        public void setPersonId(int personId)
        {
            this.personId = personId;
        }

        public void setWarehouseId(int warehouseId)
        {
            this.warehouseId = warehouseId;
        }

        public void setPayNoteId(int payNoteId)
        {
            this.payNoteId = payNoteId;
        }

        public void setComment(String comment)
        {
            this.comment = comment;
        }

        public void setVoucherInfo(String voucherInfo)
        {
            this.voucherInfo = voucherInfo;
        }

        public void setSummary(String summary)
        {
            this.summary = summary;
        }

        private int personId;

        private int warehouseId;

        private int payNoteId;

        private String comment;

        private String voucherInfo;

        private String summary;
    }
}
