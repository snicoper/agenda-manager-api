using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace AgendaManager.WebApi.UnitTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureTestServices(
            services =>
            {
                var dbContextDescriptor =
                    services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (dbContextDescriptor is not null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<AppDbContext>(
                    (provider, options) =>
                    {
                        options.UseNpgsql(_dbContainer.GetConnectionString());
                        options.AddInterceptors(provider.GetServices<ISaveChangesInterceptor>());
                    });
            });
    }
}
