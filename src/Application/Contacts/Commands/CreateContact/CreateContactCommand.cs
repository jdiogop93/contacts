using Contacts.Application.Common.Interfaces;
using Contacts.Application.Contacts.Commands.Common;
using Contacts.Domain.Entities;
using Contacts.Domain.Enums;
using MediatR;

namespace Contacts.Application.Contacts.Commands.CreateContact;

public record CreateContactCommand : IRequest<int>
{
    //photo //TODOzd

    public string FirstName { get; set; }
    public string LastName { get; set; }

    #region Address
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    #endregion

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
        var entity = new Contact
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = new Domain.ValueObjects.Address(request.Street, request.ZipCode, request.City, request.Country),
            Email = request.Email,
            Numbers = request.Numbers
                .Select(n => new ContactNumber
                {
                    CountryCode = n.CountryCode,
                    PhoneNumber = n.PhoneNumber,
                    Type = (ContactNumberType)n.Type,
                    Default = n.Default
                })
                .ToList()
        };
        entity.Initials = $"{entity.FirstName[0]}{entity.LastName[0]}";

        entity.Created = DateTime.UtcNow;

        _context.Contacts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

}
