using AgendaManager.Domain.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgendaManager.Infrastructure.Resources.Persistence;

public class ResourceScheduleConfiguration : IEntityTypeConfiguration<ResourceSchedule>
{
    public void Configure(EntityTypeBuilder<ResourceSchedule> builder)
    {
        builder.ToTable("ResourceSchedules");

        builder.HasKey(rs => rs.Id);
    }
}
