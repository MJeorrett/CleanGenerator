﻿namespace CleanGenerator.Templates.UpdateCommand;

public partial class UpdateCommandTemplate
{
    private readonly TemplateModel _model;

    public UpdateCommandTemplate(TemplateModel model)
    {
        _model = model.CloneWithoutIdPropertyConfig();
    }
}
