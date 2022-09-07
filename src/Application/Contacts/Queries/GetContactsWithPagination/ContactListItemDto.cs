using Contacts.Application.Common.Mappings;
using Contacts.Domain.Entities;

namespace Contacts.Application.Contacts.Queries.GetContactsWithPagination;

public class ContactListItemDto : IMapFrom<Contact>
{
    public int Id { get; set; }

    //Photo //TODO

    public string Name { get; set; }

    public string DefaultPhoneNumber { get; set; } //TODO
}