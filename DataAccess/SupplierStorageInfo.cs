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
    
    public partial class SupplierStorageInfo
    {
        public int ID { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<decimal> AreaIncrement { get; set; }
        public Nullable<decimal> StorageDays { get; set; }
        public Nullable<decimal> StorageFee { get; set; }
    
        public virtual Supplier Supplier { get; set; }
    }
}
