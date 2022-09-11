using FluentValidation;

namespace Contacts.Application.Messages.Commands;

public class SendEmailContactGroupValidator : AbstractValidator<SendEmailContactGroupCommand>
{
    public SendEmailContactGroupValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().GreaterThan(0).WithMessage("The field «Id» (ContactGroupId) must be greater than 1.");

        RuleFor(x => x.Subject)
            .NotNull().NotEmpty().WithMessage("The field «Subject» is mandatory.");

        RuleFor(x => x.Content)
            .NotNull().NotEmpty().WithMessage("The field «Content» is mandatory.");
    }
}
