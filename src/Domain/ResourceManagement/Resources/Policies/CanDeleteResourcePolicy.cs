using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Domain.ResourceManagement.Resources.Policies;

public class CanDeleteResourcePolicy(IAppointmentRepository appointmentRepository) : ICanDeleteResourcePolicy
{
    public async Task<bool> CanDeleteAsync(ResourceId resourceId, CancellationToken cancellationToken)
    {
        var exists = await appointmentRepository.ExistsByResourceIdAsync(resourceId, cancellationToken);

        return !exists;
    }
}
