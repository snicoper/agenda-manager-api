using AgendaManager.TestCommon.TestConstants;

namespace AgendaManager.WebApi.UnitTests.HomeControllerTests;

public class GetTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private readonly string _uri = $"{SharedTestContext.BaseUri}/home";
}
