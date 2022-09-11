using System.ComponentModel.DataAnnotations;
using Contacts.Application.Contacts.Commands.Common;

namespace Contacts.Application.Contacts.Commands.UpdateContact;

public class ContactNumberItemDto : ContactNumberDto
{
    [Required]
    public int Type { get; set; }
}