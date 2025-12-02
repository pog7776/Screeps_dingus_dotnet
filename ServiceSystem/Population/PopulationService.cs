using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using dingus_net.Creeps;
using dingus_net.Creeps.Roles;
using dingus_net.ServiceSystem.Rooms;

using ScreepsDotNet.API;
using ScreepsDotNet.API.World;

namespace dingus_net.ServiceSystem.Population
{
public class PopulationService : Service
{
    private static Dictionary<IRole, int> Roles => RoleRegistry.Population;

    private readonly HashSet<ICreep> _allCreeps = [];

    public IEnumerable<ICreep> AllCreeps => _allCreeps;

    private readonly RoomService _room;

    /// <inheritdoc />
    public PopulationService(RoomService room, IGame game, string name = nameof(PopulationService)) : base(game, name)
    {
        _room = room;
    }

    /// <inheritdoc />
    public override void OnTick()
    {
        // Check for any creeps we're tracking that no longer exist
        foreach (ICreep creep in _allCreeps.ToImmutableArray())
        {
            if (creep.Exists) { continue; }
            _allCreeps.Remove(creep);
            OnCreepDied(creep);
        }

        // Check the latest creep list for any new creeps
        HashSet<ICreep> newCreepList = new(_room.Room.Find<ICreep>().Where(static x => x.My));

        foreach (ICreep creep in newCreepList)
        {
            if (!_allCreeps.Add(creep)) { continue; }
            OnNewCreep(creep);
        }

        foreach(KeyValuePair<IRole, int> rolePopulation in Roles)
        {
            int roleCount = _allCreeps.Count(creep => rolePopulation.Key.Equals(creep));

            if(roleCount >= rolePopulation.Value) { continue; }

            Spawn(rolePopulation.Key);
        }
    }

    public SpawnCreepResult Spawn(IRole role)
    {
        IStructureSpawn? spawner = GetSpawner(_room.Room);
        if(spawner == null)
        {
            Console.WriteLine($"Unable to find spawner in room: {_room.Room}");
            return SpawnCreepResult.Busy;
        }

        BodyType<BodyPartType> body = role.CreateBody();
        string name = $"{role.Name} {Random.Shared.Next()}";

        SpawnCreepResult testResult = TestSpawn(spawner, body);
        if(testResult != SpawnCreepResult.Ok)
        {
            Console.WriteLine($"Failed to spawn creep: {testResult}");
            return testResult;
        }

        IMemoryObject initialMemory = Game.CreateMemoryObject();
        initialMemory.SetValue(RoleRegistry.RoleKey, role.Name);

        Console.WriteLine($"{this}: spawning a {role.Name} ({body}) from {spawner}..");

        return spawner.SpawnCreep(body, name, new SpawnCreepOptions(dryRun: false, memory: initialMemory));
    }

    private static IStructureSpawn? GetSpawner(IRoom room)
    {
        IStructureSpawn? spawner = room.Find<IStructureSpawn>()
            .FirstOrDefault(static spawn => spawn.Spawning == null);

        spawner ??= room.Find<IStructureSpawn>().FirstOrDefault();

        return spawner;
    }

    private static SpawnCreepResult TestSpawn(IStructureSpawn spawner, BodyType<BodyPartType> body, string name = "Dingus")
        => spawner.SpawnCreep(body, name, new SpawnCreepOptions(dryRun: true));

    private static void OnNewCreep(ICreep creep) { }

    private void OnCreepDied(ICreep creep)
    {
        Console.WriteLine($"{this}: {creep} died");

        //Universe.CleanMemory(Game);
        DisposeCreep(creep);
    }

    private void DisposeCreep(ICreep creep)
    {
        if(Game.Creeps.ContainsKey(creep.Name)) { return; }

        if (!Game.Memory.TryGetObject("creeps", out IMemoryObject? creepsObj)) { return; }

        creepsObj.ClearValue(creep.Name);
        Console.WriteLine($"Removed {creep.Name} from memory");

    }
}
}
