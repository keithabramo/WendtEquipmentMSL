//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WendtEquipmentTracking.DataAccess.SQL
{
    using System;
    using System.Collections.Generic;
    
    public partial class WorkOrderPrice
    {
        public int WorkOrderPriceId { get; set; }
        public int ProjectId { get; set; }
        public string WorkOrderNumber { get; set; }
        public double SalePrice { get; set; }
        public double CostPrice { get; set; }
        public double TotalWeight { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual Project Project { get; set; }
    }
}