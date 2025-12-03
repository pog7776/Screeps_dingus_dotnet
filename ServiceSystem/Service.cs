using System;

using ScreepsDotNet.API.World;

namespace dingus_net.ServiceSystem
{
public abstract class Service : IServiceRunner
{
    public string Name { get; }
    protected IGame Game { get; }

    protected virtual string LogTag => Name;

    protected Service(IGame game, string name)
    {
        Name = name;
        Game = game;

        Log($"Created service: '{Name}'");
    }

    protected void Log(string? value) => Console.WriteLine($"[{LogTag}] {value}");

    public abstract void OnTick();
}
}
