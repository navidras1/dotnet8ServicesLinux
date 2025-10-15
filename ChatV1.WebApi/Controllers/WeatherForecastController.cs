using Asp.Versioning;
using ChatV1.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatV1.WebApi.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly IActions _actions;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IActions actions)
        {
            _actions = actions;
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var mm = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _logger.LogWarning("Weather warning");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

        }


        [HttpGet(Name = "GetWeatherForecast"), ApiVersion(2)]
        public IEnumerable<WeatherForecast> GetV2()
        {
            var mm = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _logger.LogWarning("Weather warning");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();

        }

        [HttpGet(Name = "GetWeatherForecast"), ApiVersion(3)]
        public string GetV3()
        {
            var mm = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _logger.LogWarning("Weather warning");
            return "Hello";

        }

        [HttpPost("Test"), ApiVersion(3)]
        public string Test(Test1 test1)
        {
            return "test1";
        }

        [HttpPost("Test"), ApiVersion(2)]
        public string Test2(Test2 test2)
        {
            return "test2";
        }

    }

    public class Test1
    {
        public string Name1 { get; set; }
    }
    public class Test2
    {
        public string Name2 { get; set; }
    }

}