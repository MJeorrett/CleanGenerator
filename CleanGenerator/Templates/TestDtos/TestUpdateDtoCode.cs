namespace CleanGenerator.Templates.TestDtos;

public partial class TestUpdateDtoTemplate
{
    private readonly TemplateModel _model;

    public TestUpdateDtoTemplate(TemplateModel model)
    {
        _model = model.CloneWithoutIdPropertyConfig();
    }
}
