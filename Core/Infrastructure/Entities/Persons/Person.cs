using Infrastructure.Utils;

namespace Infrastructure.Entities.Persons;

public class Person : BaseEntity
{
    public string Name { get; set; }
    public string Family { get; set; }
    public string FatherName { get; set; }
    public string PhoneNumber { get; set; }
    public string NatinalCode { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
}