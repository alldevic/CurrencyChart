using System.IO;
using Nancy;

namespace CurrencyChart.Server.Modules
{
    public sealed class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", _ =>
            {
                if (!File.Exists("index.html"))
                {
                    return HttpStatusCode.NotFound;
                }

                return View["index.html"];
            });
        }
    }
}