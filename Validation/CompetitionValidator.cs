using FluentValidation;
using GlsLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlsLeague.Validation
{
    public class CompetitionValidator : AbstractValidator<Competition>
    {
        public CompetitionValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Musisz podać nazwę zawodów!");
        }
    }
}