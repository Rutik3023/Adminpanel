using AdminPanel.CommonRepo;
using AdminPanel.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
CommonFunction.Initialize(builder.Configuration);
// Add services to the container.
builder.Services.AddControllersWithViews();
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IConfiguration>(config);
builder.Services.AddTransient<IUserMaster, UserMaster_services>();
builder.Services.AddTransient<IUserRoleMapping, UserRoleMapping_Services>();
builder.Services.AddTransient<IUserValidationService, User_Validation_Services>();
builder.Services.AddTransient<IMenuMaster, MenuMaster_Service>();
builder.Services.AddTransient<IRoleMenuMapping, RoleMenuMapping_Services>();
builder.Services.AddTransient<IRoleMaster, RoleMaster_Service>();
builder.Services.AddTransient<IMenuService, MenuService>();
builder.Services.AddTransient<IGeneral, General_Services>();
builder.Services.AddTransient<IDeviceDetailse, DeviceDetailse_Service>();
builder.Services.AddTransient<IEmployee, Employee_Services>();
builder.Services.AddTransient<IOrganization, Organization_Services>();
builder.Services.AddResponseCaching();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o => o.LoginPath = new PathString("/Login/Index"));

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpfdHRdRmlfWU1+XEo=");


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
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
