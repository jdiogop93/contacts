using AutoMapper;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Application.Contacts.Commands.Common;
using Contacts.Application.Contacts.Common;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Contacts.Queries.GetDetailedContact;

//[Authorize]
public record GetDetailedContactQuery(int Id) : IRequest<ContactDetailedDto>;

public class GetDetailedContactQueryHandler : IRequestHandler<GetDetailedContactQuery, ContactDetailedDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDetailedContactQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContactDetailedDto> Handle(GetDetailedContactQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .Where(l => l.Id == request.Id && l.Active)
            .Include(x => x.Numbers.Where(n => n.Active))
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Contact), request.Id);
        }

        var contactDto = new ContactDetailedDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Initials = entity.Initials,
            Street = entity.Address.Street,
            ZipCode = entity.Address.ZipCode,
            City = entity.Address.City,
            Country = entity.Address.Country,
            Email = entity.Email
        };

        if (entity.Numbers != null)
        {
            contactDto.Numbers = entity.Numbers
                .Select(n => new ContactNumberDetailedDto
                {
                    Id = n.Id,
                    CountryCode = n.CountryCode,
                    PhoneNumber = n.PhoneNumber,
                    Type = (ContactNumberTypeEnum)n.Type,
                    Default = n.Default
                })
                .ToList();
        }

        return contactDto;
    }
}
