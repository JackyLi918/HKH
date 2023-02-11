using System.Composition;

namespace Sample.Service.MefServices
{
    [Export(typeof(IMefService))]
    [ExportMetadata("id", "MefService1")]
    public class MefService1 : MefServiceBase
    {
        private static int counter = 0;
        public override string Name => "MefService1";

        public MefService1()
        {
            counter++;
            CreateCounter = counter;
        }
    }
}
