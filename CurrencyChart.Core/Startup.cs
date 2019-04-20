using System;
using CurrencyChart.Core;
using LiteDB;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using NLog;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CurrencyChart.Core
{
    public class Startup
    {
        private readonly LiteRepository _documentStore;

        public Startup(LiteRepository documentStore)
        {
            _documentStore = documentStore;
        }

        public void Configuration(IAppBuilder app)
        {
            var resolver = new DefaultDependencyResolver();
            resolver.Register(typeof(Chat), () => new Chat(_documentStore));
            var hubConfig = new HubConfiguration
            {
                Resolver = resolver,
            };
            var sampleBootstrapper = new SampleBootstrapper(_documentStore);
            app
                .UseFileServer(new FileServerOptions
                {
                    RequestPath = new PathString("/scripts"),
                    FileSystem = new PhysicalFileSystem("scripts")
                })
                .MapHubs(hubConfig)
                .UseNancy(cfg => cfg.Bootstrapper = sampleBootstrapper);
        }

        public static IDisposable Start(string url)
        {
            var db = new LiteDatabase(@"MyData.db");
            var rep = new LiteRepository(db, true);
            return WebApp.Start(url, app => new Startup(rep).Configuration(app));
        }
    }
}