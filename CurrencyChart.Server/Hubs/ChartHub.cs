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
                LogCount = 30,
                LineCount = 15,
                Exchanges = new List<string> {"RUB/USD", "RUB/EUR", "RUB/CNY"},
                Providers = new List<string> {"Provider1", "Provider2", "Provider3"},
                DefaultCourse = "RUB/USD",
                TimesInit = new List<string>
                {
                    "Long Time One", "Long Time Two", "Long Time Three", "Long Time Four", "Long Time Five",
                    "Long Time Six", "Long Time Seven", "Long Time Eight", "Long Time Nine", "Long Time Ten",
                },
                ShortTimesInit = new List<string>
                    {"Time1", "Time2", "Time3", "Time4", "Time5", "Time6", "Time7", "Time8", "Time9", "Time10"},
                DataValuesInit = new Dictionary<string, List<string>>
                {
                    {"Provider1", new List<string> {"1", "2", "1", "2", "1", "2", "1", "2", "1", "2"}},
                    {"Provider2", new List<string> {"2", "3", "4", "2", "3", "6", "2", "7", "4", "9"}},
                    {"Provider3", new List<string> {"7", "4", "9", "5", "2", "3", "8", "3", "7", "1"}}
                }
            };

            Clients.All.getInitData(initData);
        }

        public void FetchExchangeData(string exchangeCourse)
        {
            var initData = new InitData
            {
                LogCount = 30,
                LineCount = 15,
                Exchanges = new List<string> {"RUB/USD", "RUB/EUR", "RUB/CNY"},
                Providers = new List<string> {"Provider1", "Provider2", "Provider3"},
                DefaultCourse = "RUB/USD",
                TimesInit = new List<string>
                {
                    "Long Time One", "Long Time Two", "Long Time Three", "Long Time Four", "Long Time Five",
                    "Long Time Six", "Long Time Seven", "Long Time Eight", "Long Time Nine", "Long Time Ten",
                },
                ShortTimesInit = new List<string>
                    {"Time1", "Time2", "Time3", "Time4", "Time5", "Time6", "Time7", "Time8", "Time9", "Time10"},
                DataValuesInit = new Dictionary<string, List<string>>
                {
                    {"Provider1", new List<string> {"1", "2", "1", "2", "1", "2", "1", "2", "1", "2"}},
                    {"Provider2", new List<string> {"2", "3", "4", "2", "3", "6", "2", "7", "4", "9"}},
                    {"Provider3", new List<string> {"7", "4", "9", "5", "2", "3", "8", "3", "7", "1"}}
                }
            };
            Clients.Caller.GetExchangeData(initData);
        }
    }
}