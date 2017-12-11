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
    
    public partial class ShipmentTicketItem
    {
        public int ID { get; set; }
        public int ShipmentTicketID { get; set; }
        public Nullable<int> StockInfoID { get; set; }
        public Nullable<decimal> ExpectedShipmentAmount { get; set; }
        public Nullable<decimal> AssignedAmount { get; set; }
        public Nullable<decimal> PickingAmount { get; set; }
        public Nullable<decimal> ShipmentInAdvanceAmount { get; set; }
        public Nullable<decimal> ShipmentAmount { get; set; }
        public Nullable<decimal> ExceedStockAmount { get; set; }
        public Nullable<System.DateTime> OnlineTime { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public string RequirePackageNo { get; set; }
        public string TargetPlace { get; set; }
        public string InnerShipmentPath { get; set; }
        public Nullable<decimal> LookBoardCount { get; set; }
        public string Unit { get; set; }
    
        public virtual ShipmentTicket ShipmentTicket { get; set; }
    }
}
