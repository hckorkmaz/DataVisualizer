using DataVisualizerApi.Models;
using DataVisualizerAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace DataVisualizerApi.DatabaseProcedures
{
    public class DatabaseObjectProvider
    {
        /// <summary>
        /// Returns list of tables in a data table for given connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable GetTables(DatabaseConfig databaseConfig)
        {
            using (SqlConnection connection = new SqlConnection(databaseConfig.GetConnectionString))
            {
                connection.Open();
                var dataTable = connection.GetSchema("Tables", new string[4] { null, null, null, DatabaseObjectType.TABLE });
                dataTable.Columns["TABLE_CATALOG"].ColumnName = "Database";
                dataTable.Columns["TABLE_SCHEMA"].ColumnName = "Schema";
                dataTable.Columns["TABLE_NAME"].ColumnName = "Name";
                dataTable.Columns["TABLE_TYPE"].ColumnName = "Type";

                return dataTable;
            }
        }

        /// <summary>
        /// Returns list of views in a data table for given connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable GetViews(DatabaseConfig databaseConfig)
        {
            using (SqlConnection connection = new SqlConnection(databaseConfig.GetConnectionString))
            {
                connection.Open();
                var dataTable = connection.GetSchema("Tables", new string[4] { null, null, null, DatabaseObjectType.VIEW });
                dataTable.Columns["TABLE_CATALOG"].ColumnName = "Database";
                dataTable.Columns["TABLE_SCHEMA"].ColumnName = "Schema";
                dataTable.Columns["TABLE_NAME"].ColumnName = "Name";
                dataTable.Columns["TABLE_TYPE"].ColumnName = "Type";

                return dataTable;
            }
        }

        /// <summary>
        /// Returns list of procedures in a data table for given database config
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable GetProcedures(DatabaseConfig databaseConfig)
        {
            using (SqlConnection connection = new SqlConnection(databaseConfig.GetConnectionString))
            {
                connection.Open();
                var dataTable =  connection.GetSchema("Procedures", new string[4] { null, null, null, DatabaseObjectType.PROCEDURE });

                dataTable.Columns["ROUTINE_CATALOG"].ColumnName = "Database";
                dataTable.Columns["ROUTINE_SCHEMA"].ColumnName = "Schema";
                dataTable.Columns["ROUTINE_NAME"].ColumnName = "Name";
                dataTable.Columns["ROUTINE_TYPE"].ColumnName = "Type";

                return dataTable;
            }
        }

        /// <summary>
        /// Returns list of functions in a data table for given database config
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DataTable GetFunctions(DatabaseConfig databaseConfig)
        {
            using (SqlConnection connection = new SqlConnection(databaseConfig.GetConnectionString))
            {
                connection.Open();
                var dataTable = connection.GetSchema("Procedures", new string[4] { null, null, null, DatabaseObjectType.FUNCTION });

                dataTable.Columns["ROUTINE_CATALOG"].ColumnName = "Database";
                dataTable.Columns["ROUTINE_SCHEMA"].ColumnName = "Schema";
                dataTable.Columns["ROUTINE_NAME"].ColumnName = "Name";
                dataTable.Columns["ROUTINE_TYPE"].ColumnName = "Type";

                return dataTable;
            }
        }
    }
}