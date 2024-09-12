using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.WebApi.Infrastructure.Results;

public record ResultResponse(bool IsSuccess, ResultType ResultType);
