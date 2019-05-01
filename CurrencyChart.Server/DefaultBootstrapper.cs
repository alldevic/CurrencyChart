using LiteDB;
using Nancy;
using Nancy.TinyIoc;

namespace CurrencyChart.Server
{
    public class DefaultBootstrapper : DefaultNancyBootstrapper
    {
        private readonly LiteRepository _documentStore;

        public DefaultBootstrapper(LiteRepository documentStore)
        {
            _documentStore = documentStore;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(_documentStore);
        }
    }
}






























