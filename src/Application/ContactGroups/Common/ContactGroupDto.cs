using Contacts.Application.Contacts.Common;

namespace Contacts.Application.ContactGroups.Common;

public class ContactGroupDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public IList<ContactDetailedDto> Contacts { get; set; } = new List<ContactDetailedDto>();
}