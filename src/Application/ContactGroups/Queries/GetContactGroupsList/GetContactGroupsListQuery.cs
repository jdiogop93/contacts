using AutoMapper;
using Contacts.Application.Common;
using Contacts.Application.Common.Interfaces;
using Contacts.Application.Common.Mappings;
using Contacts.Application.Common.Models;
using Contacts.Application.ContactGroups.Queries.GetContactGroupsList;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Contacts.Queries.GetContactsList;

//[Authorize]
public record GetContactGroupsListQuery : IRequest<PaginatedList<ContactGroupListItemDto>>
{
    public string? SortBy { get; set; }
    public bool? SortDesc { get; set; }
    public int RowsPerPage { get; set; }
    public int Page { get; set; }
    public string? Search { get; set; }
}

public class GetContactGroupsListQueryHandler : IRequestHandler<GetContactGroupsListQuery, PaginatedList<ContactGroupListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactGroupsListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ContactGroupListItemDto>> Handle(GetContactGroupsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ContactGroups
            .Include(x => x.Contacts.Where(c => c.Active))
            .ThenInclude(x => x.Contact.Numbers.Where(n => n.Default && x.Active).Take(1))
            .Where(x => x.Active);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                x.Name.Contains(request.Search)
                || x.Contacts.Any(c =>
                    c.Contact.FirstName.Contains(request.Search)
                    || c.Contact.LastName.Contains(request.Search)
                    || c.Contact.Initials.Contains(request.Search)
                    || c.Contact.LastName.Contains(request.Search)
                    || c.Contact.Email.Contains(request.Search)
                )
            );
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy) && request.SortDesc.HasValue) //TODO => sort by other fields
        {
            if (request.SortDesc.Value)
            {
                query = query.OrderByDescending(request.SortBy);
            }
            else
            {
                query = query.OrderBy(request.SortBy);
            }
        }

        return await query
            .Select(x => new ContactGroupListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Contacts = x.Contacts == null
                    ? new List<ContactGroupContactListItemDto>()
                    : x.Contacts.Select(c => new ContactGroupContactListItemDto
                    {
                        Id = c.Id,
                        ContactId = c.ContactId,
                        FirstName = c.Contact.FirstName,
                        LastName = c.Contact.LastName,
                        Initials = c.Contact.Initials,
                        Email = c.Contact.Email
                    }).ToList()
            })
            .PaginatedListAsync(request.Page, request.RowsPerPage);
    }

}