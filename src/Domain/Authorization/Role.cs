namespace AgendaManager.Domain.Authorization;

public class Role
{
    public Role(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }

    public string Description { get; set; }
}
