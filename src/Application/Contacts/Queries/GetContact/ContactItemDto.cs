namespace Contacts.Application.Contacts.Queries.GetContact;

public class ContactItemDto
{
    //Photo //TODO
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    #region Address
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    #endregion

    public string DefaultPhoneNumber { get; set; }
}