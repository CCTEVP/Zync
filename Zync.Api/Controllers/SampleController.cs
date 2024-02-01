using Microsoft.AspNetCore.Mvc;
using Zync.Api.Models.Input;

namespace Zync.API.Controllers
{
    [ApiController]
    [Route("sample/post")]
    public class SamplePostController : ControllerBase
    {
        [HttpPost(Name = "postParameter")]
        public IActionResult PostSampleParameter([FromBody] Sampler requestModel)
        {
            return Ok("Data transferred successfully!");
        }
    }
    [ApiController]
    [Route("sample/get")]
    public class SampleGetController : ControllerBase
    {
        [HttpGet(Name = "getParameter")]
        public IActionResult GetSampleParameter([FromQuery] string ParameterOne, string ParameterTwo = "test2", string ParameterThree = "test3")
        {
            return Ok("Data retrieved successfully!");
        }
    }
    [ApiController]
    [Route("sample/route")]
    public class SampleRouteController : ControllerBase
    {
        [HttpGet("{routeParameter}")]
        public IActionResult RouteSampleParameter(int parameterId)
        {
            return Ok($"Product details for ID {parameterId}");
        }
    }
}