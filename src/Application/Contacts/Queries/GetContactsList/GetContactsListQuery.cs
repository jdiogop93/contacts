using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AutoMapper;
using Contacts.Application.Common;
using Contacts.Application.Common.Interfaces;
using Contacts.Application.Common.Mappings;
using Contacts.Application.Common.Models;
using Contacts.Application.Contacts.Common;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Contacts.Queries.GetContactsList;

//[Authorize]
public record GetContactsListQuery : IRequest<PaginatedList<ContactListItemDto>>
{
    public string? SortBy { get; set; }
    public bool? SortDesc { get; set; }
    public int RowsPerPage { get; set; }
    public int Page { get; set; }
    public string? Search { get; set; }
}

public class GetContactsListQueryHandler : IRequestHandler<GetContactsListQuery, PaginatedList<ContactListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetContactsListQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ContactListItemDto>> Handle(GetContactsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Contacts
            .Include(x => x.Numbers.Where(n => n.Default && n.Active).Take(1))
            .Where(x => x.Active);

        if (!string.IsNullOrWhiteSpace(request.Search)) //TODO => filter by other fields
        {
            query = query.Where(x =>
                x.FirstName.Contains(request.Search)
                || x.LastName.Contains(request.Search)
                || x.Numbers.Any(n => n.CountryCode.Contains(request.Search) 
                || n.PhoneNumber.Contains(request.Search))
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
            .Select(x => new ContactListItemDto
            {
                Id = x.Id,
                Name = $"{x.FirstName} {x.LastName}",
                DefaultPhoneNumber = x.Numbers.Count > 0 ? $"{x.Numbers.First().CountryCode} {x.Numbers.First().PhoneNumber}" : "-"
            })
            .PaginatedListAsync(request.Page, request.RowsPerPage);
    }

}