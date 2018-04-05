using DataVisualizerApi.DatabaseProcedures;
using DataVisualizerApi.Helper;
using DataVisualizerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataVisualizerApi.Controllers
{
    [Produces("application/json")]
    [Route("api/DatabaseObject")]
    public class DatabaseObjectController : Controller
    {
        /// <summary>
        /// Returns tables for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost("GetTables")]
        public IActionResult GetTables(DatabaseConfig databaseConfig)
        {
            if (ModelState.IsValid)
            {
                var tables = DatabaseObjectProvider.GetTables(databaseConfig);

                return Ok(tables.DataTableToList<DatabaseObject>());
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Returns procedures for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost("GetProcedures")]
        public IActionResult GetProcedures(DatabaseConfig databaseConfig)
        {
            if (ModelState.IsValid)
            {
                var procedures = DatabaseObjectProvider.GetProcedures(databaseConfig);

                return Ok(procedures.DataTableToList<DatabaseObject>());
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Returns functions for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost("GetFunctions")]
        public IActionResult GetFunctions(DatabaseConfig databaseConfig)
        {
            if (ModelState.IsValid)
            {
                var procedures = DatabaseObjectProvider.GetFunctions(databaseConfig);

                return Ok(procedures.DataTableToList<DatabaseObject>());
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Returns views for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost("GetViews")]
        public IActionResult GetViews(DatabaseConfig databaseConfig)
        {
            if (ModelState.IsValid)
            {
                var views = DatabaseObjectProvider.GetViews(databaseConfig);

                return Ok(views.DataTableToList<DatabaseObject>());
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Returns all database objects for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost("GetAll")]
        public IActionResult GetAll(DatabaseConfig databaseConfig)
        {
            if (ModelState.IsValid)
            {
                var tables = DatabaseObjectProvider.GetTables(databaseConfig);
                var views = DatabaseObjectProvider.GetViews(databaseConfig);
                var functions = DatabaseObjectProvider.GetFunctions(databaseConfig);
                var procedures = DatabaseObjectProvider.GetProcedures(databaseConfig);

                var objects = tables.Copy();
                objects.Merge(views);
                objects.Merge(functions);
                objects.Merge(procedures);

                var result = objects.DataTableToList<DatabaseObject>();
                return Ok(result);
            }

            return BadRequest(ModelState);
        }
    }
}
