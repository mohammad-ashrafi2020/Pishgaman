using Application.Utils;

namespace Application.Persons.DTOs;

public class PersonFilterParams : BaseFilterParam
{
    public string? Name { get; set; }
    public string? NatinalCode { get; set; }
}