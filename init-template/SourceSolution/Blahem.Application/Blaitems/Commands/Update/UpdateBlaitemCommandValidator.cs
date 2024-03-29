﻿using Blahem.Application.Blaitems.Commands.Create;
using FluentValidation;

namespace Blahem.Application.Blaitems.Commands.Update;

public class UpdateBlaitemCommandValidator : AbstractValidator<UpdateBlaitemCommand>
{
    public UpdateBlaitemCommandValidator()
    {
        RuleFor(_ => _.Title)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);
    }
}
