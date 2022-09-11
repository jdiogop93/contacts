using Contacts.Application.Contacts.Commands.Common;
using FluentValidation;

namespace Contacts.Application.Contacts.Commands.UpdateContact;

public class UpdateContactValidator : AbstractValidator<UpdateContactCommand>
{
    public UpdateContactValidator()
    {
        #region FirstName
        RuleFor(x => x.FirstName)
            .NotNull().NotEmpty().WithMessage("The field «FirstName» is mandatory.");

        RuleFor(x => x.FirstName)
            .MaximumLength(200).WithMessage("You can write a maximum of 200 characters in «FirstName».");
        #endregion

        #region LastName
        RuleFor(x => x.LastName)
            .NotNull().NotEmpty().WithMessage("The field «LastName» is mandatory.");

        RuleFor(x => x.LastName)
            .MaximumLength(200).WithMessage("You can write a maximum of 200 characters in «LastName».");
        #endregion

        #region Address
        RuleFor(x => x.Address)
            .NotNull().WithMessage("You should complete contact address.");

        #region Street
        RuleFor(x => x.Address.Street)
            .NotNull().NotEmpty().WithMessage("The field «Address.Street» is mandatory.");

        RuleFor(x => x.Address.Street)
            .MaximumLength(400).WithMessage("You can write a maximum of 400 characters in «Address.Street».");
        #endregion

        #region ZipCode
        RuleFor(x => x.Address.ZipCode)
            .NotNull().NotEmpty().WithMessage("The field «Address.ZipCode» is mandatory.");

        RuleFor(x => x.Address.ZipCode)
            .MaximumLength(20).WithMessage("You can write a maximum of 20 characters in «Address.ZipCode».");
        #endregion

        #region City
        RuleFor(x => x.Address.City)
            .NotNull().NotEmpty().WithMessage("The field «Address.City» is mandatory.");

        RuleFor(x => x.Address.City)
            .MaximumLength(100).WithMessage("You can write a maximum of 100 characters in «Address.City».");
        #endregion

        #region Country
        RuleFor(x => x.Address.Country)
            .NotNull().NotEmpty().WithMessage("The field «Address.Country» is mandatory.");

        RuleFor(x => x.Address.Country)
            .MaximumLength(100).WithMessage("You can write a maximum of 100 characters in «Address.Country».");
        #endregion
        #endregion

        #region Email
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().WithMessage("The field «Email» is mandatory.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("You should insert a valid email.");

        RuleFor(x => x.Email)
            .MaximumLength(200).WithMessage("You can write a maximum of 200 characters in «Email».");
        #endregion

        #region Numbers
        var numberTypes = new List<ContactNumberTypeEnum>
        {
            ContactNumberTypeEnum.HOME,
            ContactNumberTypeEnum.MOBILE,
            ContactNumberTypeEnum.WORK
        };
        RuleForEach(x => x.Numbers)
            .ChildRules(n =>
            {
                n.RuleFor(y => y.CountryCode)
                    .NotNull().NotEmpty().WithMessage("The field «Numbers.CountryCode» is mandatory.")
                    .MaximumLength(5).WithMessage("You can write a maximum of 5 characters in «Numbers.CountryCode».");

                n.RuleFor(y => y.PhoneNumber)
                    .NotNull().NotEmpty().WithMessage("The field «Numbers.PhoneNumber» is mandatory.")
                    .MaximumLength(20).WithMessage("You can write a maximum of 20 characters in «Numbers.PhoneNumber».");

                n.RuleFor(y => y.Type)
                    .NotNull().WithMessage("The field «Numbers.Type» is mandatory.")
                    .IsInEnum().WithMessage($"You did not insert a valid value for «Type». The available values are: {string.Join(", ", numberTypes.Select(x => $"{(int)x}-{x}"))}.");
            })
            .When(x => x.Numbers != null && x.Numbers.Count > 0);
        #endregion
    }
}
