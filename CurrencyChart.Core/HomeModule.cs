using Nancy;

namespace CurrencyChart.Core
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => View["views/chat.html"];
        }
    }
}