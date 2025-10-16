using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatV1.WebApi.Controllers
{
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SecutiryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SecutiryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: api/<SecutiryController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SecutiryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SecutiryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SecutiryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SecutiryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("GenerateToken")]
        public IActionResult GenerateToken(GenerateTokenReq request)
        {
            Random random = new Random();
            var randNum = random.Next();
            var userName = request.username;
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, randNum.ToString()));
            GenerateJwtTokenRequest generateJwtTokenRequest = new GenerateJwtTokenRequest { username = "", Claims = claims };
            var token = GenerateJwtToken(generateJwtTokenRequest);
            var res = new { userName = userName, token = token };
            //GenerateJwtToken()
            return Ok(res);
        }

        private string GenerateJwtToken(GenerateJwtTokenRequest request)
        {
            var jwtKey = _configuration["tokenManagement:secret"];
            var issuer = _configuration["tokenManagement:issuer"];
            var audience = _configuration["tokenManagement:audience"];
            var expiresInMinutes = double.Parse(_configuration["tokenManagement:accessExpiration"]);
            //var claims = new[]{
            //new Claim(JwtRegisteredClaimNames.Sub, request.username),
            //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            //};



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: request.Claims,
                expires: DateTime.Now.AddMinutes(expiresInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class GenerateJwtTokenRequest
    {
        public string username { get; set; }
        public List<Claim> Claims { get; set; }
    }

    public class GenerateTokenReq
    {
        public string username { get; set; }
    }
}
