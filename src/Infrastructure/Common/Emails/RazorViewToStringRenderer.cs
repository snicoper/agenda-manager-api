using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace AgendaManager.Infrastructure.Common.Emails;

public sealed class RazorViewToStringRenderer(
    IRazorViewEngine viewEngine,
    ITempDataProvider tempDataProvider,
    IServiceProvider serviceProvider)
    : IRazorViewToStringRenderer
{
    public async Task<string> RenderViewToStringAsync(
        string viewName,
        object model,
        Dictionary<string, object?> viewData)
    {
        var context = new ActionContext(
            new DefaultHttpContext { RequestServices = serviceProvider },
            new RouteData(),
            new ActionDescriptor());

        var view = GetView(context, viewName);

        var viewDataDict = new ViewDataDictionary(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary()) { Model = model };

        viewData.ToList().ForEach(x => viewDataDict[x.Key] = x.Value);

        var viewContext = new ViewContext(
            context,
            view,
            viewDataDict,
            new TempDataDictionary(context.HttpContext, tempDataProvider),
            new StringWriter(),
            new HtmlHelperOptions());

        await view.RenderAsync(viewContext);

        return viewContext.Writer.ToString() ?? string.Empty;
    }

    private static string CreateViewNotFoundError(string viewName, IEnumerable<string> locations)
    {
        var viewError = $"""
                         Unable to find view '{viewName}'. The following locations were searched:
                         {string.Join(Environment.NewLine, locations)}
                         """;

        return viewError;
    }

    private IView GetView(ActionContext context, string viewName)
    {
        var getViewResult = viewEngine.GetView(null, viewName, true);
        if (getViewResult.Success)
        {
            return getViewResult.View;
        }

        var findViewResult = viewEngine.FindView(context, viewName, true);
        if (findViewResult.Success)
        {
            return findViewResult.View;
        }

        var locations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);

        throw new InvalidOperationException(CreateViewNotFoundError(viewName, locations));
    }
}
