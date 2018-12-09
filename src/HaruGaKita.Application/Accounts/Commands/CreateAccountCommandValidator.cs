using FluentValidation;

namespace HaruGaKita.Application.Accounts.Commands
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).Equal(x => x.Password).MinimumLength(8);
        }
    }
}