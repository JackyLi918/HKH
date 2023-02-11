using System;
using System.Composition;

namespace HKH.Mef2.Integration
{
    /// <summary>
    /// Wrap the scoped DI Container like Autofac to get scoped object
    /// </summary>
    [Export(Constants.CONTRCTNAME_RESOLVERWRAPPER, typeof(IResolver))]
    [Shared]
    public sealed class ResolverWrapper : ResolverBase
    {
        private IResolver _resolver;
        public ResolverWrapper() { }
        internal void SetResolver(IResolver resolver)
        {
            _resolver = resolver;
        }

        public override object Resolve(Type type, string name = "")
        {
            if (_resolver == null) return null;
            return _resolver.Resolve(type, name);
        }
    }
}
