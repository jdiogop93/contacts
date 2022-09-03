namespace Contacts.Domain.Entities;

public class ContactNumber : BaseAuditableEntity
{
    public int ContactId { get; set; }
    public Contact Contact { get; set; }
    public string CountryCode { get; set; }
    public string PhoneNumber { get; set; }
    public ContactNumberType Type { get; set; }
}