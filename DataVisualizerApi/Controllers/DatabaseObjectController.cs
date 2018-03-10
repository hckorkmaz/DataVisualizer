using DataVisualizerApi.DatabaseProcedures;
using DataVisualizerApi.Helper;
using DataVisualizerApi.Models;
using DataVisualizerAPI.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataVisualizerAPI.Controllers
{
    public class DatabaseObjectController : ApiController
    {
        /// <summary>
        /// Returns tables for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetTables(DatabaseConfig databaseConfig)
        {
            if (ModelState.IsValid)
            {
                var tables = DatabaseObjectProvider.GetTables(databaseConfig);

                return Request.CreateResponse(HttpStatusCode.OK, tables.DataTableToList<DatabaseObject>());
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Returns procedures for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetProcedures(DatabaseConfig databaseConfig)
        {
            if (ModelState.IsValid)
            {
                var procedures = DatabaseObjectProvider.GetProcedures(databaseConfig);

                return Request.CreateResponse(HttpStatusCode.OK, procedures.DataTableToList<DatabaseObject>());
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Returns functions for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetFunctions(DatabaseConfig databaseConfig)
        {
            if (ModelState.IsValid)
            {
                var procedures = DatabaseObjectProvider.GetFunctions(databaseConfig);

                return Request.CreateResponse(HttpStatusCode.OK, procedures.DataTableToList<DatabaseObject>());
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Returns views for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetViews(DatabaseConfig databaseConfig)
        {
            if (ModelState.IsValid)
            {
                var views = DatabaseObjectProvider.GetViews(databaseConfig);

                return Request.CreateResponse(HttpStatusCode.OK, views.DataTableToList<DatabaseObject>());
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Returns all database objects for given database configuration.
        /// </summary>
        /// <param name="databaseConfig"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetAll(DatabaseConfig databaseConfig)
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

                return Request.CreateResponse(HttpStatusCode.OK, objects.DataTableToList<DatabaseObject>());
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
