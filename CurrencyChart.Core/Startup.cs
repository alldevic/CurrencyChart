using System;
using CurrencyChart.Core;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace CurrencyChart.Core
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();

        }

        public static IDisposable Start(string url)
        {
            return WebApp.Start<Startup>(url);
        }
    }
}