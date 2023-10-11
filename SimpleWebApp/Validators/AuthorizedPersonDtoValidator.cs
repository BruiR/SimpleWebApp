using FluentValidation;
using SimpleWebApp.DTOs.AuthorizedPerson;

namespace SimpleWebApp.Validators
{
    public class AuthorizedPersonDtoValidator : AbstractValidator<AuthorizedPersonDto>
    {
        public AuthorizedPersonDtoValidator()
        {
            RuleFor(person => person.Login).NotEmpty();
            RuleFor(person => person.Password).NotEmpty();
        }
    }
}
