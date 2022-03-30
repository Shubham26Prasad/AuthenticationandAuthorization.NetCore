using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AuthJwt.Service
{
    public class BasicAuthenticationOptions : AuthenticationSchemeOptions { }
    public class CustomAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        public ICustomAuthenticationManager CustomAuthenticationManager { get; set; }
        public CustomAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ICustomAuthenticationManager customAuthenticationManager) : base(options, logger, encoder, clock)
        {
            this.CustomAuthenticationManager = customAuthenticationManager;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("UnAuthorized");
            }
            string tokenval = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(tokenval))
                return AuthenticateResult.Fail("UnAuthorized");
            if (!tokenval.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Fail("UnAuthorized");
            string tokenreturn = tokenval.Substring("Bearer".Length).Trim();
            if (String.IsNullOrEmpty(tokenreturn))
                return AuthenticateResult.Fail("UnAuthorized");
            try 
            {
                return validateToken(tokenreturn);
            }
            catch(Exception) {
                return AuthenticateResult.Fail("UnAuthorize");
            }
            

        }
        public AuthenticateResult validateToken(string tokenreturn)
        {
            var tokenval = CustomAuthenticationManager.Token.FirstOrDefault(t=>t.Key == tokenreturn);
            if (tokenval.Value == null)
                return AuthenticateResult.Fail("unAuthorized");
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, tokenval.Value) };
            var identity = new ClaimsIdentity(claims,Scheme.Name);
            var principal = new GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal,Scheme.Name);

            return AuthenticateResult.Success(ticket);


        }
    }
}
