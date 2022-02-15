using System.Text.RegularExpressions;

using FluentValidation;

using Project_Login.ViewModels;

namespace Project_Login.Validators;

public class RegisterUserViewModelValidator : AbstractValidator<RegisterUserViewModel>
{
    public RegisterUserViewModelValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress()
            .WithMessage("O e-mail está em um formato inválido");

        RuleFor(u => u.Password)
            .Must(ValidPassword)
            .WithMessage("Senha deve conter pelo menos 6 caracteres, um número, uma letra maiúscula, uma minúscula, e um caractere especial");
        
        RuleFor(u => u.ConfirmPassword)
            .Must((model, field) => model.Password.Equals(field))
            .WithMessage("As senhas não são iguais.");
    }
    
    public bool ValidPassword(string password)
    {
        var regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$");

        return regex.IsMatch(password);
    }
}
