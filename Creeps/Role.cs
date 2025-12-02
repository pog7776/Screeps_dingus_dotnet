using dingus_net.ServiceSystem.Rooms;

using ScreepsDotNet.API;
using ScreepsDotNet.API.World;

namespace dingus_net.Creeps
{
public abstract class Role : IRole
{
    public abstract string Name { get; }

    /// <inheritdoc />
    public abstract void PerformRole(ICreep creep, RoomService room);

    /// <inheritdoc />
    public abstract BodyType<BodyPartType> CreateBody();

    /// <inheritdoc />
    public bool Equals(ICreep? other) => other?.GetRoleName() == Name;
}
}
