using Nancy;

namespace CurrencyChart.Core
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ => "Hello world!";
        }
    }
}