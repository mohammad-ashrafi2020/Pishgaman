using System.ComponentModel.DataAnnotations;

namespace Api.ViewModels;

public class LoginViewModel
{
    [Display(Name = "شماره موبایل")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MinLength(11, ErrorMessage = "شماره موبایل نامعتبر است")]
    [MaxLength(11, ErrorMessage = "شماره موبایل نامعتبر است")]
    public string PhoneNumber { get; set; }


    [Display(Name = "کلمه عبور")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Password { get; set; }
}