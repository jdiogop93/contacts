using System.ComponentModel.DataAnnotations;
using Contacts.Application.Common.Interfaces;
using Contacts.Application.Contacts.Commands.Common;
using Contacts.Application.Contacts.Common;
using Contacts.Domain.Entities;
using Contacts.Domain.Enums;
using Contacts.Domain.ValueObjects;
using MediatR;

namespace Contacts.Application.Contacts.Commands.CreateContact;

public record CreateContactCommand : IRequest<int>
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public AddressDto Address { get; set; }

    [Required]
    public string Email { get; set; }

    public HashSet<ContactNumberDto> Numbers { get; set; }
}


public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateContactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var currentTime = DateTime.UtcNow;

        var entity = new Contact
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = new Address(request.Address.Street, request.Address.ZipCode, request.Address.City, request.Address.Country),
            Email = request.Email,
            Numbers = request.Numbers
                .Select(n => new ContactNumber
                {
                    CountryCode = n.CountryCode,
                    PhoneNumber = n.PhoneNumber,
                    Type = (ContactNumberType)n.Type,
                    Default = n.Default,
                    Created = currentTime,
                })
                .ToList(),
            Created = currentTime
        };
        entity.Initials = ContactHelper.RetrieveInitialsOfNames(entity.FirstName, entity.LastName);

        _context.Contacts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

}
