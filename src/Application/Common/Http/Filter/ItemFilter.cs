namespace AgendaManager.Application.Common.Http.Filter;

public record ItemFilter(string PropertyName, string RelationalOperator, string Value, string LogicalOperator);
