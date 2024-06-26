using API.KM58;
using API.KM58.Data;
using API.KM58.Extensions;
using API.KM58.Model;
using API.KM58.Service.IService;
using API.KM58.Service;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddSingleton(MappingConfig.RegisterMaps().CreateMapper());
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IBOService, BOService>();
builder.Services.AddHttpClient<IGoogleSheetService, GoogleSheetService>();

builder.Services.AddScoped<IBOService, BOService>();
builder.Services.AddScoped<IGoogleSheetService, GoogleSheetService>();

builder.Services.AddScoped<ICheckConditions, CheckConditions>();


builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the bearer scheme",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[]{}
        }
    });
});
builder.AddAppAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHangfire((sp, config) =>
{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    config.UseSqlServerStorage(connectionString);

});
builder.Services.AddHangfireServer();

builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddHostedService<WorkerService>();

var app = builder.Build();

app.UseHangfireDashboard("/myDashboard", new DashboardOptions
{
    //AppPath = "https://telebot.attapps.net/",
    Authorization = new[] { new MyAuthorizationFilter() }
});
app.UseCors(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://bo-xf.khuyenmaiapp.com", "http://localhost:3000");
});

IWebHostEnvironment environment = builder.Environment;
environment.ContentRootPath = AppDomain.CurrentDomain.BaseDirectory;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();