using ChatV1.Service.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatV1.Service.Services.Minio
{
    public interface IMinIOService
    {
        Task<MinIOUploadFileResponse> UploadFileAsync(Stream fileBytes, string fileName, string extension, string contentType);
        Task<MemoryStream> DownloadFile(string fileName);
    }

    public class ChatMinIOService : IMinIOService
    {
        private IMinioClient _minioClient;
        private readonly IConfiguration _configuration;
        private ILogger<ChatMinIOService> _logger;

        public ChatMinIOService(IConfiguration configuration, ILogger<ChatMinIOService> logger)
        {
            _configuration = configuration;
            _minioClient = new MinioClient().WithEndpoint(_configuration["ChatMinIO:url"])
            .WithCredentials(_configuration["ChatMinIO:accessKey"],_configuration["ChatMinIO:secretKey"])
            .Build();
            _logger = logger;
        }

        public async Task<MinIOUploadFileResponse> UploadFileAsync(Stream fileBytes, string fileName, string extension, string contentType)
        {
            _logger.LogInformation("Minio Service File Size:" + fileBytes.Length);
            MinIOUploadFileResponse minIOUploadFileResponse = new MinIOUploadFileResponse();
            try
            {
                PutObjectResponse minioUploadRes;
                //string fileNameWithExtension = $"{fileName}{extension}";

                //using (var fileStream = new MemoryStream(fileBytes))
                //{
                minioUploadRes = await _minioClient.PutObjectAsync(new PutObjectArgs()
                   .WithBucket(_configuration["ChatMinIO:bucket"])
                   .WithObject(fileName)
                   .WithStreamData(fileBytes)
                   .WithObjectSize(fileBytes.Length)
                   .WithContentType(contentType));
                //.WithContentType("application/pdf"));
                //}
                var fileAddress = $"https://fs.flygriffin.com/{_configuration["ChatMinIO:bucket"]}/{fileName}";
                minIOUploadFileResponse.FileAddress = fileAddress;
                minIOUploadFileResponse.BucketName = _configuration["ChatMinIO:bucket"];
            }
            catch (Exception ex)
            {

                minIOUploadFileResponse.Message = ex.Message;
                minIOUploadFileResponse.IsSuccess = false; _logger.LogInformation("Minio Service File Size:" + fileBytes.Length);

            }

            return minIOUploadFileResponse;
            //minioUploadRes.
        }

        public async Task<MemoryStream> DownloadFile(string fileName)
        {
            var memStream = new MemoryStream();
            
            
            var getObjectArgs = new GetObjectArgs().WithBucket(_configuration["ChatMinIO:bucket"]).WithObject(fileName).WithCallbackStream((stream) => { stream.CopyTo(memStream); });
            
            object objectData = await _minioClient.GetObjectAsync(getObjectArgs);
            
            memStream.Position = 0;
     
            return memStream;

        }

    }
}
