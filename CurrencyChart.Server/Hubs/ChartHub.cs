using System;
using CurrencyChart.Server.Models;
using CurrencyChart.Server.Services;
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
            var lineChart = new ChartNode();
            lineChart.SetLineChartData();
            Clients.All.UpdateChart(lineChart);
            var time = DateTime.UtcNow;
            Clients.All.addMessage(time, lineChart.ToString());
            _documentStore.Insert(new ChatMessage {Created = time, Message = lineChart.ToString()});
        }
    }
}