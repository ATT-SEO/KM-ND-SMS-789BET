using FE.ADMIN.Services;
using FE.ADMIN.Services.IService;
using FE.ADMIN.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddHttpClient<IBOService, BOService>();
builder.Services.AddHttpClient<ISiteService, SiteService>();
builder.Services.AddHttpClient<IPhoneNumberService, PhoneNumberService>();
builder.Services.AddHttpClient<ISMSRawDataService, SMSRawDataService>();
builder.Services.AddHttpClient<ISMSService, SMSService>();
builder.Services.AddHttpClient<ILogAccountService, LogAccountService>();


builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
//builder.Services.AddScoped<IBOService, BOService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IPhoneNumberService, PhoneNumberService>();
builder.Services.AddScoped<ISMSRawDataService, SMSRawDataService>();
builder.Services.AddScoped<ISMSService, SMSService>();
builder.Services.AddScoped<ILogAccountService, LogAccountService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});

SD.ApiKM58 = builder.Configuration["ServiceURLs:ApiKM58"];
SD.AuthAPIBase = builder.Configuration["ServiceURLs:AuthAPIBase"];

builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
