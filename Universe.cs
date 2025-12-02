using System;

using dingus_net.ServiceSystem;
using dingus_net.ServiceSystem.Rooms;

using ScreepsDotNet.API.World;

namespace dingus_net
{
public class Universe : IServiceRunner
{
    public IGame Game { get; }

    private readonly ServiceCollection _rooms = [];
    //private readonly ServiceCollection _services = [];

    public Universe(IGame game)
    {
        Game = game;
        CleanMemory(Game);

        Console.WriteLine("Creating Universe..");

        CreateRooms();

        //CreateServices();

        Console.WriteLine("Universe created");
    }

    // private void CreateServices()
    // {
    //     Console.WriteLine("Creating Universe Services..");
    //
    //     _services.Add(new PopulationService(Game));
    //     _services.Add(new CreepConductor(Game));
    // }

    private void CreateRooms()
    {
        Console.WriteLine("Creating Universe Rooms..");

        foreach(IRoom room in Game.Rooms.Values)
        {
            _rooms.Add(new RoomService(room, Game));
        }
    }

    public void OnTick()
    {
        _rooms.OnTick();
        //_services.OnTick();
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
