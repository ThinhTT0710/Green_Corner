using GreenCorner.MVC.Controllers;
using GreenCorner.MVC.Services;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ITrashEventService, TrashEventService>();
builder.Services.AddHttpClient<IRewardService, RewardService>();
builder.Services.AddHttpClient<IRewardPointService, RewardPointService>();


SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
SD.EventAPIBase = builder.Configuration["ServiceUrls:EventAPI"];
SD.RewardAPIBase = builder.Configuration["ServiceUrls:RewardAPI"];


builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITrashEventService, TrashEventService>();
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IPointTransactionService, PointTransactionService>();
builder.Services.AddScoped<IRewardPointService, RewardPointService>();
builder.Services.AddScoped<IRewardRedemptionHistoryService, RewardRedemptionHistoryService>();



// Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    }).AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"];
        options.ClientSecret = builder.Configuration["Google:ClientSecret"];
        options.ClaimActions.MapJsonKey("picture", "picture", "url");
        options.SaveTokens = true;
    }).AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = builder.Configuration["FaceBook:AppId"];
        facebookOptions.AppSecret = builder.Configuration["FaceBook:AppSecret"];
        facebookOptions.Fields.Add("picture");
        facebookOptions.SaveTokens = true;
    }); ;

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
