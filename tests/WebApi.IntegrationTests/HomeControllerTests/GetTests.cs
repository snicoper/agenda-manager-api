namespace AgendaManager.WebApi.UnitTests.HomeControllerTests;

public class GetTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private readonly string _uri = $"{TestCommon.TestConstants.Constants.Shared.BaseUri}/home";
}
