using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMusic.API
{
    public class MusicToSaveResourceValidator: AbstractValidator<MusicToSaveResource>
    {
        public MusicToSaveResourceValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(m => m.ArtistId)
                .NotEmpty()
                .WithMessage("Artist ID must not be 0 !");
        }
    }
}
