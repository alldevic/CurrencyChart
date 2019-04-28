using System.Collections.Generic;

namespace CurrencyChart.Server.Models
{
    public class MessageLog
    {
        public List<ChatMessage> Messages { get; set; }

        public MessageLog(List<ChatMessage> messages)
        {
            Messages = messages;
        }
    }
}