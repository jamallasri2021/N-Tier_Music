using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API
{
    public class ArtistToSaveResourceValidator: AbstractValidator<ArtistToSaveResource>
    {
        public ArtistToSaveResourceValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);
        }
    }
}
