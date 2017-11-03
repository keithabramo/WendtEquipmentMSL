using System;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class ProjectBO
    {
        public int ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public string FreightTerms { get; set; }
        public string ShipToCompany { get; set; }
        public string ShipToAddress { get; set; }
        public string ShipToCSZ { get; set; }
        public string ShipToContact1 { get; set; }
        public string ShipToContact1PhoneFax { get; set; }
        public string ShipToContact1Email { get; set; }
        public string ShipToContact2 { get; set; }
        public string ShipToContact2PhoneFax { get; set; }
        public string ShipToContact2Email { get; set; }
        public string ShipToBroker { get; set; }
        public string ShipToBrokerPhoneFax { get; set; }
        public string ShipToBrokerEmail { get; set; }
        public bool IsCustomsProject { get; set; }
        public bool IncludeSoftCosts { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}