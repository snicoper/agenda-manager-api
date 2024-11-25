namespace AgendaManager.Infrastructure.Common.Emails.Models;

public record EmailTemplate<TModel>(
    IEnumerable<string> To,
    string Subject,
    string TemplateName,
    TModel Model)
    where TModel : class;
