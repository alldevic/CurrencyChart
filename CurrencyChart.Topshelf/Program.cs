using Topshelf;

namespace CurrencyChart.Topshelf
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var url = "http://+:8080";

            var host = HostFactory.New(x =>
            {
                x.UseNLog();

                x.Service<CurrencyChartService>(s =>
                {
                    s.ConstructUsing(settings => new CurrencyChartService());
                    s.WhenStarted(service => service.Start(url));
                    s.WhenStopped(service => service.Stop());
                });
                x.StartAutomatically();
                x.SetServiceName("curchartsvc");
                x.SetDisplayName("CurrencyChart Server");
                x.SetDescription("CurrencyChart for Hackathon Light 2019");
                x.RunAsNetworkService();
            });

            host.Run();
        }
    }
}