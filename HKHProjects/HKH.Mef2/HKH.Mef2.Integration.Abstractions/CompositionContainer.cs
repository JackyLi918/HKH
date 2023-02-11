using System;
using System.Composition;
using System.Composition.Hosting;
using System.Composition.Hosting.Core;

namespace HKH.Mef2.Integration
{
    /// <summary>
    /// Custom CompositionContext to keep create new context for each scoped(for web this is pre-request) and connect MEF to DI Container
    /// </summary>
    public sealed class CompositionContainer : CompositionContext, IDisposable
    {
        private readonly CompositionHost _compositionHost;
        private readonly IResolver _scopedResolver;
        public CompositionContainer(IResolver resolver, ContainerConfiguration containerConfiguration)
        {
            _compositionHost = containerConfiguration.CreateContainer();
            _scopedResolver = resolver;
            SetScopedResolver(_compositionHost.GetExport<IResolver>(Constants.CONTRCTNAME_RESOLVERWRAPPER) as ResolverWrapper);
        }

        public override bool TryGetExport(CompositionContract contract, out object export)
        {
            return _compositionHost.TryGetExport(contract, out export);
        }

        private void SetScopedResolver(ResolverWrapper containerWrapper)
        {
            if (containerWrapper == null) return;
            containerWrapper.SetResolver(_scopedResolver);
        }

        public void Dispose()
        {
            _compositionHost.Dispose();
        }
    }
}
