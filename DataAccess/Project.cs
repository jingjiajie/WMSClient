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
    
    public partial class Project
    {
        public Project()
        {
            this.Component = new HashSet<Component>();
            this.ShipmentTicket = new HashSet<ShipmentTicket>();
            this.ReceiptTicket = new HashSet<ReceiptTicket>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Component> Component { get; set; }
        public virtual ICollection<ShipmentTicket> ShipmentTicket { get; set; }
        public virtual ICollection<ReceiptTicket> ReceiptTicket { get; set; }
    }
}
