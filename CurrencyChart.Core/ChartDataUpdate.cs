using System;
using System.Security.Cryptography;
using System.Threading;
using System.Web.Hosting;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace CurrencyChart.Core
{
    public class RandomNumberGenerator
    {
        static Random rnd1 = new Random();

        public static int randomScalingFactor()
        {
            return rnd1.Next(100);
        }

        public static int randomColorFactor()
        {
            return rnd1.Next(255);
        }
    }

    public class LineChart
    {
        [JsonProperty("lineChartData")] private int[] _lineChartData;
        [JsonProperty("colorString")] private string _colorString;

        public void SetLineChartData()
        {
            _lineChartData = new int[100];

            for (int i = 0; i < 100; i++)
            {
                _lineChartData[i] = RandomNumberGenerator.randomScalingFactor();
            }


            _colorString = "rgba(" + RandomNumberGenerator.randomColorFactor() + "," +
                           RandomNumberGenerator.randomColorFactor() + "," + RandomNumberGenerator.randomColorFactor() +
                           ",.3)";
        }
    }

    public class ChartDataUpdate : IRegisteredObject
    {
        private readonly IHubContext _chartHub;
        private Timer _timer;
        private volatile bool _sendingChartData;
        private readonly object _chartUpateLock = new object();
        LineChart lineChart = new LineChart();

        public ChartDataUpdate()
        {
            _chartHub = GlobalHost.ConnectionManager.GetHubContext<Chart>();
            StartTimer();
        }

        private void StartTimer()
        {
            var delayStartby = TimeSpan.FromSeconds(2);
            var repeatEvery = TimeSpan.FromSeconds(5);

            _timer = new Timer(BroadcastDataToClients, null, delayStartby, repeatEvery);
        }

        private void BroadcastDataToClients(object state)
        {
            if (_sendingChartData)
            {
                return;
            }

            lock (_chartUpateLock)
            {
                if (_sendingChartData)
                {
                    return;
                }

                _sendingChartData = true;
                lineChart.SetLineChartData();
                _chartHub.Clients.All.updateChart(lineChart);
                _sendingChartData = false;
            }
        }

        public void Stop(bool immediate)
        {
            _timer.Dispose();

            HostingEnvironment.UnregisterObject(this);
        }
    }
}