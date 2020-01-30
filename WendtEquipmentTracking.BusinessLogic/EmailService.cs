using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.Common.DTO;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class EmailService : IEmailService
    {
        public void SendRevisionSummary(IEnumerable<EquipmentRevisionBO> equipmentRevisionBOs)
        {
            var csv = createRevisionCSV(equipmentRevisionBOs);

            string username = ActiveDirectoryHelper.CurrentUserUsername();
            var user = ActiveDirectoryHelper.GetUser(username);

            if(user != null)
            {
                var drawingNumber = equipmentRevisionBOs.FirstOrDefault().DrawingNumber;

                if(string.IsNullOrEmpty(drawingNumber))
                {
                    drawingNumber = equipmentRevisionBOs.FirstOrDefault().NewDrawingNumber;
                }

                var emailDTO = new EmailDTO
                {
                    To = user.Email,
                    Subject = "Equipment Revision Summary",
                    Body = "Attached is the equipment revision summary for drawing #: " + drawingNumber,
                    Attachment = new AttachmentDTO
                    {
                        File = Encoding.ASCII.GetBytes(csv),
                        FileName = "Revision Summary.csv",
                        ContentType = "text/csv"
                    }
                };


                Mail.Send(emailDTO);
            } else
            {
                throw new Exception("User not found when sending equipment revision email");
            }
        }

        private string createRevisionCSV (IEnumerable<EquipmentRevisionBO> equipmentRevisionBOs)
        {
            var csvBuilder = new StringBuilder();

            var header = createRevisionCSVHeader();
            csvBuilder.AppendLine(header);

            foreach (var equipmentRevision in equipmentRevisionBOs)
            {
                var columns = new List<string>
                {
                    (equipmentRevision.EquipmentName ?? string.Empty).Trim().ToUpperInvariant(),
                    equipmentRevision.ReleaseDate?.ToString("M/d/yy"),
                    (equipmentRevision.DrawingNumber ?? string.Empty).Trim().ToUpperInvariant(),
                    equipmentRevision.Quantity.ToString(),
                    equipmentRevision.ShippedQuantity?.ToString(),
                    (equipmentRevision.ShippingTagNumber ?? string.Empty).Trim().ToUpperInvariant(),
                    (equipmentRevision.Description ?? string.Empty).Trim().ToUpperInvariant(),
                    equipmentRevision.UnitWeight?.ToString(),
                    (equipmentRevision.NewEquipmentName ?? string.Empty).Trim().ToUpperInvariant(),
                    equipmentRevision.NewReleaseDate?.ToString("M/d/yy"),
                    (equipmentRevision.NewDrawingNumber ?? string.Empty).Trim().ToUpperInvariant(),
                    equipmentRevision.NewQuantity.ToString(),
                    (equipmentRevision.NewShippingTagNumber ?? string.Empty).Trim().ToUpperInvariant(),
                    (equipmentRevision.NewDescription ?? string.Empty).Trim().ToUpperInvariant(),
                    equipmentRevision.NewUnitWeight?.ToString()
                };

                var line = string.Join(",", columns);
                csvBuilder.AppendLine(line);
            }

            return csvBuilder.ToString();
        }

        private string createRevisionCSVHeader()
        {
            var columns = new List<string>
            {
                "Equipment",
                "Released",
                "Dwg #",
                "Qty",
                "ShipQty",
                "Ship Tag #",
                "Description",
                "Unit Wt",
                "New Equipment",
                "New Released",
                "New Dwg #",
                "New Qty",
                "New Ship Tag #",
                "New Description",
                "New Unit Wt",
            };

            return string.Join(",", columns);
        }
    }
}
