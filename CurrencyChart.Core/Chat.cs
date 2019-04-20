using Microsoft.AspNet.SignalR;

namespace CurrencyChart.Core
{
    public class Chat : Hub
    {
        public void Send(string message)
        {
            Clients.All.addMessage(message);
        }
    }
}
