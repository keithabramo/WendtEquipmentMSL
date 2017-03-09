using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Models
{
    public class PriorityModel : BaseModel
    {
        public int? PriorityId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Priority")]
        [Remote("ValidPriorityNumber", "Validate", AdditionalFields = "PriorityId", ErrorMessage = "This priority number already exists")]
        [Required]
        [Range(Int32.MinValue, 99, ErrorMessage = "Priority Number cannot exceed 99")]
        public int PriorityNumber { get; set; }

        [DisplayName("Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public DateTime DueDate { get; set; }

        [DisplayName("Equipment")]
        [Required]
        public string EquipmentName { get; set; }
    }
}