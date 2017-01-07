using Excel.Helper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Helper
{
    public static class ImportHelper
    {
        private static IDictionary<string, ExcelDataLocation> dataLocations = new Dictionary<string, ExcelDataLocation>
        {
            { "Work Order Prices", new ExcelDataLocation
                {
                    ColumnStart = 1,
                    RowStart = 2,
                    NumberOfColumns = 2
                }
            },
            { "Mechanical", new ExcelDataLocation
                {
                    ColumnStart = 14,
                    RowStart = 2,
                    NumberOfColumns = 9
                }
            },
            { "Advance Steel Assembly List", new ExcelDataLocation
                {
                    ColumnStart = 8,
                    RowStart = 3,
                    NumberOfColumns = 9
                }
            },
            { "Advanced Steel Hardware", new ExcelDataLocation
                {
                    ColumnStart = 9,
                    RowStart = 3,
                    NumberOfColumns = 9
                }
            }
        };

        public static IEnumerable<EquipmentRow> GetEquipmentFromSheet(string sheetName, ExcelDataReaderHelper excelHelper)
        {
            var dataLocation = dataLocations[sheetName];

            object[][] values = excelHelper.GetRangeCells(sheetName, dataLocation.ColumnStart, dataLocation.RowStart, dataLocation.NumberOfColumns);
            var records = values.Where(r => r[0] != null).Select(rowValues => new EquipmentRow
            {
                Equipment = rowValues[0].ToString(),
                Priority = rowValues[1].ToString(),
                ReleaseDate = rowValues[2].ToString(),
                DrawingNumber = rowValues[3].ToString(),
                WorkOrderNumber = rowValues[4].ToString(),
                Quantity = rowValues[5].ToString(),
                ShippingTagNumber = rowValues[6].ToString(),
                Description = rowValues[7].ToString(),
                UnitWeight = rowValues[8].ToString()
            }).ToList();

            return records;
        }

        public static IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(ExcelDataReaderHelper excelHelper)
        {
            var dataLocation = dataLocations["Work Order Prices"];

            object[][] values = excelHelper.GetRangeCells(0, dataLocation.ColumnStart, dataLocation.RowStart, dataLocation.NumberOfColumns);
            var records = values.Where(r => r[0] != null).Select(rowValues => new WorkOrderPriceRow
            {
                WorkOrderNumber = rowValues[0].ToString(),
                SalesPrice = rowValues[1].ToString()
            }).ToList();

            return records;
        }

        private class ExcelDataLocation
        {
            public int ColumnStart { get; set; }
            public int RowStart { get; set; }
            public int NumberOfColumns { get; set; }
        }
    }


}
