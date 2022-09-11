namespace Contacts.Application.Contacts.Commands.Common;

public class ContactDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AddressDto Address { get; set; }
    public string Email { get; set; }
    public IList<ContactNumberDto> Numbers { get; set; }
}