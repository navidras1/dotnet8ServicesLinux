using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;

namespace ChatV1.WebApi.Middleware
{
    public class NLogMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var originalBodyStream = context.Response.Body;

            // Create new memory stream for reading the response; Response body streams are write-only, therefore memory stream is needed here to read
            await using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            var request = context.Request;
            if (request.Method == HttpMethods.Post && request.ContentLength > 0)
            {
                
                request.EnableBuffering();

                if (request.HasFormContentType)
                {
                    var fileInfo = new { fileSize = request.ContentLength , fileContentType= request.Form.Files[0].ContentType, fileName = request.Form.Files[0].FileName };
                    context.Items.Add("param", fileInfo);
                }
                else
                {
                    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                    await request.Body.ReadAsync(buffer, 0, buffer.Length);
                    //get body string here...
                    var requestContent = Encoding.UTF8.GetString(buffer);
                    context.Items.Add("param", requestContent);
                }

                request.Body.Position = 0;  //rewinding the stream to 0
            }

            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> items = identity.Claims;
                Claim claim = items.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    int pkEmployee = int.Parse(claim.Value);
                    context.Items.Add("pkemployee", pkEmployee);
                }
                //var userName = User.Identity.Name;
                var userName = identity.Name;
                context.Items.Add("userName", userName);
            }


            //Pass to the next middleware
            await next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);

            // Read the body from the stream
            var responseBodyText = await new StreamReader(memoryStream).ReadToEndAsync();
            context.Items.Add("response", responseBodyText);


            // Reset the position to 0 after reading
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Do this last, that way you can ensure that the end results end up in the response.
            // (This resulting response may come either from the redirected route or other special routes if you have any redirection/re-execution involved in the middleware.)
            // This is very necessary. ASP.NET doesn't seem to like presenting the contents from the memory stream.
            // Therefore, the original stream provided by the ASP.NET Core engine needs to be swapped back.
            // Then write back from the previous memory stream to this original stream.
            // (The content is written in the memory stream at this point; it's just that the ASP.NET engine refuses to present the contents from the memory stream.)
            context.Response.Body = originalBodyStream;
            await context.Response.Body.WriteAsync(memoryStream.ToArray());


        }
    }
}
