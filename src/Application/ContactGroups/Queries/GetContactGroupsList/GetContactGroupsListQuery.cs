using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Contacts.Application.Common;
using Contacts.Application.Common.Interfaces;
using Contacts.Application.Common.Mappings;
using Contacts.Application.Common.Models;
using Contacts.Application.ContactGroups.Queries.GetContactGroupsList;
using Contacts.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.Contacts.Queries.GetContactsList;

//[Authorize]
public record GetContactGroupsListQuery : IRequest<PaginatedList<ContactGroupListItemDto>>
{
    public string? SortBy { get; set; }

    public bool? SortDesc { get; set; }

    [Required]
    public int RowsPerPage { get; set; }

    [Required]
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

    private enum SortByValuesEnum
    {
        Name,
        ContactName,
        ContactInitials,
        ContactEmail,
        DefaultPhoneNumber
    }

    public static string[] GetSortByValues()
    {
        return new string[]
        {
            SortByValuesEnum.Name.ToString().ToLower(),
            SortByValuesEnum.ContactName.ToString().ToLower(),
            SortByValuesEnum.ContactInitials.ToString().ToLower(),
            SortByValuesEnum.ContactEmail.ToString().ToLower(),
            SortByValuesEnum.DefaultPhoneNumber.ToString().ToLower()
        };
    }

    public async Task<PaginatedList<ContactGroupListItemDto>> Handle(GetContactGroupsListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ContactGroups
            .Include(x => x.Contacts.Where(c => c.Active && c.Contact.Active))
            .ThenInclude(x => x.Contact.Numbers)
            .Where(x => x.Active);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                x.Name.Contains(request.Search)
                || x.Contacts.Any(c =>
                    c.Contact.FirstName.Contains(request.Search)
                    || c.Contact.LastName.Contains(request.Search)
                    || c.Contact.Initials.Contains(request.Search)
                    || c.Contact.Email.Contains(request.Search)
                    || c.Contact.Numbers.Any(n => n.CountryCode.Contains(request.Search) || n.PhoneNumber.Contains(request.Search))
                )
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
            .Select(x => new ContactGroupListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Contacts = x.Contacts
                    .Select(c => new ContactGroupContactListItemDto
                    {
                        Id = c.Id,
                        ContactId = c.ContactId,
                        FirstName = c.Contact.FirstName,
                        LastName = c.Contact.LastName,
                        Initials = c.Contact.Initials,
                        Email = c.Contact.Email,
                        DefaultPhoneNumber = c.Contact.Numbers.Count(y => y.Default && y.Active) > 0 ? $"{c.Contact.Numbers.First(y => y.Default && y.Active).CountryCode} {c.Contact.Numbers.First(y => y.Default && y.Active).PhoneNumber}" : "-"
                    })
                    .ToList()
            })
            .PaginatedListAsync(request.Page, request.RowsPerPage);
    }

    #region private methods
    private IOrderedQueryable<ContactGroup> ApplyOrderBy(IQueryable<ContactGroup> query, string field, bool sortDesc)
    {
        if (sortDesc)
        {
            if (field == SortByValuesEnum.Name.ToString().ToLower())
            {
                return query.OrderByDescending(x => x.Name);
            }
            else if (field == SortByValuesEnum.ContactName.ToString().ToLower())
            {
                return query
                    .OrderByDescending(x =>
                        x.Contacts.Select(c => c.Contact).Max(y => y.FirstName))
                    .ThenByDescending(x =>
                        x.Contacts.Select(c => c.Contact).Max(y => y.LastName));
            }
            else if (field == SortByValuesEnum.ContactInitials.ToString().ToLower())
            {
                return query
                    .OrderByDescending(x =>
                        x.Contacts.Select(c => c.Contact).Max(y => y.Initials));
            }
            else if (field == SortByValuesEnum.ContactEmail.ToString().ToLower())
            {
                return query
                    .OrderByDescending(x =>
                        x.Contacts.Select(c => c.Contact).Max(y => y.Email));
            }
            else //DefaultPhoneNumber
            {
                return query
                    .OrderByDescending(x =>
                        x.Contacts.SelectMany(c => c.Contact.Numbers).Max(y => y.CountryCode))
                    .ThenByDescending(x =>
                        x.Contacts.SelectMany(c => c.Contact.Numbers).Max(y => y.PhoneNumber));
            }
        }
        else
        {
            if (field == SortByValuesEnum.Name.ToString().ToLower())
            {
                return query.OrderBy(x => x.Name);
            }
            else if (field == SortByValuesEnum.ContactName.ToString().ToLower())
            {
                return query
                    .OrderBy(x =>
                        x.Contacts.Select(c => c.Contact).Min(y => y.FirstName))
                    .ThenBy(x =>
                        x.Contacts.Select(c => c.Contact).Min(y => y.LastName));
            }
            else if (field == SortByValuesEnum.ContactInitials.ToString().ToLower())
            {
                return query
                    .OrderBy(x =>
                        x.Contacts.Select(c => c.Contact).Min(y => y.Initials));
            }
            else if (field == SortByValuesEnum.ContactEmail.ToString().ToLower())
            {
                return query
                    .OrderBy(x =>
                        x.Contacts.Select(c => c.Contact).Min(y => y.Email));
            }
            else //DefaultPhoneNumber
            {
                return query
                    .OrderBy(x =>
                        x.Contacts.SelectMany(c => c.Contact.Numbers).Min(y => y.CountryCode))
                    .ThenBy(x =>
                        x.Contacts.SelectMany(c => c.Contact.Numbers).Min(y => y.PhoneNumber));
            }
        }
    }
    #endregion

}