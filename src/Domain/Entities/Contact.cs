namespace Contacts.Domain.Entities;

public class Contact : BaseAuditableEntity
{
    public Media? Photo { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Initials { get; set; }
    public Address Address { get; set; }
    public string Email { get; set; }

    public ICollection<ContactNumber> Numbers { get; set; } //used to save numbers at the same time of this contact
    public ICollection<ContactGroupContact> Groups { get; set; }
}
