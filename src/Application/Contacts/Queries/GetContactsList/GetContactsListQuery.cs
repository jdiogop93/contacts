using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        #region the best way - less code repetitions
        //var query = _context.Contacts;

        //if (!string.IsNullOrWhiteSpace(request.Search)) //TODO => filter by other fields
        //{
        //    query.Where(x => x.FirstName.Contains(request.Search) || x.LastName.Contains(request.Search));
        //}

        //if (!string.IsNullOrWhiteSpace(request.SortBy) && request.SortDesc.HasValue) //TODO => sort by other fields
        //{
        //    if (request.SortDesc.Value)
        //    {
        //        query.OrderByDescending(x => x.FirstName);
        //    }
        //    else
        //    {
        //        query.OrderBy(x => x.FirstName);
        //    }
        //}

        //return await query
        //    .Include(x => x.Numbers.Take(1))
        //    .Select(x => new ContactListItemDto
        //    {
        //        Id = x.Id,
        //        Name = $"{x.FirstName} {x.LastName}",
        //        DefaultPhoneNumber = x.Numbers.Count > 0 ? $"{x.Numbers.First().CountryCode} {x.Numbers.First().PhoneNumber}" : "-"
        //    })
        //    .PaginatedListAsync(request.Page, request.RowsPerPage);
        #endregion

        #region the way that works
        // sort + search
        if (!string.IsNullOrWhiteSpace(request.SortBy) && request.SortDesc.HasValue && !string.IsNullOrWhiteSpace(request.Search))
        {
            if (request.SortDesc.Value) //desc
            {
                return await _context.Contacts
                    .Include(x => x.Numbers.Take(1))
                    .Where(x => x.FirstName.Contains(request.Search) || x.LastName.Contains(request.Search))
                    .OrderByDescending(x => x.FirstName)
                    .Select(x => new ContactListItemDto
                    {
                        Id = x.Id,
                        Name = $"{x.FirstName} {x.LastName}",
                        DefaultPhoneNumber = x.Numbers.Count > 0 ? $"{x.Numbers.First().CountryCode} {x.Numbers.First().PhoneNumber}" : "-"
                    })
                    .PaginatedListAsync(request.Page, request.RowsPerPage);
            }
            else //asc
            {
                return await _context.Contacts
                    .Include(x => x.Numbers.Take(1))
                    .Where(x => x.FirstName.Contains(request.Search) || x.LastName.Contains(request.Search))
                    .OrderBy(x => x.FirstName)
                    .Select(x => new ContactListItemDto
                    {
                        Id = x.Id,
                        Name = $"{x.FirstName} {x.LastName}",
                        DefaultPhoneNumber = x.Numbers.Count > 0 ? $"{x.Numbers.First().CountryCode} {x.Numbers.First().PhoneNumber}" : "-"
                    })
                    .PaginatedListAsync(request.Page, request.RowsPerPage);
            }
        }
        // just sort
        else if (!string.IsNullOrWhiteSpace(request.SortBy) && request.SortDesc.HasValue)
        {
            if (request.SortDesc.Value) //desc
            {
                return await _context.Contacts
                    .Include(x => x.Numbers.Take(1))
                    .OrderByDescending(x => x.FirstName)
                    .Select(x => new ContactListItemDto
                    {
                        Id = x.Id,
                        Name = $"{x.FirstName} {x.LastName}",
                        DefaultPhoneNumber = x.Numbers.Count > 0 ? $"{x.Numbers.First().CountryCode} {x.Numbers.First().PhoneNumber}" : "-"
                    })
                    .PaginatedListAsync(request.Page, request.RowsPerPage);
            }
            else //asc
            {
                return await _context.Contacts
                    .Include(x => x.Numbers.Take(1))
                    .OrderBy(x => x.FirstName)
                    .Select(x => new ContactListItemDto
                    {
                        Id = x.Id,
                        Name = $"{x.FirstName} {x.LastName}",
                        DefaultPhoneNumber = x.Numbers.Count > 0 ? $"{x.Numbers.First().CountryCode} {x.Numbers.First().PhoneNumber}" : "-"
                    })
                    .PaginatedListAsync(request.Page, request.RowsPerPage);
            }
        }
        // just search
        else if (!string.IsNullOrWhiteSpace(request.Search))
        {
            return await _context.Contacts
                .Include(x => x.Numbers.Take(1))
                .Where(x => x.FirstName.Contains(request.Search) || x.LastName.Contains(request.Search))
                .Select(x => new ContactListItemDto
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    DefaultPhoneNumber = x.Numbers.Count > 0 ? $"{x.Numbers.First().CountryCode} {x.Numbers.First().PhoneNumber}" : "-"
                })
                .PaginatedListAsync(request.Page, request.RowsPerPage);
        }
        else
        {
            return await _context.Contacts
                .Include(x => x.Numbers.Take(1))
                .Select(x => new ContactListItemDto
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.LastName}",
                    DefaultPhoneNumber = x.Numbers.Count > 0 ? $"{x.Numbers.First().CountryCode} {x.Numbers.First().PhoneNumber}" : "-"
                })
                .PaginatedListAsync(request.Page, request.RowsPerPage);
        }
        #endregion
    }

    private string RetrieveCompleteName(Contact x)
    {
        return $"{x.FirstName} {x.LastName}";
    }

    private ContactListItemDto BuildContactListItemDto(Contact x)
    {
        return new ContactListItemDto
        {
            Id = x.Id,
            Name = $"{x.FirstName} {x.LastName}",
            DefaultPhoneNumber = x.Numbers.Count > 0 ? $"{x.Numbers.First().CountryCode} {x.Numbers.First().PhoneNumber}" : "-"
        };
    }

}
