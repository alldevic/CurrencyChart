using System;
using System.IO;
using System.Linq;
using CurrencyChart.Server.Models;
using LiteDB;
using Nancy;

namespace CurrencyChart.Server.Modules
{
    public sealed class HomeModule : NancyModule
    {
        public HomeModule(LiteRepository documentStore)
        {
            Get("/", _ =>
            {
                if (!File.Exists("views/chart.html"))
                {
                    return "JsClient not found!";
                }

                var messages = documentStore.Fetch<ChatMessage>().OrderBy(d => d.Created).ToList();
                var model = new MessageLog(messages.Skip(Math.Max(0, messages.Count() - 10)).ToList());

                return View["views/chart.html", model];
            });
        }
    }
}