namespace Contacts.Application.Contacts.Queries.GetDetailedContact;

public class ContactDetailedDto
{
    //Photo //TODO
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Initials { get; set; }

    #region Address
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    #endregion

    public string Email { get; set; }

    public IList<ContactNumberDetailedDto> Numbers { get; set; } = new List<ContactNumberDetailedDto>();
}

