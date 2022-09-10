using Contacts.Application.ContactGroups.Common;

namespace Contacts.Application.ContactGroups.Queries.GetContactGroupsList;

public class ContactGroupContactListItemDto
{
    public int Id { get; set; }
    public int ContactId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Initials { get; set; }
    public string Email { get; set; }
}