using System;
using System.Collections.Generic;

using ScreepsDotNet.API;
using ScreepsDotNet.API.World;

namespace dingus_net.ServiceSystem.System
{
public class CpuManager : Service, ICpu
{
    private ICpu Cpu => Game.Cpu;

    /// <inheritdoc />
    protected override string LogTag => "CPU";

    public BucketService BucketService { get; }

    private bool _canGeneratePixels = true;

    /// <inheritdoc />
    public CpuManager(IGame game) : base(game, nameof(CpuManager))
    {
        BucketService = new BucketService(this, game);
    }

    /// <inheritdoc />
    public override void OnTick()
    {
        BucketService.OnTick();
    }

    private void PrintCpu()
    {
        //if(! Game.Memory.TryGetBool("debugCPU", out bool debug) || ! debug) { return; }

        Log(
            $"Used: {GetUsed()} | Limit: {Limit} | Bucket: {Bucket} | TickLimit: {TickLimit}"
        );
    }

#region ICpu Implementation

    /// <inheritdoc />
    public HeapInfo GetHeapStatistics() => Cpu.GetHeapStatistics();

    /// <inheritdoc />
    public double GetUsed() => Cpu.GetUsed();

    /// <inheritdoc />
    public void Halt() => Cpu.Halt();

    /// <inheritdoc />
    public CpuSetShardLimitsResult SetShardLimits(IReadOnlyDictionary<string, double> shardLimits)
        => Cpu.SetShardLimits(shardLimits);

    /// <inheritdoc />
    public CpuUnlockResult Unlock() => Cpu.Unlock();

    /// <inheritdoc />
    public CpuGeneratePixelResult GeneratePixel()
    {
        if(! _canGeneratePixels) { return CpuGeneratePixelResult.Ok; }

        try
        {
            CpuGeneratePixelResult result = Cpu.GeneratePixel();

            Log($"Generated pixel: {result.ToString()}");

            return result;
        }
        catch
        {
            Log("Failed to generate pixel - Disabling pixel generation");
            _canGeneratePixels = false;
        }

        return CpuGeneratePixelResult.Ok;
    }

    /// <inheritdoc />
    public double Limit => Game.Cpu.Limit;

    /// <inheritdoc />
    public double TickLimit => Game.Cpu.TickLimit;

    /// <inheritdoc />
    public double Bucket => Game.Cpu.Bucket;

    /// <inheritdoc />
    public IReadOnlyDictionary<string, double> ShardLimits => Game.Cpu.ShardLimits;

    /// <inheritdoc />
    public bool Unlocked => Game.Cpu.Unlocked;

    /// <inheritdoc />
    public long? UnlockedTime => Game.Cpu.UnlockedTime;

#endregion
}
}
