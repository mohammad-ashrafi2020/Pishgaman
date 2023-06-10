using Application.Persons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{

    public class AjaxController : Controller
    {
        private readonly IPersonService _personService;

        public AjaxController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> EmailIsExist(string email)
        {
            var exists = await _personService.IsEmailExist(email);
            if (exists == false) return new JsonResult(true);
            return new JsonResult("ایمیل قبلا استفاده شده.");
        }

        [HttpGet]
        public async Task<IActionResult> PhoneNumberIsExist(string phoneNumber)
        {
            var exists = await _personService.IsPhoneNumberExist(phoneNumber);
            if (exists == false) return new JsonResult(true);
            return new JsonResult("شماره تلفن قبلا توسط شخص دیگری استفاده شده است.");
        }
        [HttpGet]
        public async Task<IActionResult> NactinalCodeIsExist(string nactinalCode)
        {
            var exists = await _personService.IsNatinalCodeExist(nactinalCode);
            if (exists == false) return new JsonResult(true);
            return new JsonResult("کد ملی وارد شده قبلا توسط شخص دیگری استفاده شده است.");
        }
    }
}
