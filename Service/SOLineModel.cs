//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class SOLineModel
    {
        public string SOLine { get; set; }
        public string SONumber { get; set; }
        public string CompanyCode { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> Qty { get; set; }
        public string UNIT { get; set; }
        public Nullable<decimal> OverTolerance { get; set; }
        public Nullable<decimal> UnderTolerance { get; set; }
        public Nullable<bool> isUnlimited { get; set; }
        public Nullable<System.DateTime> DocumentDate { get; set; }
        public string PONumber { get; set; }
        public Nullable<bool> isComplete { get; set; }
        public Nullable<decimal> SoLuongDaXuat { get; set; }
        public Nullable<bool> isClosed { get; set; }
    }
}
