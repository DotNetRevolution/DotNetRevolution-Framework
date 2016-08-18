using DotNetRevolution.Core.GuidGeneration;
using DotNetRevolution.Core.Hashing;
using DotNetRevolution.EventSourcing;
using DotNetRevolution.EventSourcing.Sql;
using DotNetRevolution.Json;
using DotNetRevolution.Test.EventStoreDomain.Account;
using DotNetRevolution.Test.EventStoreDomain.Account.Commands;
using DotNetRevolution.Test.EventStoreDomain.Account.DomainEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Text;

namespace DotNetRevolution.Test.EventStourcing.Sql
{
    [TestClass]
    public class EventProviderRepositoryTests
    {
        private IEventStore _eventStore;
        private AccountRepository _repository;

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

            _repository = new AccountRepository(_eventStore);
        }

        [TestMethod]
        public void CanGetEventStream()
        {
            AccountAggregateRoot account;
            AccountAggregateRoot result;

            var command = new Create(100);
            var domainEvents = AccountAggregateRoot.Create(100, out account);

            _repository.Commit(command, new EventStream(domainEvents), account);

            var eventStream = _repository.GetByIdentity(account.Identity, out result);

            Assert.IsNotNull(eventStream);
            Assert.IsNotNull(result);
        }
    }
}
