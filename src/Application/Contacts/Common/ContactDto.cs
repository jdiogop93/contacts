namespace Contacts.Application.Contacts.Commands.Common;

public class ContactDto
{
    //photo //TODOzd

    public string FirstName { get; set; }
    public string LastName { get; set; }

    #region Address
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    #endregion

    public string Email { get; set; }
    public IList<ContactNumberDto> Numbers { get; set; }
}