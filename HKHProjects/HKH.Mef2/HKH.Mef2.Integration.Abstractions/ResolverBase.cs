using System;

namespace HKH.Mef2.Integration
{
    /// <summary>
    /// Represents an abstract base class which all <see cref="IResolver"/>
    /// implementators can inherit from.
    /// </summary>
    public abstract class ResolverBase : IResolver
    {
        public virtual CompositionContainer MefContainer => Resolve<CompositionContainer>();
        public abstract object Resolve(Type type, string name = "");
        public T Resolve<T>(string name = "")
        {
            return (T)Resolve(typeof(T), name);
        }
    }
}
