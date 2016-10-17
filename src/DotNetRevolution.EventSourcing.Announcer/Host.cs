using DotNetRevolution.ShuttleESB.Host;
using Shuttle.Core.Host;
using System;
using Shuttle.Core.Infrastructure;
using System.Collections.Generic;
using Shuttle.Esb;
using DotNetRevolution.ShuttleESB.Messaging;
using DotNetRevolution.Json;
using DotNetRevolution.ShuttleESB.Serialization;
using System.Text;
using Shuttle.Esb.SqlServer;
using System.Linq;

namespace DotNetRevolution.EventSourcing.Announcer
{
    public class Host : ShuttleESBHost, IHost
    {
        private readonly ShuttleMessageCatalog _messageCatalog;

        public Host()
        {
            _messageCatalog = new ShuttleMessageCatalog();
        }

        protected override IList<Type> MessagesRequiringSubscriptions
        {
            get
            {
                return _messageCatalog.Entries.Select(x => x.MessageType).ToList();
            }
        }
                
        protected override void InitializeContainer()
        {
            
        }
        
        protected override IMessageHandlerFactory InitializeMessageHandlerFactory()
        {
            return new CatalogMessageHandlerFactory(_messageCatalog);
        }

        protected override ISerializer InitializeMessageSerializer()
        {
            return new MessageSerializer(Encoding.UTF8, new JsonSerializer());
        }

        protected override ISubscriptionManager InitializeSubscriptionManager()
        {
            return SubscriptionManager.Default();
        }        
    }
}