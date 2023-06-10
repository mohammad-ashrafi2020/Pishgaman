using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Application.Persons.Commands;
using Application.Utils;
using WebApp.Infrastructure.CustomValidation.IFormFile;
using WebApp.Infrastructure.Models;
using WebApp.Infrastructure.RazorUtils;
using WebApp.Services.Auth;
using WebApp.Services.Person;

namespace WebApp.Pages.Persons;


[BindProperties]
[ValidateAntiForgeryToken]
public class EditModel : BaseRazor
{
    private readonly IHttpPersonService _personService;
    private IAuthService _authService;
    public EditModel(IHttpPersonService personService, IAuthService authService)
    {
        _personService = personService;
        _authService = authService;
    }

    [Display(Name = "نام")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Name { get; set; }
    [Display(Name = "نام خانوادگی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string Family { get; set; }

    [Display(Name = "نام پدر")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    public string FatherName { get; set; }

    [Display(Name = "شماره تلفن")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MinLength(11, ErrorMessage = "شماره موبایل نامعتبر است")]
    [MaxLength(11, ErrorMessage = "شماره موبایل نامعتبر است")]
    public string PhoneNumber { get; set; }

    [Display(Name = "کدملی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MinLength(10, ErrorMessage = " {0} نامعتبر است")]
    [MaxLength(10, ErrorMessage = " {0} نامعتبر است")]
    public string NatinalCode { get; set; }

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل نامعتبر است")]
    public string Email { get; set; }


    [Display(Name = "عکس")]
    [FileImage(ErrorMessage = "عکس وارد شده نامعتبر است")]
    [MaxFileSize(2, ErrorMessage = "حجم عکس نباید بیشتر از 2 مگابایت باشد")]
    public IFormFile? Avatar { get; set; }


    public async Task<IActionResult> OnGet(int id)
    {

        var data = await _personService.GetById(id);
        if (data.Data == null)
            return RedirectToPage("Index");


        var user = await _authService.GetCurrentUser();
        if (user.Data.Role == UserRole.Client)
        {
            ErrorAlert("شما مجوز انجام عملیات را ندارید");
            return RedirectToPage("Index");
        }


        var person = data.Data;

        Name = person.Name;
        Family = person.Family;
        FatherName = person.FatherName;
        NatinalCode = person.NatinalCode;
        Email = person.Email;
        PhoneNumber = person.PhoneNumber;

        return Page();
    }

    public async Task<IActionResult> OnPost(int id)
    {
        var res = await _personService.EditPerson(new EditPersonCommand
        {
            Id = id,
            Name = Name,
            Family = Family,
            FatherName = FatherName,
            PhoneNumber = PhoneNumber,
            NatinalCode = NatinalCode,
            Email = Email,
            Avatar = Avatar
        });

        if (res.IsSuccess)
        {
            SuccessAlert();
        }
        else
        {
            ErrorAlert(res.MetaData.Message);
            return Page();
        }
        return RedirectToPage("Index");
    }
}