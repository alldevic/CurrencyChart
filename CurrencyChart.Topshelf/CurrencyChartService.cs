using NLog;

namespace CurrencyChart.Topshelf
{
    public class CurrencyChartService
    {
        private Logger _log = LogManager.GetCurrentClassLogger();
        public void Start()
        {
            _log.Info("Server started");
        }

        public void Stop()
        {
            _log.Info("Server stopped");

        }
    }
}