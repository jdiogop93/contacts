using Contacts.Application.Contacts.Commands.Common;

namespace Contacts.Application.Contacts.Commands.CreateContact;

public class CreateContactNumberItemDto
{
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public ContactNumberTypeEnum Type { get; set; }
    public bool Default { get; set; }
}