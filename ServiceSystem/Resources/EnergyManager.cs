using System.Collections.Generic;
using System.Linq;

using dingus_net.ServiceSystem.Rooms;

using ScreepsDotNet.API.World;

namespace dingus_net.ServiceSystem.Resources
{
public class EnergyManager : Service
{
    private readonly RoomService _room;
    private IRoom Room => _room.Room;

    private List<ISource>? _cachedSources;
    public IEnumerable<ISource> Sources => _cachedSources ??= GetRoomSources().ToList();

    public int SourceEnergyAvailable => Sources.Sum(static source => source.Energy);
    public int SourceEnergyCapacity => Sources.Sum(static source => source.EnergyCapacity);

    /// <seealso cref="IRoom.EnergyAvailable"/>
    public int EnergyAvailable => Room.EnergyAvailable;

    /// <seealso cref="IRoom.EnergyCapacityAvailable"/>
    public int EnergyCapacity => Room.EnergyCapacityAvailable;

    /// <inheritdoc />
    public EnergyManager(RoomService room, IGame game) : base(game, nameof(EnergyManager))
    {
        _room = room;
    }

    /// <inheritdoc />
    public override void OnTick() { }

    public IEnumerable<ISource> GetRoomSources() => Room.Find<ISource>();
}
}
