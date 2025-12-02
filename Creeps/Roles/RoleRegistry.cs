using System.Collections.Generic;

namespace dingus_net.Creeps.Roles
{
public static class RoleRegistry
{
    public const string RoleKey = "Role";

    private static readonly MinerRole s_minerRole = new();

    public static readonly Dictionary<string, IRole> Roles = new()
    {
        { s_minerRole.Name, s_minerRole }
    };

    // TODO Calculate based on room resources
    public static readonly Dictionary<IRole, int> Population = new()
    {
        { s_minerRole, 3 }
    };
}
}
