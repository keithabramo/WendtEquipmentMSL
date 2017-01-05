using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Utils
{
    public class EquipmentLogic
    {
        private IEquipmentService equipmentService;

        public EquipmentLogic()
        {
            equipmentService = new EquipmentService();
        }

        public void TotalWeightAdjustment(EquipmentBO equipmentBO)
        {
            equipmentBO.TotalWeight = equipmentBO.Quantity.HasValue && equipmentBO.UnitWeight.HasValue ? equipmentBO.Quantity.Value * equipmentBO.UnitWeight.Value : 0;
        }

        public void ReadyToShipAdjustment(EquipmentBO equipmentBO)
        {
            equipmentBO.LeftToShip = equipmentBO.ReadyToShip - equipmentBO.ShippedQuantity;
        }

        public void BOLAdjustment(int billOfLadingId)
        {
            var equipmentBOs = equipmentService.GetByBillOfLadingId(billOfLadingId);

            foreach (var equipmentBO in equipmentBOs)
            {
                var shippedQuantity = equipmentBO.BillOfLadingEquipments.Where(be => be.BillOfLading.IsCurrentRevision).Sum(be => be.Quantity);
                var totalWeightShipped = equipmentBO.UnitWeight * shippedQuantity;
                var leftToShip = equipmentBO.ReadyToShip - shippedQuantity;
                var fullyShipped = shippedQuantity >= equipmentBO.Quantity;

                equipmentBO.ShippedQuantity = shippedQuantity;
                equipmentBO.TotalWeightShipped = totalWeightShipped;
                equipmentBO.LeftToShip = leftToShip;
                equipmentBO.FullyShipped = fullyShipped;

                equipmentService.Update(equipmentBO);
            }
        }

    }
}
