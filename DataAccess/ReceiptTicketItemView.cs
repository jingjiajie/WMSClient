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
    
    public partial class ReceiptTicketItemView
    {
        public Nullable<int> ReceiptTicketID { get; set; }
        public string PackageName { get; set; }
        public Nullable<decimal> ExpectedPackageAmount { get; set; }
        public Nullable<decimal> ExpectedAmount { get; set; }
        public Nullable<decimal> ReceiviptAmount { get; set; }
        public Nullable<decimal> WrongComponentAmount { get; set; }
        public Nullable<decimal> ShortageAmount { get; set; }
        public Nullable<decimal> DisqualifiedAmount { get; set; }
        public string ManufactureNo { get; set; }
        public Nullable<System.DateTime> InventoryDate { get; set; }
        public Nullable<System.DateTime> ManufactureDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string RealRightProperty { get; set; }
        public string BoxNo { get; set; }
        public string ComponentName { get; set; }
        public string ProjectName { get; set; }
        public string CreateUserUsername { get; set; }
        public string CreateUserPassword { get; set; }
        public Nullable<int> CreateUserAuthority { get; set; }
        public string CreateUserAuthorityName { get; set; }
        public Nullable<int> CreateUserSupplierID { get; set; }
        public string LastUpdateUserUsername { get; set; }
        public string LastUpdateUserPassword { get; set; }
        public Nullable<int> LastUpdateUserAuthority { get; set; }
        public string LastUpdateUserAuthorityName { get; set; }
        public Nullable<int> LastUpdateUserSupplierID { get; set; }
        public int ID { get; set; }
        public string State { get; set; }
        public string ReceiptTicketState { get; set; }
        public string ReceiptTicketNo { get; set; }
        public string ReceiptTicketNumber { get; set; }
        public Nullable<int> ReceiptTicketSupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNumber { get; set; }
        public Nullable<int> JobPersonID { get; set; }
        public Nullable<int> ConfirmPersonID { get; set; }
        public string JobPersonName { get; set; }
        public string ConfirmPersonName { get; set; }
        public Nullable<int> SupplyComponentID { get; set; }
        public string SupplyNumber { get; set; }
        public string SupplyNo { get; set; }
        public Nullable<decimal> ComponentDefaultShipmentUnitAmount { get; set; }
        public string ComponentDefaultShipmentUnit { get; set; }
        public Nullable<decimal> ComponentDefaultReceiptUnitAmount { get; set; }
        public string ComponentDefaultReceiptUnit { get; set; }
        public Nullable<decimal> UnitAmount { get; set; }
        public string Unit { get; set; }
        public Nullable<int> SupplyID { get; set; }
        public string SupplyDefaultReceiptUnit { get; set; }
        public Nullable<decimal> SupplyDefaultReceiptUnitAmount { get; set; }
        public string SupplyDefaultShipmentUnit { get; set; }
        public Nullable<decimal> SupplyDefaultShipmentUnitAmount { get; set; }
        public Nullable<decimal> UnitCount { get; set; }
        public Nullable<decimal> HasPutwayAmount { get; set; }
        public Nullable<int> WarehouseID { get; set; }
        public string WarehouseName { get; set; }
    }
}
