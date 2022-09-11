using FluentValidation;

namespace Contacts.Application.Contacts.Queries.GetContactsList;

public class GetContactsListQueryValidator : AbstractValidator<GetContactsListQuery>
{
    public GetContactsListQueryValidator()
    {
        var sortByRangeValues = GetContactsListQueryHandler.GetSortByValues();

        RuleFor(x => x.SortBy)
            .Must((x, y) => sortByRangeValues.Any(s => s.ToLower() == x.SortBy?.ToLower()))
            .WithMessage($"You did not insert a valid value for «SortBy». You can only sort the list by: {string.Join(",", sortByRangeValues)}.")
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy) && x.SortDesc.HasValue);

        RuleFor(x => x.RowsPerPage)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");
    }
}
