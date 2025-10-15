using Asp.Versioning;
using ChatV1.Service.Request;
using ChatV1.Service.Services;
using ChatV1.Service.Services.CallApi;
using ChatV1.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatV1.WebApi.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class BackendUtilityController : ControllerBase
    {

        private IActions _actions;
        private IGriffinAirAvation _griffinAirAvation;

        public BackendUtilityController(IActions actions, IGriffinAirAvation griffinAirAvation)
        {
            _actions = actions;
            _griffinAirAvation = griffinAirAvation;
        }

        [HttpPost("GetMastersOfContact")]
        public IActionResult GetMastersOfContact()
        {
            var userName = User.Identity.Name;
            GetMastersOfContactRequest request = new GetMastersOfContactRequest { UserName = userName };
            var res = _actions.GetMastersOfContact(request);
            return Ok(res);
        }

        [HttpPost("GetEmployeeDetail")]
        public async Task<IActionResult> GetEmployeeDetail(GetEployeeDetailsRequest request)
        {
            var res = await _griffinAirAvation.GetEployeeDetails(request);
            return Ok(res);
        }

        [HttpPost("CountOfUnreadMessage")]
        public IActionResult CountOfUnreadMessage(CountOfUnreadMessageRequestForUser request)
        {
            var res =  _actions.CountOfUnreadMessage(request.FromUserName, request.ToUserName);
            return Ok(res);
        }

        

    }
}
