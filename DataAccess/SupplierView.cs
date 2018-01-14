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
    
    public partial class SupplierView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ContractNo { get; set; }
        public string FullName { get; set; }
        public string TaxpayerNumber { get; set; }
        public string Address { get; set; }
        public string Tel { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankNo { get; set; }
        public string ZipCode { get; set; }
        public string RecipientName { get; set; }
        public Nullable<decimal> InvoiceDelayMonth { get; set; }
        public Nullable<decimal> BalanceDelayMonth { get; set; }
        public string Number { get; set; }
        public string ContractState { get; set; }
        public Nullable<int> IsHistory { get; set; }
        public Nullable<System.DateTime> StartingTime { get; set; }
        public Nullable<System.DateTime> EndingTime { get; set; }
        public Nullable<int> NewestSupplierID { get; set; }
        public Nullable<int> CreateUserID { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<int> LastUpdateUserID { get; set; }
        public Nullable<System.DateTime> LastUpdateTime { get; set; }
        public Nullable<decimal> NetArea { get; set; }
        public Nullable<decimal> FixedStorageCost { get; set; }
        public Nullable<decimal> ContractStorageArea { get; set; }
        public string CreateUserUsername { get; set; }
        public string LastUpdateUserUsername { get; set; }
        public string Code { get; set; }
    }
}
