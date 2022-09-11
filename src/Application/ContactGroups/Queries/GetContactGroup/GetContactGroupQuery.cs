using AutoMapper;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Interfaces;
using Contacts.Application.ContactGroups.Common;
using Contacts.Application.Contacts.Commands.Common;
using Contacts.Application.Contacts.Common;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.ContactGroups.Queries.GetContactGroup;

//[Authorize]
public record GetContactGroupQuery(int Id) : IRequest<ContactGroupDto>;

public class GetContactGroupQueryHandler : IRequestHandler<GetContactGroupQuery, ContactGroupDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactGroupQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ContactGroupDto> Handle(GetContactGroupQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.ContactGroups
            .Where(l => l.Id == request.Id && l.Active)
            .Include(x => x.Contacts.Where(n => n.Active))
            .ThenInclude(x => x.Contact.Numbers.Where(n => n.Active))
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(ContactGroup), request.Id);
        }

        var contactGroupDto = new ContactGroupDto
        {
            Id = entity.Id,
            Name = entity.Name
        };

        if (entity.Contacts != null)
        {
            foreach (var cgc in entity.Contacts)
            {
                var c = cgc.Contact;
                var contact = new ContactDetailedDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Initials = c.Initials,
                    Address = new AddressDto
                    {
                        Street = c.Address.Street,
                        ZipCode = c.Address.ZipCode,
                        City = c.Address.City,
                        Country = c.Address.Country
                    },
                    Email = c.Email
                };

                foreach (var n in c.Numbers)
                {
                    contact.Numbers.Add(new ContactNumberDetailedDto
                    {
                        Id = n.Id,
                        CountryCode = n.CountryCode,
                        PhoneNumber = n.PhoneNumber,
                        Type = (ContactNumberTypeEnum)n.Type,
                        Default = n.Default
                    });
                }

                contactGroupDto.Contacts.Add(contact);
            }
        }

        return contactGroupDto;
    }
}
