using System.Net.Http.Headers;
using System.Net.Http.Json;
using AgendaManager.Application.Auth.Commands.Login;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.WebApi.UnitTests.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AgendaManager.WebApi.UnitTests;

public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>
{
    protected HttpClient Client => factory.CreateClient();

    protected async Task LoginAsync(string? email = null, string? password = null)
    {
        email ??= SharedTestContext.AliceEmail;
        password ??= SharedTestContext.UserPassword;

        var result = await Client.PostAsJsonAsync("api/auth/login", new LoginCommand(email, password));
        var response = await result.Content.ReadFromJsonAsync<Result<LoginResponse>>();

        if (response?.Value is null)
        {
            throw new ApplicationException("Invalid login response");
        }

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            response.Value.AccessToken);
    }
}
