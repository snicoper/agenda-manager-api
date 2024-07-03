using AgendaManager.WebApi.UnitTests.Shared;

namespace AgendaManager.WebApi.UnitTests;

public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>
{
    protected HttpClient HttpClient => factory.CreateClient();

    protected static string ComposeUrl(string uri)
    {
        return $"{SharedTestContext.BaseUrl}/{uri}";
    }
}
