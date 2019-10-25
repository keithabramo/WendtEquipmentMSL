using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IEmailService
    {
        void SendRevisionSummary(IEnumerable<EquipmentRevisionBO> equipmentRevisionBOs);

        bool SendEquipmentSnippet(double projectNumber, string dataURL);
    }
}