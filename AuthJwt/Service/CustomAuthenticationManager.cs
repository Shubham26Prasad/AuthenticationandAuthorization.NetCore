using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AuthJwt.Service
{
    public interface ICustomAuthenticationManager
    {
        string Authenticate(string userName, string password);
        public IDictionary<string, string> Token { get; }
    }
    public class CustomAuthenticationManager : ICustomAuthenticationManager
    {
        public readonly IDictionary<string, string> user = new Dictionary<string, string> { {"shubham","password" },{"prasad","password" } };
        public readonly IDictionary<string, string> token = new Dictionary<string, string>();
        public IDictionary<string, string> Token => token;

        public string Authenticate(string userName, string password)
        {
            if(!user.Any(u=>u.Key==userName && u.Value == password)) { return null; }
            var generated_token = Guid.NewGuid().ToString();
            token.Add(generated_token, userName);   

            return generated_token;
        }
    }
}
