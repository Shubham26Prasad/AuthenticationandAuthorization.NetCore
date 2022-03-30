using AuthJwt.Models;
using AuthJwt.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthJwt.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {

        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly ICustomAuthenticationManager customAuthenticationManager;
        //public SampleController(IJwtAuthenticationManager jwtAuthenticationManager)
        //{
        //    _jwtAuthenticationManager = jwtAuthenticationManager;
        //}
        public SampleController(ICustomAuthenticationManager customAuthenticationManager)
        {
            this.customAuthenticationManager = customAuthenticationManager;
        }
        // GET: api/<SampleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SampleController>/5
        [HttpGet("{id}")]
        public string[] Get(int id)
        {
            string[] returnvalues =  { "Demo", "Authentication" };
            return returnvalues;
        }
        [AllowAnonymous]
        // POST api/<SampleController>
        [HttpPost("Authentication")]
        public IActionResult Token([FromBody] UserCreds usercred)
        {
            var token = _jwtAuthenticationManager.Authenticate(usercred.username, usercred.password);
            if (token != null)
            {
                return Ok(token);
            }
            return Unauthorized();
        }
        [AllowAnonymous]
        [HttpPost("CustomAuthentication")]
        public IActionResult Tokenz([FromBody] UserCreds usercred)
        {
            var token = customAuthenticationManager.Authenticate(usercred.username, usercred.password);
            if (token != null)
            {
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}
