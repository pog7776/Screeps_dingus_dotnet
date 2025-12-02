using System;

using ScreepsDotNet.API.World;

namespace dingus_net.ServiceSystem
{
public abstract class Service : IServiceRunner
{
    public string Name { get; }
    protected IGame Game { get; }

    protected Service(IGame game, string name)
    {
        Name = name;
        Game = game;

        LogCreation();
    }

    private void LogCreation() => Console.WriteLine($"Created service: '{Name}'");

    public abstract void OnTick();
}
}
