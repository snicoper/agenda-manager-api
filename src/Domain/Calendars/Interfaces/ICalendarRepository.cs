namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarRepository
{
    Task AddAsync(Calendar calendar, CancellationToken cancellationToken = default);

    void Update(Calendar calendar);

    Task<bool> NameIsUniqueAsync(Calendar calendar, CancellationToken cancellationToken = default);

    Task<bool> DescriptionIsUniqueAsync(Calendar calendar, CancellationToken cancellationToken = default);
}
