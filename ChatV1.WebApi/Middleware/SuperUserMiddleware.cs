
using ChatV1.DataAccess.Models;
using ChatV1.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;

namespace ChatV1.WebApi.Middleware
{
    public class SuperUserMiddleware : IMiddleware
    {
        private readonly IChatV1Repository<EmpMaster> _empMaster;
        private readonly IChatV1Repository<SuperUserApi> _superUserApi;

        private readonly ILogger<SuperUserMiddleware> _logger;

        public SuperUserMiddleware(IChatV1Repository<EmpMaster> empMaster, ILogger<SuperUserMiddleware> logger, IChatV1Repository<SuperUserApi> superUserApi)
        {
            _empMaster = empMaster;
            _logger = logger;
            _superUserApi = superUserApi;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var endpoint = context.GetEndpoint();
            var ends = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await next(context);
                return;
            }

            var userName = context.User.Identity.Name;


            var path = context.Request.Path.Value;


            var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor.ControllerName;
            
            var foundConroller = _superUserApi.Exists(x=> x.ControllerName == controllerName);
            if (foundConroller == true) {
                
                var founduser = _empMaster.Exists(x=> x.UserName == userName && x.IsSuperUser == true); 
                if (founduser == false)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Headers.Add("SuperUser", "User is not a super user");
                    await context.Response.WriteAsync("not a super user");
                    return;
                }
                    
            }


            var foundPath = _superUserApi.Exists(x => x.ApiPath == path);
            if (foundPath == true)
            {
                var foundUser = _empMaster.Find(x => x.UserName==userName.ToLower() && x.IsSuperUser == true).FirstOrDefault();
                if (foundUser == null)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Headers.Add("SuperUser", "User is not a super user");
                    await context.Response.WriteAsync("not a super user");
                    return;
                }
            }


            

            await next(context);
            //return Task.CompletedTask;
        }
    }
}
