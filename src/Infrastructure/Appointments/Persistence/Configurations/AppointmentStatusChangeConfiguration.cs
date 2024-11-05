using AgendaManager.Domain.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Appointments.Persistence.Configurations;

public class AppointmentStatusChangeConfiguration : IEntityTypeConfiguration<AppointmentStatusChange>
{
    public void Configure(EntityTypeBuilder<AppointmentStatusChange> builder)
    {
        throw new NotImplementedException();
    }
}
