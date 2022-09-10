using Contacts.Application.Contacts.Common;

namespace Contacts.Application.Contacts.Commands.Common;

public class ContactNumberDto : ContactNumberDetailedDto
{
    public int? Id { get; set; }
    public bool ToDelete { get; set; }
}