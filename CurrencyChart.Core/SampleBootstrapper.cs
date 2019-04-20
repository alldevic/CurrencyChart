using LiteDB;
using Nancy;
using Nancy.TinyIoc;

namespace CurrencyChart.Core
{
    public class SampleBootstrapper : DefaultNancyBootstrapper
    {
        private readonly LiteRepository _documentStore;

        public SampleBootstrapper(LiteRepository documentStore)
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