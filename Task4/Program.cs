using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Task4.Data;
using Task4.Helpers;

var builder = WebApplication.CreateBuilder(args);

var databaseUrl = Environment.GetEnvironmentVariable("postgres://u9hqp8p5l4bo2g:p65f86c1c8f44ed264c373f936b06d44ad6ef517d2e560bd5cf270212bffd6148@c7u1tn6bvvsodf.cluster-czz5s0kz4scl.eu-west-1.rds.amazonaws.com:5432/d4crvi0sbtue6s");
var connectionString = HerokuConnectionHelper.ConvertHerokuConnectionString(databaseUrl);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.HttpContext.Items.ContainsKey("BlockedMessage")) 
            {
                context.Response.Redirect("/Account/Register");
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        };
    });


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}
else {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<UserValidationMiddleware>();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();