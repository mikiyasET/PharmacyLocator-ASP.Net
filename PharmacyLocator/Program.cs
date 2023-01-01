using Jose;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PharmacyLocator;
using PharmacyLocator.Models;
using PharmacyLocator.Models.Services;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


var port = builder.Configuration["PORT"];
Debug.Print($"port: {port}");
builder.WebHost.UseUrls($"http://0.0.0.0:{port};http://localhost:3000");


ConfigurationManager Configuration = builder.Configuration;
// Add services to the container.
/*builder.Services.AddDbContext<PharmaDbContext>(options => {
    options.UseSqlServer(Configuration.GetConnectionString("DbContext"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});*/


builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureMyCookie>();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
   .AddNegotiate();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) // Sets the default scheme to cookies
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/account/denied";
                options.LoginPath = "/account/login";
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:Issuer"],
                    ValidIssuer = Configuration["JWT:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    ValidateIssuerSigningKey = true,

                };
            });
builder.Services.AddAuthorization(options =>
{
    var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
        JwtBearerDefaults.AuthenticationScheme);
    defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
});
builder.Services.AddRazorPages();

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<IPharmacyService, PharmacyService>();
builder.Services.AddScoped<IRecordService, RecordService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

var mysqlHost = builder.Configuration["MYSQLHOST"];
var mysqlPort = builder.Configuration["MYSQLPORT"];
var mysqlUser = builder.Configuration["MYSQLUSER"];
var mysqlPassword = builder.Configuration["MYSQLPASSWORD"];
var mysqlDatabase = builder.Configuration["MYSQLDATABASE"];
// var url = $"Server={mysqlHost};Database={mysqlDatabase};Port={mysqlPort};user={mysqlUser};password={mysqlPassword}";
var url = builder.Configuration.GetConnectionString("mysqlConnection");


builder.Services.AddDbContext<PharmaDbContext>(options => {
    options.UseMySql(url, ServerVersion.AutoDetect(url));
});

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
var cookiePolicyOptions = new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
};
app.UseCookiePolicy(cookiePolicyOptions);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");
app.Run();