using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.Visual.Api;

namespace WendtEquipmentTracking.DataAccess.Visual
{
    public class HTSCodeEngine : IHTSCodeEngine
    {
        private ILog logger = LogManager.GetLogger("File");

        private readonly string connectionString;

        public HTSCodeEngine()
        {
            connectionString = new SqlConnectionStringBuilder()
            {
                DataSource = "wal-visual.wendtcorp.local",
                InitialCatalog = "VMFG",
                UserID = @"wendtcorp\MSL",
                Password = @"LCrMQc9g\}*;Y9Eb"
            }.ConnectionString;
        }

        public IEnumerable<string> ListAll()
        {
            var htsCodes = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT HTSCode, Description FROM HARMONIZED_TARIFF", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                htsCodes.Add(String.Format("{0} - {1}", reader[0], reader[1]));
                            }
                        }
                    }
                    connection.Close();
                }
            } 
            catch (Exception ex)
            {
                logger.Error("Visual Error " + ActiveDirectoryHelper.CurrentUserUsername() + ": ", ex);
            }

            return htsCodes;
        }
    }
}
