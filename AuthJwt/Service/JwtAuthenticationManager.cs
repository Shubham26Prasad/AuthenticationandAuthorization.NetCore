using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System;

namespace AuthJwt.Service
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {

        private readonly IDictionary<string, string> users = new Dictionary<string, string> { {"shubham","password" }, {"prasad", "prasad" } };
        private readonly string _key;

        public JwtAuthenticationManager(string key)
        {
            _key = key;
        }
        public  string Authenticate(string username, string password)
        {
            if (!users.Any(u => u.Key == username && u.Value == password))
                return null;
            else
            {
                //Jwt Token Handler class
                var tokenHandler = new JwtSecurityTokenHandler();
                //key which is used to decrypt token
                var tokenKey = Encoding.ASCII.GetBytes(_key);
                //Token Description which later we can see in jwt.io
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username)
                        
                        
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha384Signature)

                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);


            }
        }
    }
}
