using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Models
{
    public class ProjectModel : BaseModel
    {
        public int ProjectId { get; set; }

        [DisplayName("Project Number")]
        [Required]
        [Remote("ValidProjectNumber", "Validate", AdditionalFields = "ProjectId", ErrorMessage = "This project number already exists")]
        public double ProjectNumber { get; set; }

        [DisplayName("Freight Terms")]
        public string FreightTerms { get; set; }

        [DisplayName("Ship To Company")]
        public string ShipToCompany { get; set; }

        [DisplayName("Ship To Address")]
        public string ShipToAddress { get; set; }

        [DisplayName("Ship To CSZ")]
        public string ShipToCSZ { get; set; }

        [DisplayName("Ship To Contact 1")]
        public string ShipToContact1 { get; set; }

        [DisplayName("Ship To Contact 1 Phone\\Fax")]
        public string ShipToContact1PhoneFax { get; set; }

        [DisplayName("Ship To Contact 1 Email")]
        public string ShipToContact1Email { get; set; }

        [DisplayName("Ship To Contact 2")]
        public string ShipToContact2 { get; set; }

        [DisplayName("Ship To Contact 2 Phone\\Fax")]
        public string ShipToContact2PhoneFax { get; set; }

        [DisplayName("Ship To Contact 2 Email")]
        public string ShipToContact2Email { get; set; }

        [DisplayName("Ship To Broker")]
        public string ShipToBroker { get; set; }

        [DisplayName("Ship To Broker Phone\\Fax")]
        public string ShipToBrokerPhoneFax { get; set; }

        [DisplayName("Ship To Broker Email")]
        public string ShipToBrokerEmail { get; set; }

        [DisplayName("Is Export Project?")]
        public bool IsCustomsProject { get; set; }

        [DisplayName("Include Soft Costs on Commercial Invoice?")]
        public bool IncludeSoftCosts { get; set; }

        [DisplayName("Is Completed?")]
        public bool IsCompleted { get; set; }
    }
}