using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using DotNetRevolution.Core.Base;
using Shuttle.Core.Infrastructure;
using Shuttle.Esb;

namespace DotNetRevolution.ShuttleESB.Host
{
    public abstract class ShuttleESBHost : Disposable
    {
        public IServiceBus Bus { [Pure] get; private set; }
        
        public virtual void Start()
        {
            // init logging
            Log.Assign(new ConsoleLog(typeof(ShuttleESBHost)) { LogLevel = LogLevel.Trace });

            // init depedency injection container
            InitializeContainer();

            // init message handler factory
            var messageHandlerFactory = InitializeMessageHandlerFactory();

            // init subscription manager
            var subscriptionManager = InitializeSubscriptionManager();

            // init message serializer
            var messageSerializer = InitializeMessageSerializer();

            // init compression algorithm
            var compressionAlgorithm = InitializeCompressionAlgorithm();

            // init encryption algorithm
            var encryptionAlgorithm = InitializeEncryptionAlgorithm();
            
            // start service bus
            Bus = ServiceBus
                .Create(c => 
                    {
                        if (subscriptionManager != null)
                        {
                            c.SubscriptionManager(subscriptionManager);
                        }

                        if (messageHandlerFactory != null)
                        {
                            c.MessageHandlerFactory(messageHandlerFactory);
                        }

                        if (messageSerializer != null)
                        {
                            c.MessageSerializer(messageSerializer);
                        }

                        if (compressionAlgorithm != null)
                        {
                            c.AddCompressionAlgorithm(compressionAlgorithm);
                        }

                        if (encryptionAlgorithm != null)
                        {
                            c.AddEnryptionAlgorithm(encryptionAlgorithm);
                        }
                    });

            Contract.Assume(Bus != null);

            Bus.Start();
        }

        protected abstract List<Type> MessagesRequiringSubscriptions { get; }

        protected abstract ISerializer InitializeMessageSerializer();

        protected abstract void InitializeContainer();

        protected abstract ISubscriptionManager InitializeSubscriptionManager();

        protected abstract IMessageHandlerFactory InitializeMessageHandlerFactory();

        protected virtual IEncryptionAlgorithm InitializeEncryptionAlgorithm()
        {
            return new TripleDesEncryptionAlgorithm();
        }

        protected virtual ICompressionAlgorithm InitializeCompressionAlgorithm()
        {
            return new GZipCompressionAlgorithm();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing == false)
            {
                return;
            }

            if (Bus == null)
            {
                return;
            }

            Bus.Dispose();
        }
    }
}
