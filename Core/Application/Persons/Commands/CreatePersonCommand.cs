using Microsoft.AspNetCore.Http;

namespace Application.Persons.Commands;

public class CreatePersonCommand
{
    public string Name { get; set; }
    public string Family { get; set; }
    public string FatherName { get; set; }
    public string PhoneNumber { get; set; }
    public string NatinalCode { get; set; }
    public string Email { get; set; }
    public IFormFile Avatar { get; set; }
}
public class EditPersonCommand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Family { get; set; }
    public string FatherName { get; set; }
    public string PhoneNumber { get; set; }
    public string NatinalCode { get; set; }
    public string Email { get; set; }
    public IFormFile? Avatar { get; set; }

}