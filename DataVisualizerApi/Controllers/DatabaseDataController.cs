using DataVisualizerApi.DatabaseProcedures;
using DataVisualizerApi.Helper;
using DataVisualizerApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml.Linq;

namespace DataVisualizerApi.Controllers
{
    [Produces("application/json")]
    [Route("api/DatabaseData")]
    public class DatabaseDataController : Controller
    {
        /// <summary>
        /// Gets the data from the database that has been given inside of the model.
        /// </summary>
        /// <param name="dataReciever"></param>
        /// <returns></returns>
        [HttpPost("GetData")]
        public IActionResult GetData(DatabaseDataReciever dataReciever)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var tables = DatabaseDataProvider.GetData(dataReciever);
                    var nullFilledTable = Helper.Helper.GetNullFilledDataTableForXML(tables);

                    var xml = new XDocument();
                    using (var writer = xml.CreateWriter())
                    {
                        nullFilledTable.WriteXml(writer);
                        writer.Flush();
                    }

                    return Ok(Json(xml));
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest(ModelState.GetModelStateErrors());
        }
    }
}
