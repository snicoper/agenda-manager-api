namespace AgendaManager.Infrastructure.Common.Emails.Interfaces;

public interface IRazorViewToStringRenderer
{
    Task<string> RenderViewToStringAsync(string viewName, object model, Dictionary<string, object?> viewData);
}
