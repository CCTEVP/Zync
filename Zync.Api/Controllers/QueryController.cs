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
            Creative document = connection.filterCreativeById(creativeId);
            return Ok(document);
        }
    }

    [Route("api/[controller]")]
    public class QueryCampaignController : ControllerBase
    {
        [HttpGet(Name = "queryCampaign")]
        public IActionResult queryCampaign([FromQuery] string campaignId)
        {
            Response.ContentType = "application/json";
            MongoConnection connection = new MongoConnection("Spain");
            Campaign document = connection.filterCampaignById(campaignId);
            return Ok(document);
        }

    }

}
