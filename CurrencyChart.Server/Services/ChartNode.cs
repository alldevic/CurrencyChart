using System.Collections.Generic;
using Newtonsoft.Json;

namespace CurrencyChart.Server.Services
{
    public class ChartNode
    {
        [JsonProperty("lineChartData")] public List<int> LineChartData { get; set; }

        public void SetLineChartData()
        {
            LineChartData = new List<int>
            {
                RandomNumberGenerator.RandomScalingFactor(),
                RandomNumberGenerator.RandomScalingFactor(),
                RandomNumberGenerator.RandomScalingFactor()
            };
        }
    }
}