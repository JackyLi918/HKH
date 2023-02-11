using System.Composition;

namespace HKH.Mef2.Integration
{
    /// <summary>
    /// all the MEF object to access DI Container should extend this class.
    /// </summary>
    public abstract class MefBase
    {
        [Import(Constants.CONTRCTNAME_RESOLVERWRAPPER)]
        public IResolver Resolver { get; set; }
     }
}
