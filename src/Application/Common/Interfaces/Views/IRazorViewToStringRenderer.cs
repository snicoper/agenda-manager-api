namespace AgendaManager.Application.Common.Interfaces.Views;

public interface IRazorViewToStringRenderer
{
    Task<string> RenderViewToStringAsync(string viewName, object model, Dictionary<string, object?> viewData);
}
