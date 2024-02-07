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
        //[ApiExplorerSettings(GroupName = "schooljob")]
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
        //[ApiExplorerSettings(GroupName = "hospitaljob")]
        public IActionResult queryCampaign([FromQuery] string campaignId)
        {
            Response.ContentType = "application/json";
            MongoConnection connection = new MongoConnection("Spain");
            Campaign document = connection.filterCampaignById(campaignId);
            return Ok(document);
        }

    }
    [Route("api/[controller]")]
    public class QueryFormatController : ControllerBase
    {

        [HttpGet(Name = "queryFormat")]
        public IActionResult queryFormat([FromQuery] string formatId)
        {
            Response.ContentType = "application/json";
            MongoConnection connection = new MongoConnection("Spain");
            Format document = connection.filterFormatById(formatId);
            return Ok(document);
        }

    }

    [Route("api/[controller]")]
    public class QueryPlayerController : ControllerBase
    {

        [HttpGet(Name = "queryPlayer")]
        public IActionResult queryPlayer([FromQuery] string playerId)
        {
            Response.ContentType = "application/json";
            MongoConnection connection = new MongoConnection("Spain");
            Player document = connection.filterPlayerById(playerId);
            return Ok(document);
        }

    }

    //[Route("api/[controller]")]
    //public class QueryPlayersController : ControllerBase
    //{

    //    [HttpGet(Name = "queryPlayers")]

    //    public IEnumerable<Players> queryPlayersList()
    //    {
    //        Response.ContentType = "application/json";
    //        MongoConnection connection = new MongoConnection("Spain");
    //        Players document = connection.getAllPlayers();
    //        return Ok(document);
    //    }



    //    public IActionResult queryPlayersList()
    //    {
    //        Response.ContentType = "application/json";
    //        MongoConnection connection = new MongoConnection("Spain");
    //        Players document = connection.getAllPlayers();
    //        return Ok(document);
    //    }

    //}

}
