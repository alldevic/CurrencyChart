using System;
using CurrencyChart.Core.Models;
using LiteDB;
using Microsoft.AspNet.SignalR;

namespace CurrencyChart.Core
{
    public class Chat : Hub
    {
        private readonly LiteRepository _documentStore;

        public Chat(LiteRepository documentStore)
        {
            _documentStore = documentStore;
        }

        public void Send(string message)
        {
            var time = DateTime.UtcNow;
            Clients.All.addMessage(time, message);
            _documentStore.Insert(new ChatMessage {Created = time, Message = message});
        }
    }
}