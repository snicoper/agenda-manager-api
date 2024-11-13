using AgendaManager.Domain.ResourceTypes;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.Constants;
using AgendaManager.Domain.Users.Entities;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Modules;

public class ResourceTypeSeed
{
    public static async Task InitializeAsync(AppDbContext context, List<Role> roles)
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

        // Evitar el error SA1118.
        const string periodoncistaDescription =
            "Especialista en la prevención, diagnóstico y tratamiento de enfermedades de las encías y " +
            "las estructuras de soporte de los dientes";

        const string odontologoDescription =
            "Profesional especializado en diagnóstico, tratamiento y prevención de enfermedades bucales";

        var resourceTypes = new List<ResourceType>
        {
            // Con role.
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Odontólogo",
                description: odontologoDescription,
                roleId: role.Id),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Cirujano Maxilofacial",
                description: "Especialista en cirugía oral, maxilar y facial",
                roleId: role.Id),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Odontopediatra",
                description: "Especialista en el cuidado dental de niños y adolescentes",
                roleId: role.Id),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Endodoncista",
                description: "Especialista en la restauración de los dientes afectados por enfermedades o lesiones",
                roleId: role.Id),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Periodoncista",
                description: periodoncistaDescription,
                roleId: role.Id),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Auxiliar Dental",
                description: "Profesional que brinda apoyo en la atención dental",
                roleId: role.Id),
            ResourceType.Create(
                ResourceTypeId.Create(),
                "Higienista",
                "Profesional especializado en limpieza dental y prevención",
                role.Id),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Recepcionista",
                description: "Personal encargado de gestión de citas y atención al paciente",
                roleId: role.Id),

            // Sin role.
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Gabinete Dental",
                description: "Espacio físico dedicado a la atención dental"),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Consultorio",
                description: "Espacio físico dedicado a la atención dental"),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Sala Rayos X",
                description: "Espacio especializado para diagnóstico radiológico dental"),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Sala de Cirugía",
                description: "Espacio especializado para cirugía oral y maxilar"),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Equipo Radiografía",
                description: "Equipo utilizado para la generación de imágenes radiológicas"),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Equipo de Ultrasonido",
                description: "Equipo utilizado para la generación de imágenes ultrasónicas"),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Instrumental Quirúrgico",
                description: "Equipo utilizado en procedimientos quirúrgicos"),
            ResourceType.Create(
                id: ResourceTypeId.Create(),
                name: "Equipo Esterilización",
                description: "Equipo utilizado para la esterilización de instrumentos")
        };

        await context.ResourceTypes.AddRangeAsync(resourceTypes);
        await context.SaveChangesAsync();
    }
}
