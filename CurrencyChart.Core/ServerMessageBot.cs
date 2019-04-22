using System;
using System.Threading;
using System.Web.Hosting;
using Microsoft.AspNet.SignalR;

namespace CurrencyChart.Core
{
    public class ServerMessageBot : IRegisteredObject
    {
        private readonly IHubContext _uptimeHub;
        private Timer _timer;
 
 
        public ServerMessageBot()
        {
            _uptimeHub = GlobalHost.ConnectionManager.GetHubContext<Chat>();
 
            StartTimer();
        }
        private void StartTimer()
        {
            var delayStartby = TimeSpan.FromSeconds(2);
            var repeatEvery = TimeSpan.FromSeconds(5);
 
            _timer = new Timer(BroadcastUptimeToClients, null, delayStartby, repeatEvery);
        }
        private void BroadcastUptimeToClients(object state)
        {
            _uptimeHub.Clients.All.addMessage(DateTime.Now, "Message from back");
        }
     
        public void Stop(bool immediate)
        {
            _timer.Dispose();
 
            HostingEnvironment.UnregisterObject(this);
        }
    }
}