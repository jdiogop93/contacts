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
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.")
            .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
    }

    public async Task<bool> BeUniqueName(UpdateContactGroupCommand model, string name, CancellationToken cancellationToken)
    {
        return await _context.ContactGroups
            .Where(l => l.Id != model.Id && l.Active)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
