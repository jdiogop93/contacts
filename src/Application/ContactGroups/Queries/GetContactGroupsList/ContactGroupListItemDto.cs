using Contacts.Application.ContactGroups.Common;
using Contacts.Application.Contacts.Common;

namespace Contacts.Application.ContactGroups.Queries.GetContactGroupsList;

public class ContactGroupListItemDto : ContactGroupDto
{
    public IList<ContactGroupContactListItemDto> Contacts { get; set; } = new List<ContactGroupContactListItemDto>();
}