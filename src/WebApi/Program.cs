using AgendaManager.Application;
using AgendaManager.Infrastructure;
using AgendaManager.WebApi;
using AgendaManager.WebApi.Endpoints;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddWebApi();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("api/home").MapHomeEndpoints();

app.Run();

public partial class Program
{
}
