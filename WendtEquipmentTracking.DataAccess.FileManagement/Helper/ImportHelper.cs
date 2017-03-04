using Excel;
using Excel.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                    NumberOfColumns = 3
                }
            }
        };

        public static IEnumerable<EquipmentRow> GetEquipment(Import import)
        {

            FileStream stream = File.Open(import.FilePath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader;

            //1. Reading Excel file
            if (Path.GetExtension(import.FilePath).ToUpper() == ".XLS")
            {
                //1.1 Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                //1.2 Reading from a OpenXml Excel file (2007 format; *.xlsx)
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }


            //2. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();

            //5. Data Reader methods
            IList<EquipmentRow> records = new List<EquipmentRow>();
            while (excelReader.Read())
            {
                var item = excelReader[0];
                var quantity = excelReader[1];
                var description = excelReader[2];
                var partNumber = excelReader[3];
                var length = excelReader[4];
                var width = excelReader[5];
                var specification = excelReader[6];
                var um = excelReader[7];
                var unitWeight = excelReader[8];
                var totalWeight = excelReader[9];

                var hardwareCommercialCode = import.hardwareCommercialCodes.SingleOrDefault(h => h.PartNumber == partNumber.ToString());
                string equipmentName = string.Empty;
                if (hardwareCommercialCode != null)
                {
                    equipmentName = hardwareCommercialCode.CommodityCode;
                }

                int quantityNumber = 0;
                if (!Int32.TryParse(quantity.ToString(), out quantityNumber))
                {
                    quantityNumber = 0;
                }

                double unitWeightNumber = 0;
                if (!Double.TryParse(unitWeight.ToString(), out unitWeightNumber))
                {
                    unitWeightNumber = 0;
                }

                var equipmentRecord = new EquipmentRow
                {
                    EquipmentName = equipmentName,
                    Priority = import.Priority,
                    ReleaseDate = DateTime.Now,
                    DrawingNumber = import.DrawingNumber,
                    WorkOrderNumber = import.WorkOrderNumber,
                    Quantity = import.QuantityMultiplier * quantityNumber,
                    ShippingTagNumber = partNumber.ToString(),
                    Description = description.ToString(),
                    UnitWeight = unitWeightNumber
                };

                records.Add(equipmentRecord);
            }

            return records;
        }

        public static IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(ExcelDataReaderHelper excelHelper)
        {
            var dataLocation = dataLocations["Work Order Prices"];

            object[][] values = excelHelper.GetRangeCells(0, dataLocation.ColumnStart, dataLocation.RowStart, dataLocation.NumberOfColumns);
            var records = values.Where(r => r[0] != null).Select(rowValues => new WorkOrderPriceRow
            {
                WorkOrderNumber = rowValues[0].ToString(),
                CostPrice = Convert.ToDouble(rowValues[1].ToString().Replace("$", "").Replace(",", "")),
                SalePrice = Convert.ToDouble(rowValues[2].ToString().Replace("$", "").Replace(",", ""))
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
