using DotNetRevolution.Core.Querying;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DotNetRevolution.Core.Tests.Querying
{
    [TestClass]
    public class QueryCatalogTests
    {
        private IQueryCatalog _catalog;

        [TestInitialize]
        public void Init()
        {
            _catalog = new QueryCatalog();
        }

        [TestMethod]
        public void CanAddEntry()
        {
            _catalog.Add(new QueryEntry(typeof(Query1), typeof(MockQueryHandler<Query1, Query1.Result>)));

            Assert.IsNotNull(_catalog.GetEntry(typeof(Query1)));
        }

        [TestMethod]
        public void CanAddEntriesForTwoDifferentQuerys()
        {
            _catalog.Add(new QueryEntry(typeof(Query1), typeof(MockQueryHandler<Query1, Query1.Result>)));
            _catalog.Add(new QueryEntry(typeof(Query2), typeof(MockQueryHandler<Query2, Query2.Result>)));

            Assert.IsNotNull(_catalog.GetEntry(typeof(Query1)));
            Assert.IsNotNull(_catalog.GetEntry(typeof(Query2)));
        }

        [TestMethod]
        public void CanAddEntriesUsingFluentApi()
        {
            _catalog.Add(new QueryEntry(typeof(Query1), typeof(MockQueryHandler<Query1, Query1.Result>)))
                    .Add(new QueryEntry(typeof(Query2), typeof(MockQueryHandler<Query2, Query2.Result>)));

            Assert.IsNotNull(_catalog.GetEntry(typeof(Query1)));
            Assert.IsNotNull(_catalog.GetEntry(typeof(Query2)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotAddDuplicateEntry()
        {
            var entry = new QueryEntry(typeof(Query1), typeof(MockQueryHandler<Query1, Query1.Result>));

            _catalog.Add(entry);
            _catalog.Add(entry);            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotAddMoreThanOneEntryForSameTypeOfQuery()
        {
            var secondEntry = new QueryEntry(typeof(Query1), typeof(MockQueryHandler<Query1, Query1.Result>));

            _catalog.Add(new QueryEntry(typeof(Query1), typeof(MockQueryHandler<Query1, Query1.Result>)));
            _catalog.Add(secondEntry);
        }        
    }
}
