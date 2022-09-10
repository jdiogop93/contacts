using Contacts.Application.Contacts.Commands.Common;

namespace Contacts.Application.Contacts.Common;

public class ContactDetailedDto : ContactDto
{
    //Photo //TODO
    public int Id { get; set; }
    public string Initials { get; set; }

    public IList<ContactNumberDetailedDto> Numbers { get; set; } = new List<ContactNumberDetailedDto>();
}