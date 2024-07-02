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
    protected HttpClient HttpClient => factory.CreateClient();

    protected static string ComposeUrl(string uri)
    {
        return $"{SharedTestContext.BaseUrl}/{uri}";
    }

    protected async Task<HttpClient> GetHttpClientWithLoginAsync()
    {
        return await GetHttpClientWithLoginAsync(SharedTestContext.AliceEmail, SharedTestContext.UserPassword);
    }

    protected async Task<HttpClient> GetHttpClientWithLoginAsync(string email, string password)
    {
        var uri = ComposeUrl("auth/login");
        var result = await HttpClient.PostAsJsonAsync(uri, new LoginCommand(email, password));
        var response = await result.Content.ReadFromJsonAsync<Result<LoginResponse>>();

        if (response?.Value is null)
        {
            throw new ApplicationException("Invalid login response");
        }

        var httpClient = factory.CreateClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            response.Value.AccessToken);

        return httpClient;
    }
}
