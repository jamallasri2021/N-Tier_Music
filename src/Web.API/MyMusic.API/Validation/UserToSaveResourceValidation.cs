using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API
{
    public class UserToSaveResourceValidation: AbstractValidator<UserToSave>
    {
        public UserToSaveResourceValidation()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(u => u.LastName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(u => u.UserName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);
        }
    }
}
