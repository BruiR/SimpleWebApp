using FluentValidation;
using SimpleWebApp.DTOs.User;

namespace SimpleWebApp.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator(CreateUserDtoValidator createUserDtoValidator)
        {
            Include(createUserDtoValidator);
        }
    }
}
