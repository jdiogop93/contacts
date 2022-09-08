using FluentValidation;

namespace Contacts.Application.Contacts.Queries.GetContactsList;

public class GetContactsListQueryValidator : AbstractValidator<GetContactsListQuery>
{
    public GetContactsListQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.RowsPerPage)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
