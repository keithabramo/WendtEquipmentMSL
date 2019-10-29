using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.Visual.Api;
using WendtEquipmentTracking.DataAccess.Visual.Domain;

namespace WendtEquipmentTracking.DataAccess.Visual
{
    public class HTSCodeEngine : IHTSCodeEngine
    {
        private ILog logger = LogManager.GetLogger("File");

        private readonly string connectionString;

        public HTSCodeEngine()
        {
            connectionString = ConfigurationManager.ConnectionStrings["Visual"].ConnectionString;
        }

        public IEnumerable<HTSCodeDTO> ListAll()
        {
            var htsCodes = new List<HTSCodeDTO>();

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT HTS_Code, Description FROM HARMONIZED_TARIFF", connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                htsCodes.Add(new HTSCodeDTO {
                                    HTSCode = reader[0].ToString(), 
                                    Description = reader[1].ToString()
                                });
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
