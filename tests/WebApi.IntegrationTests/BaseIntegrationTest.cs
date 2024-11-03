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
        email ??= TestCommon.Seeds.Users.UserAlice.Email.Value;
        password ??= TestCommon.Seeds.Users.UserAlice.RawPassword;

        var result = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, new LoginCommand(email, password));
        var response = await result.Content.ReadFromJsonAsync<Result<TokenResult>>();

        var client = factory.CreateClient();

        client
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("Bearer", response?.Value?.AccessToken);

        return client;
    }
}
