namespace AgendaManager.WebApi.UnitTests;

public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>
{
    protected HttpClient Client => factory.CreateClient();

    protected Task CreateClientWithLoginAsync(string? email = null, string? password = null)
    {
        // email ??= SharedTestContext.AliceEmail;
        // password ??= SharedTestContext.UserPassword;
        //
        // var result = await Client.PostAsJsonAsync("api/auth/login", new LoginCommand(email, password));
        // var response = await result.Content.ReadFromJsonAsync<Result<LoginResponse>>();
        //
        // var client = factory.CreateClient();
        // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response?.Value?.AccessToken);
        //
        // return client;
        return Task.CompletedTask;
    }
}
