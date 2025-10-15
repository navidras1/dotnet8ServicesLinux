using ChatV1.Service.Request.MessageToChatRoom;
using ChatV1.Service.Response;
using ChatV1.Service.Response.PrivateChatSocketIO;
using ChatV1.Service.Services.CallApi;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Services.SocketIo
{
    public interface IRobotChat
    {

        Task<PrivateChatSocketIOResponse> MessageToChatRoomIOAsRobot(MessageToChatRoomIORequest request);
        public Task<ConnectToChatAsRobotResponse> ConnectToChatAsRobotAsync();
    }

    public class RobotChat : IRobotChat
    {
        private ILogger<RobotChat> _logger;
        private readonly IGriffinAirAvation _griffinAirAvation;
        private SocketIOClient.SocketIO _socketIO;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public RobotChat(ILogger<RobotChat> logger, IGriffinAirAvation griffinAirAvation, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _griffinAirAvation = griffinAirAvation;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            var res = ConnectToChatAsRobotAsync().Result;
        }
        public async Task<ConnectToChatAsRobotResponse> ConnectToChatAsRobotAsync()
        {
            ConnectToChatAsRobotResponse result = new();

            try
            {
                var token = _griffinAirAvation.GetGriffinToken();
                if (!token.StartsWith("bearer "))
                {
                    token = $"bearer {token}";
                }
                var serverAddress = "";
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    serverAddress = _configuration["ChatSocketIO:HostDevelope"].ToString();
                }
                else
                {
                    serverAddress = _configuration["ChatSocketIO:Host"].ToString();
                }

                _socketIO = new SocketIOClient.SocketIO($"{serverAddress}/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", token } } });
                _socketIO.OnConnected += async (sender, e) => { };

                //await _socketIO.ConnectAsync();
                var task = Task.Run(() => _socketIO.ConnectAsync());

                _socketIO.On("myInfo", response =>
                {
                    result.ResponseMessages.Add(response.ToString());
                });
                await Task.Delay(500);

                if (task.Wait(TimeSpan.FromSeconds(10)))
                {
                    result.Message = "Connected";
                }
                else
                {
                    _logger.LogError("SocketIO connection takes more than 10 seconds");
                    result.Message = "Not Connected";
                    result.IsSuccess = false;
                }

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return result;
        }
        async public Task<PrivateChatSocketIOResponse> MessageToChatRoomIOAsRobot(MessageToChatRoomIORequest request)
        {



            PrivateChatSocketIOResponse result = new();
            ConnectToChatAsRobotResponse conres = new();
            //lock (conres)
            //{



            if (!_socketIO.Connected)
            {
                for (int i = 0; i < 10; i++)
                {
                    conres = await ConnectToChatAsRobotAsync();
                    if (_socketIO.Connected)
                    {
                        break;
                    }
                }
            }
            if (conres.IsSuccess == false)
            {
                result.Message = conres.Message;
                result.IsSuccess = false;
                return result;
            }


            try
            {
                var httpClient = _httpClientFactory.CreateClient("ChatSocketIO");
                var token = _griffinAirAvation.GetGriffinToken();
                if (!token.StartsWith("bearer "))
                {
                    token = $"bearer {token}";
                }

                string ApiAddress = httpClient.BaseAddress + _configuration["ChatSocketIO:MessageToChatRoom"].ToString();


                var strObj = JsonConvert.SerializeObject(request);


                StringContent httpContent = new StringContent(strObj, System.Text.Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token.ToString());
                var response = httpClient.PostAsync(ApiAddress, httpContent).Result;
                var resStr = response.Content.ReadAsStringAsync().Result;
                var res = JsonConvert.DeserializeObject<PrivateChatSocketIOResponse>(resStr);
                result.status = res.status;
                result.result = res.result;
            }
            catch (Exception ex)
            {

                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            //}

            return result;

        }


    }
}
