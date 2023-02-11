using System.Text;
using Microsoft.AspNetCore.Mvc;
using Sample.Service.MefServices;

namespace HKH.Mef2.Integration.Autofac.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;
        private readonly CompositionContainer _mefContainer;

        public SampleController(ILogger<SampleController> logger, CompositionContainer mefContainer)
        {
            _logger = logger;
            _mefContainer = mefContainer;
        }

        [HttpGet(Name = "GetExports")]
        public object Get()
        {
            StringBuilder builder = new StringBuilder();
            
            var mefServices = _mefContainer.GetExports<IMefService>();  //get many
            foreach (var mef in mefServices)
            {
                builder.Append("multi-service intance: ");
                builder.AppendLine(mef.ToString());
            }

            // the MefSerice2 instance should have same CreateCounter since it is shared.
            var mefServices2 = _mefContainer.GetExports<Lazy<IMefService, Dictionary<string, object>>>();    //get with metadata
            foreach (var mef in mefServices2)
            {
                builder.Append($"multi-service with metadata intance (id={mef.Metadata.GetValueOrDefault("id")}): ");
                builder.AppendLine(mef.Value.ToString());
            }
                        
            return builder.ToString();
        }
    }
}