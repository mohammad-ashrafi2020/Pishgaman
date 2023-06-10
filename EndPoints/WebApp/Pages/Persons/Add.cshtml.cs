using System.ComponentModel.DataAnnotations;
using Application.Persons;
using Application.Persons.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Infrastructure.CustomValidation.IFormFile;
using WebApp.Infrastructure.RazorUtils;
using WebApp.Services.Person;

namespace WebApp.Pages.Persons;


[BindProperties]
[ValidateAntiForgeryToken]
public class AddModel : BaseRazor
{
    IHttpPersonService _personService;
    public AddModel(IHttpPersonService personService)
    {
        _personService = personService;
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
    [Remote("PhoneNumberIsExist", "Ajax")]
    public string PhoneNumber { get; set; }

    [Display(Name = "کدملی")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [MinLength(10, ErrorMessage = " {0} نامعتبر است")]
    [MaxLength(10, ErrorMessage = " {0} نامعتبر است")]
    [Remote("NactinalCodeIsExist", "Ajax")]
    public string NatinalCode { get; set; }

    [Display(Name = "ایمیل")]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل نامعتبر است")]
    [Remote("EmailIsExist", "Ajax")]
    public string Email { get; set; }


    [Display(Name = "عکس")]
    [FileImage(ErrorMessage = "عکس وارد شده نامعتبر است")]
    [MaxFileSize(2, ErrorMessage = "حجم عکس نباید بیشتر از 2 مگابایت باشد")]
    public IFormFile Avatar { get; set; }
  

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        var res = await _personService.CreatePerson(new CreatePersonCommand
        {
            Name = Name,
            Family = Family,
            FatherName = FatherName,
            PhoneNumber = PhoneNumber,
            NatinalCode = NatinalCode,
            Email = Email,
            Avatar = Avatar
        });

        if (res.IsSuccess)
            SuccessAlert();
        else
            ErrorAlert(res.MetaData.Message);
        return RedirectToPage("Index");
    }
}