namespace CleanGenerator.Templates.CreateCommand;

public partial class CreateCommandValidatorTemplate
{
    private readonly TemplateModel _model;

    public CreateCommandValidatorTemplate(TemplateModel model)
    {
        _model = model.CloneWithoutIdPropertyConfig();
    }
}
