using System;
using System.Collections.Generic;
using Shuttle.ESB.Core;

namespace DotNetRevolution.ShuttleESB.SubscriptionManager
{
    public class EmptySubscriptionManager : ISubscriptionManager
    {
        public void Subscribe(IEnumerable<string> messageTypeFullNames)
        {
        }

        public void Subscribe(string messageTypeFullName)
        {
        }

        public void Subscribe(IEnumerable<Type> messageTypes)
        {
        }

        public void Subscribe(Type messageType)
        {
        }

        public void Subscribe<T>()
        {
        }

        public IEnumerable<string> GetSubscribedUris(object message)
        {
            return null;
        }
    }
}
