using System;
using System.Web.Hosting;
using CurrencyChart.Server;
using CurrencyChart.Server.Hubs;
using CurrencyChart.Server.Services;
using LiteDB;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

[assembly: OwinStartup(typeof(Startup))]

namespace CurrencyChart.Server
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
            
            var serializer = new JsonSerializer()
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                DateFormatString = "dd.MM.yyyy hh:mm:ss"
                
            };
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
            GlobalHost.DependencyResolver.Register(typeof(ChartHub), () => new ChartHub(_documentStore));
            
            HostingEnvironment.RegisterObject(new DataUpdateService(_documentStore));
            
            var sampleBootstrapper = new DefaultBootstrapper(_documentStore);

            app
                .UseFileServer(new FileServerOptions
                {
                    RequestPath = new PathString("/scripts"),
                    FileSystem = new PhysicalFileSystem("scripts")
                })
                .UseFileServer(new FileServerOptions
                {
                    RequestPath = new PathString("/codebehind"),
                    FileSystem = new PhysicalFileSystem("codebehind")
                })
                .Map("/signalr", map =>
                {
                    map.UseCors(CorsOptions.AllowAll);
                    var hubConfiguration = new HubConfiguration();
                    map.RunSignalR(hubConfiguration);
                })
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