using System.Composition;
using HKH.Mef2.Integration;

namespace Sample.Service.MefServices
{
    public interface IMefService
    {
        string Name { get; }
        string Description { get; }
        ISingleService? SingleService { get; }
        IScopedService? ScopedService { get; }
        int CreateCounter { get; }
    }

    public abstract class MefServiceBase : MefBase, IMefService
    {
        public abstract string Name { get; }
        public virtual string Description => string.Empty;

        public ISingleService? SingleService => Resolver?.Resolve<ISingleService>();

        public IScopedService? ScopedService => Resolver?.Resolve<IScopedService>();

        public int CreateCounter { get; protected set; }
        public override string ToString()
        {
            var s = Description;
            return $"{{Name: {Name}, CreateCounter: {CreateCounter}, SingleService: {SingleService?.CreateCounter}, ScopedService: {ScopedService?.CreateCounter}}}";
        }
    }
}
