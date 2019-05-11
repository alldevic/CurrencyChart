using System;
using System.Threading;
using System.Web.Hosting;
using CurrencyChart.Server.Hubs;
using CurrencyChart.Server.Models;
using LiteDB;
using Microsoft.AspNet.SignalR;

namespace CurrencyChart.Server.Services
{
    public class DataUpdateService : IRegisteredObject
    {
        private readonly IHubContext _chartHub;
        private Timer _timer;
        private volatile bool _sendingChartData;
        private readonly object _chartUpdateLock = new object();
        private readonly ChartNode _chartNode = new ChartNode();
        private readonly LiteRepository _documentStore;
        
        public DataUpdateService(LiteRepository documentStore)
        {
            _documentStore = documentStore;
            _chartHub = GlobalHost.ConnectionManager.GetHubContext<ChartHub>();
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
                var shorttime = time.ToString("mm:ss");
                _chartHub.Clients.All.addMessage(time, shorttime, _chartNode.LineChartData);
                
                _documentStore.Insert(new ChatMessage {Created = time, Message = _chartNode.LineChartData});
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