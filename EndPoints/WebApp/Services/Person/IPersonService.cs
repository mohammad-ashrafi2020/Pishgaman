

using Application.Persons.Commands;
using Application.Persons.DTOs;
using Newtonsoft.Json;
using WebApp.Infrastructure;

namespace WebApp.Services.Person;

public interface IHttpPersonService
{
    Task<ApiResult> CreatePerson(CreatePersonCommand command);
    Task<ApiResult> EditPerson(EditPersonCommand command);
    Task<ApiResult> DeletePerson(int id);


    Task<ApiResult<PersonDto?>> GetById(int id);
    Task<ApiResult<PersonFilterResult>> GetByFilter(PersonFilterParams filterParams);
}
class HttpPersonService : IHttpPersonService
{
    private readonly HttpClient _client;

    public HttpPersonService(HttpClient client)
    {
        _client = client;
    }
    public async Task<ApiResult> CreatePerson(CreatePersonCommand command)
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(command.Name), "Name");
        formData.Add(new StringContent(command.Family), "Family");
        formData.Add(new StringContent(command.Email), "Email");
        formData.Add(new StringContent(command.PhoneNumber), "PhoneNumber");
        formData.Add(new StringContent(command.NatinalCode), "NatinalCode");
        formData.Add(new StringContent(command.FatherName), "FatherName");
        formData.Add(new StreamContent(command.Avatar.OpenReadStream()), "Avatar", command.Avatar.FileName);

        var result = await _client.PostAsync("person", formData);
        return await result.Content.ReadFromJsonAsync<ApiResult>();
    }

    public async Task<ApiResult> EditPerson(EditPersonCommand command)
    {
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(command.Id.ToString()), "Id");
        formData.Add(new StringContent(command.Name), "Name");
        formData.Add(new StringContent(command.Family), "Family");
        formData.Add(new StringContent(command.Email), "Email");
        formData.Add(new StringContent(command.PhoneNumber), "PhoneNumber");
        formData.Add(new StringContent(command.NatinalCode), "NatinalCode");
        formData.Add(new StringContent(command.FatherName), "FatherName");
        if (command.Avatar != null)
            formData.Add(new StreamContent(command.Avatar.OpenReadStream()), "Avatar", command.Avatar.FileName);

        var result = await _client.PutAsync("person", formData);
        return await result.Content.ReadFromJsonAsync<ApiResult>();
    }

    public async Task<ApiResult> DeletePerson(int id)
    {
        var result = await _client.DeleteAsync("person/" + id);
        return await result.Content.ReadFromJsonAsync<ApiResult>();
    }

    public async Task<ApiResult<PersonDto?>> GetById(int id)
    {
        return await _client.GetFromJsonAsync<ApiResult<PersonDto>>("person/" + id);
    }

    public async Task<ApiResult<PersonFilterResult>> GetByFilter(PersonFilterParams filterParams)
    {
        var url = $"person?pageId={filterParams.PageId}&take={filterParams.Take}";


        if (string.IsNullOrWhiteSpace(filterParams.NatinalCode) == false)
            url += $"&NatinalCode={filterParams.NatinalCode}";

        if (string.IsNullOrWhiteSpace(filterParams.Name) == false)
            url += $"&Name={filterParams.Name}";

        var res = await _client.GetStringAsync(url);
        return JsonConvert.DeserializeObject<ApiResult<PersonFilterResult>>(res);
    }
}