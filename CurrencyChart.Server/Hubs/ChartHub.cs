using System.Collections.Generic;
using CurrencyChart.Server.Models;
using LiteDB;
using Microsoft.AspNet.SignalR;

namespace CurrencyChart.Server.Hubs
{
    public class ChartHub : Hub
    {
        private readonly LiteRepository _documentStore;

        public ChartHub(LiteRepository documentStore)
        {
            _documentStore = documentStore;
        }

        public void InitChartData()
        {
//            var lineChart = new ChartNode();
//            lineChart.SetLineChartData();
//            Clients.All.UpdateChart(lineChart);
//            var time = DateTime.UtcNow;
//            Clients.All.addMessage(time, lineChart.ToString());
//            _documentStore.Insert(new ChatMessage {Created = time, Message = lineChart.ToString()});

            var initData = new InitData
            {
                LogCount = 5,
                LineCount = 10,
                Exchanges = new List<string> {"RUB/USD", "RUB/EUR", "RUB/CNY"},
                Providers = new List<string> {"Provider1", "Provider2", "Provider3"},
                DefaultCourse = "RUB/USD"
            };
            
            Clients.All.getInitData(initData);
        }
    }
}