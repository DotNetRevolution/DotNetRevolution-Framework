using DotNetRevolution.Core.Domain;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DotNetRevolution.Core.Tests.Domain
{
    [TestClass]
    public class DomainEventCatalogTests
    {
        private IDomainEventCatalog _catalog;
        
        [TestInitialize]
        public void Init()
        {
            _catalog = new DomainEventCatalog();
        }

        [TestMethod]
        public void CanAddEntry()
        {
            _catalog.Add(new DomainEventEntry(typeof(DomainEvent1), typeof(MockDomainEventHandler<DomainEvent1>)));

            var entries = _catalog.GetEntries(typeof(DomainEvent1));

            Assert.IsTrue(entries.Count == 1);
        }

        [TestMethod]
        public void CanAddDuplicateEntry()
        {
            var entry = new DomainEventEntry(typeof(DomainEvent1), typeof(MockDomainEventHandler<DomainEvent1>));

            _catalog.Add(entry);
            _catalog.Add(entry);

            var entries = _catalog.GetEntries(typeof(DomainEvent1));

            Assert.IsTrue(entries.Count == 2);
        }

        [TestMethod]
        public void CanAddMoreThanOneEntryForSameTypeOfDomainEvent()
        {
            _catalog.Add(new DomainEventEntry(typeof(DomainEvent1), typeof(MockDomainEventHandler<DomainEvent1>)));
            _catalog.Add(new DomainEventEntry(typeof(DomainEvent1), typeof(MockDomainEventHandler<DomainEvent1>)));

            var entries = _catalog.GetEntries(typeof(DomainEvent1));

            Assert.IsTrue(entries.Count == 2);
        }

        [TestMethod]
        public void CanRemoveEntry()
        {
            var entry = new DomainEventEntry(typeof(DomainEvent1), typeof(MockDomainEventHandler<DomainEvent1>));

            _catalog.Add(entry);
            var entries = _catalog.GetEntries(typeof(DomainEvent1));            
            Assert.IsTrue(entries.Count == 1);

            _catalog.Remove(entry);
            entries = _catalog.GetEntries(typeof(DomainEvent1));            
            Assert.IsTrue(entries.Count == 0);
        }

        [TestMethod]
        public void CanTryGetEntries()
        {
            _catalog.Add(new DomainEventEntry(typeof(DomainEvent1), typeof(MockDomainEventHandler<DomainEvent1>)));

            IReadOnlyCollection<IDomainEventEntry> entries;

            var result = _catalog.TryGetEntries(typeof(DomainEvent1), out entries);

            Assert.IsTrue(result);
            Assert.IsTrue(entries.Count == 1);
        }

        //[TestMethod]
        //public void CanCreateTemporaryEntry()
        //{
        //    IReadOnlyCollection<IDomainEventEntry> entries;
        //    bool result;

        //    using (_catalog.Add(new DomainEventEntry(typeof(DomainEvent1), typeof(MockDomainEventHandler<DomainEvent1>))))
        //    {
        //        result = _catalog.TryGetEntries(typeof(DomainEvent1), out entries);

        //        Assert.IsTrue(result);
        //        Assert.IsTrue(entries.Count == 1);
        //    }
                        
        //    result = _catalog.TryGetEntries(typeof(DomainEvent1), out entries);

        //    Assert.IsTrue(result);
        //    Assert.IsTrue(entries.Count == 0);
        //}
    }
}
