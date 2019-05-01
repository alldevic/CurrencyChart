using Newtonsoft.Json;

namespace CurrencyChart.Server.Services
{
    public class ChartNode
    {
        [JsonProperty("lineChartData")] private int[] _lineChartData;

        public void SetLineChartData()
        {
            _lineChartData = new[]
            {
                RandomNumberGenerator.RandomScalingFactor(),
                RandomNumberGenerator.RandomScalingFactor(),
                RandomNumberGenerator.RandomScalingFactor()
            };
        }

        public override string ToString() => JsonConvert.SerializeObject(_lineChartData);
    }
}