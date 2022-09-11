using Contacts.Application.Contacts.Commands.Common;

namespace Contacts.Application.Contacts.Common;

public class ContactItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public AddressDto Address { get; set; }
    public string DefaultPhoneNumber { get; set; }
}