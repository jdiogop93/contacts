namespace Contacts.Domain.Entities;

public class ContactGroup : BaseAuditableEntity
{
    public string Name { get; set; }

    public ICollection<Contact> Contacts { get; set; }
}