using Contacts.Application.Contacts.Common;

namespace Contacts.Application.ContactGroups.Common;

public class ContactGroupContactDto : ContactNumberDetailedDto
{
    public int ContactId { get; set; }
    public int ContactNumberId { get; set; }
}