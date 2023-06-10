using Application.Persons;
using Application.Persons.DTOs;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Infrastructure;
using WebApp.Infrastructure.RazorUtils;
using WebApp.Services.Person;

namespace WebApp.Pages.Persons
{
    public class IndexModel : BaseRazorFilter<PersonFilterParams>
    {
        private readonly IHttpPersonService _personService;

        public IndexModel(IHttpPersonService personService)
        {
            _personService = personService;
        }

        public PersonFilterResult FilterResult { get; set; }
        public async Task<IActionResult> OnGet()
        {
            FilterParams.Take = 1;
            var res = await _personService.GetByFilter(FilterParams);
            if (res.IsSuccess==false)
            {
                ErrorAlert(res.MetaData.Message);
                return Redirect("/");
            }
            FilterResult = res.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            return await AjaxTryCatch(async () =>
            {
                var res = await _personService.DeletePerson(id);
                if (res.IsSuccess)
                {
                    return OperationResult.Success();
                }

                return OperationResult.Error(res.MetaData.Message);
            });
        }
    }
}
