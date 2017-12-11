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
    
    public partial class PutOutStorageTicket
    {
        public PutOutStorageTicket()
        {
            this.PutOutStorageTicketItem = new HashSet<PutOutStorageTicketItem>();
        }
    
        public int ID { get; set; }
        public string No { get; set; }
        public string TruckLoadingTicketNo { get; set; }
        public string Sourse { get; set; }
        public string WorkFlow { get; set; }
        public string State { get; set; }
        public string CarNum { get; set; }
        public string Driver { get; set; }
        public string SerialNo { get; set; }
        public string OriginalTicketType { get; set; }
        public string PullTicketNo { get; set; }
        public string CrossingNo { get; set; }
        public string ReceiverNo { get; set; }
        public string SortTypeNo { get; set; }
        public Nullable<System.DateTime> TruckLoadingTime { get; set; }
        public Nullable<System.DateTime> DeliverTime { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public Nullable<int> JobTicketID { get; set; }
        public Nullable<int> LastUpdateUserID { get; set; }
    
        public virtual ICollection<PutOutStorageTicketItem> PutOutStorageTicketItem { get; set; }
    }
}
