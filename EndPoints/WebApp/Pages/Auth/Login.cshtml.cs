using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebApp.Services.Auth;

namespace WebApp.Pages.Auth;


[BindProperties]
[ValidateAntiForgeryToken]
public class LoginModel : PageModel
{
    private IAuthService _authService;

    public LoginModel(IAuthService authService)
    {
        _authService = authService;
    }

    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string PhoneNumber { get; set; }

    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MinLength(3, ErrorMessage = "کلمه عبور باید بزرگتر ار 3 کاراکتر باشد")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string RedirectTo { get; set; }
    public IActionResult OnGet(string redirectTo)
    {
        if (User.Identity.IsAuthenticated)
            return Redirect("/persons");

        RedirectTo = redirectTo;
        return Page();
    }
    public async Task<IActionResult> OnPost()
    {
        var result = await _authService.Login(PhoneNumber, Password);
        if (result.IsSuccess == false)
        {
            ModelState.AddModelError(nameof(PhoneNumber), result.MetaData.Message);
            return Page();
        }

        var token = result.Data;
        HttpContext.Response.Cookies.Append("token", token, new CookieOptions()
        {
            HttpOnly = true,
            //Expires = DateTimeOffset.Now.AddDays(1)
        });
        if (string.IsNullOrWhiteSpace(RedirectTo) == false)
        {
            return LocalRedirect(RedirectTo);
        }
        return Redirect("/");
    }
}