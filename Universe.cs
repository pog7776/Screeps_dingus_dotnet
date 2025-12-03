using System;

using dingus_net.ServiceSystem;
using dingus_net.ServiceSystem.Rooms;
using dingus_net.ServiceSystem.System;

using ScreepsDotNet.API.World;

namespace dingus_net
{
public class Universe : Service
{
    public static CpuManager? Cpu { get; private set; }

    private readonly ServiceCollection _services = [];
    private readonly ServiceCollection _rooms = [];

    public Universe(IGame game) : base(game, nameof(Universe))
    {
        CleanMemory(Game);

        Log("Creating Universe..");

        CreateServices();
        CreateRooms();

        Log("Universe created");
    }

    private void CreateServices()
    {
        Log("Creating Universe Services..");

        _services.Add(Cpu ??= new CpuManager(Game));
    }

    private void CreateRooms()
    {
        Log("Creating Universe Rooms..");

        foreach(IRoom room in Game.Rooms.Values)
        {
            _rooms.Add(new RoomService(room, Game));
        }
    }

    /// <inheritdoc/>
    public override void OnTick()
    {
        _services.OnTick();
        _rooms.OnTick();
    }

    public static void CleanMemory(IGame game)
    {
        if (!game.Memory.TryGetObject("creeps", out IMemoryObject? creepsObj)) { return; }

        // Delete all creeps in memory that no longer exist
        int clearCnt = 0;
        foreach (string creepName in creepsObj.Keys)
        {
            if (!game.Creeps.ContainsKey(creepName))
            {
                creepsObj.ClearValue(creepName);
                ++clearCnt;
            }
        }
        if (clearCnt > 0) { Console.WriteLine($"Cleared {clearCnt} dead creeps from memory"); }
    }
}
}
