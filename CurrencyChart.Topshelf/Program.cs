using System;
using NLog;

namespace CurrencyChart.Topshelf
{
    internal class Program
    {
        static readonly Logger  Log = LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            Log.Info("Hello world!");
            Console.ReadKey();
        }
    }
}