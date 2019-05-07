using System;
using System.Collections.Generic;

namespace CurrencyChart.Server.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public List<int> Message { get; set; }
    }
}