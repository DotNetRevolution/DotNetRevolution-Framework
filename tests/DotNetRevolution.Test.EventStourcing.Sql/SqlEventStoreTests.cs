using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.Sql;
using DotNetRevolution.Json;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Threading.Tasks;
using System;
using DotNetRevolution.Core.Hashing;
using System.Text;
using DotNetRevolution.Core.GuidGeneration;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents;

namespace DotNetRevolution.Test.EventStourcing.Sql
{
    [TestClass]
    public class SqlEventStoreTests
    {
        private IEventStore _eventStore;
        
        [TestInitialize]
        public void Init()
        {
            GuidGenerator.Default = new SequentialGuidGenerator(SequentialGuidType.SequentialAtEnd);
            var typeFactory = new DefaultTypeFactory(new MD5HashProvider(Encoding.UTF8));

            var hash = typeFactory.GetHash(typeof(Created));
            var e = typeFactory.GetType(hash);

            _eventStore = new SqlEventStore(                
                new DefaultUsernameProvider("UnitTester"),
                new JsonSerializer(),
                typeFactory,
                Encoding.UTF8,
                ConfigurationManager.ConnectionStrings["SqlEventStore"].ConnectionString);
        }

        [TestMethod]
        public void CanCommitTransactionAndGetEventProvider()
        {
            Assert.Fail();
            //AccountAggregateRoot account;

            //var command = new Create(100);
            //var domainEvents = AccountAggregateRoot.Create(100, out account);

            //_eventStore.Commit(new EventProviderTransaction(command, new EventStream(domainEvents), account));

            //var eventProvider = _eventStore.GetEventStream<AccountAggregateRoot>(domainEvents.AggregateRoot.Identity);

            //Assert.IsNotNull(eventProvider);
        }
        
        [TestMethod]
        public void AddManyRecords()
        {
            Parallel.For(0, 1000, i =>
             {
                 try
                 {
                     CanCommitTransactionAndGetEventProvider();
                 }
                 catch (Exception e)
                 { Assert.Fail(e.ToString()); }
             });
        }
        
        private bool CanDepositAmount(AccountAggregateRoot account, decimal amount, out string declinationReason)
        {
            declinationReason = string.Empty;

            return true;
        }
    }
}
