using System;
using System.Threading;
using System.Web.Hosting;
using CurrencyChart.Core.Models;
using LiteDB;
using Microsoft.AspNet.SignalR;
using Nancy.TinyIoc;
using Newtonsoft.Json;

namespace CurrencyChart.Core
{
    public static class RandomNumberGenerator
    {
        static readonly Random Rnd1 = new Random();

        public static int RandomScalingFactor() => Rnd1.Next(10);
    }

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

    public class ChartDataUpdate : IRegisteredObject
    {
        private readonly IHubContext _chartHub;
        private Timer _timer;
        private volatile bool _sendingChartData;
        private readonly object _chartUpdateLock = new object();
        private readonly ChartNode _chartNode = new ChartNode();
        private readonly LiteRepository _documentStore;
        
        public ChartDataUpdate(LiteRepository documentStore)
        {
            _documentStore = documentStore;
            _chartHub = GlobalHost.ConnectionManager.GetHubContext<Chart>();
            StartTimer();
        }

        private void StartTimer()
        {
            var delayStandby = TimeSpan.FromSeconds(1);
            var repeatEvery = TimeSpan.FromSeconds(2);

            _timer = new Timer(BroadcastDataToClients, null, delayStandby, repeatEvery);
        }

        private void BroadcastDataToClients(object state)
        {
            if (_sendingChartData)
            {
                return;
            }

            lock (_chartUpdateLock)
            {
                if (_sendingChartData)
                {
                    return;
                }

                _sendingChartData = true;
                _chartNode.SetLineChartData();
                _chartHub.Clients.All.updateChart(_chartNode);
                var time = DateTime.UtcNow;
                _chartHub.Clients.All.addMessage(time, _chartNode.ToString());
                
                _documentStore.Insert(new ChatMessage {Created = time, Message = _chartNode.ToString()});
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