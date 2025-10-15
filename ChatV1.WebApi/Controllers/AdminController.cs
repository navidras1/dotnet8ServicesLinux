using Asp.Versioning;
using ChatV1.Service.Model.Admin;
using ChatV1.Service.Request;
using ChatV1.Service.Services;
using ChatV1.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatV1.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }





        // GET: api/<AdminController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<AdminController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}


        // POST api/<AdminController>
        [HttpPost("GetAllRoomMessages")]
        public ActionResult GetAllRoomMessagesV1()
        {
            return Ok();
        }

        
        [HttpPost("CreateNoticeChannelAndSupportGroup")]
        public async Task<ActionResult> CreateNoticeChannelAndSupportGroup()
        {
            var userName = User.Identity.Name.ToLower();
            if (_adminService.IsSuperUser(userName)==true)
            {

                var res = await _adminService.CreateNoticeChannelAndSupportGroup(new Service.Model.Admin.CreateNoticeChannelAndSupportGroupRequest { CreatorUserName = userName });
                return Ok(res);
            }
            else
            {
                return Unauthorized("User is not a SuperUser");
            }
        }


        [HttpPost("CanSendMessageToRoom")]
        public ActionResult CanSendMessageToRoom(IsUserRoomAdminRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            request.userName = userName;
            var res =  _adminService.CanSendMessageToRoom(request);
            return Ok(res);
        }

        [HttpPost("CanSendMessageToRoom"), ApiVersion("2.0")]
        public ActionResult CanSendMessageToRoomV2(IsUserRoomAdminRequestV2 request)
        {
            var userName = User.Identity.Name.ToLower();
            request.userName = userName;
            var res = _adminService.CanSendMessageToRoomV2(request);
            return Ok(res);
        }

        [HttpPost("AddUserFromGriffin")]
        public ActionResult AddUserFromGriffin(AddUserFromGriffinRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            if (_adminService.IsSuperUser(userName) == true)
            {
                var res =  _adminService.AddUserFromGriffin(request);

                return Ok(res);
            }
            else
            {
                return Unauthorized("User is not a SuperUser");
            }
        }

        [HttpPost("DeleteFromGriffin")]
        public ActionResult DeleteUserFromGriffin(AddUserFromGriffinRequest request)
        {
            var userName = User.Identity.Name.ToLower();
            if (_adminService.IsSuperUser(userName) == true)
            {
                var res = _adminService.AddUserFromGriffin(request);

                return Ok(res);
            }
            else
            {
                return Unauthorized("User is not a SuperUser");
            }
        }

        // PUT api/<AdminController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AdminController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
