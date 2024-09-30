using Web.Shared.Helpers;
using Serilog;
using Web.Api;
using Web.Domain.Repositories.Users;
using Web.Infrastructure.Repositories.Users;
using Web.Domain.Repositories.Products;
using Web.Infrastructure.Repositories.Products;
using Web.Domain.UnitOfWork;
using Web.Infrastructure.UnitOfWork;
using Web.Infrastructure.Data;
using Web.Application.Services.Products;
using Web.Application.Services.Users;
using Microsoft.EntityFrameworkCore;
using Web.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Web.Application.Services.Login;
using CMC.TS.EDU.UMS.Api.Infrastructure;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


Log.Information($"Start {builder.Environment.ApplicationName} up");
try
{
    // Add services to the container.
    //builder.Host.UseSerilog((context, services, configuration) =>
    //{
    //    var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
    //    var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";
    //    configuration
    //           .WriteTo.Debug()
    //            .WriteTo.Console(outputTemplate:
    //                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
    //            .Enrich.FromLogContext()
    //            .Enrich.WithMachineName()
    //            .Enrich.WithProperty("Environment", environmentName)
    //            .Enrich.WithProperty("Application", applicationName)
    //           .ReadFrom.Configuration(context.Configuration);
    //});
    // Cấu hình Serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.File("logs/log-{Date}.txt", // Sử dụng {Date} để thêm thời gian vào tên tệp
                          rollingInterval: RollingInterval.Day) // Tạo tệp mới mỗi ngày
            .CreateLogger();
  
    // Đăng ký Serilog làm Logger chính
    builder.Host.UseSerilog();

    // Đăng ký Serilog.ILogger vào Dependency Injection
    builder.Services.AddSingleton(Log.Logger);
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ILoginService, LoginService>();
    builder.Services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
    builder.Services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddControllers();
    // Cấu hình Redis cache
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "localhost:6379"; // Địa chỉ Redis
        options.InstanceName = "SampleInstance"; // Tên instance (tuỳ chọn)
    });
    JwtTokenConfig jwtTokenConfig = builder.Configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
    builder.Services.AddSingleton(jwtTokenConfig);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = true;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtTokenConfig.Issuer,
            ValidateIssuerSigningKey = true ,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
            ValidAudience = jwtTokenConfig.Audience,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
            
        };
    });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")); // Role = 1
        options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));   // Role = 2
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    // Register Swagger services
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

        // Add JWT Bearer token support
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Enter 'Bearer' [space] and then your token",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                new string[] {}
            }
        });
    });


    var app = builder.Build();
    app.UseMiddleware<ErrorHandlerMiddleware>();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}