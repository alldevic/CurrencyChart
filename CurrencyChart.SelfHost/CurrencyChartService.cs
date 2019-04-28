using System;
using CurrencyChart.Server;

namespace CurrencyChart.SelfHost
{
    public class CurrencyChartService
    {
        private IDisposable _owin;

        public void Start(string url)
        {
            _owin = Startup.Start(url);
        }

        public void Stop()
        {
            _owin?.Dispose();
        }
    }
}