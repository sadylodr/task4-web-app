using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Task4.Data;

public class UserValidationMiddleware
{
    private readonly RequestDelegate _next;

    public UserValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        var path = context.Request.Path;
        if (path.StartsWithSegments("/Account/Login") ||
            path.StartsWithSegments("/Account/Register") ||
            path.StartsWithSegments("/Account/Logout")) {
            await _next(context);
            return;
        }

        var userEmail = context.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        if (!string.IsNullOrEmpty(userEmail)) {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                context.Response.Redirect("/Account/Register");
                return;
            }

            if (user.IsBlocked)
            {
                context.Items["BlockedMessage"] = "Ваш аккаунт заблокирован. Обратитесь к администратору.";
                context.Response.Redirect("/Account/Register");
                return;
            }
        }

        await _next(context);
    }
}