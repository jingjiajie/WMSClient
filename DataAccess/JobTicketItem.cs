//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobTicketItem
    {
        public int ID { get; set; }
        public int JobTicketID { get; set; }
        public Nullable<int> StockInfoID { get; set; }
        public string State { get; set; }
        public Nullable<int> PersonID { get; set; }
        public Nullable<System.DateTime> HappenTime { get; set; }
        public Nullable<decimal> ScheduledAmount { get; set; }
        public Nullable<decimal> RealAmount { get; set; }
        public string Unit { get; set; }
        public Nullable<int> JobPersonID { get; set; }
        public Nullable<int> ConfirmPersonID { get; set; }
        public Nullable<decimal> ScheduledPutOutAmount { get; set; }
        public Nullable<decimal> UnitAmount { get; set; }
        public Nullable<int> ShipmentTicketItemID { get; set; }
    
        public virtual JobTicket JobTicket { get; set; }
    }
}
