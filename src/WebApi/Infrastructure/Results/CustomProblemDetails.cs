using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Infrastructure.Results;

public class CustomProblemDetails : ProblemDetails
{
    public string? Code { get; set; }
}
