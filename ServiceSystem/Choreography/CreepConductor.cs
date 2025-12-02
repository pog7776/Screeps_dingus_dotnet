using dingus_net.Creeps;
using dingus_net.ServiceSystem.Rooms;

using ScreepsDotNet.API.World;

namespace dingus_net.ServiceSystem.Choreography
{
public class CreepConductor : Service
{
    private readonly RoomService _room;

    /// <inheritdoc />
    public CreepConductor(RoomService room, IGame game, string name = nameof(CreepConductor)) : base(game, name)
    {
        _room = room;
    }

    /// <inheritdoc />
    public override void OnTick()
    {
        foreach(ICreep creep in _room.Creeps)
        {
            creep.PerformRole(_room);
        }
    }

}
}
