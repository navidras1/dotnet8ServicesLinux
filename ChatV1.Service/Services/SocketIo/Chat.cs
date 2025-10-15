using ChatV1.DataAccess.Models;
using ChatV1.DataAccess.Repository;
using ChatV1.Service.Model;
using ChatV1.Service.Request;
using ChatV1.Service.Request.MessageToChatRoom;
using ChatV1.Service.Request.PrivateChatSocketIO;
using ChatV1.Service.Response;
using ChatV1.Service.Response.PrivateChatSocketIO;
using ChatV1.Service.Services.CallApi;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatV1.Service.Services.SocketIo
{
    public interface IChat
    {
       public Task<PrivateChatResponse> PrivateChat(PrivateChatMessageRequest request);
        public Task<SendMessaageToRoomResponse> SendMessaageToRoom(SendMessaageToRoomRequest request);
        public Task<SendMessaageToRoomResponse> SendMessaageFlightChannel(SendMessaageToRoomRequest request);
        public Task<PrivateChatResponse> PrivateChatBulkMessages(PrivateChatMessageRequest request);
        public Task<PrivateChatSocketIOResponse> PrivateChatSocketIO(PrivateChatSocketIORequest request);
        public Task<PrivateChatSocketIOResponse> MessageToChatRoomIO(MessageToChatRoomIORequest request);
        public Task<PrivateChatResponse> PrivateChatToUsers(PrivateChatMessageToUsersRequest request);
    }

    public class Chat : IChat
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGriffinAirAvation _griffinAirAvation;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IChatV1Repository<ChatLog> _chatLog;

        public Chat(IHttpContextAccessor httpContextAccessor, IGriffinAirAvation griffinAirAvation, IConfiguration configuration, IHttpClientFactory httpClientFactory, IChatV1Repository<ChatLog> chatLog)
        {
            _httpContextAccessor = httpContextAccessor;
            _griffinAirAvation = griffinAirAvation;
            this._configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _chatLog = chatLog;
        }

        public async Task<SendMessaageToRoomResponse> SendMessaageToRoom(SendMessaageToRoomRequest request)
        {
            SendMessaageToRoomResponse roomMessageResponse = new();

            MessageToChatRoom messageToChatRoom = new();
            messageToChatRoom.data = new()
            {
                attachmentId = request.attachmentId, 
                chatRoomName = request.chatRoomName,
                clientTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                isRtl = request.isRtl,
                message = request.message
            };

            //var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var token = _griffinAirAvation.GetGriffinToken();
            if(!token.StartsWith("bearer "))
            {
                token = $"bearer {token}";
            }
            using var client = new SocketIOClient.SocketIO("http://localhost:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", token } } });
            //using var client = new SocketIOClient.SocketIO("http://192.168.40.60:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiTi5SYXNvdWxpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIyNzkwIiwicGtFbXBsb3llZVNlc3Npb24iOiIyODQ0NTQxIiwiZXhwIjoxNzIwNTExNTI2LCJpc3MiOiJmbHlzZXBlaHJhbi5pciIsImF1ZCI6ImZseXNlcGVocmFuLlNlY3VyaXR5LkJlYXJlciJ9.O-bsyHavwPx6F5WPN4GlrhDw5NhUgwVUzwqSO6O8JlI" } } });
            SocketIOClient.SocketIOResponse rr = null;

            client.OnConnected += async (sender, e) => { };
            await client.ConnectAsync();

            client.On("myInfo", response =>
            {
                roomMessageResponse.ResponseMessage.Add(response.ToString());
            });

            await Task.Delay(500);

            await client.EmitAsync("chat", response =>
            {
                //Thread.Sleep(3000);
                rr = response;
                roomMessageResponse.ResponseMessage.Add(response.ToString());
                rr = null;
            }, messageToChatRoom);

            await Task.Delay(1000);
            return roomMessageResponse;
        }

        public async Task<SendMessaageToRoomResponse> SendMessaageFlightChannel(SendMessaageToRoomRequest request)
        {
            SendMessaageToRoomResponse roomMessageResponse = new();

            MessageToChatRoom messageToChatRoom = new();
            messageToChatRoom.data = new()
            {
                attachmentId = request.attachmentId,
                chatRoomName = "Flight_Channel",
                clientTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                isRtl = request.isRtl,
                message = request.message
            };

            //var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var token = _griffinAirAvation.GetGriffinToken();
            if (!token.StartsWith("bearer "))
            {
                token = $"bearer {token}";
            }
            using var client = new SocketIOClient.SocketIO("http://192.168.40.60:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", token } } });
            //using var client = new SocketIOClient.SocketIO("http://192.168.40.60:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiTi5SYXNvdWxpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIyNzkwIiwicGtFbXBsb3llZVNlc3Npb24iOiIyODQ0NTQxIiwiZXhwIjoxNzIwNTExNTI2LCJpc3MiOiJmbHlzZXBlaHJhbi5pciIsImF1ZCI6ImZseXNlcGVocmFuLlNlY3VyaXR5LkJlYXJlciJ9.O-bsyHavwPx6F5WPN4GlrhDw5NhUgwVUzwqSO6O8JlI" } } });
            SocketIOClient.SocketIOResponse rr = null;

            client.OnConnected += async (sender, e) => { };
            await client.ConnectAsync();

            client.On("myInfo", response =>
            {
                roomMessageResponse.ResponseMessage.Add(response.ToString());
            });

            await Task.Delay(500);

            await client.EmitAsync("chat", response =>
            {
                //Thread.Sleep(3000);
                rr = response;
                roomMessageResponse.ResponseMessage.Add(response.ToString());
                rr = null;
            }, messageToChatRoom);

            await Task.Delay(1000);
            return roomMessageResponse;
        }

        public async Task<PrivateChatResponse> PrivateChat(PrivateChatMessageRequest request)
        {

            PrivateChatResponse privateChatResponse = new();


            PrivateChatMessage privateChat = new();
            PrivateChatData data = new();

            data.isRtl = request.IsRtl;
            data.to = request.ToUserName;



            privateChat.data = data;

            try
            {
                //var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                var token = _griffinAirAvation.GetGriffinToken();
                if(!token.StartsWith("bearer "))
                {
                    token = $"bearer {token}";
                }
                using var client = new SocketIOClient.SocketIO("http://192.168.40.60:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", token } } });
                //using var client = new SocketIOClient.SocketIO("http://192.168.40.60:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiTi5SYXNvdWxpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIyNzkwIiwicGtFbXBsb3llZVNlc3Npb24iOiIyODQ0NTQxIiwiZXhwIjoxNzIwNTExNTI2LCJpc3MiOiJmbHlzZXBlaHJhbi5pciIsImF1ZCI6ImZseXNlcGVocmFuLlNlY3VyaXR5LkJlYXJlciJ9.O-bsyHavwPx6F5WPN4GlrhDw5NhUgwVUzwqSO6O8JlI" } } });
                SocketIOClient.SocketIOResponse rr = null;

                client.OnConnected += async (sender, e) => { };
                await client.ConnectAsync();
                client.On("myInfo", response =>
                {
                    privateChatResponse.ResponseMessage.Add(response.ToString());
                });
                Thread.Sleep(500);
                var timer = new Stopwatch();
                timer.Start();
                double secondTime = 0;
                request.NumberOfMessages = 1;
                for (int i = 1; i <= request.NumberOfMessages; i++)
                {
                    privateChat.data.message = $"{request.Message}";
                    privateChat.data.clientTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    await client.EmitAsync("chat", response =>
                    {
                        //Thread.Sleep(3000);
                        rr = response;
                        privateChatResponse.ResponseMessage.Add(response.ToString());
                        rr = null;
                        timer.Restart();

                    }, privateChat);

                    while (rr == null)
                    {

                        TimeSpan timeTaken = timer.Elapsed;
                        secondTime = timeTaken.TotalSeconds;

                        if (secondTime > 5)
                        {
                            timer.Stop();
                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                privateChatResponse.IsSuccess = false;
                privateChatResponse.Message = ex.Message;

            }

            return privateChatResponse;

        }

        public async Task<PrivateChatResponse> PrivateChatToUsers(PrivateChatMessageToUsersRequest request)
        {

            PrivateChatResponse privateChatResponse = new();


            PrivateChatMessage privateChat = new();
            foreach (var j in request.ToUserNames)
            {
                var foundChatLog = _chatLog.Find(x => x.FromUserName.ToLower() == "griffin" && x.ToUserName.ToLower() == j.ToLower()).OrderByDescending(x => x.Id).Take(10).Select(x => x.Message).ToList();



                if (foundChatLog != null && foundChatLog.Contains(request.Message))
                {

                    privateChatResponse.Warnings.Add("This chat log already exists");
                    continue;
                }

                PrivateChatData data = new();

                data.isRtl = request.IsRtl;
                data.to = j.ToLower();
                data.attachmentId = request.attachmentId;



                privateChat.data = data;

                try
                {
                    //var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                    var token = _griffinAirAvation.GetGriffinToken();
                    if (!token.StartsWith("bearer "))
                    {
                        token = $"bearer {token}";
                    }
                    using var client = new SocketIOClient.SocketIO("http://192.168.40.60:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", token } } });
                    //using var client = new SocketIOClient.SocketIO("http://192.168.40.60:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiTi5SYXNvdWxpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIyNzkwIiwicGtFbXBsb3llZVNlc3Npb24iOiIyODQ0NTQxIiwiZXhwIjoxNzIwNTExNTI2LCJpc3MiOiJmbHlzZXBlaHJhbi5pciIsImF1ZCI6ImZseXNlcGVocmFuLlNlY3VyaXR5LkJlYXJlciJ9.O-bsyHavwPx6F5WPN4GlrhDw5NhUgwVUzwqSO6O8JlI" } } });
                    SocketIOClient.SocketIOResponse rr = null;

                    client.OnConnected += async (sender, e) => { };
                    await client.ConnectAsync();
                    client.On("myInfo", response =>
                    {
                        privateChatResponse.ResponseMessage.Add(response.ToString());
                    });
                    Thread.Sleep(500);
                    var timer = new Stopwatch();
                    timer.Start();
                    double secondTime = 0;
                    request.NumberOfMessages = 1;
                    for (int i = 1; i <= request.NumberOfMessages; i++)
                    {
                        privateChat.data.message = $"{request.Message}";
                        privateChat.data.clientTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                        await client.EmitAsync("chat", response =>
                        {
                            //Thread.Sleep(3000);
                            rr = response;
                            privateChatResponse.ResponseMessage.Add(response.ToString());
                            rr = null;
                            timer.Restart();

                        }, privateChat);

                        while (rr == null)
                        {

                            TimeSpan timeTaken = timer.Elapsed;
                            secondTime = timeTaken.TotalSeconds;

                            if (secondTime > 5)
                            {
                                timer.Stop();
                                break;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {

                    privateChatResponse.IsSuccess = false;
                    privateChatResponse.Message = ex.Message;

                }
            }

            return privateChatResponse;

        }

        public async Task<PrivateChatResponse> PrivateChatBulkMessages(PrivateChatMessageRequest request)
        {

            PrivateChatResponse privateChatResponse = new();


            PrivateChatMessage privateChat = new();
            PrivateChatData data = new();

            data.isRtl = request.IsRtl;
            data.to = request.ToUserName;



            privateChat.data = data;

            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                //var token = _griffinAirAvation.GetGriffinToken();
                if (!token.StartsWith("bearer "))
                {
                    token = $"bearer {token}";
                }
                using var client = new SocketIOClient.SocketIO("http://192.168.40.60:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", token } } });
                //using var client = new SocketIOClient.SocketIO("http://192.168.40.60:8000/", new SocketIOClient.SocketIOOptions { ExtraHeaders = new Dictionary<string, string> { { "Authorization", "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiTi5SYXNvdWxpIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIyNzkwIiwicGtFbXBsb3llZVNlc3Npb24iOiIyODQ0NTQxIiwiZXhwIjoxNzIwNTExNTI2LCJpc3MiOiJmbHlzZXBlaHJhbi5pciIsImF1ZCI6ImZseXNlcGVocmFuLlNlY3VyaXR5LkJlYXJlciJ9.O-bsyHavwPx6F5WPN4GlrhDw5NhUgwVUzwqSO6O8JlI" } } });
                SocketIOClient.SocketIOResponse rr = null;

                client.OnConnected += async (sender, e) => { };
                await client.ConnectAsync();
                client.On("myInfo", response =>
                {
                    privateChatResponse.ResponseMessage.Add(response.ToString());
                });
                Thread.Sleep(500);
                var timer = new Stopwatch();
                timer.Start();
                double secondTime = 0;
                //request.NumberOfMessages = 1;
                for (int i = 1; i <= request.NumberOfMessages; i++)
                {
                    privateChat.data.message = $"{request.Message} {i}";
                    privateChat.data.clientTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    await client.EmitAsync("chat", response =>
                    {
                        //Thread.Sleep(3000);
                        rr = response;
                        privateChatResponse.ResponseMessage.Add(response.ToString());
                        rr = null;
                        timer.Restart();

                    }, privateChat);

                    while (rr == null)
                    {

                        TimeSpan timeTaken = timer.Elapsed;
                        secondTime = timeTaken.TotalSeconds;

                        if (secondTime > 5)
                        {
                            timer.Stop();
                            break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                privateChatResponse.IsSuccess = false;
                privateChatResponse.Message = ex.Message;

            }

            return privateChatResponse;

        }

        async public Task<PrivateChatSocketIOResponse> PrivateChatSocketIO(PrivateChatSocketIORequest request)
        {

            PrivateChatSocketIOResponse result = new();
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ChatSocketIO");
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

                string ApiAddress = httpClient.BaseAddress + _configuration["ChatSocketIO:PrivateChat"].ToString();


                var strObj = JsonConvert.SerializeObject(request);


                StringContent httpContent = new StringContent(strObj, System.Text.Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token.ToString());
                var response = await httpClient.PostAsync(ApiAddress, httpContent);
                var resStr = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<PrivateChatSocketIOResponse>(resStr);
                result.status = res.status;
                result.result = res.result;
            }
            catch (Exception ex)
            {

                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;

        }

        async public Task<PrivateChatSocketIOResponse> MessageToChatRoomIO(MessageToChatRoomIORequest request)
        {

            PrivateChatSocketIOResponse result = new();
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ChatSocketIO");
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

                string ApiAddress = httpClient.BaseAddress + _configuration["ChatSocketIO:MessageToChatRoom"].ToString();


                var strObj = JsonConvert.SerializeObject(request);


                StringContent httpContent = new StringContent(strObj, System.Text.Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token.ToString());
                var response = await httpClient.PostAsync(ApiAddress, httpContent);
                var resStr = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<PrivateChatSocketIOResponse>(resStr);
                result.status = res.status;
                result.result = res.result;
            }
            catch (Exception ex)
            {

                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;

        }


    }
}
