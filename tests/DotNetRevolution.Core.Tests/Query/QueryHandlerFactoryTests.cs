using DotNetRevolution.Core.Query;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DotNetRevolution.Core.Tests.Query
{
    [TestClass]
    public class QueryHandlerFactoryTests
    {
        private IQueryHandlerFactory _factory;

        [TestInitialize]
        public void Init()
        {
            var catalog = new QueryCatalog(new Collection<QueryEntry>
            {
                new QueryEntry(typeof(Query1), typeof(MockQueryHandler<Query1, Query1.Result>)),
                new QueryEntry(typeof(Query2), typeof(MockReusableQueryHandler<Query2, Query2.Result>))
            });

            _factory = new QueryHandlerFactory(catalog);
        }

        [TestMethod]
        public void HasCatalog()
        {
            Assert.IsTrue(_factory.Catalog != null);
        }

        [TestMethod]
        public void CanGetHandlerForRegisteredQuery()
        {
            Assert.IsNotNull(_factory.GetHandler(typeof(Query1)));
        }

        [TestMethod]
        public void CanGetCachedHandler()
        {
            var handler = _factory.GetHandler(typeof(Query2));

            Assert.AreEqual(handler, _factory.GetHandler(typeof(Query2)));
        }

        [TestMethod]
        public void AssertHandlerDoesNotCache()
        {
            var handler = _factory.GetHandler(typeof(Query1));

            Assert.AreNotEqual(handler, _factory.GetHandler(typeof(Query1)));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void CannotGetHandlerForUnregisteredQuery()
        {
            _factory.GetHandler(typeof(object));
        }
    }
}
