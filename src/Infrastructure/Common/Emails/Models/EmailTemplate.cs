namespace AgendaManager.Infrastructure.Common.Emails.Models;

public sealed record EmailTemplate<TModel>(
    IEnumerable<string> To,
    string Subject,
    string TemplateName,
    TModel Model)
    where TModel : class;
