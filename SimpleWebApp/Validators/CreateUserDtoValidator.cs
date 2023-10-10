using FluentValidation;
using SimpleWebApp.DTOs.User;

namespace SimpleWebApp.Validators
{
    public class CreateUserDtoValidator: AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator() 
        {
            RuleFor(user => user.Age)
                .NotNull()
                .GreaterThan(0);

            RuleFor(user => user.Name)
                .NotEmpty();

            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
