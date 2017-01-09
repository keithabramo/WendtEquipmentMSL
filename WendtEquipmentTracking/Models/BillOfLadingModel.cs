using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class BillOfLadingModel : BaseModel
    {
        public int BillOfLadingId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Revision")]
        public int Revision { get; set; }

        [DisplayName("Is Current Revision")]
        public bool IsCurrentRevision { get; set; }

        [DisplayName("BOL #")]
        [Required]
        public string BillOfLadingNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Date Shipped")]
        [Required]
        public DateTime? DateShipped { get; set; }

        [DisplayName("Shipped From")]
        [Required]
        public string ShippedFrom { get; set; }

        [DisplayName("To Storage")]
        public bool ToStorage { get; set; }

        public IList<BillOfLadingEquipmentModel> BillOfLadingEquipments { get; set; }

        public IndicatorsModel Indicators { get; set; }

        public void SetBillOfLadingIndicators()
        {
            Indicators = new IndicatorsModel();

            //Bill Of Lading Number
            if (string.IsNullOrEmpty(BillOfLadingNumber))
            {
                Indicators.BillOfLadingNumberColor = IndicatorsModel.Colors.Red;
            }

            //Bill Of Lading Number
            if (DateShipped == null)
            {
                Indicators.DateShippedNumberColor = IndicatorsModel.Colors.Red;
            }

            //Bill Of Lading Number
            if (string.IsNullOrEmpty(ShippedFrom))
            {
                Indicators.ShippedFromColor = IndicatorsModel.Colors.Red;
            }
        }
    }
}