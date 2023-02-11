using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace HKH.Tasks.Configuration
{
    public class HKHTaskSettings : Dictionary<string, HKHTaskSetting>
    {
        internal static HKHTaskSettings Load(IConfiguration configuration)
        {
            var connStrings = new HKHTaskSettings();
            if (configuration != null)
            {
                var configs = configuration.GetSection("hkhTasks").GetChildren();
                if (configs != null)
                {
                    foreach (var config in configs)
                    {
                        var connString = HKHTaskSetting.Load(config);
                        if (connString != null) { connStrings.TryAdd(connString.Name, connString); }
                    }
                }
            }
            return connStrings;
        }
    }
}
