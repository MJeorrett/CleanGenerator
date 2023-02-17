using FluentValidation;

namespace Blahem.Application.Blaitems.Commands.Create;

public class CreateBlaitemCommandValidator : AbstractValidator<CreateBlaitemCommand>
{
    public CreateBlaitemCommandValidator()
    {
        RuleFor(_ => _.Title)
            .NotNull()
            .NotEmpty()
            .MaximumLength(256);
    }
}
