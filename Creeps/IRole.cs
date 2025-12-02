using System;

using dingus_net.ServiceSystem.Rooms;

using ScreepsDotNet.API;
using ScreepsDotNet.API.World;

namespace dingus_net.Creeps
{
public interface IRole : IEquatable<ICreep>
{
    string Name { get; }
    void PerformRole(ICreep creep, RoomService room);
    BodyType<BodyPartType> CreateBody();
}
}
