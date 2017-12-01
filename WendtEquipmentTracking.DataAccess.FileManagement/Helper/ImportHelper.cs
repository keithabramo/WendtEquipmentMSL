using Excel.Helper;
using ExcelDataReader;
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
                    NumberOfColumns = 5
                }
            },
            { "Raw Equipment", new ExcelDataLocation
                {
                    ColumnStart = 1,
                    RowStart = 2,
                    NumberOfColumns = 14
                }
            }
        };

        public static IEnumerable<EquipmentRow> GetEquipment(EquipmentImport import)
        {
            IList<EquipmentRow> records = new List<EquipmentRow>();

            foreach (var keyValuePair in import.FilePaths)
            {
                //splitting these. They were joined in the ImportEquipment.js file
                var drawingNumber = keyValuePair.Key;
                var filePath = keyValuePair.Value;

                using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader excelReader;

                    //1. Reading Excel file
                    if (Path.GetExtension(filePath).ToUpper() == ".XLS")
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
                    DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            // Gets or sets a value indicating whether to use a row from the 
                            // data as column names.
                            UseHeaderRow = true
                        }
                    });

                    //5. Data Reader methods
                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        var item = row["ITEM"];
                        var quantity = row["QTY"];
                        var description = row["DESCRIPTION"];
                        var partNumber = row["PART NUMBER"];
                        var length = row["LENGTH"];
                        var width = row["WIDTH"];
                        var specification = row["SPECIFICATION"];
                        var um = row["UM"];
                        var unitWeight = row["UNIT WT. (LBS)"];
                        var totalWeight = row["TOTAL WT. (LBS)"];

                        if ((quantity == null || string.IsNullOrWhiteSpace(quantity.ToString())) && (description == null || string.IsNullOrWhiteSpace(description.ToString())) && (partNumber == null || string.IsNullOrWhiteSpace(partNumber.ToString())))
                        {
                            continue;
                        }

                        var hardwareCommercialCode = import.hardwareCommercialCodes.SingleOrDefault(h => h.PartNumber == partNumber.ToString());
                        string equipmentName = string.Empty;
                        if (hardwareCommercialCode != null)
                        {
                            equipmentName = hardwareCommercialCode.CommodityCode;
                        }
                        else
                        {
                            equipmentName = import.Equipment;
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
                        if (!string.IsNullOrEmpty(equipmentName) && equipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase))
                        {
                            unitWeightNumber = .01;
                        }

                        //round up to .01 for 0 unit weights
                        if (unitWeightNumber == 0)
                        {
                            unitWeightNumber = .01;
                        }

                        var equipmentRecord = new EquipmentRow
                        {
                            EquipmentName = equipmentName,
                            PriorityId = import.PriorityId,
                            ReleaseDate = DateTime.Now,
                            DrawingNumber = drawingNumber,
                            //DrawingNumber = import.DrawingNumber,
                            WorkOrderNumber = import.WorkOrderNumber,
                            Quantity = import.QuantityMultiplier * quantityNumber,
                            ShippingTagNumber = partNumber.ToString(),
                            Description = description.ToString(),
                            UnitWeight = unitWeightNumber
                        };

                        records.Add(equipmentRecord);
                    }
                }
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
                SalePrice = Convert.ToDouble(rowValues[2].ToString().Replace("$", "").Replace(",", "")),
                ReleasedPercent = Convert.ToDouble(rowValues[3].ToString().Replace("%", "")) * 100, //percent columns come back as decimals between 0 - 1
                ShippedPercent = Convert.ToDouble(rowValues[4].ToString().Replace("%", "")) * 100 //percent columns come back as decimals between 0 - 1
            }).ToList();

            return records;
        }

        public static IEnumerable<RawEquipmentRow> GetRawEquipment(ExcelDataReaderHelper excelHelper)
        {
            var dataLocation = dataLocations["Raw Equipment"];

            object[][] values = excelHelper.GetRangeCells(0, dataLocation.ColumnStart, dataLocation.RowStart, dataLocation.NumberOfColumns);

            var records = new List<RawEquipmentRow>();
            foreach (var rowValues in values.Where(r => r[0] != null))
            {
                var equipmentName = rowValues[0] != null ? rowValues[0].ToString() : "";
                var priorityNumberString = rowValues[1] != null ? rowValues[1].ToString() : "";
                var releaseDateString = rowValues[2] != null ? rowValues[2].ToString() : "";
                var drawingNumber = rowValues[3].ToString();
                var workOrderNumber = rowValues[4].ToString();
                var quantityString = rowValues[5] != null ? rowValues[5].ToString() : "";
                var shippingTagNumber = rowValues[6].ToString();
                var description = rowValues[7].ToString();
                var unitWeightString = rowValues[8] != null ? rowValues[8].ToString() : "";
                var readyToShipString = rowValues[9] != null ? rowValues[9].ToString() : "";
                var shippedFrom = rowValues[10] != null ? rowValues[10].ToString() : "";
                var htsCode = rowValues[11] != null ? rowValues[11].ToString() : "";
                var countryOfOrigin = rowValues[12] != null ? rowValues[12].ToString() : "";
                var notes = rowValues[13] != null ? rowValues[13].ToString() : "";

                int priorityNumber = 0;
                if (!Int32.TryParse(priorityNumberString, out priorityNumber))
                {
                    priorityNumber = 0;
                }

                DateTime releaseDate = DateTime.Now;
                if (!DateTime.TryParse(releaseDateString, out releaseDate))
                {
                    releaseDate = DateTime.Now;
                }

                int quantity = 0;
                if (!Int32.TryParse(quantityString, out quantity))
                {
                    quantity = 0;
                }

                double unitWeight = 0;
                if (!Double.TryParse(unitWeightString, out unitWeight))
                {
                    unitWeight = 0;
                }

                int readyToShip = 0;
                if (!Int32.TryParse(readyToShipString, out readyToShip))
                {
                    readyToShip = 0;
                }


                var record = new RawEquipmentRow
                {
                    EquipmentName = equipmentName,
                    PriorityNumber = priorityNumber,
                    ReleaseDate = releaseDate,
                    DrawingNumber = drawingNumber,
                    WorkOrderNumber = workOrderNumber,
                    Quantity = quantity,
                    ShippingTagNumber = shippingTagNumber,
                    Description = description,
                    UnitWeight = unitWeight,
                    ReadyToShip = readyToShip,
                    ShippedFrom = shippedFrom,
                    HTSCode = htsCode,
                    CountryOfOrigin = countryOfOrigin,
                    Notes = notes,
                };

                records.Add(record);
            }

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
