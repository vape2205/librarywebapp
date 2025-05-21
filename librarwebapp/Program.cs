using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using librarwebapp.Infraestructure.ExternalServices.Auth;
using librarwebapp.Infraestructure.ExternalServices.Library;
using librarwebapp.Infraestructure.ExternalServices.Suscriptions;
using librarwebapp.Infraestructure.Middleware;
using librarwebapp.Interfaces.Services;
using librarwebapp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddServerSideBlazor();

builder.Services.AddControllersWithViews();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

builder.Services.AddMudServices();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login/";
    });

builder.Services.AddScoped<IAppAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IExternalAuthService, ExternalAuthService>();
builder.Services.AddScoped<IBookExternalService, BookExternalService>();
builder.Services.AddScoped<ISuscriptionExternalService, SuscriptionExternalService>();
builder.Services.AddScoped<ISuscriptionService, SuscriptionService>();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .RequireAuthorization();

app.MapBlazorHub();

app.Run();
