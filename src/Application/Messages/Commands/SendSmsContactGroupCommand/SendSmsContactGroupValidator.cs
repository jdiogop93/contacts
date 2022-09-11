using FluentValidation;

namespace Contacts.Application.Messages.Commands;

public class SendSmsContactGroupValidator : AbstractValidator<SendSmsContactGroupCommand>
{
    public SendSmsContactGroupValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().GreaterThan(0).WithMessage("The field «Id» (ContactGroupId) must be greater than 1.");

        RuleFor(x => x.Message)
            .NotNull().NotEmpty().WithMessage("The field «Message» is mandatory.");
    }
}
