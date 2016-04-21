using DotNetRevolution.Core.Query;
using DotNetRevolution.Core.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace DotNetRevolution.Core.Tests.Query
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
            var result = _dispatcher.Dispatch<Query1.Result>(new Query1());
        }

        [TestMethod]
        [ExpectedException(typeof(QueryHandlingException))]
        public void CannotDispatchUnregisteredQuery()
        {
            var result = _dispatcher.Dispatch<Query1.Result>(new Query2());
        }
    }
}
