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
                var model = new MessageLog(documentStore
                    .Fetch<ChatMessage>()
                    .OrderBy(d => d.Created).ToList());

                return View["views/chat.sshtml", model];
            };
        }
    }
}