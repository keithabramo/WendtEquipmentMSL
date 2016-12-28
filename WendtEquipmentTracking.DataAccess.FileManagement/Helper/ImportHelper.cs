using Excel.Helper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WendtEquipmentTracking.DataAccess.FileManagement.Domain;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Helper
{
    public static class ImportHelper
    {
        private static IDictionary<string, ExcelDataLocation> sheetDataLocations = new Dictionary<string, ExcelDataLocation>
        {
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

        public static IEnumerable<ImportRow> GetImportRecordsForSheet(string sheetName, ExcelDataReaderHelper excelHelper)
        {
            var dataLocation = sheetDataLocations[sheetName];

            object[][] values = excelHelper.GetRangeCells(sheetName, dataLocation.ColumnStart, dataLocation.RowStart, dataLocation.NumberOfColumns);
            var sheetRecords = values.Where(r => r[0] != null).Select(rowValues => new ImportRow
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

            return sheetRecords;
        }

        private class ExcelDataLocation
        {
            public int ColumnStart { get; set; }
            public int RowStart { get; set; }
            public int NumberOfColumns { get; set; }
        }
    }


}
