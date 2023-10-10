using FluentValidation;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs.AuthorizedPerson;
using SimpleWebApp.DTOs.User;

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
