using DotNetRevolution.Core.Querying;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace DotNetRevolution.Core.Tests.Querying
{
    [TestClass]
    public class QueryDispatcherTests
    {        
        private IQueryDispatcher _dispatcher;

        [TestInitialize]
        public void Init()
        {            
            var catalog = new QueryCatalog(new Collection<QueryEntry>
                {
                    new QueryEntry(typeof(Query1), typeof(MockQueryHandler<Query1, Query1.Result>))
                });            
            
            _dispatcher = new QueryDispatcher(new QueryHandlerFactory(catalog));
        }

        [TestMethod]        
        public void CanDispatchRegisteredQuery()
        {
            var result = _dispatcher.Dispatch(new Query1());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CannotDispatchUnregisteredQuery()
        {
            var query = new Query2();

            try
            {
                var result = _dispatcher.Dispatch(query);
                Assert.Fail();
            }
            catch (QueryHandlingException exception)
            {
                Assert.AreEqual(query, exception.Query);
            }
        }
    }
}
