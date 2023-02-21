using Blahem.Application;
using Blahem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

#region addServices

services.AddControllers();

services.AddApplication();
services.AddInfrastructure(builder.Configuration);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region configurePipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion

public partial class Program { }