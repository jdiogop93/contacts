using System.ComponentModel.DataAnnotations;
using Contacts.Application.Contacts.Common;

namespace Contacts.Application.Contacts.Queries.GetDetailedContact;

public class ContactDetailedItemDto : ContactDetailedDto
{
    public IList<ContactNumberItemDetailedDto> Numbers { get; set; } = new List<ContactNumberItemDetailedDto>();
}

public class ContactNumberItemDetailedDto : ContactNumberDetailedDto
{
    [Required]
    public string Type { get; set; }
}