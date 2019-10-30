using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IEmailService
    {
        void SendRevisionSummary(IEnumerable<EquipmentRevisionBO> equipmentRevisionBOs);

        bool SendEquipmentSnippet(string to, string subject, string body, string dataURL);
    }
}