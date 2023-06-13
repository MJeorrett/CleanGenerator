namespace CleanGenerator.Templates.TestDtos;

public partial class TestCreateDtoTemplate
{
    private readonly TemplateModel _model;

    public TestCreateDtoTemplate(TemplateModel model)
    {
        _model = model.CloneWithoutIdPropertyConfig();
    }
}
