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
            Clients.All.addMessage(message);
            _documentStore.Insert(new ChatMessage {Created = DateTime.UtcNow, Message = message});
        }
    }
}