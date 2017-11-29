﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WMSEntities : DbContext
    {
        public WMSEntities()
            : base("name=WMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Component> Component { get; set; }
        public DbSet<ComponentOuterPackingSize> ComponentOuterPackingSize { get; set; }
        public DbSet<ComponentShipmentInfo> ComponentShipmentInfo { get; set; }
        public DbSet<ComponentSingleBoxTranPackingInfo> ComponentSingleBoxTranPackingInfo { get; set; }
        public DbSet<ComponentStatistics> ComponentStatistics { get; set; }
        public DbSet<JobTicket> JobTicket { get; set; }
        public DbSet<JobTicketItem> JobTicketItem { get; set; }
        public DbSet<PutawayTicket> PutawayTicket { get; set; }
        public DbSet<PutawayTicketComponentInfo> PutawayTicketComponentInfo { get; set; }
        public DbSet<PutInStorageTicketComponentInfo> PutInStorageTicketComponentInfo { get; set; }
        public DbSet<PutOutStorageTicket> PutOutStorageTicket { get; set; }
        public DbSet<ReceiptTicket> ReceiptTicket { get; set; }
        public DbSet<ShipmentTicket> ShipmentTicket { get; set; }
        public DbSet<StockInfo> StockInfo { get; set; }
        public DbSet<SubmissionTicket> SubmissionTicket { get; set; }
        public DbSet<SubmissionTicketItem> SubmissionTicketItem { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<SupplierAnnualInfo> SupplierAnnualInfo { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
    }
}
