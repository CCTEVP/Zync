
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Zync.Api
//{
//    public class JwtIssuer
//    {
//        private readonly string _secretKey;
//        private readonly SigningCredentials _signingCredentials;

//        public JwtIssuer(string secretKey)
//        {
//            _secretKey = secretKey;
//            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256);
//        }

//        public string GenerateToken(string subject, string issuer, string audience, DateTime expiresAt, Claim[] claims = null)
//        {
//            if (claims == null)
//            {
//                claims = new Claim[] {
//                new Claim("sub", subject),
//                new Claim("iss", issuer),
//                new Claim("aud", audience),
//                new Claim("exp", expiresAt.ToUniversalTime().ToString("o"))
//            };
//            }

//            var token = new JwtSecurityToken(
//                _signingCredentials,
//                claims,
//                null,
//                expiresAt,
//                null,
//                issuer,
//                audience
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}