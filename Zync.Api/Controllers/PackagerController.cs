using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Zync.Api.Models.Input;
using Zync.Api.Models.Output;
using Zync.Api.Middleware;


namespace Zync.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class PackageCreativeController : ControllerBase
    {
        [HttpPost(Name = "packageCreative")]
        public IActionResult ProcessData([FromBody] Packager bodyParameters)
        {
            MongoConnection connection = new MongoConnection(bodyParameters.Country);
            Creative document = connection.filterCreativeById(bodyParameters.CreativeId);

            Response response = new Response();
            response.Result = document.Name;
            response.TS = DateTime.Now;
            return Ok(response.ToString());
        }
    }
}
