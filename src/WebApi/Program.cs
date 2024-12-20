using AgendaManager.Application;
using AgendaManager.Domain;
using AgendaManager.Infrastructure;
using AgendaManager.Infrastructure.Common.Middlewares;
using AgendaManager.Infrastructure.Common.Persistence.Seeds;
using AgendaManager.WebApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddDomain();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddWebApi(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseInfrastructureMiddleware();
await app.InitialiseDatabaseAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

var defaultPolicyName = builder.Configuration["Cors:DefaultPolicyName"];

if (!string.IsNullOrWhiteSpace(defaultPolicyName))
{
    app.UseCors(defaultPolicyName);
}

app.UseHttpsRedirection();

app.UseExceptionHandler(_ => { });

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public abstract partial class Program
{
}
