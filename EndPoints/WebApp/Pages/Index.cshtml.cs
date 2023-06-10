using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Infrastructure;
using WebApp.Infrastructure.Models;
using WebApp.Services.Auth;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        private IAuthService _authService;

        public UserDto? CurrentUser { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                var result = await _authService.GetCurrentUser();
                if (result.MetaData.AppStatusCode == AppStatusCode.UnAuthorize)
                {
                    HttpContext.Response.Cookies.Delete("token");
                    return Redirect("/auth/login");
                }

                CurrentUser = result.Data;
            }
            return Page();
        }
    }
}