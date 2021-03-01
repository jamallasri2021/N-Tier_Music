using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API
{
    public class ComposerToSaveResourceValidator: AbstractValidator<ComposerToSaveResource>
    {
        public ComposerToSaveResourceValidator()
        {
            RuleFor(c => c.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(c => c.LastName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);
        }
    }
}
