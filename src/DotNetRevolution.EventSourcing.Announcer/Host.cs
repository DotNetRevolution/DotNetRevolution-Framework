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

namespace DotNetRevolution.EventSourcing.Announcer
{
    public class Host : ShuttleESBHost, IHost
    {
        protected override List<Type> MessagesRequiringSubscriptions
        {
            get
            {
                throw new NotImplementedException();
            }
        }
                
        protected override void InitializeContainer()
        {

        }
        
        protected override IMessageHandlerFactory InitializeMessageHandlerFactory()
        {
            return new CatalogMessageHandlerFactory(new ShuttleMessageCatalog());
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