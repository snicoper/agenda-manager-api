namespace AgendaManager.Application.Common.Abstractions.Views;

public interface IRazorViewToStringRenderer
{
    Task<string> RenderViewToStringAsync(string viewName, object model, Dictionary<string, object?> viewData);
}
