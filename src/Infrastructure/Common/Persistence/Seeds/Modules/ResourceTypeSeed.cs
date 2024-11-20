using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.ResourceTypes.Services;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Modules;

public class ResourceTypeSeed
{
    public static async Task InitializeAsync(AppDbContext context, List<Role> roles, IServiceProvider serviceProvider)
    {
        if (context.ResourceTypes.Any())
        {
            return;
        }

        if (roles.Count == 0)
        {
            throw new Exception("Roles not found for create new resource type");
        }

        var role = roles.FirstOrDefault(r => r.Name == SystemRoles.Employee);

        if (role is null)
        {
            throw new Exception("Role not found for create new resource type");
        }

        var resourceTypeManager = serviceProvider.GetRequiredService<ResourceTypeManager>();

        // Evitar el error SA1118.
        const string periodoncistaDescription =
            "Especialista en la prevención, diagnóstico y tratamiento de enfermedades de las encías y " +
            "las estructuras de soporte de los dientes";

        const string odontologoDescription =
            "Profesional especializado en diagnóstico, tratamiento y prevención de enfermedades bucales";

        // Con role.
        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Odontólogo",
            description: odontologoDescription,
            CancellationToken.None,
            roleId: role.Id);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Cirujano Maxilofacial",
            description: "Especialista en cirugía oral, maxilar y facial",
            CancellationToken.None,
            roleId: role.Id);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Odontopediatra",
            description: "Especialista en el cuidado dental de niños y adolescentes",
            CancellationToken.None,
            roleId: role.Id);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Endodoncista",
            description: "Especialista en la restauración de los dientes afectados por enfermedades o lesiones",
            CancellationToken.None,
            roleId: role.Id);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Periodoncista",
            description: periodoncistaDescription,
            CancellationToken.None,
            roleId: role.Id);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Auxiliar Dental",
            description: "Profesional que brinda apoyo en la atención dental",
            CancellationToken.None,
            roleId: role.Id);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Higienista",
            description: "Profesional especializado en limpieza dental y prevención",
            cancellationToken: CancellationToken.None,
            roleId: role.Id);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Recepcionista",
            description: "Personal encargado de gestión de citas y atención al paciente",
            CancellationToken.None,
            roleId: role.Id);

        // Sin role.
        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Gabinete Dental",
            description: "Espacio físico dedicado a la atención dental",
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Consultorio",
            description: "Espacio físico dedicado a la atención dental",
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Sala Rayos X",
            description: "Espacio especializado para diagnóstico radiológico dental",
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Sala de Cirugía",
            description: "Espacio especializado para cirugía oral y maxilar",
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Equipo Radiografía",
            description: "Equipo utilizado para la generación de imágenes radiológicas",
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Equipo de Ultrasonido",
            description: "Equipo utilizado para la generación de imágenes ultrasónicas",
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Instrumental Quirúrgico",
            description: "Equipo utilizado en procedimientos quirúrgicos",
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Equipo Esterilización",
            description: "Equipo utilizado para la esterilización de instrumentos",
            CancellationToken.None);

        await context.SaveChangesAsync();
    }
}
