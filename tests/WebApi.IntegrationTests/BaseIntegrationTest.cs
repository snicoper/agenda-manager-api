using System.Net.Http.Headers;
using System.Net.Http.Json;
using AgendaManager.Application.Authentication.Commands.Login;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.WebApi.UnitTests;

public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>
{
    protected HttpClient HttpClient => factory.CreateClient();

    protected async Task<HttpClient> CreateClientWithLoginAsync(string? email = null, string? password = null)
    {
        email ??= TestCommon.TestConstants.Constants.Users.Email.Value;
        password ??= TestCommon.TestConstants.Constants.Users.Password;

        var result = await HttpClient.PostAsJsonAsync("api/v1/authentication/login", new LoginCommand(email, password));
        var response = await result.Content.ReadFromJsonAsync<Result<TokenResult>>();

        var client = factory.CreateClient();

        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("Bearer", response?.Value?.AccessToken);

        return client;
    }
}
