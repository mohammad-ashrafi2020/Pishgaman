using Application.Persons.Commands;
using Application.Persons.DTOs;
using Application.Utils;
using Application.Utils.FileUtil.Services;
using Infrastructure;
using Infrastructure.Entities.Persons;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Persons;

public interface IPersonService
{
    Task<OperationResult> CreatePerson(CreatePersonCommand command);
    Task<OperationResult> DeletePerson(int id);
    Task<OperationResult> EditPerson(EditPersonCommand command);


    Task<PersonDto?> GetPersonById(int id);
    Task<PersonFilterResult> GetPersonByFilters(PersonFilterParams filterParams);
    Task<bool> IsEmailExist(string email);
    Task<bool> IsPhoneNumberExist(string phoneNumber);
    Task<bool> IsNatinalCodeExist(string natinalCode);


}

/// <summary>
/// با الگوی یونیت آف ورک موافق نیستم ولی چون توی تمرین گفته بودین استفاده کردم
/// </summary>
class PersonService : IPersonService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<Person> _personRepository;
    private readonly LocalFileService _localFileService;
    public PersonService(IUnitOfWork unitOfWork, LocalFileService localFileService)
    {
        _unitOfWork = unitOfWork;
        _localFileService = localFileService;
        _personRepository = _unitOfWork.Repository<Person>();

    }

    public async Task<OperationResult> CreatePerson(CreatePersonCommand command)
    {
        if (await IsEmailExist(command.Email))
        {
            return OperationResult.Error("ایمیل تکراری است");
        }
        if (await IsPhoneNumberExist(command.PhoneNumber))
        {
            return OperationResult.Error("شماره تلفن تکراری است");

        }
        if (await IsNatinalCodeExist(command.NatinalCode))
        {
            return OperationResult.Error(" کد ملی تکراری است");
        }

        var imageName = await SaveAvatar(command.Avatar);

        _personRepository.Add(new Person
        {
            Name = command.Name,
            Family = command.Family,
            FatherName = command.FatherName,
            PhoneNumber = command.PhoneNumber,
            NatinalCode = command.NatinalCode,
            Email = command.Email,
            Avatar = imageName
        });
        await _unitOfWork.CommitAsync();
        return OperationResult.Success();
    }

    public async Task<OperationResult> DeletePerson(int id)
    {
        var person = await _personRepository.GetAsync(id);
        if (person == null)
        {
            return OperationResult.NotFound();
        }
        _personRepository.Delete(person);
        await _unitOfWork.CommitAsync();
        DeleteOldAvatar(person.Avatar);
        return OperationResult.Success();
    }

    public async Task<OperationResult> EditPerson(EditPersonCommand command)
    {
        var person = await _personRepository.GetTracking(command.Id);
        if (person == null)
        {
            return OperationResult.NotFound();
        }

        if (command.Email != person.Email)
            if (await IsEmailExist(command.Email))
            {
                return OperationResult.Error("ایمیل تکراری است");
            }

        if (command.PhoneNumber != person.PhoneNumber)
            if (await IsPhoneNumberExist(command.PhoneNumber))
            {
                return OperationResult.Error("شماره تلفن تکراری است");
            }

        if (command.NatinalCode != person.NatinalCode)
            if (await IsNatinalCodeExist(command.NatinalCode))
            {
                return OperationResult.Error(" کد ملی تکراری است");
            }

        person.Email = command.Email;
        person.PhoneNumber = command.PhoneNumber;
        person.NatinalCode = command.NatinalCode;
        person.Name = command.Name;
        person.Family = command.Family;
        person.FatherName = command.FatherName;


        string? oldAvatar = person.Avatar;
        if (command.Avatar != null)
        {
            person.Avatar = await SaveAvatar(command.Avatar);
        }

        await _unitOfWork.CommitAsync();
        DeleteOldAvatar(oldAvatar);
        return OperationResult.Success();
    }
    private async Task<string> SaveAvatar(IFormFile avatarFile)
    {
        if (avatarFile.IsImage() == false)
        {
            throw new InvalidDataException("Invalid ImageFile");
        }
        return await _localFileService.SaveFileAndGenerateName(avatarFile, Directories.PersonAvatar);
    }
    private void DeleteOldAvatar(string? avatar)
    {
        if (string.IsNullOrWhiteSpace(avatar) == false)
            _localFileService.DeleteFile(Directories.PersonAvatar, avatar);
    }
    public async Task<PersonDto?> GetPersonById(int id)
    {
        var person = await _personRepository.GetAsync(id);
        if (person == null)
        {
            return null;
        }
        return MapPerson(person);
    }

    public async Task<PersonFilterResult> GetPersonByFilters(PersonFilterParams filterParams)
    {
        var result = _personRepository.Query(f => true);

        if (string.IsNullOrWhiteSpace(filterParams.NatinalCode) == false)
        {
            result = result.Where(r => r.NatinalCode.Contains(filterParams.NatinalCode));
        }
        if (string.IsNullOrWhiteSpace(filterParams.Name) == false)
        {
            result = result.Where(r => r.Name.Contains(filterParams.Name));
        }

        var skip = (filterParams.PageId - 1) * filterParams.Take;
        var data = await result.Skip(skip).Take(filterParams.Take).ToListAsync();
        var model = new PersonFilterResult()
        {
            Data = data.Select(MapPerson).ToList()
        };
        model.GeneratePaging(result, filterParams.Take, filterParams.PageId);
        return model;
    }
    public async Task<bool> IsEmailExist(string email)
    {
        return await _personRepository.ExistsAsync(f => f.Email == email.ToLower());
    }
    public async Task<bool> IsPhoneNumberExist(string phoneNumber)
    {
        return await _personRepository.ExistsAsync(f => f.PhoneNumber == phoneNumber);

    }
    public async Task<bool> IsNatinalCodeExist(string natinalCode)
    {
        return await _personRepository.ExistsAsync(f => f.NatinalCode == natinalCode);
    }

    private PersonDto MapPerson(Person person)
    {
        return new PersonDto
        {
            Name = person.Name,
            Family = person.Family,
            FatherName = person.FatherName,
            PhoneNumber = person.PhoneNumber,
            NatinalCode = person.NatinalCode,
            Email = person.Email,
            Avatar = person.Avatar,
            Id = person.Id
        };
    }
}