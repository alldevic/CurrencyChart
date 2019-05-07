using System.Collections.Generic;
using Newtonsoft.Json;

namespace CurrencyChart.Server.Models
{
    public class InitData
    {
        [JsonProperty("lineCount")] public int LineCount { get; set; }
        [JsonProperty("defaultCourse")] public string DefaultCourse { get; set; }

        [JsonProperty("logCount")] public int LogCount { get; set; }

        [JsonProperty("providers")] public List<string> Providers { get; set; }
        [JsonProperty("exchanges")] public List<string> Exchanges { get; set; }

        [JsonProperty("timesInit")] public List<string> TimesInit { get; set; }

        [JsonProperty("shortTimesInit")] public List<string> ShortTimesInit { get; set; }

        [JsonProperty("dataValuesInit")] public Dictionary<string, List<string>> DataValuesInit { get; set; }
    }
}