using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Utils
{
    public class EquipmentLogic
    {

        public void EquipmentNew(EquipmentBO equipmentBO)
        {
            equipmentBO.TotalWeight = equipmentBO.Quantity.HasValue && equipmentBO.UnitWeight.HasValue ? equipmentBO.Quantity.Value * equipmentBO.UnitWeight.Value : 0;
            equipmentBO.LeftToShip = equipmentBO.Quantity;
        }

        public void EquipmentUpdated(EquipmentBO equipmentBO)
        {
            equipmentBO.TotalWeight = equipmentBO.Quantity.HasValue && equipmentBO.UnitWeight.HasValue ? equipmentBO.Quantity.Value * equipmentBO.UnitWeight.Value : 0;

            var shippedQuantity = equipmentBO.BillOfLadingEquipments.Where(be => be.BillOfLading.IsCurrentRevision).Sum(be => be.Quantity);
            var leftToShip = equipmentBO.Quantity - shippedQuantity;

            equipmentBO.LeftToShip = leftToShip;
        }

        public void ReadyToShipAdjustment(EquipmentBO equipmentBO)
        {
            equipmentBO.LeftToShip = equipmentBO.ReadyToShip - equipmentBO.ShippedQuantity;
        }

        public void BOLAdjustment(IEnumerable<EquipmentBO> equipmentBOs)
        {

            foreach (var equipmentBO in equipmentBOs)
            {
                var shippedQuantity = equipmentBO.BillOfLadingEquipments.Where(be => be.BillOfLading.IsCurrentRevision).Sum(be => be.Quantity);
                var totalWeightShipped = equipmentBO.UnitWeight * shippedQuantity;
                var leftToShip = equipmentBO.Quantity - shippedQuantity;
                var fullyShipped = shippedQuantity >= equipmentBO.Quantity;

                equipmentBO.ShippedQuantity = shippedQuantity;
                equipmentBO.TotalWeightShipped = totalWeightShipped;
                equipmentBO.LeftToShip = leftToShip;
                equipmentBO.FullyShipped = fullyShipped;
            }
        }

    }
}
