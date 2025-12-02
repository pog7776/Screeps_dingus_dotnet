using dingus_net.Creeps.Roles;
using dingus_net.ServiceSystem.Rooms;

using ScreepsDotNet.API.World;

namespace dingus_net.Creeps
{
public static class CreepExtensionMethods
{
    public static void AssignRole(this ICreep creep, IRole role)
        => creep.Memory.SetValue(RoleRegistry.RoleKey, role.Name);

    public static string GetRoleName(this ICreep creep)
    {
        creep.TryGetRoleName(out string? role);
        return role ?? string.Empty;
    }

    public static bool TryGetRoleName(this ICreep creep, out string? role)
        => creep.Memory.TryGetString(RoleRegistry.RoleKey, out role);

    public static IRole? GetRole(this ICreep creep)
    {
        creep.TryGetRole(out IRole? role);
        return role;
    }

    public static bool TryGetRole(this ICreep creep, out IRole? role)
    {
        role = null;

        return creep.TryGetRoleName(out string? roleName) &&
               RoleRegistry.Roles.TryGetValue(roleName, out role);
    }

    public static void PerformRole(this ICreep creep, RoomService room) => creep.TryPerformRole(room);

    public static bool TryPerformRole(this ICreep creep, RoomService room)
    {
        if(! creep.TryGetRole(out IRole? role)) { return false; }

        role?.PerformRole(creep, room);
        return true;

    }
}
}
