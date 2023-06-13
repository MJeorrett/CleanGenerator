namespace CleanGenerator.Templates.TestDtos;

public partial class TestDtoTemplate
{
    private readonly TemplateModel _model;

    public TestDtoTemplate(TemplateModel model)
    {
        _model = model.CloneWithoutIdPropertyConfig();
    }
}
