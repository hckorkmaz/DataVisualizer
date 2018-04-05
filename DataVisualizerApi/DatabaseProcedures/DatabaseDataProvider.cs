using DataVisualizerApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace DataVisualizerApi.DatabaseProcedures
{
    public class DatabaseDataProvider
    {
        /// <summary>
        /// Gets the data from database object for provided database configuration and object
        /// </summary>
        /// <param name="dataReciever"></param>
        /// <returns></returns>
        public static DataTable GetData(DatabaseDataReciever dataReciever)
        {
            bool objectExists = VerifyObject(dataReciever.DbObjectName, dataReciever.DatabaseConfig);

            if (!objectExists)
            {
                return new DataTable();
            }

            using (SqlConnection connection = new SqlConnection(dataReciever.DatabaseConfig.GetConnectionString))
            {
                string query = string.Empty;

                if (dataReciever.DbObjectType == DatabaseObjectType.TABLE
                        || dataReciever.DbObjectType == DatabaseObjectType.VIEW)
                {
                    query = string.Format(@"SELECT * FROM {0}", dataReciever.DbObjectName);
                }
                else if (dataReciever.DbObjectType == DatabaseObjectType.PROCEDURE)
                {
                    query = string.Format(@"EXEC {0}", dataReciever.DbObjectName);
                }
                else if (dataReciever.DbObjectType == DatabaseObjectType.FUNCTION)
                {
                    query = string.Format(@"SELECT * FROM {0}()", dataReciever.DbObjectName);
                }

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection))
                {
                    connection.Open();

                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add("DataTable");
                    dataAdapter.Fill(dataSet, "DataTable");

                    var dataTable = dataSet.Tables["DataTable"];
                    return dataTable;
                }
            }
        }

        /// <summary>
        /// Since parameters cannot be used as table name 
        /// using this method before select query to get data from database in order to prevent SQL injection.
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        private static bool VerifyObject(string objectName, DatabaseConfig databaseConfig)
        {
            // Build query with parameter in order to prevent SQL injection
            string queryVerifyObject = @"SELECT TOP 1 *
                                        FROM sys.objects
                                        WHERE object_id = OBJECT_ID(N'[dbo].[@objectName')";


            using (SqlConnection connection = new SqlConnection(databaseConfig.GetConnectionString))
            {
                using (SqlCommand command = new SqlCommand(null, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = queryVerifyObject;
                    command.Parameters.AddWithValue("@objectName", objectName.Trim());

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    var rowCounter = 0;
                    while (rowCounter < 1)
                    {
                        rowCounter++;
                    }

                    return (rowCounter == 1);
                }
            }
        }
    }
}