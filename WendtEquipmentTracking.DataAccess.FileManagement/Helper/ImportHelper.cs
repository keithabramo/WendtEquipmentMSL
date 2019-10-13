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
                    var table = getTableFromFile(stream);

                    //5. Data Reader methods
                    foreach (DataRow row in table.Rows)
                    {
                        //var item = row["ITEM"];
                        var quantity = row["QTY"];
                        var description = row["DESCRIPTION"];
                        var partNumber = row["PART NUMBER"]; //This is ship tag #
                        //var length = row["LENGTH"];
                        //var width = row["WIDTH"];
                        //var specification = row["SPECIFICATION"];
                        //var um = row["UM"];
                        var unitWeight = row["UNIT WT. (LBS)"];
                        //var totalWeight = row["TOTAL WT. (LBS)"];

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

        public static IEnumerable<WorkOrderPriceRow> GetWorkOrderPrices(string filePath)
        {
            var records = new List<WorkOrderPriceRow>();

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var table = getTableFromFile(stream);

                foreach (DataRow row in table.Rows)
                {
                    var workOrderNumber = row[0] != null ? row[0].ToString() : "";
                    var costPriceString = row[1] != null ? row[1].ToString().Replace("$", "").Replace(",", "") : "";
                    var salePriceString = row[2] != null ? row[2].ToString().Replace("$", "").Replace(",", "") : "";
                    var releasedPercentString = row[3] != null ? row[3].ToString().Replace("%", "") : ""; //percent columns come back as decimals between 0 - 1
                    var shippedPercentString = row[4] != null ? row[4].ToString().Replace("%", "") : ""; //percent columns come back as decimals between 0 - 1

                    double costPrice = 0;
                    if (!Double.TryParse(costPriceString, out costPrice))
                    {
                        costPrice = 0;
                    }

                    double salePrice = 0;
                    if (!Double.TryParse(salePriceString, out salePrice))
                    {
                        salePrice = 0;
                    }

                    double releasedPercent = 0;
                    if (!Double.TryParse(releasedPercentString, out releasedPercent))
                    {
                        releasedPercent = 0;
                    }
                    releasedPercent = releasedPercent * 100;

                    double shippedPercent = 0;
                    if (!Double.TryParse(shippedPercentString, out shippedPercent))
                    {
                        shippedPercent = 0;
                    }
                    shippedPercent = shippedPercent * 100;



                    var record = new WorkOrderPriceRow
                    {
                        WorkOrderNumber = workOrderNumber,
                        CostPrice = costPrice,
                        SalePrice = salePrice,
                        ReleasedPercent = releasedPercent, //percent columns come back as decimals between 0 - 1
                        ShippedPercent = shippedPercent //percent columns come back as decimals between 0 - 1
                    };

                    records.Add(record);
                }
            }
            return records;
        }

        public static IEnumerable<RawEquipmentRow> GetRawEquipment(string filePath)
        {
            var records = new List<RawEquipmentRow>();

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var table = getTableFromFile(stream);

                foreach (DataRow row in table.Rows)
                {
                    var equipmentName = row[0] != null ? row[0].ToString() : "";
                    var priorityNumberString = row[1] != null ? row[1].ToString() : "";
                    var releaseDateString = row[2] != null ? row[2].ToString() : "";
                    var drawingNumber = row[3].ToString();
                    var workOrderNumber = row[4].ToString();
                    var quantityString = row[5] != null ? row[5].ToString() : "";
                    var shippingTagNumber = row[6].ToString();
                    var description = row[7].ToString();
                    var unitWeightString = row[8] != null ? row[8].ToString() : "";
                    var readyToShipString = row[9] != null ? row[9].ToString() : "";
                    var shippedFrom = row[10] != null ? row[10].ToString() : "";
                    var htsCode = row[11] != null ? row[11].ToString() : "";
                    var countryOfOrigin = row[12] != null ? row[12].ToString() : "";
                    var notes = row[13] != null ? row[13].ToString() : "";

                    int? priorityNumber = null;
                    if (!string.IsNullOrEmpty(priorityNumberString))
                    {
                        int priorityNumberInt = 0;
                        if (!Int32.TryParse(priorityNumberString, out priorityNumberInt))
                        {
                            priorityNumber = null;
                        }
                        else
                        {
                            priorityNumber = priorityNumberInt;
                        }
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
            }

            return records;
        }

        public static IEnumerable<PriorityRow> GetPriorities(string filePath)
        {
            var records = new List<PriorityRow>();

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var table = getTableFromFile(stream);

                foreach (DataRow row in table.Rows)
                {
                    var priorityNumberString = row[0] != null ? row[0].ToString() : "";
                    var dueDateString = row[1] != null ? row[1].ToString() : "";
                    var endDateString = row[2] != null ? row[2].ToString() : "";
                    var contractualShipDateString = row[3].ToString();
                    var equipmentName = row[4].ToString();

                    int priorityNumber = 0;
                    if (!Int32.TryParse(priorityNumberString, out priorityNumber))
                    {
                        priorityNumber = 0;
                    }

                    DateTime dueDate = DateTime.Now;
                    if (!DateTime.TryParse(dueDateString, out dueDate))
                    {
                        dueDate = DateTime.Now;
                    }

                    DateTime endDate = DateTime.Now;
                    if (!DateTime.TryParse(endDateString, out endDate))
                    {
                        endDate = DateTime.Now;
                    }

                    DateTime contractualShipDate = DateTime.Now;
                    if (!DateTime.TryParse(contractualShipDateString, out contractualShipDate))
                    {
                        contractualShipDate = DateTime.Now;
                    }

                    var record = new PriorityRow
                    {
                        EquipmentName = equipmentName,
                        PriorityNumber = priorityNumber,
                        ContractualShipDate = contractualShipDate,
                        DueDate = dueDate,
                        EndDate = endDate
                    };

                    records.Add(record);
                }
            }

            return records;
        }

        public static IEnumerable<VendorRow> GetVendors(string filePath)
        {
            var records = new List<VendorRow>();

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var table = getTableFromFile(stream);

                foreach (DataRow row in table.Rows)
                {
                    var name = row[0] != null ? row[0].ToString() : "";
                    var address = row[1] != null ? row[1].ToString() : "";
                    var contact1 = row[2] != null ? row[2].ToString() : "";
                    var phoneFax = row[3] != null ? row[3].ToString() : "";
                    var email = row[4] != null ? row[4].ToString() : "";

                    var record = new VendorRow
                    {
                        Name = name,
                        PhoneFax = phoneFax,
                        Email = email,
                        Contact1 = contact1, 
                        Address = address 
                    };

                    records.Add(record);
                }
            }
            return records;
        }

        public static IEnumerable<BrokerRow> GetBrokers(string filePath)
        {
            var records = new List<BrokerRow>();

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var table = getTableFromFile(stream);

                foreach (DataRow row in table.Rows)
                {
                    var name = row[0] != null ? row[0].ToString() : "";
                    var address = row[1] != null ? row[1].ToString() : "";
                    var contact1 = row[2] != null ? row[2].ToString() : "";
                    var phoneFax = row[3] != null ? row[3].ToString() : "";
                    var email = row[4] != null ? row[4].ToString() : "";

                    var record = new BrokerRow
                    {
                        Name = name,
                        PhoneFax = phoneFax,
                        Email = email,
                        Contact1 = contact1,
                        Address = address
                    };

                    records.Add(record);
                }
            }
            return records;
        }

        public static IEnumerable<HardwareCommercialCodeRow> GetHardwareCommercialCodes(string filePath)
        {
            var records = new List<HardwareCommercialCodeRow>();

            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                var table = getTableFromFile(stream);

                foreach (DataRow row in table.Rows)
                {
                    var partNumber = row[0] != null ? row[0].ToString() : "";
                    var description = row[1] != null ? row[1].ToString() : "";
                    var commodityCode = row[2] != null ? row[2].ToString() : "";

                    var record = new HardwareCommercialCodeRow
                    {
                        PartNumber = partNumber,
                        Description = description,
                        CommodityCode = commodityCode
                    };

                    records.Add(record);
                }
            }
            return records;
        }


        private static DataTable getTableFromFile(FileStream stream)
        {
            var reader = ExcelReaderFactory.CreateReader(stream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            };

            var result = reader.AsDataSet(conf);

            return result.Tables[0];
        }

    }


}
