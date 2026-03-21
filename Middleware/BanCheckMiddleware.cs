using LoanApp.Models;
using Microsoft.AspNetCore.Identity;

namespace LoanApp.Middleware
{
    public class BanCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public BanCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var path = context.Request.Path.Value?.ToLower() ?? "";

                // Allow access to restricted page, logout, and static files
                if (path.Contains("/account/restricted") ||
                    path.Contains("/account/logout") ||
                    path.StartsWith("/lib/") ||
                    path.StartsWith("/css/") ||
                    path.StartsWith("/js/") ||
                    path.StartsWith("/_framework"))
                {
                    await _next(context);
                    return;
                }

                var user = await userManager.GetUserAsync(context.User);
                if (user != null && user.IsBanned)
                {
                    context.Response.Redirect("/Identity/Account/Restricted");
                    return;
                }
            }

            await _next(context);
        }
    }
}
