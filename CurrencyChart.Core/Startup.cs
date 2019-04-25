using System;
using System.Web.Hosting;
using CurrencyChart.Core;
using LiteDB;
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
        private readonly LiteRepository _documentStore;

        public Startup(LiteRepository documentStore)
        {
            _documentStore = documentStore;
        }

        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver = new DefaultDependencyResolver();
            GlobalHost.DependencyResolver.Register(typeof(Chart), () => new Chart(_documentStore));
            HostingEnvironment.RegisterObject(new ChartDataUpdate(_documentStore));
            var sampleBootstrapper = new SampleBootstrapper(_documentStore);

            app
                .UseFileServer(new FileServerOptions
                {
                    RequestPath = new PathString("/scripts"),
                    FileSystem = new PhysicalFileSystem("scripts")
                })
                .MapSignalR()
                .UseNancy(cfg => cfg.Bootstrapper = sampleBootstrapper);
        }

        public static IDisposable Start(string url)
        {
            var db = new LiteDatabase(@"storage.db");
            var rep = new LiteRepository(db, true);
            return WebApp.Start(url, app => new Startup(rep).Configuration(app));
        }
    }
}