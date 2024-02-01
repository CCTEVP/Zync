//using System.Threading.Tasks;
//using JWT;
//using Microsoft.AspNetCore.Mvc;

//namespace Zync.Api.Controllers
//{

//    public class UserController : Controller
//    {
//        private readonly JwtService _jwtService;

//        public UserController(JwtService jwtService)
//        {
//            _jwtService = jwtService;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login(LoginRequest request)
//        {
//            var user = await _userService.AuthenticateUser(request.Username, request.Password);
//            if (user == null)
//            {
//                return Unauthorized();
//            }

//            var token = _jwtService.GenerateToken(user.Username, "api", "web");
//            return Ok(new { token });
//        }
//    }
//}
