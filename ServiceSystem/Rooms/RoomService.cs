using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using dingus_net.ServiceSystem.Choreography;
using dingus_net.ServiceSystem.Population;
using dingus_net.ServiceSystem.Resources;

using ScreepsDotNet.API;
using ScreepsDotNet.API.World;

namespace dingus_net.ServiceSystem.Rooms
{
public class RoomService : Service, IRoom
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

#region IRoom Implementation

    /// <inheritdoc />
    public void SetUserData<T>(T? userData) where T : class => Room.SetUserData(userData);

    /// <inheritdoc />
    public bool TryGetUserData<T>([MaybeNullWhen(false)] out T userData) where T : class
        => Room.TryGetUserData(out userData);

    /// <inheritdoc />
    public T? GetUserData<T>() where T : class => Room.GetUserData<T>();

    /// <inheritdoc />
    public bool HasUserData<T>() where T : class => Room.HasUserData<T>();

    /// <inheritdoc />
    public RoomCreateConstructionSiteResult CreateConstructionSite<T>(Position position, string? name = null) where T : class, IStructure
        => Room.CreateConstructionSite<T>(position, name);

    /// <inheritdoc />
    public RoomCreateConstructionSiteResult CreateConstructionSite(Position position, Type structureType, string? name = null)
        => Room.CreateConstructionSite(position, structureType, name);

    /// <inheritdoc />
    public RoomCreateFlagResult CreateFlag(Position position, out string newFlagName, string? name = null, FlagColor? color = null,
        FlagColor? secondaryColor = null)
        => Room.CreateFlag(position, out newFlagName, name, color, secondaryColor);

    /// <inheritdoc />
    public IEnumerable<T> Find<T>(bool? my = null) where T : class, IRoomObject => Room.Find<T>(my);

    /// <inheritdoc />
    public IEnumerable<Position> FindExits(ExitDirection? exitFilter = null) => Room.FindExits(exitFilter);

    /// <inheritdoc />
    public RoomFindExitResult FindExitTo(IRoom room) => Room.FindExitTo(room);

    /// <inheritdoc />
    public RoomFindExitResult FindExitTo(string roomName) => Room.FindExitTo(roomName);

    /// <inheritdoc />
    public IEnumerable<PathStep> FindPath(RoomPosition fromPos, RoomPosition toPos, FindPathOptions? opts = null)
        => Room.FindPath(fromPos, toPos, opts);

    /// <inheritdoc />
    public string GetRawEventLog() => Room.GetRawEventLog();

    /// <inheritdoc />
    public RoomPosition GetPositionAt(Position position) => Room.GetPositionAt(position);

    /// <inheritdoc />
    public IRoomTerrain GetTerrain() => Room.GetTerrain();

    /// <inheritdoc />
    public IEnumerable<IRoomObject> LookAt(Position position) => Room.LookAt(position);

    /// <inheritdoc />
    public IEnumerable<IRoomObject> LookAtArea(Position min, Position max) => Room.LookAtArea(min, max);

    /// <inheritdoc />
    public IEnumerable<T> LookForAt<T>(Position position) where T : class, IRoomObject => Room.LookForAt<T>(position);

    /// <inheritdoc />
    public IEnumerable<T> LookForAtArea<T>(Position min, Position max) where T : class, IRoomObject => Room.LookForAtArea<T>(min, max);

    /// <inheritdoc />
    public bool Exists => Room.Exists;

    /// <inheritdoc />
    public RoomCoord Coord => Room.Coord;

    /// <inheritdoc />
    public IStructureController? Controller => Room.Controller;

    /// <inheritdoc />
    public int EnergyAvailable => Room.EnergyAvailable;

    /// <inheritdoc />
    public int EnergyCapacityAvailable => Room.EnergyCapacityAvailable;

    /// <inheritdoc />
    public IMemoryObject Memory => Room.Memory;

    /// <inheritdoc />
    public IStructureStorage? Storage => Room.Storage;

    /// <inheritdoc />
    public IStructureTerminal? Terminal => Room.Terminal;

    /// <inheritdoc />
    public IRoomVisual Visual => Room.Visual;

#endregion
}
}
