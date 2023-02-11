using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace HKH.Data.Configuration
{
    public class HKHConnectionStrings : Dictionary<string, HKHConnectionString>
    {
        internal static HKHConnectionStrings Load(IConfiguration configuration)
        {
            var connStrings = new HKHConnectionStrings();
            if (configuration != null)
            {
                var configs = configuration.GetSection("hkhConnectionStrings").GetChildren();
                if (configs != null)
                {
                    foreach (var config in configs)
                    {
                        var connString = HKHConnectionString.Load(config);
                        if (connString != null) { connStrings.TryAdd(connString.Name, connString); }
                    }
                }
            }
            return connStrings;
        }

        public IEnumerable<HKHConnectionString> ConnectionStrings => this.Values;
    }
}
