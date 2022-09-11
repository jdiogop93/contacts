using Contacts.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.ContactGroups.Commands.CreateContactGroup;

public class CreateContactGroupValidator : AbstractValidator<CreateContactGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateContactGroupValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotNull().NotEmpty().WithMessage("The field «Name» is mandatory.")
            .MaximumLength(200).WithMessage("You can write a maximum of 200 characters in «Name».")
            .MustAsync(BeUniqueName).WithMessage("The specified «Name» already exists.");
    }

    public async Task<bool> BeUniqueName(CreateContactGroupCommand model, string name, CancellationToken cancellationToken)
    {
        return await _context.ContactGroups
            .Where(l => l.Active)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
