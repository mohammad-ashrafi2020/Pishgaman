using Api.Infrastructure;
using Api.Infrastructure.ActionFilters;
using Api.Infrastructure.Models;
using Application.Persons;
using Application.Persons.Commands;
using Application.Persons.DTOs;
using Application.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class PersonController : ApiController
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpGet]
    public async Task<ApiResult<PersonFilterResult>> GetPersons([FromQuery] PersonFilterParams filterParams)
    {
        var res = await _personService.GetPersonByFilters(filterParams);
        return QueryResult(res);
    }

    [ServiceFilter(typeof(UserRequestActionFilter))]
    [HttpGet("{id}")]
    public async Task<ApiResult<PersonDto>> GetById(int id)
    {
        var res = await _personService.GetPersonById(id);
        return QueryResult(res);
    }

    [HttpPost]
    public async Task<ApiResult> Create([FromForm] CreatePersonCommand command)
    {
        if (UserIsNotAdmin(User.GetUserId()))
        {
            return CommandResult(OperationResult.Error("شما دسترسی انجام عملیات را ندارید"));
        }
        
        var res = await _personService.CreatePerson(command);
        return CommandResult(res);
    }
    [HttpPut]
    public async Task<ApiResult> Edit([FromForm] EditPersonCommand command)
    {
        if (UserIsNotAdmin(User.GetUserId()))
        {
            return CommandResult(OperationResult.Error("شما دسترسی انجام عملیات را ندارید"));
        }

        var res = await _personService.EditPerson(command);
        return CommandResult(res);
    }

    [HttpDelete("{id}")]
    public async Task<ApiResult> Delete(int id)
    {
        if (UserIsNotAdmin(User.GetUserId()))
        {
            return CommandResult(OperationResult.Error("شما دسترسی انجام عملیات را ندارید"));
        }

        var res = await _personService.DeletePerson(id);
        return CommandResult(res);
    }

    /// <summary>
    /// می تونستیم نقش کاربر رو به توکن اش اضافه کنیم و مستقیم از توکن دریافت اش کنیم ولی من ترجیح میدم یه درخواست اضافی ارسال کنم سمت دیتابیس ولی مطمعا بشم که کاربر نقش مورد نظر رو داره
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool UserIsNotAdmin(int id)
    {
        var user = UsersDb.Users.FirstOrDefault(f => f.Id == id);
        if (user == null)
            return false;

        return user.Role == UserRole.Client;
    }
}