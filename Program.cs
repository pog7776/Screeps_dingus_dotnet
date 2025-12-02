using System;
using System.Diagnostics.CodeAnalysis;

using dingus_net;

using ScreepsDotNet.API.World;

namespace ScreepsDotNet
{
public static partial class Program
{
    private static IGame? s_game;

    private static Universe? s_universe;

    [DynamicDependency(DynamicallyAccessedMemberTypes.PublicMethods, typeof(Program))]
    public static void Main()
    {
        // Keep the entrypoint platform independent and let Init (which is called from js) create the game instance
        // This keeps the door open for unit testing later down the line
    }

    [System.Runtime.Versioning.SupportedOSPlatform("wasi")]
    public static void Init()
    {
        try
        {
            s_game = new Native.World.NativeGame();
            // TODO: Add startup logic here!

            s_universe = new Universe(s_game);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    [System.Runtime.Versioning.SupportedOSPlatform("wasi")]
    public static void Loop()
    {
        if (s_game == null) { return; }
        try
        {
            s_game.Tick();

            s_universe?.OnTick();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
}
