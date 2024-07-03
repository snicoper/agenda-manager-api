using System.Text.RegularExpressions;

namespace AgendaManager.Domain.Common.Utils;

public static partial class DomainRegex
{
    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    public static partial Regex Email();
}
