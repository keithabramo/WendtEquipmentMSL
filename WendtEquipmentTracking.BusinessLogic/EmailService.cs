﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.Common;

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


                Mail.Send(user.Email, "Equipment Revision Summary", "Attached is the equipment revision summary for drawing #: " + drawingNumber, Encoding.ASCII.GetBytes(csv));
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
                    equipmentRevision.EquipmentName,
                    equipmentRevision.PriorityNumber?.ToString(),
                    equipmentRevision.ReleaseDate?.ToString("M/D/yy"),
                    equipmentRevision.DrawingNumber,
                    equipmentRevision.WorkOrderNumber,
                    equipmentRevision.Quantity.ToString(),
                    equipmentRevision.ShippedQuantity,
                    equipmentRevision.ShippingTagNumber,
                    equipmentRevision.Description,
                    equipmentRevision.UnitWeight?.ToString(),
                    equipmentRevision.ShippedFrom,
                    equipmentRevision.NewEquipmentName,
                    equipmentRevision.NewPriorityNumber?.ToString(),
                    equipmentRevision.NewReleaseDate?.ToString("M/D/yy"),
                    equipmentRevision.NewDrawingNumber,
                    equipmentRevision.NewWorkOrderNumber,
                    equipmentRevision.NewQuantity.ToString(),
                    equipmentRevision.NewShippingTagNumber,
                    equipmentRevision.NewDescription,
                    equipmentRevision.NewUnitWeight?.ToString(),
                    equipmentRevision.NewShippedFrom,
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
                "Prty",
                "Released",
                "Dwg #",
                "Work Order #",
                "Qty",
                "ShipQty",
                "Ship Tag #",
                "Description",
                "Unit Wt",
                "Shipped From",
                "New Equipment",
                "New Prty",
                "New Released",
                "New Dwg #",
                "New Work Order #",
                "New Qty",
                "New Ship Tag #",
                "New Description",
                "New Unit Wt",
                "New Shipped From"
            };

            return string.Join(",", columns);
        }
    }
}
