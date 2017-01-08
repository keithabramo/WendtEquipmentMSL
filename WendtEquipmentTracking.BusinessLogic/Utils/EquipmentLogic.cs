using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Utils
{
    public class EquipmentLogic
    {

        /* Things that can be updated:
         * 
         * EQUIPMENT PROPERTIES:
         * TotalWeight
         * ShippedQuantity
         * LeftToShip
         * ReadyToShip
         * 
         * BOL PROPERTIES:
         * ReadyToShipHistory
        */


        /* Could have changed:
         * Quantity
         * UnitWeight
        */
        public void EquipmentNew(EquipmentBO equipmentBO)
        {
            equipmentBO.TotalWeight = equipmentBO.Quantity.HasValue && equipmentBO.UnitWeight.HasValue ? equipmentBO.Quantity.Value * equipmentBO.UnitWeight.Value : 0;
            equipmentBO.LeftToShip = equipmentBO.Quantity;
        }

        /* Could have changed:
         * Quantity
         * UnitWeight
        */
        public void EquipmentUpdated(EquipmentBO equipmentBO)
        {
            equipmentBO.TotalWeight = equipmentBO.Quantity.HasValue && equipmentBO.UnitWeight.HasValue ? equipmentBO.Quantity.Value * equipmentBO.UnitWeight.Value : 0;

            var shippedQuantity = equipmentBO.BillOfLadingEquipments.Where(be => be.BillOfLading.IsCurrentRevision).Sum(be => be.Quantity);
            var leftToShip = equipmentBO.Quantity - shippedQuantity;

            equipmentBO.LeftToShip = leftToShip;
        }

        /* Could have changed:
         * Ready To Ship
        */
        public void ReadyToShipUpdated(EquipmentBO equipmentBO)
        {
            equipmentBO.LeftToShip = equipmentBO.ReadyToShip - equipmentBO.ShippedQuantity;
        }

        /* Could have changed:
         * BOL Quantities
        */
        public void BOLAdded(IEnumerable<EquipmentBO> equipmentBOs)
        {

            foreach (var equipmentBO in equipmentBOs)
            {
                var shippedQuantity = equipmentBO.BillOfLadingEquipments.Where(be => be.BillOfLading.IsCurrentRevision).Sum(be => be.Quantity);
                var totalWeightShipped = equipmentBO.UnitWeight * shippedQuantity;
                var leftToShip = equipmentBO.Quantity - shippedQuantity;
                var fullyShipped = shippedQuantity >= equipmentBO.Quantity;
                var readyToShip = equipmentBO.ReadyToShip - (shippedQuantity - equipmentBO.ShippedQuantity);

                equipmentBO.ShippedQuantity = shippedQuantity;
                equipmentBO.TotalWeightShipped = totalWeightShipped;
                equipmentBO.LeftToShip = leftToShip;
                equipmentBO.FullyShipped = fullyShipped;
                equipmentBO.ReadyToShip = readyToShip;
            }
        }

        /* Could have changed:
         * BOL Quantities
        */
        public void BOLUpdated(IEnumerable<EquipmentBO> equipmentBOs)
        {

            foreach (var equipmentBO in equipmentBOs)
            {
                var shippedQuantity = equipmentBO.BillOfLadingEquipments.Where(be => be.BillOfLading.IsCurrentRevision).Sum(be => be.Quantity);
                var totalWeightShipped = equipmentBO.UnitWeight * shippedQuantity;
                var leftToShip = equipmentBO.Quantity - shippedQuantity;
                var fullyShipped = shippedQuantity >= equipmentBO.Quantity;
                var readyToShip = equipmentBO.ReadyToShip - (shippedQuantity - equipmentBO.ShippedQuantity);

                equipmentBO.ShippedQuantity = shippedQuantity;
                equipmentBO.TotalWeightShipped = totalWeightShipped;
                equipmentBO.LeftToShip = leftToShip;
                equipmentBO.FullyShipped = fullyShipped;
                equipmentBO.ReadyToShip = readyToShip;
            }
        }

    }
}
