using FluentValidation;

using Project_Login.ViewModels;

namespace Project_Login.Validators;

public class LoginUserViewModelValidator : AbstractValidator<LoginUserViewModel>
{
    public LoginUserViewModelValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress()
            .WithMessage("O e-mail está em um formato inválido");
    }
}
