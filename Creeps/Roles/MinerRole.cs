using System;
using System.Linq;

using dingus_net.ServiceSystem.Resources;
using dingus_net.ServiceSystem.Rooms;

using ScreepsDotNet.API;
using ScreepsDotNet.API.World;

namespace dingus_net.Creeps.Roles
{
public class MinerRole : IRole
{
    /// <inheritdoc />
    public string Name => "Miner";

    private static ReadOnlySpan<(BodyPartType, int)> BodyParts
        => new([
            (BodyPartType.Move, 1),
            (BodyPartType.Work, 1),
            (BodyPartType.Carry, 1)
        ]);

    public enum Task
    {
        Idle,
        Harvesting,
        Hauling
    }

    private const string _taskKey = "Task";

    /// <inheritdoc />
    public void PerformRole(ICreep creep, RoomService room)
    {
        Task currentTask = GetCurrentTask(creep);
        Task nextTask = DetermineNextTask(creep, currentTask);

        if(nextTask != currentTask)
        {
            SetTask(creep, nextTask);
            currentTask = nextTask;
        }

        switch(currentTask)
        {
            case Task.Harvesting: DoHarvesting(creep, room); break;
            case Task.Hauling: DoHauling(creep, room); break;
            case Task.Idle:
            default:
                creep.Say("⚠️ Error/Idle");
                break;
        }
    }

    /// <inheritdoc />
    public BodyType<BodyPartType> CreateBody() => new(BodyParts);

    private static Task GetCurrentTask(ICreep creep)
    {
        if (creep.Memory.TryGetString(_taskKey, out string? taskStr) &&
            Enum.TryParse(taskStr, out Task task))
        {
            return task;
        }

        return Task.Idle;
    }

    private static Task DetermineNextTask(ICreep creep, Task currentTask)
    {
        bool isFull = creep.Store.GetFreeCapacity(ResourceType.Energy) == 0;
        bool isEmpty = creep.Store.GetUsedCapacity(ResourceType.Energy) == 0;
        bool hasResource = creep.Store.GetUsedCapacity(ResourceType.Energy) > 0;

        return currentTask switch
        {
            // Core transition logic (Harvesting -> Hauling)
            Task.Harvesting when isFull  => Task.Hauling,

            // Core transition logic (Hauling -> Harvesting)
            Task.Hauling    when isEmpty => Task.Harvesting,

            // Idle transition logic (forces an immediate decision)
            Task.Idle when isFull || hasResource => Task.Hauling,
            Task.Idle when isEmpty               => Task.Harvesting,
            _ => currentTask
        };
    }

    private static void DoHarvesting(ICreep creep, RoomService room)
    {
        //ISource energySource = creep.Room.Find<ISource>().First();
        ISource? energySource = room.EnergyManager?.Sources.FirstOrDefault();

        // Try to load the target source from memory
        // if (!creep.Memory.TryGetString(_targetIdKey, out string? targetId) ||
        //     !creep.Room.TryGetWorldObject<ISource>(targetId, out ISource? energySource) ||
        //     energySource == null)
        if(energySource == null)
        {
            // No valid target, find a new one
            energySource = creep.Room.Find<ISource>().FirstOrDefault();
            if (energySource == null)
            {
                creep.Say("🛑 No Source");
                //creep.Memory.RemoveValue(_targetIdKey);
                SetTask(creep, Task.Idle);
                return;
            }

            // Store the new target ID
            //creep.Memory.SetValue(_targetIdKey, energySource.Id);
        }

        if (creep.LocalPosition.IsNextTo(energySource.LocalPosition))
        {
            creep.Harvest(energySource);
        }
        else
        {
            creep.MoveTo(energySource.LocalPosition);
        }
    }

    private static void DoHauling(ICreep creep, RoomService room)
    {
        EnergyManager? energyManager = room.EnergyManager;

        IStructure? targetStructure = room.Controller;

        // TODO cache the target and check if it's full or something?
        // Finding each tick sucks
        // Should really make target priority a RoomService responsibility
        if(energyManager?.EnergyAvailable < energyManager?.EnergyCapacity)
        {
            targetStructure = room.Find<IStructureSpawn>()
                .FirstOrDefault(static spawn => spawn.Store.GetFreeCapacity(ResourceType.Energy) >= 0);
        }

        if(targetStructure == null) { return; }

        if (creep.LocalPosition.IsNextTo(targetStructure.LocalPosition))
        {
            // We are next to the controller, transfer energy
            // We use TransferAll to ensure we empty the store
            creep.Transfer(targetStructure, ResourceType.Energy, creep.Store.GetUsedCapacity(ResourceType.Energy));

            // The DetermineNextTask function will catch the "isEmpty" state next tick.
            //TODO However, if the target is full and we can't transfer, we might need a fallback here.
        }
        else
        {
            creep.MoveTo(targetStructure.LocalPosition);
        }
    }

    private static void SetTask(ICreep creep, Task task)
    {
        creep.Memory.SetValue(_taskKey, task.ToString());
        creep.Say($"🤖 {task.ToString()}");
    }

    /// <inheritdoc />
    public bool Equals(ICreep? other) => other?.GetRoleName() == Name;
}
}
