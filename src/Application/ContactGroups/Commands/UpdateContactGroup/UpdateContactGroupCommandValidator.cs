using Contacts.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Application.ContactGroups.Commands.UpdateContactGroup;

public class UpdateContactGroupCommandValidator : AbstractValidator<UpdateContactGroupCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateContactGroupCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty().NotEmpty().WithMessage("The field «Name» is mandatory.")
            .MaximumLength(200).WithMessage("You can write a maximum of 200 characters in «Name».")
            .MustAsync(BeUniqueName).WithMessage("The specified «Name» already exists.");

        RuleFor(x => x.ContactsIdsToSave)
            .NotNull().Must((x,y) => x.ContactsIdsToSave.Count > 0).WithMessage("The list «ContactsIdsToSave» must have elements.")
            .When(x => x.ContactsIdsToDelete.Count == 0);

        RuleFor(x => x.ContactsIdsToDelete)
            .NotNull().Must((x, y) => x.ContactsIdsToDelete.Count > 0).WithMessage("The list «ContactsIdsToDelete» must have elements.")
            .When(x => x.ContactsIdsToSave.Count == 0);
    }

    public async Task<bool> BeUniqueName(UpdateContactGroupCommand model, string name, CancellationToken cancellationToken)
    {
        return await _context.ContactGroups
            .Where(l => l.Id != model.Id && l.Active)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
