using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public bool SendEquipmentSnippet(string to, string subject, string body, string dataURL)
        {
            bool success;

            string username = ActiveDirectoryHelper.CurrentUserUsername();
            var user = ActiveDirectoryHelper.GetUser(username);

            if (user != null)
            {
                var regexMatches = Regex.Match(dataURL, @"data:image/(?<type>.+?),(?<data>.+)");
                var base64Data = regexMatches.Groups["data"].Value;
                
                var attachment = Convert.FromBase64String(base64Data);
                var attachmentName = "EquipmentSnippet.png";
                var contentType = "image/png";
                body += "<br/><br/><br/><img src=\"cid:" + attachmentName + "\" />";

                var emailDTO = new EmailDTO
                {
                    From = user.Email,
                    To = to,
                    Subject = subject,
                    Body = body,
                    Attachment = new AttachmentDTO
                    {
                        File = attachment,
                        FileName = attachmentName,
                        ContentType = contentType
                    }
                };

                success = Mail.Send(emailDTO);

            }
            else
            {
                throw new Exception("User not found when sending equipment snippet email.");
            }

            return success;
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
                    equipmentRevision.EquipmentName,
                    equipmentRevision.ReleaseDate?.ToString("M/d/yy"),
                    equipmentRevision.DrawingNumber,
                    equipmentRevision.WorkOrderNumber,
                    equipmentRevision.Quantity.ToString(),
                    equipmentRevision.ShippedQuantity,
                    equipmentRevision.ShippingTagNumber,
                    equipmentRevision.Description,
                    equipmentRevision.UnitWeight?.ToString(),
                    equipmentRevision.NewEquipmentName,
                    equipmentRevision.NewReleaseDate?.ToString("M/d/yy"),
                    equipmentRevision.NewDrawingNumber,
                    equipmentRevision.NewWorkOrderNumber,
                    equipmentRevision.NewQuantity.ToString(),
                    equipmentRevision.NewShippingTagNumber,
                    equipmentRevision.NewDescription,
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
                "Work Order #",
                "Qty",
                "ShipQty",
                "Ship Tag #",
                "Description",
                "Unit Wt",
                "New Equipment",
                "New Released",
                "New Dwg #",
                "New Work Order #",
                "New Qty",
                "New Ship Tag #",
                "New Description",
                "New Unit Wt",
            };

            return string.Join(",", columns);
        }
    }
}
