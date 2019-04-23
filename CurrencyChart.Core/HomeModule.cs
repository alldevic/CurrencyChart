using System;
using System.Linq;
using CurrencyChart.Core.Models;
using LiteDB;
using Nancy;

namespace CurrencyChart.Core
{
    public class HomeModule : NancyModule
    {
        public HomeModule(LiteRepository documentStore)
        {
            Get["/"] = _ =>
            {
                var messages = documentStore.Fetch<ChatMessage>().OrderBy(d => d.Created).ToList();
                var model = new MessageLog(messages
                    .Skip(Math.Max(0, messages.Count() - 10))
                    .ToList());

                return View["views/chart.sshtml", model];
            };
        }
    }
}