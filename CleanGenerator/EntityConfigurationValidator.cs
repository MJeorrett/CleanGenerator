using FluentValidation;

namespace CleanGenerator;
internal class EntityConfigurationValidator : AbstractValidator<EntityConfiguration>
{
    public EntityConfigurationValidator()
    {
        RuleFor(_ => _.EntityName)
            .NotEmpty()
            .WithMessage("EntityName must be provided.");

        RuleForEach(_ => _.Properties)
            .ChildRules(_ =>
            {
                _.RuleFor(_ => _.Name)
                    .NotEmpty()
                    .WithMessage($"Property[{{CollectionIndex}}].Name must be provided.");

                _.RuleFor(_ => _.Type)
                    .NotEmpty()
                    .WithMessage($"Property[{{CollectionIndex}}].Type must be provided.");
            });
    }
}