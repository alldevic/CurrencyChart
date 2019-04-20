using System;
using CurrencyChart.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace CurrencyChart.Core
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var resolver = new DefaultDependencyResolver();
            resolver.Register(typeof(Chat), () => new Chat());
            var hubConfig = new HubConfiguration
            {
                Resolver = resolver,
            };

            app
                .UseFileServer(new FileServerOptions
                {
                    RequestPath = new PathString("/scripts"),
                    FileSystem = new PhysicalFileSystem("scripts")
                })
                .MapHubs(hubConfig)
                .UseNancy();
        }

        public static IDisposable Start(string url)
        {
            return WebApp.Start<Startup>(url);
        }
    }
}