using AutoMapper;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Contacts.Queries.GetContact;

//[Authorize]
public record GetContactQuery(int Id) : IRequest<ContactItemDto>;

public class GetContactQueryHandler : IRequestHandler<GetContactQuery, ContactItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContactItemDto> Handle(GetContactQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Contacts
            .Where(l => l.Id == request.Id && l.Active)
            .Include(x => x.Numbers.Where(n => n.Default && n.Active).Take(1))
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Contact), request.Id);
        }

        return new ContactItemDto
        {
            Id = entity.Id,
            Name = $"{entity.FirstName} {entity.LastName}",
            Email = entity.Email,
            Street = entity.Address.Street,
            ZipCode = entity.Address.ZipCode,
            City = entity.Address.City,
            Country = entity.Address.Country,
            DefaultPhoneNumber = entity.Numbers.Count > 0 ? $"{entity.Numbers.First().CountryCode} {entity.Numbers.First().PhoneNumber}" : "-"
        };
    }
}
