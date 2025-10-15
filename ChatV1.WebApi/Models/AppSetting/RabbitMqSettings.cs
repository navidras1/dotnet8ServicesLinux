namespace ChatV1.WebApi.Models.AppSetting
{
    public class RabbitMqSettings
    {
        public string Address { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LogMicroservice { get; set; }
        public string LogMicroserviceLocal { get; set; }
        public string LogChatRoomMessage { get; set; }
        public string LogChatRoomMessageLocal { get; set; }
        public string RobotChatRoomLocal {  get; set; }
        public string RobotChatRoom {  get; set; }
    }
}
