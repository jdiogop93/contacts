using Contacts.Application.Contacts.Commands.Common;

namespace Contacts.Application.ContactGroups.Common;

public class ContactGroupContactDto
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public int ContactNumberId { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public ContactNumberTypeEnum Type { get; set; }
    public bool Default { get; set; }
}