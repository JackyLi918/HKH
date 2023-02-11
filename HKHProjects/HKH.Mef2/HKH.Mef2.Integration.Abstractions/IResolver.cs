using System;
using System.Collections.Generic;

namespace HKH.Mef2.Integration
{
    /// <summary>
    /// Represents an abstraction over a dependency injection container.
    /// </summary>
    public interface IResolver
    {
        CompositionContainer MefContainer { get; }
        object Resolve(Type type, string name = "");
        T Resolve<T>(string name = "");
    }
}
