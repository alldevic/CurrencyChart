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
            Get["/"] = _ => View["views/chat.html"];
            Get["/logs"] = ctx =>
            {
                var model = new MessageLog(documentStore
                    .Fetch<ChatMessage>()
                    .OrderBy(d => d.Created).ToList());


                return View["views/ChatLog.sshtml", model];
            };
        }
    }
}