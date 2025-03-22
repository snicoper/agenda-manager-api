using System.Net.Http.Headers;
using System.Net.Http.Json;
using AgendaManager.Application.Authentication.Commands.Login;
using AgendaManager.Application.Authentication.Models;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.TestCommon.Constants;

namespace AgendaManager.WebApi.UnitTests;

public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>
{
    protected IntegrationTestWebAppFactory Factory { get; } = factory;

    protected HttpClient HttpClient => Factory.CreateClient();

    protected async Task<HttpClient> CreateClientWithLoginAsync(string? email = null, string? password = null)
    {
        email ??= UserConstants.UserAlice.Email.Value;
        password ??= UserConstants.UserAlice.RawPassword;

        var result = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, new LoginCommand(email, password));
        var response = await result.Content.ReadFromJsonAsync<Result<TokenResult>>();

        var client = Factory.CreateClient();

        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("Bearer", response?.Value?.AccessToken);

        return client;
    }
}
