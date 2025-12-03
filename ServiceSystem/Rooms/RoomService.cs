using System;
using System.Collections.Generic;

using dingus_net.ServiceSystem.Choreography;
using dingus_net.ServiceSystem.Population;
using dingus_net.ServiceSystem.Resources;

using ScreepsDotNet.API.World;

namespace dingus_net.ServiceSystem.Rooms
{
public class RoomService : Service
{
    public IRoom Room { get; }

    /// <inheritdoc />
    protected override string LogTag => "Room";

    private readonly ServiceCollection _services = [];

    public EnergyManager? EnergyManager { get; private set; }
    public PopulationService? Population { get; private set; }
    public CreepConductor? CreepConductor { get; private set; }

    public IEnumerable<ICreep> Creeps => Population?.AllCreeps ?? Array.Empty<ICreep>();

    /// <inheritdoc />
    public RoomService(IRoom room, IGame game) : base(game, CreateRoomName(room))
    {
        Room = room;

        CreateServices();
    }

    private static string CreateRoomName(IRoom room) => $"{nameof(RoomService)} - {room.Name}";

    /// <inheritdoc />
    public override void OnTick() => _services.OnTick();

    private void CreateServices()
    {
        Log($"Creating {Name} Services..");

        _services.Add(EnergyManager = new EnergyManager(this, Game));
        _services.Add(Population = new PopulationService(this, Game));
        _services.Add(CreepConductor = new CreepConductor(this, Game));
    }
}
}
