using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TestApplication.Data.Other
{
    public class JwtService
    {   

        private static string secret = "from zippy fabric snarl";

        private static SymmetricSecurityKey makeSymmetricSecurityKey() {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }

        public string Generate(int userId) {
            var symmetricSecurityKey = makeSymmetricSecurityKey();
            //generate key from secret
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            //sets JWT Header, tells browser we are using HMAC 256 for signing.
            //var payload = new JwtPayload(userId.ToString(),"",null,null,null, DateTime.Today.AddDays(1));
            var payload = new JwtPayload(userId.ToString(),audience:"",claims:new List<Claim>(),notBefore:null,expires: DateTime.Today.AddDays(1));
            var securityToken = new JwtSecurityToken(header,payload);
            //combines header and payload and appends a signature depending on the algo specified in header, in this case HMAC-SHA256

            return new JwtSecurityTokenHandler().WriteToken(securityToken);


        }

        public JwtSecurityToken Verify(string jwt) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken) validatedToken;
        }
    }
}
