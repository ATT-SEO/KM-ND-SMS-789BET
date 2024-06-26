using FE.JunCMD.Client.Services;
using FE.JunCMD.Client.Services.IService;
using FE.JunCMD.Client.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IBOService, BOService>();
builder.Services.AddHttpClient<ISiteService, SiteService>();
builder.Services.AddHttpClient<IPhoneNumberService, PhoneNumberService>();
builder.Services.AddHttpClient<ISMSService, SMSService>();
builder.Services.AddHttpClient<ILogAccountService, LogAccountService>();


builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<IBOService, BOService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IPhoneNumberService, PhoneNumberService>();
builder.Services.AddScoped<ISMSService, SMSService>();
builder.Services.AddScoped<ILogAccountService, LogAccountService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
});

SD.ApiKM58 = builder.Configuration["ServiceURLs:ApiKM58"];


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
