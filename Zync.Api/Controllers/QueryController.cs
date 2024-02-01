using Microsoft.AspNetCore.Mvc;
using Zync.Api.Models.Output;
using Zync.Api.Middleware;

namespace Zync.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueryCreativeController : ControllerBase
    {
        [HttpGet(Name = "queryCreative")]
        public IActionResult queryCreative([FromQuery] string creativeId)
        {
            Response.ContentType = "application/json";

            MongoConnection connection = new MongoConnection("Spain");
            Creative document = connection.filterCreativeById(creativeId);//
            return Ok(document);
        }

    }
}
