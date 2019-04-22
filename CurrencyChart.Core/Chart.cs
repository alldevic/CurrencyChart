using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace CurrencyChart.Core
{
    public class Chart : Hub
    {
        public void InitChartData()
        {
            var lineChart = new ChartNode();
            lineChart.SetLineChartData();
            Clients.All.UpdateChart(lineChart);
        }
        
        public Task SendValue(int value) => Clients.All.SendAsync("ReceiveValue", value);
    }
}