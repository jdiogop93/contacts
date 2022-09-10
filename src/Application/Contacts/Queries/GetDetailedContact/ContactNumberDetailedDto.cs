using Contacts.Application.Contacts.Commands.Common;

namespace Contacts.Application.Contacts.Queries.GetDetailedContact;

public class ContactNumberDetailedDto
{
    //Photo //TODO
    public int Id { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public ContactNumberTypeEnum Type { get; set; }
    public bool Default { get; set; }
}