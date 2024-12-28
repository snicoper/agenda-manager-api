using AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Features;

public class ResourceTypeSeed
{
    public static async Task InitializeAsync(AppDbContext context, IServiceProvider serviceProvider)
    {
        if (context.ResourceTypes.Any())
        {
            return;
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
            ResourceCategory.Staff,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Cirujano Maxilofacial",
            description: "Especialista en cirugía oral, maxilar y facial",
            ResourceCategory.Staff,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Odontopediatra",
            description: "Especialista en el cuidado dental de niños y adolescentes",
            ResourceCategory.Staff,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Endodoncista",
            description: "Especialista en la restauración de los dientes afectados por enfermedades o lesiones",
            ResourceCategory.Staff,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Periodoncista",
            description: periodoncistaDescription,
            ResourceCategory.Staff,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Auxiliar Dental",
            description: "Profesional que brinda apoyo en la atención dental",
            ResourceCategory.Staff,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Higienista",
            description: "Profesional especializado en limpieza dental y prevención",
            ResourceCategory.Staff,
            cancellationToken: CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Recepcionista",
            description: "Personal encargado de gestión de citas y atención al paciente",
            ResourceCategory.Staff,
            CancellationToken.None);

        // Sin role.
        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Gabinete Dental",
            description: "Espacio físico dedicado a la atención dental",
            ResourceCategory.Place,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Consultorio",
            description: "Espacio físico dedicado a la atención dental",
            ResourceCategory.Place,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Sala Rayos X",
            description: "Espacio especializado para diagnóstico radiológico dental",
            ResourceCategory.Place,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Sala de Cirugía",
            description: "Espacio especializado para cirugía oral y maxilar",
            ResourceCategory.Place,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Equipo Radiografía",
            description: "Equipo utilizado para la generación de imágenes radiológicas",
            ResourceCategory.Equipment,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Equipo de Ultrasonido",
            description: "Equipo utilizado para la generación de imágenes ultrasónicas",
            ResourceCategory.Equipment,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Instrumental Quirúrgico",
            description: "Equipo utilizado en procedimientos quirúrgicos",
            ResourceCategory.Equipment,
            CancellationToken.None);

        await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: "Equipo Esterilización",
            description: "Equipo utilizado para la esterilización de instrumentos",
            ResourceCategory.Equipment,
            CancellationToken.None);

        await context.SaveChangesAsync();
    }
}
