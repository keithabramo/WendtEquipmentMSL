using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;

namespace WendtEquipmentTracking.App.Models
{
    public class PriorityModel : BaseModel
    {
        public int? PriorityId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Prty")]
        [Remote("ValidPriorityNumber", "Validate", AdditionalFields = "PriorityId", ErrorMessage = "This priority number already exists")]
        [Required]
        [Range(Int32.MinValue, 99, ErrorMessage = "Priority Number cannot exceed 99")]
        public int PriorityNumber { get; set; }

        [DisplayName("Start Date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public DateTime DueDate { get; set; }

        [DisplayName("End Date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Contractual Ship Date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ContractualShipDate { get; set; }

        [DisplayName("Equipment")]
        [Required]
        public string EquipmentName { get; set; }

        public bool IsDuplicate { get; set; }
    }
}