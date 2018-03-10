using DataVisualizerApi.DatabaseProcedures;
using DataVisualizerAPI.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataVisualizerAPI.Controllers
{
    public class DatabaseDataController : ApiController
    {
        /// <summary>
        /// Gets the data from the database that has been given inside of the model.
        /// </summary>
        /// <param name="dataReciever"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GetData(DatabaseDataReciever dataReciever)
        {
            if (ModelState.IsValid)
            {
                var tables = DatabaseDataProvider.GetData(dataReciever);

                return Request.CreateResponse(HttpStatusCode.OK, tables);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
