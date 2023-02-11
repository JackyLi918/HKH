using System.Composition;

namespace Sample.Service.MefServices
{
    [Export(typeof(IMefService))]
    [Shared]
    [ExportMetadata("id", "MefService2")]
    public class MefService2 : MefServiceBase
    {
        private static int counter = 0;
        public override string Name => "MefService2";

        public MefService2()
        {
            counter++;
            CreateCounter = counter;
        }
    }
}
