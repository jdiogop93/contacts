using System.ComponentModel.DataAnnotations;
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

    [Required]
    public int RowsPerPage { get; set; }

    [Required]
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

    private enum SortByValuesEnum
    {
        Name,
        DefaultPhoneNumber
    }

    public static string[] GetSortByValues()
    {
        return new string[]
        {
            SortByValuesEnum.Name.ToString().ToLower(),
            SortByValuesEnum.DefaultPhoneNumber.ToString().ToLower()
        };
    }

    public async Task<PaginatedList<ContactListItemDto>> Handle(GetContactsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Contacts
            .Include(x => x.Numbers.Where(n => n.Default && n.Active).Take(1))
            .Where(x => x.Active);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                x.FirstName.Contains(request.Search)
                || x.LastName.Contains(request.Search)
                || x.Numbers.Any(n => n.CountryCode.Contains(request.Search)
                || n.PhoneNumber.Contains(request.Search))
            );
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy) && request.SortDesc.HasValue)
        {
            var validValues = GetSortByValues().Select(x => x.ToLower());
            var sortByLower = request.SortBy.ToLower();

            var field = validValues.FirstOrDefault(x => x == sortByLower);
            if (field != null)
            {
                query = ApplyOrderBy(query, field, request.SortDesc.Value);
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

    #region private methods
    private IOrderedQueryable<Contact> ApplyOrderBy(IQueryable<Contact> query, string field, bool sortDesc)
    {
        if (sortDesc)
        {
            if (field == SortByValuesEnum.DefaultPhoneNumber.ToString().ToLower())
            {
                return query.OrderByDescending(x => x.Numbers.First().CountryCode)
                    .ThenByDescending(x => x.Numbers.First().PhoneNumber);
            }
            else //Name
            {
                return query.OrderByDescending(x => x.FirstName)
                    .ThenByDescending(x => x.LastName);
            }
        }
        else
        {
            if (field == SortByValuesEnum.DefaultPhoneNumber.ToString().ToLower())
            {
                return query.OrderBy(x => x.Numbers.First().CountryCode)
                    .ThenBy(x => x.Numbers.First().PhoneNumber);
            }
            else //Name
            {
                return query.OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName);
            }
        }
    }
    #endregion

}