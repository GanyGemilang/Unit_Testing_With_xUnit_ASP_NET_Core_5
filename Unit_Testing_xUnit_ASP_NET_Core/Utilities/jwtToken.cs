using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Unit_Testing_xUnit_ASP_NET_Core.Utilities
{
    public class jwtToken
    {
        private IConfiguration configuration;
        public jwtToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public dynamic jwt(string username)
        {
            
            var claims = new List<Claim>
            {
                new Claim("Username", username),
            };

            var timejwt = Utilities.Time.timezone(DateTime.Now).AddMinutes(15);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token1 = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: timejwt, signingCredentials: signIn);

            var Token = new JwtSecurityTokenHandler().WriteToken(token1);
            return new { Token, timejwt };
        }
    }
}
