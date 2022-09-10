namespace Contacts.Application.Contacts.Commands.Common;

public class ContactNumberDto
{
    public int? Id { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public ContactNumberTypeEnum Type { get; set; }
    public bool Default { get; set; }
    public bool ToDelete { get; set; }
}