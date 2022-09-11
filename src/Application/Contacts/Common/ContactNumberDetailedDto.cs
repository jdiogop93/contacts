using System.ComponentModel.DataAnnotations;
using Contacts.Application.Contacts.Commands.Common;

namespace Contacts.Application.Contacts.Common;

public class ContactNumberDetailedDto
{
    public int Id { get; set; }

    [Required]
    public string CountryCode { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public ContactNumberTypeEnum Type { get; set; }

    public bool Default { get; set; }
}