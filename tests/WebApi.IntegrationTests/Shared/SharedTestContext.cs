namespace AgendaManager.WebApi.UnitTests.Shared;

// Mover a TestCommon.
public static class SharedTestContext
{
    public static string BaseUrl { get; } = "api/v1";

    public static string AliceEmail { get; } = "alice@example.com";

    public static string BobEmail { get; } = "bob@example.com";

    public static string CarolEmail { get; } = "carol@example.com";

    public static string CarlosEmail { get; } = "carlos@example.com";

    public static string UserPassword { get; } = "Password4!";
}
