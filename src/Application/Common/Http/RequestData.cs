namespace AgendaManager.Application.Common.Http;

public class RequestData
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 25;

    public string Order { get; set; } = string.Empty;

    public string Filters { get; set; } = string.Empty;
}
