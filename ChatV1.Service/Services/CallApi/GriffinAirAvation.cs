using ChatV1.Service.Request;
using ChatV1.Service.Response;
using ChatV1.Service.Response.GetAllEmployeeForChat;
using ChatV1.Service.Response.GetEployeeDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RTools_NTS.Util;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ChatV1.Service.Services.CallApi
{
    public interface IGriffinAirAvation
    {
        public Task<GetEployeeDetailsResponse?> GetEployeeDetails(GetEployeeDetailsRequest request);

        public Task<GetAllEmployeeForChatResponse?> GetAllEmployeeForChat(GetUsersToChatRequest request);
        public Task<RequestTokenResponse?> RequestToken();
        public Task<GetAllEmployeesForChannelChatResponse> GetAllEmployeesForChannelChat();
        public string GetGriffinToken();
    }

    public class GriffinAirAvation : IGriffinAirAvation
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<GriffinAirAvation> _logger;


        public GriffinAirAvation(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IConfiguration configuration, IConnectionMultiplexer connectionMultiplexer, ILogger<GriffinAirAvation> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            this._httpClientFactory = httpClientFactory;
            this._configuration = configuration;
            _connectionMultiplexer = connectionMultiplexer;
            _logger = logger;

        }

        public async Task<GetEployeeDetailsResponse?> GetEployeeDetails(GetEployeeDetailsRequest request)
        {
            var userName = _configuration["GriffinUser:UserName"];
            var password = _configuration["GriffinUser:Password"];
            var token = "";
            if (_httpContextAccessor.HttpContext != null)
            {
                token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            }
            else
            {
                //var requestTokenRes = await RequestToken();
                //token = requestTokenRes.token;
                token = _connectionMultiplexer.GetDatabase(0).StringGet("IGriffinToken_ToEx");
                token = $"bearer {token}";

                int mm = 0;

            }
            var strObj = JsonConvert.SerializeObject(request);
            using HttpClient httpClient = _httpClientFactory.CreateClient("griffinApi");
            StringContent httpContent = new StringContent(strObj, System.Text.Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"{token}");

            var response = await httpClient.PostAsync("Employee/GetEmployeeDetails", httpContent);
            var resStr = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<GetEployeeDetailsResponse>(resStr);


            return res;
        }

        public string GetGriffinToken()
        {
            var token = _connectionMultiplexer.GetDatabase(0).StringGet("IGriffinToken_ToEx");
            return token;
        }

        public async Task<GetAllEmployeeForChatResponse?> GetAllEmployeeForChat(GetUsersToChatRequest request)
        {


            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var strObj = JsonConvert.SerializeObject(request);
            using HttpClient httpClient = _httpClientFactory.CreateClient("griffinApi");
            StringContent httpContent = new StringContent(strObj, System.Text.Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token.ToString());

            var response = await httpClient.PostAsync("Employee/GetAllEmployeeForChat", httpContent);
            var resStr = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<GetAllEmployeeForChatResponse>(resStr);


            return res;

        }

        public async Task<GetAllEmployeesForChannelChatResponse> GetAllEmployeesForChannelChat()
        {


            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            //var strObj = JsonConvert.SerializeObject(request);
            using HttpClient httpClient = _httpClientFactory.CreateClient("griffinApi");
            //StringContent httpContent = new StringContent(strObj, System.Text.Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token.ToString());

            var response = await httpClient.PostAsync("Employee/GetAllEmployeesForChannelChat",null);
            var resStr = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<GetAllEmployeesForChannelChatResponse> (resStr);


            return res;

        }

        public async Task<RequestTokenResponse?> RequestToken()
        {

            var userName = _configuration["GriffinUser:UserName"];
            var password = _configuration["GriffinUser:Password"];
            var request = new
            {
                username = userName,
                password = password,
                applicationType = 960,
                iP = "string"
            };
            _logger.LogInformation($"userName:{userName} , password:{password}");
            var strObj = JsonConvert.SerializeObject(request);
            _logger.LogInformation($"{strObj}");
            using HttpClient httpClient = _httpClientFactory.CreateClient("griffinApi");
            StringContent httpContent = new StringContent(strObj, System.Text.Encoding.UTF8, "application/json");
            _logger.LogInformation($"{JsonConvert.SerializeObject(httpContent)}");
            var response = await httpClient.PostAsync("Authentication/RequestToken", httpContent);
            var resStr = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<RequestTokenResponse>(resStr);
            return res;
        }

    }
}
