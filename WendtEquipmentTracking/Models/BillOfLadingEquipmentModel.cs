using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class BillOfLadingEquipmentModel
    {
        public int BillOfLadingEquipmentId { get; set; }
        public int BillOfLadingId { get; set; }
        public int EquipmentId { get; set; }

        public bool Checked { get; set; }

        [DisplayName("Quantity Shipped with this BOL")]
        [Required]
        public double Quantity { get; set; }

        [DisplayName("Shipped From")]
        public string ShippedFrom { get; set; }

        [DisplayName("HTS Code")]
        public string HTSCode { get; set; }

        [DisplayName("Country Of Origin")]
        public string CountryOfOrigin { get; set; }

        public BillOfLadingModel BillOfLading { get; set; }
        public EquipmentModel Equipment { get; set; }
    }
}