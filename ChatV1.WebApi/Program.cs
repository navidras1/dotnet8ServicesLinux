using ChatV1.Service;
using ChatV1.Service.Services.CallApi;
using ChatV1.WebApi.BackgroundServices;
using ChatV1.WebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using NLog.Web;
using NLog;
using ChatV1.WebApi.Middleware;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using ChatV1.WebApi.Configs;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ChatV1.WebApi.Models.AppSetting;
using Microsoft.Extensions.DependencyInjection;
using ChatV1.Service.Services;
using ChatV1.Service.Services.Minio;
using ChatV1.Service.Services.SocketIo;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");



try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    }).AddNewtonsoftJson();
    //builder.Services.AddControllers().AddNewtonsoftJson();
    builder.Services.AddApiVersioning().AddMvc().AddApiExplorer();
    builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
    builder.Services.AddHostedService<ChatLogBackgroundService>();
    builder.Services.AddHostedService<ChatRoomCreateBackgroundService>();
    builder.Services.AddHostedService<ChatRoomMessageLogBackgroundService>();
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    //builder.Services.AddHostedService<RobotToken>();
    //builder.Services.AddHostedService<GriffinRobotBackgroundService>();
    builder.Services.ConfigChatService(builder.Configuration);
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddHttpClient<IGriffinAirAvation, GriffinAirAvation>("griffinApi", client =>
    {


        if (builder.Environment.IsDevelopment())
        {
            client.BaseAddress = new Uri(builder.Configuration["GriffinWebApiAddress:Develope"]);
        }
        else
        {
            client.BaseAddress = new Uri(builder.Configuration["GriffinWebApiAddress:Product"]);
        }

    });
    builder.Services.AddHttpClient<IGriffinAirAvation, GriffinAirAvation>("ChatSocketIO", client =>
    {


        if (builder.Environment.IsDevelopment())
        {
            client.BaseAddress = new Uri(builder.Configuration["ChatSocketIO:HostDevelope"]);
        }
        else
        {
            client.BaseAddress = new Uri(builder.Configuration["ChatSocketIO:Host"]);
        }

    });


    builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
    builder.Services.AddOptions();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.Configure<TokenManagement>(builder.Configuration.GetSection("tokenManagement"));
    var token = builder.Configuration.GetSection("tokenManagement").Get<TokenManagement>();
    var secret = Encoding.ASCII.GetBytes(token.Secret);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
            ValidIssuer = token.Issuer,
            ValidAudience = token.Audience,
            ValidateIssuer = false,
            ValidateAudience = false,
            SignatureValidator = delegate (string token, TokenValidationParameters parameters)
            {
                var jwt = new JsonWebToken(token); // here was JwtSecurityToken
                if (parameters.ValidateIssuer && parameters.ValidIssuer != jwt.Issuer)
                    return null;
                return jwt;
            },
            RequireSignedTokens = false
        };
        x.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/inboxHub") ||
                                path.StartsWithSegments("/schedulerHub") ||
                                 path.StartsWithSegments("/notificationHub")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
    //builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
    builder.Services.AddSwaggerGen(config =>
    {
        config.OperationFilter<SwaggerDefaultValues>();
        config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        config.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
                    });
        config.EnableAnnotations();
    });
    //builder.Services.AddStackExchangeRedisCache(options => options.Configuration = "192.168.40.60:6379");

    builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(new ConfigurationOptions
    {
        EndPoints = { builder.Configuration["Redis:Host"], builder.Configuration["Redis:Port"] }
    }));

    builder.Services.AddTransient<SuperUserMiddleware>();
    builder.Services.AddScoped<NLogMiddleware>();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI(
    options =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }
    });
    //}


    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<SuperUserMiddleware>();
    app.UseMiddleware<NLogMiddleware>();
    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        //var context = services.GetRequiredService<DataAccess.Models.GolestanTestDbContext>();
        var context = services.GetRequiredService<ChatV1.DataAccess.Context.ChatV1Context>();
        context.Database.Migrate();

        var seed = services.GetRequiredService<ISeed>();
        seed.SeedAll();

        var robotChat = services.GetRequiredService<IRobotChat>();
        //await robotChat.ConnectToChatAsRobotAsync();


        IActions actions = services.GetRequiredService<IActions>();
        var resss = await actions.SendMessaageToRoom(new ChatV1.Service.Request.SendMessaageToRoomRequest { attachmentId = null, chatRoomName = "navidTest122", isRtl = false, message = "test123" });

    }




    app.Run();


}
catch (Exception exception)
{

    logger.Error(exception, "Stopped program because of exception");
    throw;
    // rerun 
}
finally
{
    NLog.LogManager.Shutdown();
}