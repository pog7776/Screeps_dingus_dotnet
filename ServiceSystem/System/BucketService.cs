using System;

using ScreepsDotNet.API.World;

namespace dingus_net.ServiceSystem.System
{
public class BucketService : Service
{
    private readonly CpuManager _cpu;

    public const double BucketSize = 10000;
    public double Bucket => _cpu.Bucket;
    public double Filled => Bucket / BucketSize;
    public bool IsFull => Bucket >= BucketSize;

    /// <inheritdoc />
    public BucketService(CpuManager cpu, IGame game) : base(game, nameof(BucketService)) => _cpu = cpu;

    /// <inheritdoc />
    public override void OnTick()
    {
        if(IsFull) { GeneratePixel(); }
    }

    public CpuGeneratePixelResult GeneratePixel() => _cpu.GeneratePixel();

    public double GetOverflow()
    {
        double used = _cpu.GetUsed();
        double total = used + _cpu.Bucket;
        return total - BucketSize;
    }
}
}
