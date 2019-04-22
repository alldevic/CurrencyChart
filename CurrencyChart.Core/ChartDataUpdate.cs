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
            return rnd1.Next(10);
        }

    }

    public class ChartNode
    {
        [JsonProperty("lineChartData")] private int _lineChartData;

        public void SetLineChartData()
        {
            _lineChartData = RandomNumberGenerator.randomScalingFactor();
        }
    }

    public class ChartDataUpdate : IRegisteredObject
    {
        private readonly IHubContext _chartHub;
        private Timer _timer;
        private volatile bool _sendingChartData;
        private readonly object _chartUpateLock = new object();
        ChartNode _chartNode = new ChartNode();

        public ChartDataUpdate()
        {
            _chartHub = GlobalHost.ConnectionManager.GetHubContext<Chart>();
            StartTimer();
        }

        private void StartTimer()
        {
            var delayStartby = TimeSpan.FromSeconds(1);
            var repeatEvery = TimeSpan.FromMilliseconds(500);

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
                _chartNode.SetLineChartData();
                _chartHub.Clients.All.updateChart(_chartNode);
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