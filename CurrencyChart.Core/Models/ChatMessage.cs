using System;

namespace CurrencyChart.Core.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }
    }
}